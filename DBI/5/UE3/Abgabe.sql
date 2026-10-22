USE mondial;

--1
SELECT   i.Name AS Insel,
         i.Area AS Flaeche,
         count(DISTINCT g.Country) AS AnzahlLaender
FROM     Island AS i
         INNER JOIN
         geo_Island AS g
         ON g.Island = i.Name
GROUP BY i.Name, i.Area
HAVING   count(DISTINCT g.Country) >= 2;

--2
SELECT c.Name AS Stadt,
       c.Province AS Departement
FROM   City AS c
       INNER JOIN
       Country AS l
       ON l.Code = c.Country
WHERE  l.Name = 'France'
       AND NOT EXISTS (SELECT *
                       FROM   located AS lo
                       WHERE  lo.City = c.Name
                              AND lo.Country = c.Country
                              AND lo.Province = c.Province
                              AND lo.River IS NOT NULL);

--3
WITH nachbar
AS   (SELECT b.Country1 AS Land,
             b.Country2 AS Nachbar,
             b.Length
      FROM   borders AS b
      UNION ALL
      SELECT b.Country2,
             b.Country1,
             b.Length
      FROM   borders AS b)
SELECT   o.Name AS Organisation,
         isnull(sum(a.Length), 0) AS Aussengrenzen
FROM     Organization AS o
         LEFT OUTER JOIN
         (SELECT m.Organization,
                 n.Length
          FROM   isMember AS m
                 INNER JOIN
                 nachbar AS n
                 ON n.Land = m.Country
          WHERE  NOT EXISTS (SELECT *
                             FROM   isMember AS m2
                             WHERE  m2.Organization = m.Organization
                                    AND m2.Country = n.Nachbar)) AS a
         ON a.Organization = o.Abbreviation
GROUP BY o.Abbreviation, o.Name
ORDER BY Aussengrenzen DESC;

--4
SELECT   k.Name AS Kontinent,
         count(l.Code) AS LaenderOhneKueste
FROM     Continent AS k
         LEFT OUTER JOIN
         (SELECT e.Continent,
                 c.Code
          FROM   Country AS c
                 INNER JOIN
                 encompasses AS e
                 ON e.Country = c.Code
          WHERE  NOT EXISTS (SELECT *
                             FROM   geo_Sea AS gs
                             WHERE  gs.Country = c.Code)
                 AND NOT EXISTS (SELECT *
                                 FROM   geo_Island AS gi
                                        INNER JOIN
                                        islandIn AS ii
                                        ON ii.Island = gi.Island
                                 WHERE  gi.Country = c.Code
                                        AND ii.Sea IS NOT NULL)) AS l
         ON l.Continent = k.Name
GROUP BY k.Name
ORDER BY LaenderOhneKueste DESC;

--5
SELECT   c.Name AS Land,
         count(ct.Name) AS AnzahlGroessereStaedte
FROM     Country AS c
         LEFT OUTER JOIN
         City AS cap
         ON cap.Country = c.Code
            AND cap.Name = c.Capital
            AND cap.Province = c.Province
         LEFT OUTER JOIN
         City AS ct
         ON ct.Country = c.Code
            AND ct.Population > cap.Population
GROUP BY c.Name
ORDER BY AnzahlGroessereStaedte DESC;

--6
WITH nachbar
AS   (SELECT b.Country1 AS Land,
             b.Country2 AS Nachbar
      FROM   borders AS b
      UNION
      SELECT b.Country2,
             b.Country1
      FROM   borders AS b)
SELECT   TOP 1 WITH TIES c.Name AS Land,
                         count(n.Nachbar) AS AnzahlNachbarn,
                         c.Area AS Flaeche,
                         count(n.Nachbar) / cast (c.Area AS FLOAT) AS NachbarnProFlaeche
FROM     Country AS c
         INNER JOIN
         nachbar AS n
         ON n.Land = c.Code
WHERE    c.Area > 0
GROUP BY c.Name, c.Area
ORDER BY NachbarnProFlaeche DESC;

--7
SELECT c.Name AS Land,
       c.Area AS Flaeche
FROM   Country AS c
WHERE  (EXISTS (SELECT *
                FROM   geo_Sea AS gs
                WHERE  gs.Country = c.Code)
        OR EXISTS (SELECT *
                   FROM   geo_Island AS gi
                          INNER JOIN
                          islandIn AS ii
                          ON ii.Island = gi.Island
                   WHERE  gi.Country = c.Code
                          AND ii.Sea IS NOT NULL))
       AND NOT EXISTS (SELECT *
                       FROM   located AS lo
                       WHERE  lo.Country = c.Code
                              AND lo.Sea IS NOT NULL)
ORDER BY c.Area DESC;

--8
SELECT   l.Name AS Sprache,
         count(*) AS AnzahlLaender,
         sum(CASE WHEN l.Percentage > 50 THEN 1 ELSE 0 END) AS AnzahlMehrheitssprache,
         cast (sum(c.Population * l.Percentage / 100) AS BIGINT) AS GeschaetzteSprecher
FROM     Language AS l
         INNER JOIN
         Country AS c
         ON c.Code = l.Country
GROUP BY l.Name
ORDER BY GeschaetzteSprecher DESC;

--9
WITH nachbar
AS   (SELECT b.Country1 AS Land,
             b.Country2 AS Nachbar
      FROM   borders AS b
      UNION
      SELECT b.Country2,
             b.Country1
      FROM   borders AS b)
SELECT c.Name AS Land,
       o.Name AS Organisation
FROM   Country AS c
       CROSS JOIN
       Organization AS o
WHERE  NOT EXISTS (SELECT *
                   FROM   isMember AS m
                   WHERE  m.Country = c.Code
                          AND m.Organization = o.Abbreviation)
       AND EXISTS (SELECT *
                   FROM   nachbar AS n
                   WHERE  n.Land = c.Code)
       AND NOT EXISTS (SELECT *
                       FROM   nachbar AS n
                       WHERE  n.Land = c.Code
                              AND NOT EXISTS (SELECT *
                                              FROM   isMember AS m
                                              WHERE  m.Country = n.Nachbar
                                                     AND m.Organization = o.Abbreviation));
