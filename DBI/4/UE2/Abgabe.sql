USE imkerei;
GO
--1) Geben Sie die Betriebsnummer und den Namen aller Landwirtschaftsbetriebe an, die als Haupterzeugnis 'Mais' haben
--   und von der Betriebsform 'GmbH' sind.
--   Sortieren Sie die Ausgabe aufsteigend nach dem Namen des Landwirtschaftsbetriebes
SELECT betriebsnr,
       name
FROM landwirtschaftsbetrieb
WHERE haupterzeugnis = 'Mais'
      AND betriebsform = 'GmbH'
ORDER BY name ASC;
--2) Geben Sie f�r alle Typen von Bienenst�cken den durchschnittlichen Honigertrag aus,
--   wobei nur jene Bienenst�cke ber�cksichtigt werden sollen, die mehr als 30 Arbeiterinnen vorweisen.
--   Sortieren Sie das Ergebnis absteigend nach dem durchschnittlichen Ertrag
SELECT bienenstock.typ,
       AVG(bienenstock.honigertrag) AS durchschnittlicher_ertrag
FROM bienenstock
     JOIN arbeiterin ON bienenstock.stocknr = arbeiterin.arbeitetinstocknr
                        AND bienenstock.typ = arbeiterin.arbeitetintyp
GROUP BY bienenstock.typ,
         bienenstock.stocknr
HAVING COUNT(arbeiterin.kennzahl) > 30
ORDER BY durchschnittlicher_ertrag
--3) Geben Sie den Namen und das Geburtsdatum aller Imker und Imkerinnen aus,
--   die keine Hilfsarbeitende haben und für keine Bienenstöcke vom Typ "Magazin" zuständig sind.
--   Ordnen Sie die Ergebnisse absteigend nach dem Geburtsdatum.
SELECT imker.name,
       imker.geborenam
FROM imker
     LEFT JOIN hilfsarbeiter ON hilfsarbeiter.stelltan = imker.imkernr
     LEFT JOIN bienenstock ON bienenstock.zustaendigfuer = imker.imkernr
                              AND bienenstock.typ = 'Magazin'
WHERE hilfsarbeiter.arbeiternr IS NULL
      AND bienenstock.stocknr IS NULL
ORDER BY imker.geborenam DESC;
--4) Geben Sie die Imkernummer und Namen des Imkers bzw. der Imkerin an,
--   der bzw. die durchschnittlich am meisten f�r angestellte Hilfsarbeitende zahlt.
SELECT imker.imkernr,
       imker.name
FROM imker
     JOIN hilfsarbeiter ON hilfsarbeiter.stelltan = imker.imkernr
GROUP BY imker.imkernr,
         imker.name
ORDER BY AVG(hilfsarbeiter.lohn) DESC
--5) Geben Sie die Namen und das Geburtsdatum der Imker und Imkerinnen aus, deren Bienen alle Felder best�uben.
SELECT imker.name,
       imker.geborenam
FROM imker
WHERE NOT EXISTS ( SELECT feld.feldkennzahl
                   FROM feld
                   WHERE NOT EXISTS ( SELECT bestaeubt.feldkennzahl
                                      FROM bestaeubt
                                           JOIN arbeiterin ON arbeiterin.kennzahl = bestaeubt.kennzahl
                                           JOIN bienenstock ON bienenstock.stocknr = arbeiterin.arbeitetinstocknr
                                                               AND bienenstock.typ = arbeiterin.arbeitetintyp
                                      WHERE bienenstock.zustaendigfuer = imker.imkernr
                                            AND bestaeubt.feldkennzahl = feld.feldkennzahl
                                            AND bestaeubt.ort = feld.ort ) )
--6) Geben Sie die Namen alle Imkerei-Meister und Meisterinnen und die Namen all derer Lehrlinge aus,
--   die selber auch einen Lehrling gelehrt haben. Falls keine passenden Lehrlinge existieren,
--   soll stattdessen in der Spalte f�r Lehrling "kein Lehrling vorhanden" ausgegeben werden.
--   Dies gilt nat�rlich auch f�r Imker und Imkerinnen, die �berhaupt keine Lehrlinge gehabt haben.
SELECT m.name AS meister_name,
       COALESCE(l.name, 'kein Lehrling vorhanden') AS lehrling_name
FROM imker m
     LEFT JOIN gelerntvon g ON g.meister = m.imkernr
     LEFT JOIN imker l ON l.imkernr = g.lehrling
     LEFT JOIN gelerntvon g2 ON g2.meister = g.lehrling
WHERE g.lehrling IS NULL
      OR g2.lehrling IS NOT NULL
ORDER BY m.name ASC;
--7) Geben Sie die Namen aller Imker und ihrer Bienenstock Typen und Stocknummern aus, falls diese Bienenst�cke einen
--   Honigertrag von 300kg oder mehr haben und mindestens 3 Brutnester besitzen.
SELECT imker.name,
       bienenstock.typ,
       bienenstock.stocknr
FROM imker
     JOIN bienenstock ON bienenstock.zustaendigfuer = imker.imkernr
     JOIN brutnest ON brutnest.liegtinstocknr = bienenstock.stocknr
                      AND brutnest.liegtintyp = bienenstock.typ
GROUP BY imker.name,
         bienenstock.typ,
         bienenstock.stocknr,
         bienenstock.honigertrag
HAVING bienenstock.honigertrag >= 300
       AND COUNT(n.nestnr) >= 3
