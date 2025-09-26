use Imkerei;

--1) Geben Sie die Betriebsnummer und den Namen aller Landwirtschaftsbetriebe an, die als Haupterzeugnis 'Mais' haben 
--   und von der Betriebsform 'GmbH' sind. 
--   Sortieren Sie die Ausgabe aufsteigend nach dem Namen des Landwirtschaftsbetriebes

select betriebsnr, name
from Landwirtschaftsbetrieb
where Haupterzeugnis = 'Mais'
and Betriebsform = 'GmbH'
order by name asc;


--2) Geben Sie für alle Typen von Bienenstöcken den durchschnittlichen Honigertrag aus, 
--   wobei nur jene Bienenstöcke berücksichtigt werden sollen, die mehr als 30 Arbeiterinnen vorweisen. 
--   Sortieren Sie das Ergebnis absteigend nach dem durchschnittlichen Ertrag

select b.Typ, avg(b.Honigertrag) as Durchschnittlicher_Ertrag
from Bienenstock b
join Arbeiterin a on b.StockNr = a.arbeitetInStockNr and b.Typ = a.arbeitetInTyp
group by b.Typ, b.StockNr
having count(a.Kennzahl) > 30
order by Durchschnittlicher_Ertrag


--3) Geben Sie den Namen und das Geburtsdatum aller Imker und Imkerinnen aus, 
--   die keine Hilfsarbeitende haben und für keine Bienenstöcke vom Typ "Magazin" zuständig sind. 
--   Ordnen Sie die Ergebnisse absteigend nach dem Geburtsdatum.

select i.Name, i.GeborenAm
from Imker i
left join Hilfsarbeiter h on h.stelltAn = i.ImkerNr
left join Bienenstock b on b.zustaendigFuer = i.ImkerNr and b.Typ = 'Magazin'
where h.ArbeiterNr is null
and b.StockNr is null
order by i.GeborenAm desc;



--4) Geben Sie die Imkernummer und Namen des Imkers bzw. der Imkerin an, 
--   der bzw. die durchschnittlich am meisten für angestellte Hilfsarbeitende zahlt.

select i.ImkerNr, i.Name
from Imker i
join Hilfsarbeiter h on h.stelltAn = i.ImkerNr
group by i.ImkerNr, i.name
order by avg(h.Lohn) desc

--5) Geben Sie die Namen und das Geburtsdatum der Imker und Imkerinnen aus, deren Bienen alle Felder bestäuben.

select i.Name, i.GeborenAm
from Imker i
where not exists (
    select f.Feldkennzahl
    from Feld f
    where not exists (
        select b.Feldkennzahl
        from bestaeubt b
        join Arbeiterin a on a.Kennzahl = b.Kennzahl
        join bienenstock s on s.StockNr = a.arbeitetInStockNr and s.Typ = a.arbeitetInTyp
        where s.zustaendigfuer = i.ImkerNr and b.Feldkennzahl = f.Feldkennzahl and b.Ort = f.Ort
    )
)

--6) Geben Sie die Namen alle Imkerei-Meister und Meisterinnen und die Namen all derer Lehrlinge aus, 
--   die selber auch einen Lehrling gelehrt haben. Falls keine passenden Lehrlinge existieren, 
--   soll stattdessen in der Spalte für Lehrling "kein Lehrling vorhanden" ausgegeben werden. 
--   Dies gilt natürlich auch für Imker und Imkerinnen, die überhaupt keine Lehrlinge gehabt haben. 
--   Ordnen Sie die Ergebnisse aufsteigend nach den Namen der Meister und Meisterinnen.




--7) Geben Sie die Namen aller Imker und ihrer Bienenstock Typen und Stocknummern aus, falls diese Bienenstöcke einen 
--   Honigertrag von 300kg oder mehr haben und mindestens 3 Brutnester besitzen.

select i.Name, b.Typ, b.StockNr
from Imker i
join Bienenstock b on b.zustaendigFuer = i.ImkerNr
join Brutnest n on n.liegtInStockNr = b.StockNr and n.liegtInTyp = b.typ
group by i.Name, b.Typ, b.StockNr, b.Honigertrag
having b.Honigertrag >= 300 and count(n.NestNr) >= 3


--8) Geben Sie für jedes Feld, gekennzeichnet durch die Feldkennzahl und den Ort, aus von wie vielen Landwirtschaftsbetrieb 
--   es verwendet wird, welchen Flächenanteil ein Betrieb im Durchschnitt verwendet und was durchschnittlich für die Bestäubung
--   dieses Feldes an Imker gezahlt wird. Auch soll für jedes Feld angegeben werden wie viele Arbeiterinnen es bestäuben.

