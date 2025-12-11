use mensa

/*select *
from menue
select *
from menue_besteht_aus
select *
from speise
select *
from speise_besteht_aus
select *
from zutat
*/
go
create or alter procedure dbo.zutatenliste @menuenr int,
                                           @anzahl int
as
begin
    declare @ausgabe table
                     (
                         zutatennr     int,
                         zutatenname   varchar(100),
                         gesamtmenge   decimal(8, 2),
                         einheit       varchar(20),
                         fehlendemenge decimal(8, 2)
                     )

    declare @speisenr int

    declare cur_speisen cursor for
        select speisenr
        from speise
        where speise.speisenr in (select mbsa.speisenr
                                  from menue_besteht_aus mbsa
                                  where mbsa.menuenr = @menuenr)

    open cur_speisen
    fetch cur_speisen into @speisenr

    while @@fetch_status = 0
        begin
            declare @zutatennr int
            declare @bezeichnung varchar(35)
            declare @einheit char(3)
            declare @menge float

            declare cur_zutaten cursor for
                select zutat.zutatennr, bezeichnung, einheit, menge
                from zutat
                         left join speise_besteht_aus
                                   on zutat.zutatennr = speise_besteht_aus.zutatennr
                where zutat.zutatennr in (select zutatennr
                                          from speise_besteht_aus
                                          where speisenr = @speisenr)

            open cur_zutaten
            fetch cur_zutaten into @zutatennr, @bezeichnung, @einheit, @menge

            while @@fetch_status = 0
                begin
                    if exists (select * from @ausgabe where zutatennr = @zutatennr)
                        begin
                            update @ausgabe set gesamtmenge += @menge * @anzahl where zutatennr = @zutatennr
                        end
                    else
                        begin
                            insert into @ausgabe (zutatennr, zutatenname, gesamtmenge, einheit, fehlendemenge)
                            values (@zutatennr, @bezeichnung, @menge * @anzahl, @einheit, 0)
                        end

                    fetch cur_zutaten into @zutatennr, @bezeichnung, @einheit, @menge
                end

            close cur_zutaten
            deallocate cur_zutaten
            fetch cur_speisen into @speisenr
        end
    close cur_speisen
    deallocate cur_speisen


    update a
    set fehlendemenge = iif(a.gesamtmenge - zutat.aktbestand < 0, 0, a.gesamtmenge - zutat.aktbestand)
    from @ausgabe a
             left join zutat on a.zutatennr = zutat.zutatennr;

    update zutat set aktbestand

    select * from @ausgabe
end
go

begin transaction
exec dbo.zutatenliste @menuenr = 22, @anzahl = 15
rollback
go
