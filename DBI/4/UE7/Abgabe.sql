use lagerverwaltung;

-- Anlieferung
go
create or alter procedure Anlieferung
    @ANr integer
    ,@Datum datetime
    ,@Stueck integer
as
begin
    declare cur_lager cursor for
        select
        lager.lnr
        ,coalesce(max(lfndNr), 0) + 1 as next_lfndnr
        ,stueckkap - coalesce(sum(stueck), 0) as space
    from
        lager
        left join lieferung
        on lager.lnr = lieferung.lnr
    group by lager.lnr, stueckkap

    open cur_lager
    declare @lnr int
    declare @next_lfndnr int
    declare @space int
    set nocount on
    fetch cur_lager into @lnr, @next_lfndnr, @space

    declare @Verteilung table (
        LNr    int
        ,Stueck int
    )

    while @@fetch_status=0
    begin
        if @Stueck <= 0
        begin
            break
        end

        if @space <= 0
        begin
            fetch cur_lager into @lnr, @next_lfndnr, @space
            continue
        end

        declare @to_add int
        if @Stueck > @space begin
            set @to_add = @space
        end
        else begin
            set @to_add = @Stueck
        end
        begin
            insert into lieferung
                (LNr, LfndNr, ANr, Datum, Stueck)
            values
                (@lnr ,@next_lfndnr ,@ANr ,@Datum ,@to_add)
            insert into @Verteilung
                (LNr, Stueck)
            values
                (@lnr ,@to_add)
            set @Stueck = @Stueck - @to_add
        end

        fetch cur_lager into @lnr, @next_lfndnr, @space
    end

    close cur_lager
    deallocate cur_lager

    select
        *
    from
        @Verteilung
    where @Stueck = 0
end
go

begin transaction;
exec Anlieferung 3, '2024-04-27', 15

exec Anlieferung 3, '2024-04-27', 50

exec Anlieferung 2, '2024-04-27', 405

exec Anlieferung 1, '2024-04-27', 200
rollback
go

-- Entnahme
go
create or alter procedure Entnahme
    @ANr integer
    ,@Stueck integer
as
begin
    declare cur_lager cursor for
    -- declare @ANr int = 3
        select
        lnr
        ,lfndNr
        ,stueck
    from
        lieferung
    where anr = @ANr
    order by datum

    open cur_lager
    declare @lnr int
    declare @lfndNr int
    declare @amount int
    set nocount on
    fetch cur_lager into @lnr, @lfndNr, @amount

    declare @Verteilung table (
        LNr    int
        ,Stueck int
    )

    while @@fetch_status=0
    begin
        if @Stueck <= 0
        begin
            break
        end

        if @Stueck > @amount
        begin
            insert into @Verteilung
                (LNr, Stueck)
            values
                (@lnr ,@amount)
            delete from lieferung
            where lnr = @lnr and lfndNr = @lfndNr and stueck = @amount
            set @Stueck = @Stueck - @amount
        end
        else
        begin
            insert into @Verteilung
                (LNr, Stueck)
            values
                (@lnr ,@Stueck)
            update lieferung set stueck = @amount - @Stueck
            where lnr = @lnr and lfndNr = @lfndNr and stueck = @amount
            set @Stueck = 0
        end

        fetch cur_lager into @lnr, @lfndNr, @amount
    end

    close cur_lager
    deallocate cur_lager

    select
        *
    from
        @Verteilung
    where @Stueck = 0
end
go

begin transaction;
exec Entnahme 3, 15

exec Entnahme 2, 5

exec Entnahme 1, 500
rollback
go

-- LagerLoeschen
go
create or alter procedure LagerLoeschen
    @LNr integer
as
begin
    delete from lieferung where lnr = @LNr
    delete from lager where lnr = @LNr
end
go

begin transaction;
exec LagerLoeschen 1
exec LagerLoeschen 3
exec LagerLoeschen 4
rollback
go

-- Bestand (von Tobi)
go
create or alter procedure dbo.stpBestand
as
begin

    set nocount on

    declare @lnr int;
    declare @lfndNR int;
    declare @anr int;
    declare @sum int = 0;

    declare @Bezeichnung varchar(30);
    declare @Ortaktuell varchar(30);
    declare @Ort varchar(30);
    declare @Datum datetime;
    declare @Stueck int;

    declare @ergebnis table (Bezeichnung varchar(50)
        ,Ort         varchar(50)
        ,Datum       date
        ,Stueck      int);

    declare crs_lieferung cursor for
        select
        lnr
        ,lfndNR
    from
        lieferung
    order by lnr,anr

    open crs_lieferung;

    fetch crs_lieferung into @lnr,@lfndNR;

    while @@FETCH_STATUS = 0
    begin

        fetch crs_lieferung into @lnr,@lfndNR

        if (@anr != (select
            anr
        from
            lieferung
        where lnr = @lnr and lfndNr = @lfndNR))
        begin
            insert into @ergebnis
            values('Summe' ,'' ,null ,@sum)
            set @sum = 0;
        end

        if (@Ortaktuell = (select
            ort
        from
            lager
        where lnr = @lnr))
        begin
            set @Ort = ''
        end
        else
        begin
            set @Ort = (select
                ort
            from
                lager
            where lnr = @lnr)
            set @Ortaktuell = @Ort
        end

        set @anr = (select
            anr
        from
            lieferung
        where lnr = @lnr and lfndNr = @lfndNR);
        set @Bezeichnung = (select
            Bezeichnung
        from
            Artikel
        where anr = @anr);
        set @Datum = (select
            datum
        from
            lieferung
        where lnr = @lnr and lfndNr = @lfndNR)
        set @Stueck = (select
            stueck
        from
            lieferung
        where lnr = @lnr and lfndNr = @lfndNR)
        set @sum += @Stueck

        insert into @ergebnis
        values
            (@Bezeichnung ,@Ort ,(select
                    convert(varchar(10), @Datum, 104)) ,@Stueck)
    end

    select
        *
    from
        @ergebnis;

    close crs_lieferung;
    deallocate crs_lieferung;
end
go

exec dbo.stpBestand;

-- Lagerbestand
go
create or alter procedure Lagerbestand
    @LNr int
as
begin
    select
        lnr
        ,ort
        ,stueckkap
    from
        lager
    where lnr = @LNr;

    select
        a.bezeichnung
        ,SUM(li.stueck) as bestand
    from
        lieferung li
        join artikel a on li.anr = a.anr
    where li.lnr = @LNr
    group by a.bezeichnung;
end
go

exec Lagerbestand 1;