select f.Feldkennzahl, f.Ort,
       count(distinct v.BetriebsNr) as anzahl_betriebe,
       avg(v.Flaechenanteil) as durchschn_flaechenanteil,
       avg(bf.Betrag) as durchschn_betrag,
       count(distinct a.Kennzahl) as anzahl_arbeiterinnen
from Feld f
join verwendetVon v on f.Feldkennzahl = v.Feldkennzahl and f.Ort = v.Ort
join bezahltFuer bf on f.Feldkennzahl = bf.Feldkennzahl and f.Ort = bf.Ort
join bestaeubt a on f.Feldkennzahl = a.Feldkennzahl and f.Ort = a.Ort
group by f.Feldkennzahl, f.Ort;


--9) Geben Sie den Namen eines Imkers bzw. einer Imkerin, einen Ortsnamen und einen Betrag aus, die folgende Bedingung erfüllen:
--   Der Imker oder die Imkerin muss für alle Felder in diesem Ort als Summe genannten Betrag bezahlt bekommen haben 
--   und dieser muss höher sein als den Betrag den jeder andere Imker oder jede andere Imkerin in diesem Ort 
--   bekommen hat ("Betrag" ist hier verstanden als die Summe, die von allen Landwirtschaftsbetriebe die Felder 
--   dieses Ortes verwenden, bezahlt wird). Ordnen Sie die Ergebnisse absteigend nach dem Betrag.


--10)Geben Sie alle Bienenstöcke (StockNr und Typ) aus, deren Arbeiterinnen weniger als 10 Felder bestäuben und 
--   die maximal 2 Brutnester besit¬zen, deren Honigertrag aber durchschnittlich oder besser 
--   (im Vergleich zu allen Bienenstöcken) ist. Ordnen Sie die aufsteigend Ergebnisse nach der Stocknummer.



--11)Geben Sie für jede Gattung Biene an, wie viele Arbeiterinnen und Königinnen vorhanden sind, 
--   und wie viele Felder von der Arbeiterinnen der jeweiligen Gattung durchschnittlich bestäubt werden, 
--   zusätzlich dazu auch die minimale und maximal Anzahl Felder die pro Biene der jeweiligen Gattung bestäubt werden.



--12)Geben Sie die Kennzahl und den Ort aller Felder aus, die von allen Landwirtschaftsbetrieben verwendet werden. 
--   (In anderen Worten, es soll keinen Landwirtschaftsbetrieb geben, der von diesen Feldern nicht einen gewissen Anteil 
--   verwendet) Ordnen Sie das Ergebnis aufsteigend nach der Kennzahl.

select f.Feldkennzahl, f.Ort
from Feld f
where not exists (
  select l.BetriebsNr
  from Landwirtschaftsbetrieb l
  where not exists (
    select v.BetriebsNr
    from verwendetVon v
    where v.Feldkennzahl = f.Feldkennzahl
      and v.Ort = f.Ort
      and v.BetriebsNr = l.BetriebsNr
  )
)
order by f.Feldkennzahl;



--13)Geben Sie die Kennzahl und Gattung aller Arbeiterinnen an, die entweder in Bienenstöcken mit mehr als 60 Bienen 
--   (inklusive Königin!) arbeiten, und/oder die in Bienenstöcken mit 5 oder mehr Brutnestern arbeiten. 
--   Es genügt wenn jeweils eine Bedingung erfüllt ist.


--14)Geben Sie neben dem Namen aller Imker und Imkerinnen auch den Betrag aus, den sie von Landwirtschaftsbetrieben in Summe 
--   ausgezahlt bekommen. Daneben soll auch eine Spalte “Bewertung” ausgegeben werden, die wie folgt definiert ist: 
--   Falls in Summe mehr als 2000 verdient wird, soll hier “hoher Ertrag” stehen 
--   Falls in Summe zwischen 2000 und 1000 verdient wird, soll “mäßiger Ertrag” ausgeben werden. 
--   Und für Summen unter 1000 soll “geringer Ertrag” ausgeben werden.



--15)Geben Sie die Imkernummer und den Namen des ältesten Imkers bzw. der ältesten Imkerin aus, der oder die für zumindest 
--   einen Bienenstock mit einem Brustnest mit einer Größe über 100 cm3 zuständig ist 
--   und allen Hilfsarbeitenden einen ungeraden Lohn auszahlt.