--8) Geben Sie f�r jedes Feld, gekennzeichnet durch die Feldkennzahl und den Ort, aus von wie vielen Landwirtschaftsbetrieb
--   es verwendet wird, welchen Fl�chenanteil ein Betrieb im Durchschnitt verwendet und was durchschnittlich f�r die Best�ubung
--   dieses Feldes an Imker gezahlt wird. Auch soll f�r jedes Feld angegeben werden wie viele Arbeiterinnen es best�uben.
SELECT feld.feldkennzahl,
       feld.ort,
       COUNT(DISTINCT verwendetvon.betriebsnr) AS anzahl_betriebe,
       AVG(verwendetvon.flaechenanteil) AS durchschn_flaechenanteil,
       AVG(bestelltfuer.betrag) AS durchschn_betrag,
       COUNT(DISTINCT bestaeubt.kennzahl) AS anzahl_arbeiterinnen
FROM feld
     JOIN verwendetvon ON feld.feldkennzahl = verwendetvon.feldkennzahl
                          AND feld.ort = verwendetvon.ort
     JOIN bezahltfuer ON feld.feldkennzahl = bezahltfuer.feldkennzahl
                         AND feld.ort = bezahltfuer.ort
     JOIN bestaeubt ON feld.feldkennzahl = bestaeubt.feldkennzahl
                       AND feld.ort = bestaeubt.ort
GROUP BY feld.feldkennzahl,
         feld.ort;
--9) Geben Sie den Namen eines Imkers bzw. einer Imkerin, einen Ortsnamen und einen Betrag aus, die folgende Bedingung erf�llen:
--   Der Imker oder die Imkerin muss f�r alle Felder in diesem Ort als Summe genannten Betrag bezahlt bekommen haben
--   und dieser muss h�her sein als den Betrag den jeder andere Imker oder jede andere Imkerin in diesem Ort
--   bekommen hat ("Betrag" ist hier verstanden als die Summe, die von allen Landwirtschaftsbetriebe die Felder
--   dieses Ortes verwenden, bezahlt wird). Ordnen Sie die Ergebnisse absteigend nach dem Betrag.
SELECT i.name,
       f.ort,
       SUM(b.betrag) AS betrag
FROM imker i
     JOIN bezahltfuer b ON b.imkernr = i.imkernr
     JOIN feld f ON f.feldkennzahl = b.feldkennzahl
                    AND f.ort = b.ort
GROUP BY i.imkernr,
         i.name,
         f.ort
HAVING SUM(b.betrag) = ( SELECT MAX(betrag_sum)
                         FROM ( SELECT SUM(b2.betrag) AS betrag_sum
                                FROM bezahltfuer b2
                                     JOIN feld f2 ON f2.feldkennzahl = b2.feldkennzahl
                                                     AND f2.ort = f.ort
                                WHERE b2.imkernr IS NOT NULL
                                GROUP BY b2.imkernr,
                                         f2.ort ) AS betrags )
ORDER BY betrag DESC;
--10)Geben Sie alle Bienenst�cke (StockNr und Typ) aus, deren Arbeiterinnen weniger als 10 Felder best�uben und
--   die maximal 2 Brutnester besit�zen, deren Honigertrag aber durchschnittlich oder besser
--   (im Vergleich zu allen Bienenst�cken) ist. Ordnen Sie die aufsteigend Ergebnisse nach der Stocknummer.
         
--11)Geben Sie f�r jede Gattung Biene an, wie viele Arbeiterinnen und K�niginnen vorhanden sind,
--   und wie viele Felder von der Arbeiterinnen der jeweiligen Gattung durchschnittlich best�ubt werden,
--   zus�tzlich dazu auch die minimale und maximal Anzahl Felder die pro Biene der jeweiligen Gattung best�ubt werden.
         
--12)Geben Sie die Kennzahl und den Ort aller Felder aus, die von allen Landwirtschaftsbetrieben verwendet werden.
--   (In anderen Worten, es soll keinen Landwirtschaftsbetrieb geben, der von diesen Feldern nicht einen gewissen Anteil
--   verwendet) Ordnen Sie das Ergebnis aufsteigend nach der Kennzahl.
SELECT feld.feldkennzahl,
       feld.ort
FROM feld
WHERE NOT EXISTS ( SELECT landwirtschaftsbetrieb.betriebsnr
                   FROM landwirtschaftsbetrieb
                   WHERE NOT EXISTS ( SELECT verwendetvon.betriebsnr
                                      FROM verwendetvon v,
                                           landwirtschaftsbetrieb l
                                      WHERE v.feldkennzahl = feld.feldkennzahl
                                            AND v.ort = feld.ort
                                            AND v.betriebsnr = l.betriebsnr ) )
ORDER BY feld.feldkennzahl;
--13)Geben Sie die Kennzahl und Gattung aller Arbeiterinnen an, die entweder in Bienenst�cken mit mehr als 60 Bienen
--   (inklusive K�nigin!) arbeiten, und/oder die in Bienenst�cken mit 5 oder mehr Brutnestern arbeiten.
--   Es gen�gt wenn jeweils eine Bedingung erf�llt ist.
         
--14)Geben Sie neben dem Namen aller Imker und Imkerinnen auch den Betrag aus, den sie von Landwirtschaftsbetrieben in Summe
--   ausgezahlt bekommen. Daneben soll auch eine Spalte �Bewertung� ausgegeben werden, die wie folgt definiert ist:
--   Falls in Summe mehr als 2000 verdient wird, soll hier �hoher Ertrag� stehen
--   Falls in Summe zwischen 2000 und 1000 verdient wird, soll �m��iger Ertrag� ausgeben werden.
--   Und f�r Summen unter 1000 soll �geringer Ertrag� ausgeben werden.
         
--15)Geben Sie die Imkernummer und den Namen des �ltesten Imkers bzw. der �ltesten Imkerin aus, der oder die f�r zumindest
--   einen Bienenstock mit einem Brustnest mit einer Gr��e �ber 100 cm3 zust�ndig ist
--   und allen Hilfsarbeitenden einen ungeraden Lohn auszahlt.