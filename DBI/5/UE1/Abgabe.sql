USE haustiere;

--1
SELECT Name,
       Jahre
FROM   Haustier
WHERE  Jahre BETWEEN 2 AND 5;

--2
SELECT   h.ID
FROM     Haustier AS h
         INNER JOIN
         Beziehung AS b
         ON b.Von = h.ID
WHERE    b.Art = 'Freund'
GROUP BY h.ID
HAVING   count(*) >= 3;

--3
SELECT h.Name
FROM   Haustier AS h
       INNER JOIN
       Wasserschwein AS w
       ON w.ID = h.ID
WHERE  w.Charisma <= 8
EXCEPT
SELECT h.Name
FROM   Haustier AS h
       INNER JOIN
       Wasserschwein AS w
       ON w.ID = h.ID
       INNER JOIN
       laufen AS l
       ON l.Tier = h.ID;

--4
SELECT   p.Nummer,
         p.Unternehmen,
         count(hd.ID) AS 'Anzahl Hunde',
         count(k.ID) AS 'Anzahl Katze',
         count(w.ID) AS 'Anzahl Capybara'
FROM     Pfleger AS p
         LEFT OUTER JOIN
         laufen AS l
         ON l.Pfleger_nummer = p.Nummer
            AND l.Pfleger_unternehmen = p.Unternehmen
         LEFT OUTER JOIN
         Haustier AS h
         ON h.ID = l.Tier
         LEFT OUTER JOIN
         Wasserschwein AS w
         ON h.ID = w.ID
         LEFT OUTER JOIN
         Katze AS k
         ON k.ID = h.ID
         LEFT OUTER JOIN
         Hund AS hd
         ON hd.ID = h.ID
GROUP BY p.Nummer, p.Unternehmen;

--5
SELECT   p.Nummer,
         p.Unternehmen,
         count(hd.ID) AS 'Anzahl Hunde',
         count(k.ID) AS 'Anzahl Katze',
         count(w.ID) AS 'Anzahl Capybara'
FROM     Pfleger AS p
         INNER JOIN
         laufen AS l
         ON l.Pfleger_nummer = p.Nummer
            AND l.Pfleger_unternehmen = p.Unternehmen
         INNER JOIN
         (SELECT   e.Start,
                   e.Ende
          FROM     Etappe AS e
          GROUP BY e.Start, e.Ende
          HAVING   count(*) >= 3) AS t
         ON t.Start = l.Tour_start
            AND t.Ende = l.Tour_ende
         LEFT OUTER JOIN
         Haustier AS h
         ON h.ID = l.Tier
         LEFT OUTER JOIN
         Wasserschwein AS w
         ON h.ID = w.ID
         LEFT OUTER JOIN
         Katze AS k
         ON k.ID = h.ID
         LEFT OUTER JOIN
         Hund AS hd
         ON hd.ID = h.ID
GROUP BY p.Nummer, p.Unternehmen;

--6
SELECT u.Name,
       p.Durchschnittslohn,
       k.MaximaleGebuehr
FROM   Unternehmen AS u
       INNER JOIN
       (SELECT   p.Unternehmen,
                 avg(p.Lohn) AS Durchschnittslohn
        FROM     Pfleger AS p
        GROUP BY p.Unternehmen
        HAVING   count(*) >= 8) AS p
       ON p.Unternehmen = u.Name
       LEFT OUTER JOIN
       (SELECT   k.Unternehmen,
                 max(k.Gebuehr) AS MaximaleGebuehr
        FROM     kundevon AS k
        GROUP BY k.Unternehmen
        HAVING   count(*) <= 5) AS k
       ON k.Unternehmen = u.Name
WHERE  NOT EXISTS (SELECT   *
                   FROM     kundevon AS k
                   WHERE    k.Unternehmen = u.Name
                   GROUP BY k.Unternehmen
                   HAVING   count(*) > 5);

--7
SELECT   t.Start,
         t.Ende,
         COALESCE (CAST (max(w.ID) AS VARCHAR (10)), 'Keine Läufe für diese Tour') AS 'Maximale Wasserschwein ID'
FROM     Tour AS t
         INNER JOIN
         Etappe AS e
         ON e.Start = t.Start
            AND e.Ende = t.Ende
         LEFT OUTER JOIN
         laufen AS l
         ON l.Tour_start = t.Start
            AND l.Tour_ende = t.Ende
         LEFT OUTER JOIN
         Wasserschwein AS w
         ON w.ID = l.Tier
GROUP BY t.Start, t.Ende
HAVING   count(DISTINCT e.Nummer) > 3;

--8
SELECT h.ID,
       h.Name
FROM   Haustier AS h
WHERE  NOT EXISTS (SELECT *
                   FROM   Tour AS t
                   WHERE  NOT EXISTS (SELECT *
                                      FROM   laufen AS l
                                      WHERE  l.Tier = h.ID
                                             AND l.Tour_start = t.Start
                                             AND l.Tour_ende = t.Ende));

--9
SELECT b.SVNR,
       b.Wohnort
FROM   Besitzer AS b
WHERE  NOT EXISTS (SELECT *
                   FROM   Unternehmen AS u
                   WHERE  NOT EXISTS (SELECT *
                                      FROM   kundevon AS k
                                      WHERE  k.Kunde = b.SVNR
                                             AND k.Unternehmen = u.Name));

--10
SELECT 'Hund' AS 'Tierart',
       count(DISTINCT concat(l.Tour_start, '|', l.Tour_ende)) AS 'Tours',
       count(DISTINCT concat(l.Pfleger_unternehmen, '|', l.Pfleger_nummer)) AS 'Pfleger'
FROM   Hund AS h
       LEFT OUTER JOIN
       laufen AS l
       ON l.Tier = h.ID
UNION ALL
SELECT 'Katze',
       count(DISTINCT concat(l.Tour_start, '|', l.Tour_ende)),
       count(DISTINCT concat(l.Pfleger_unternehmen, '|', l.Pfleger_nummer))
FROM   Katze AS k
       LEFT OUTER JOIN
       laufen AS l
       ON l.Tier = k.ID
UNION ALL
SELECT 'Wasserschwein',
       count(DISTINCT concat(l.Tour_start, '|', l.Tour_ende)),
       count(DISTINCT concat(l.Pfleger_unternehmen, '|', l.Pfleger_nummer))
FROM   Wasserschwein AS w
       LEFT OUTER JOIN
       laufen AS l
       ON l.Tier = w.ID;

--11
SELECT   b.SVNR,
         isnull(sum(u.Umsatz), 0) AS Umsatz
FROM     Besitzer AS b
         LEFT OUTER JOIN
         kundevon AS k
         ON k.Kunde = b.SVNR
         LEFT OUTER JOIN
         Unternehmen AS u
         ON u.Name = k.Unternehmen
GROUP BY b.SVNR;