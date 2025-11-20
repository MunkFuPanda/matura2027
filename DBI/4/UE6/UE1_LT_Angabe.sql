

DROP TABLE lt
go
DROP TABLE l
go
DROP TABLE t
go

---------------------------------------------------------
-- Tabelle der Lieferanten
---------------------------------------------------------
CREATE TABLE l (
       lnr    CHAR(2) PRIMARY KEY,
       lname  VARCHAR(6),
       rabatt DECIMAL(2),
       stadt  VARCHAR(6))
go

---------------------------------------------------------
-- Tabelle der Teile
---------------------------------------------------------
CREATE TABLE t (
       tnr    CHAR(2) PRIMARY KEY,
       tname  VARCHAR(8),
       farbe  VARCHAR(5),
       preis  DECIMAL(10,2),
       stadt  VARCHAR(6))
go

---------------------------------------------------------
-- Tabelle der Lieferungen
---------------------------------------------------------
CREATE TABLE lt (
       lnr    CHAR(2) REFERENCES l,
       tnr    CHAR(2) REFERENCES t,
       menge  DECIMAL(4),
       PRIMARY KEY (lnr,tnr))
go

INSERT INTO l VALUES ('L1','Schmid',20,'London')
INSERT INTO l VALUES ('L2','Jonas', 10,'Paris' )
INSERT INTO l VALUES ('L3','Berger',30,'Paris' )
INSERT INTO l VALUES ('L4','Klein', 20,'London')
INSERT INTO l VALUES ('L5','Adam',  30,'Athen' )
go
INSERT INTO t VALUES ('T1','Mutter',  'rot',  12,'London')
INSERT INTO t VALUES ('T2','Bolzen',  'gelb', 17,'Paris' )
INSERT INTO t VALUES ('T3','Schraube','blau', 17,'Rom'   )
INSERT INTO t VALUES ('T4','Schraube','rot',  14,'London')
INSERT INTO t VALUES ('T5','Welle',   'blau', 12,'Paris' )
INSERT INTO t VALUES ('T6','Zahnrad', 'rot',  19,'London')
go
INSERT INTO lt VALUES ('L1','T1',300)
INSERT INTO lt VALUES ('L1','T2',200)
INSERT INTO lt VALUES ('L1','T3',400)
INSERT INTO lt VALUES ('L1','T4',200)
INSERT INTO lt VALUES ('L1','T5',100)
INSERT INTO lt VALUES ('L1','T6',100)
INSERT INTO lt VALUES ('L2','T1',300)
INSERT INTO lt VALUES ('L2','T2',400)
INSERT INTO lt VALUES ('L3','T2',200)
INSERT INTO lt VALUES ('L4','T2',200)
INSERT INTO lt VALUES ('L4','T4',300)
INSERT INTO lt VALUES ('L4','T5',400)
go

select * from l;
select * from t;
select * from lt;

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
select * from lt
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
select * from lt
rollback
go

-- Variante 2
 
begin transaction;
select * from lt
while (select sum(lt.menge)
        from lt) < 10000 and
        not exists (select lt.menge
        from lt
        where lt.menge > 500)
begin
    update lt
    set menge = menge * 1.1
end
select * from lt
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

declare @Durchschnitt int = (select avg(lt.menge) from lt);
declare @Grenze int = 300;

begin transaction
select * from lt
if exists (select lt.menge
    from lt
    where lt.lnr = 'L1' and lt.menge > @Grenze)
begin
    update lt
    set lt.menge = lt.menge + @Durchschnitt
    where lt.lnr = 'L1' and lt.menge > @Grenze
end
select * from lt
rollback
go

-------------------------------------------------------
-------------------------------------------------------
-- Stored Procedure 1
-- die Mengen der Tabelle lt sollen um einen mit�bergebenen Prozentwert erh�ht werden
-- anlegen:

drop procedure if exists dbo.stpUpdateMengeTabelle

create procedure dbo.stpUpdateMengeTabelle
    @Prozentwert float
as
begin
    set nocount on;
    update lt
    set lt.menge = lt.menge * (1 + @Prozentwert / 100)
end
go

begin transaction
select * from lt
exec dbo.stpUpdateMengeTabelle @Prozentwert = 10;
select * from lt
rollback
go

---------------------------------------------------------
---------------------------------------------------------
-- stored Procedure 2
-- es soll der �bergebene Artikel aus lt gel�scht werden und die mengen der restlichen artikel
-- um 5 % erh�ht werden. - verschachtelter prozeduraufruf
-- anlegen:

drop procedure if exists dbo.stpDeleteArticle

create procedure dbo.stpDeleteArticle
    @ArticleID char(2)
as
begin
    set nocount on;
    delete from lt where lt.tnr = @ArticleID
    exec dbo.stpUpdateMengeTabelle @Prozentwert = 5
end
go

begin transaction
select * from lt
exec dbo.stpDeleteArticle @ArticleID = 'T1'
select * from lt
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

create procedure dbo.stpdel_l
    @Lieferant char(2)
as
begin
    declare @amountOfLines int = (select count(l.lnr) from l)
    delete from lt where lt.lnr = @Lieferant
    delete from l where l.lnr = @Lieferant
    declare @amountOfLinesAfter int = (select count(l.lnr) from l)
    print 'Amount of Deleted Lines: ' + convert(varchar, @amountOfLines - @amountOfLinesAfter)
end
go

begin transaction
select * from l
exec dbo.stpdel_l @Lieferant = 'L1'
select * from l
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

create procedure dbo.stpclear_lt
    @m int
as
begin
    declare @deletedLines int = 0
    while (select sum(lt.menge)
        from lt) > @m
    begin
        delete from lt where lt.menge in (select min(menge) from lt)
        set @deletedLines = @deletedLines + 1
    end
    print '# deleted Deliveries: ' + convert(varchar, @deletedLines)
end
go

begin transaction
select * from lt
exec dbo.stpclear_lt @m = 1000
select * from lt
rollback
go