USE imkerei;
GO
--1) Geben Sie die Betriebsnummer und den Namen aller Landwirtschaftsbetriebe an, die als Haupterzeugnis 'Mais' haben
--   und von der Betriebsform 'GmbH' sind.
--   Sortieren Sie die Ausgabe aufsteigend nach dem Namen des Landwirtschaftsbetriebes

select BetriebsNr, Name
from Landwirtschaftsbetrieb
where Haupterzeugnis = 'Mais'
     and Betriebsform = 'GmbH'
order by Name asc;

--2) Geben Sie f�r alle Typen von Bienenst�cken den durchschnittlichen Honigertrag aus,
--   wobei nur jene Bienenst�cke ber�cksichtigt werden sollen, die mehr als 30 Arbeiterinnen vorweisen.
--   Sortieren Sie das Ergebnis absteigend nach dem durchschnittlichen Ertrag

select Typ, avg(Honigertrag) as Durchschnittlicher_Honigertrag
from Bienenstock
join Arbeiterin on Bienenstock.StockNr = Arbeiterin.arbeitetInStockNr
group by Typ
having count(Arbeiterin.Kennzahl) > 30
order by Durchschnittlicher_Honigertrag desc;

--3) Geben Sie den Namen und das Geburtsdatum aller Imker und Imkerinnen aus,
--   die keine Hilfsarbeitende haben und für keine Bienenstöcke vom Typ "Magazin" zuständig sind.
--   Ordnen Sie die Ergebnisse absteigend nach dem Geburtsdatum.

select Name, GeborenAm
from Imker
where ImkerNr not in (
    select distinct stelltAn
    from Hilfsarbeiter
)
and ImkerNr not in (
    select distinct zustaendigFuer
    from Bienenstock
    where Typ = 'Magazin'
) order by GeborenAm desc

--4) Geben Sie die Imkernummer und Namen des Imkers bzw. der Imkerin an,
--   der bzw. die durchschnittlich am meisten f�r angestellte Hilfsarbeitende zahlt.

select Imker.ImkerNr, Imker.Name
from Imker
join Hilfsarbeiter on Imker.ImkerNr = Hilfsarbeiter.stelltAn
group by Imker.ImkerNr, Imker.Name
having avg(Hilfsarbeiter.Lohn) = (
    select max(DurchschnittsLohn)
    from (
        select stelltAn, avg(Lohn) as DurchschnittsLohn
        from Hilfsarbeiter
        group by stelltAn
    ) as DurchschnittsLohnProImker
);

--5) Geben Sie die Namen und das Geburtsdatum der Imker und Imkerinnen aus, deren Bienen alle Felder best�uben.
select distinct Imker.Name, Imker.GeborenAm
from Imker
join Bienenstock on Imker.ImkerNr = Bienenstock.zustaendigFuer
where not exists (
    select *
    from Feld
    where not exists (
        select *
        from Arbeiterin
        join bestaeubt on Arbeiterin.Kennzahl = bestaeubt.Kennzahl
        where Arbeiterin.arbeitetInStockNr = Bienenstock.StockNr
          and bestaeubt.FeldKennzahl = Feld.FeldKennzahl
    )
);

--6) Geben Sie die Namen alle Imkerei-Meister und Meisterinnen und die Namen all derer Lehrlinge aus,
--   die selber auch einen Lehrling gelehrt haben. Falls keine passenden Lehrlinge existieren,
--   soll stattdessen in der Spalte f�r Lehrling "kein Lehrling vorhanden" ausgegeben werden.
--   Dies gilt nat�rlich auch f�r Imker und Imkerinnen, die �berhaupt keine Lehrlinge gehabt haben.

select Imker.Name as ImkerMeister, 
       coalesce(Lehrling.Name, 'kein Lehrling vorhanden') as Lehrling
from Imker
left join gelerntVon on Imker.ImkerNr = gelerntVon.Meister
left join Imker as Lehrling on gelerntVon.Lehrling = Lehrling.ImkerNr;

--7) Geben Sie die Namen aller Imker und ihrer Bienenstock Typen und Stocknummern aus, falls diese Bienenst�cke einen
--   Honigertrag von 300kg oder mehr haben und mindestens 3 Brutnester besitzen.

