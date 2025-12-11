drop table lt
go
drop table l
go
drop table t
go

---------------------------------------------------------
-- Tabelle der Lieferanten
---------------------------------------------------------
create table l
(
    lnr    char(2) primary key,
    lname  varchar(6),
    rabatt decimal(2),
    stadt  varchar(6)
)
go

---------------------------------------------------------
-- Tabelle der Teile
---------------------------------------------------------
create table t
(
    tnr   char(2) primary key,
    tname varchar(8),
    farbe varchar(5),
    preis decimal(10, 2),
    stadt varchar(6)
)
go

---------------------------------------------------------
-- Tabelle der Lieferungen
---------------------------------------------------------
create table lt
(
    lnr   char(2) references l,
    tnr   char(2) references t,
    menge decimal(4),
    primary key (lnr, tnr)
)
go

insert into l
values ('L1', 'Schmid', 20, 'London')
insert into l
values ('L2', 'Jonas', 10, 'Paris')
insert into l
values ('L3', 'Berger', 30, 'Paris')
insert into l
values ('L4', 'Klein', 20, 'London')
insert into l
values ('L5', 'Adam', 30, 'Athen')
go
insert into t
values ('T1', 'Mutter', 'rot', 12, 'London')
insert into t
values ('T2', 'Bolzen', 'gelb', 17, 'Paris')
insert into t
values ('T3', 'Schraube', 'blau', 17, 'Rom')
insert into t
values ('T4', 'Schraube', 'rot', 14, 'London')
insert into t
values ('T5', 'Welle', 'blau', 12, 'Paris')
insert into t
values ('T6', 'Zahnrad', 'rot', 19, 'London')
go
insert into lt
values ('L1', 'T1', 300)
insert into lt
values ('L1', 'T2', 200)
insert into lt
values ('L1', 'T3', 400)
insert into lt
values ('L1', 'T4', 200)
insert into lt
values ('L1', 'T5', 100)
insert into lt
values ('L1', 'T6', 100)
insert into lt
values ('L2', 'T1', 300)
insert into lt
values ('L2', 'T2', 400)
insert into lt
values ('L3', 'T2', 200)
insert into lt
values ('L4', 'T2', 200)
insert into lt
values ('L4', 'T4', 300)
insert into lt
values ('L4', 'T5', 400)
go

select *
from l;
select *
from t;
select *
from lt;

--------------------------------------------------------------
--------------------------------------------------------------
-- Die Verwendung von IF 
-- Wenn die Anzahl der Teile am Lager 'L1' gr��er als 10 ist, dann Meldung ausgeben,
-- sonst von jedem Teil am Lager 'L1' Name des Teils, Farbe, Menge ausgeben

if (select sum(lt.menge)
    from lt
    where lt.lnr = 'L1') > 10
    begin
        print '10+ Teile am Lager'
    end
else
    begin
        select t.tname, t.farbe, lt.menge
        from lt
                 join t on t.tnr = lt.tnr
    end
go

--------------------------------------------------------------
--------------------------------------------------------------
-- Die while Anweisung
-- Solange die Summe der Menge aller Artikel im Lager kleiner als 10000 ist,
-- soll die Menge um 10 % erh�ht werden. 
-- Wenn jedoch der Maximalwert der Menge eines Teiles gr��er als 500 ist,
-- soll abgebrochen werden


-- Variante 1 (mit If und break)

begin transaction;
select *
from lt
while (select sum(lt.menge)
       from lt) < 10000
    begin
        if exists (select (lt.menge)
                   from lt
                   where menge > 500)
            begin
                break
            end
        update lt
        set menge = menge * 1.1
    end
select *
from lt
rollback
go

-- Variante 2

begin transaction;
select *
from lt
while (select sum(lt.menge)
       from lt) < 10000 and
      not exists (select lt.menge
                  from lt
                  where lt.menge > 500)
    begin
        update lt
        set menge = menge * 1.1
    end
select *
from lt
rollback
go