select Imker.Name, Bienenstock.Typ, Bienenstock.StockNr
from Bienenstock
join Imker on Bienenstock.zustaendigFuer = Imker.ImkerNr
join Brutnest on Bienenstock.StockNr = Brutnest.liegtInStockNr
where Bienenstock.Honigertrag >= 300
group by Imker.Name, Bienenstock.Typ, Bienenstock.StockNr
having count(Brutnest.NestNr) >= 3;

--8) Geben Sie f�r jedes Feld, gekennzeichnet durch die Feldkennzahl und den Ort, aus von wie vielen Landwirtschaftsbetrieb
--   es verwendet wird, welchen Fl�chenanteil ein Betrieb im Durchschnitt verwendet und was durchschnittlich f�r die Best�ubung
--   dieses Feldes an Imker gezahlt wird. Auch soll f�r jedes Feld angegeben werden wie viele Arbeiterinnen es best�uben.

select Feld.Feldkennzahl, Feld.Ort
           , count(distinct verwendetVon.BetriebsNr) as Anzahl_Landwirtschaftsbetriebe
           , avg(verwendetVon.Flaechenanteil) as Durchschnittlicher_Flaechenanteil
           , avg(bezahltFuer.Betrag) as Durchschnittlich_Bezahlt_An_Imker
           , count(distinct bestaeubt.Kennzahl) as Anzahl_Arbeiterinnen
from Feld
left join verwendetVon on Feld.Feldkennzahl = verwendetVon.FeldKennzahl and verwendetVon.Ort = Feld.Ort
left join bezahltFuer on Feld.Feldkennzahl = bezahltFuer.FeldKennzahl and bezahltFuer.Ort = Feld.Ort
left join bestaeubt on Feld.Feldkennzahl = bestaeubt.FeldKennzahl and bestaeubt.Ort = Feld.Ort
group by Feld.Feldkennzahl, Feld.Ort;

--9) Geben Sie den Namen eines Imkers bzw. einer Imkerin, einen Ortsnamen und einen Betrag aus, die folgende Bedingung erf�llen:
--   Der Imker oder die Imkerin muss f�r alle Felder in diesem Ort als Summe genannten Betrag bezahlt bekommen haben
--   und dieser muss h�her sein als den Betrag den jeder andere Imker oder jede andere Imkerin in diesem Ort
--   bekommen hat ("Betrag" ist hier verstanden als die Summe, die von allen Landwirtschaftsbetriebe die Felder
--   dieses Ortes verwenden, bezahlt wird). Ordnen Sie die Ergebnisse absteigend nach dem Betrag.

select Imker.Name, bezahltFuer.Ort, sum(bezahltFuer.Betrag) as Gesamtbetrag
from Imker
join bezahltFuer on bezahltFuer.ImkerNr = Imker.ImkerNr
join Feld on bezahltFuer.FeldKennzahl = Feld.FeldKennzahl and bezahltFuer.Ort = Feld.Ort
group by Imker.Name, bezahltFuer.Ort, bezahltFuer.ImkerNr
having sum(bezahltFuer.Betrag) > all (
    select sum(bezahltFuer2.Betrag)
    from bezahltFuer as bezahltFuer2
    join Feld as Feld2 on bezahltFuer2.FeldKennzahl = Feld2.FeldKennzahl and bezahltFuer2.Ort = Feld2.Ort
    where Feld2.Ort = bezahltFuer.Ort
      and bezahltFuer2.ImkerNr <> bezahltFuer.ImkerNr
    group by bezahltFuer2.ImkerNr
)
order by Gesamtbetrag desc;

--10)Geben Sie alle Bienenst�cke (StockNr und Typ) aus, deren Arbeiterinnen weniger als 10 Felder best�uben und
--   die maximal 2 Brutnester besit�zen, deren Honigertrag aber durchschnittlich oder besser
--   (im Vergleich zu allen Bienenst�cken) ist. Ordnen Sie die aufsteigend Ergebnisse nach der Stocknummer.

select Bienenstock.StockNr, Bienenstock.Typ
from Bienenstock
left join Arbeiterin on Bienenstock.StockNr = Arbeiterin.arbeitetInStockNr
left join bestaeubt on Arbeiterin.Kennzahl = bestaeubt.Kennzahl
left join Brutnest on Bienenstock.StockNr = Brutnest.liegtInStockNr
group by Bienenstock.StockNr, Bienenstock.Typ, Bienenstock.Honigertrag
having count(distinct bestaeubt.FeldKennzahl) < 10
   and count(distinct Brutnest.NestNr) <= 2
   and Bienenstock.Honigertrag >= (
       select avg(Honigertrag)
       from Bienenstock
   )
order by Bienenstock.StockNr asc;
         
--11)Geben Sie f�r jede Gattung Biene an, wie viele Arbeiterinnen und K�niginnen vorhanden sind,
--   und wie viele Felder von der Arbeiterinnen der jeweiligen Gattung durchschnittlich best�ubt werden,
--   zus�tzlich dazu auch die minimale und maximal Anzahl Felder die pro Biene der jeweiligen Gattung best�ubt werden.

-- fehlende Tabellenangaben, alles scheiße

--12)Geben Sie die Kennzahl und den Ort aller Felder aus, die von allen Landwirtschaftsbetrieben verwendet werden.
--   (In anderen Worten, es soll keinen Landwirtschaftsbetrieb geben, der von diesen Feldern nicht einen gewissen Anteil
--   verwendet) Ordnen Sie das Ergebnis aufsteigend nach der Kennzahl.

select Feld.Feldkennzahl, Feld.Ort
from Feld
where not exists (
    select *
    from Landwirtschaftsbetrieb
    where not exists (
        select * from verwendetVon
        where verwendetVon.FeldKennzahl = Feld.Feldkennzahl
          and verwendetVon.Ort = Feld.Ort
          and verwendetVon.BetriebsNr = Landwirtschaftsbetrieb.BetriebsNr
    )
) order by Feld.Feldkennzahl asc;

--13)Geben Sie die Kennzahl und Gattung aller Arbeiterinnen an, die entweder in Bienenst�cken mit mehr als 60 Bienen
--   (inklusive K�nigin!) arbeiten, und/oder die in Bienenst�cken mit 5 oder mehr Brutnestern arbeiten.
--   Es gen�gt wenn jeweils eine Bedingung erf�llt ist.

select distinct Arbeiterin.Kennzahl, Arbeiterin.Gattung
from Arbeiterin
join Bienenstock on Arbeiterin.arbeitetInStockNr = Bienenstock.StockNr
left join Brutnest on Bienenstock.StockNr = Brutnest.liegtInStockNr
group by Arbeiterin.Kennzahl, Arbeiterin.Gattung
having count(Arbeiterin.Kennzahl) + 1 > 60
    or count(Brutnest.NestNr) >= 5;
         
--14)Geben Sie neben dem Namen aller Imker und Imkerinnen auch den Betrag aus, den sie von Landwirtschaftsbetrieben in Summe
--   ausgezahlt bekommen. Daneben soll auch eine Spalte �Bewertung� ausgegeben werden, die wie folgt definiert ist:
--   Falls in Summe mehr als 2000 verdient wird, soll hier �hoher Ertrag� stehen
--   Falls in Summe zwischen 2000 und 1000 verdient wird, soll �m��iger Ertrag� ausgeben werden.
--   Und f�r Summen unter 1000 soll �geringer Ertrag� ausgeben werden.

select Imker.Name,
         sum(bezahltFuer.Betrag) as Gesamtbetrag,
         case 
              when sum(bezahltFuer.Betrag) > 2000 then 'hoher Ertrag'
              when sum(bezahltFuer.Betrag) between 1000 and 2000 then 'mäßiger Ertrag'
              else 'geringer Ertrag'
         end as Bewertung
from Imker
join bezahltFuer on Imker.ImkerNr = bezahltFuer.ImkerNr
group by Imker.Name, Imker.ImkerNr
         
--15)Geben Sie die Imkernummer und den Namen des �ltesten Imkers bzw. der �ltesten Imkerin aus, der oder die f�r zumindest
--   einen Bienenstock mit einem Brustnest mit einer Gr��e �ber 100 cm3 zust�ndig ist
--   und allen Hilfsarbeitenden einen ungeraden Lohn auszahlt.

select top 1 Imker.ImkerNr, Imker.Name
from Imker
join Bienenstock on Imker.ImkerNr = Bienenstock.zustaendigFuer
join Brutnest on Bienenstock.StockNr = Brutnest.liegtInStockNr
where Brutnest.Groesse > 100
and Imker.ImkerNr not in (
    select distinct stelltAn
    from Hilfsarbeiter
    where Lohn % 2 = 0
) order by Imker.GeborenAm asc