----------------------------------------------------------
----------------------------------------------------------
-- Lokale Variablen
-- 'Durchschnitt' und 'Grenze' sind zwei Variablen
-- 'Grenze' hat den fixen Wert 300
-- 'Durchschnitt' von Menge in Tabelle lt
-- Falls die Maximalmenge eines Artikels im Lager 'L1' gr��er als 'Grenze' ist,
-- soll die Menge vom 'L1'im Lager um den Durchschnitt erh�ht werden.


--------------------------
-- BEURTEILUNGSRELEVANT --
--------------------------

declare @durchschnitt int = (select avg(lt.menge)
                             from lt);
declare @grenze int = 300;

begin transaction
select *
from lt
if exists (select lt.menge
           from lt
           where lt.lnr = 'L1'
             and lt.menge > @grenze)
    begin
        update lt
        set lt.menge = lt.menge + @durchschnitt
        where lt.lnr = 'L1'
          and lt.menge > @grenze
    end
select *
from lt
rollback
go

-------------------------------------------------------
-------------------------------------------------------
-- Stored Procedure 1
-- die Mengen der Tabelle lt sollen um einen mit�bergebenen Prozentwert erh�ht werden
-- anlegen:

drop procedure if exists dbo.stpupdatemengetabelle

go
create procedure dbo.stpupdatemengetabelle @prozentwert float
as
begin
    set nocount on;
    update lt
    set lt.menge = lt.menge * (1 + @prozentwert / 100)
end
go

begin transaction
select *
from lt
exec dbo.stpupdatemengetabelle @prozentwert = 10;
select *
from lt
rollback
go

---------------------------------------------------------
---------------------------------------------------------
-- stored Procedure 2
-- es soll der �bergebene Artikel aus lt gel�scht werden und die mengen der restlichen artikel
-- um 5 % erh�ht werden. - verschachtelter prozeduraufruf
-- anlegen:

drop procedure if exists dbo.stpdeletearticle

go
create procedure dbo.stpdeletearticle @articleid char(2)
as
begin
    set nocount on;
    delete from lt where lt.tnr = @articleid
    exec dbo.stpupdatemengetabelle @prozentwert = 5
end
go

begin transaction
select *
from lt
exec dbo.stpdeletearticle @articleid = 'T1'
select *
from lt
rollback
go

-------------------------------------------------------------
-- Erstellen Sie eine Prozedur del_l (lnr) mit Output-Parameter:
-- Zeile aus L l�schen; dabei eventuell vorher entsprechende Zeilen aus lt l�schen;
-- zur�ckgeben, wie viele Zeilen aus lt gel�scht werden mu�ten


--------------------------
-- BEURTEILUNGSRELEVANT --
--------------------------

drop procedure if exists dbo.stpdel_l

go
create procedure dbo.stpdel_l @lieferant char(2)
as
begin
    declare @amountoflines int = (select count(l.lnr) from l)
    delete from lt where lt.lnr = @lieferant
    delete from l where l.lnr = @lieferant
    declare @amountoflinesafter int = (select count(l.lnr) from l)
    print 'Amount of Deleted Lines: ' + convert(varchar, @amountoflines - @amountoflinesafter)
end
go

begin transaction
select *
from l
exec dbo.stpdel_l @lieferant = 'L1'
select *
from l
rollback
go

-----------------------------------------------------------------
-- Erstellen Sie eine Prozedur clear_lt(m) returning
-- Solange die Summe der Mengen in lt gr��er als m ist, die Lieferung mit der jeweils niedrigsten Menge
-- l�schen; zur�ckgeben, wie viele Lieferungen gel�scht wurden; keine rekursive L�sung einsetzen
------------------------------------

--------------------------
-- BEURTEILUNGSRELEVANT --
--------------------------

drop procedure if exists dbo.stpclear_lt

go
create procedure dbo.stpclear_lt @m int
as
begin
    declare @deletedlines int = 0
    while (select sum(lt.menge)
           from lt) > @m
        begin
            delete from lt where lt.menge in (select min(menge) from lt)
            set @deletedlines = @deletedlines + 1
        end
    print '# deleted Deliveries: ' + convert(varchar, @deletedlines)
end
go

begin transaction
select *
from lt
exec dbo.stpclear_lt @m = 1000
select *
from lt
rollback
go