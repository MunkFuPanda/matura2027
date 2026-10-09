USE mondial;

--1
SELECT   o.Name AS Organisation,
         isnull(sum(c.Population), 0) AS Einwohner
FROM     Organization AS o
         LEFT OUTER JOIN
         isMember AS m
         ON m.Organization = o.Abbreviation
         LEFT OUTER JOIN
         Country AS c
         ON c.Code = m.Country
GROUP BY o.Name
ORDER BY Einwohner DESC;

--2
SELECT c.Name AS Land,
       c.Capital AS Hauptstadt
FROM   Country AS c
       INNER JOIN
       City AS cap
       ON cap.Country = c.Code
          AND cap.Name = c.Capital
          AND cap.Province = c.Province
WHERE  EXISTS (SELECT *
               FROM   City AS ct
               WHERE  ct.Country = c.Code
                      AND ct.Population > cap.Population);

--3
SELECT   c.Name AS Land,
         CAST (100.0 * count(m.City) / count(*) AS DECIMAL (5, 2)) AS ProzentAmMeer
FROM     Country AS c
         INNER JOIN
         City AS ct
         ON ct.Country = c.Code
         LEFT OUTER JOIN
         (SELECT DISTINCT l.City,
                          l.Country,
                          l.Province
          FROM   located AS l
          WHERE  l.Sea IS NOT NULL) AS m
         ON m.City = ct.Name
            AND m.Country = ct.Country
            AND m.Province = ct.Province
GROUP BY c.Name
HAVING   count(*) >= 10
ORDER BY ProzentAmMeer DESC;

--4
SELECT   c1.Name AS Land1,
         c2.Name AS Land2,
         count(*) AS GemeinsameFluesse
FROM     borders AS b
         INNER JOIN
         Country AS c1
         ON c1.Code = b.Country1
         INNER JOIN
         Country AS c2
         ON c2.Code = b.Country2
         INNER JOIN
         (SELECT DISTINCT r1.Country AS Country1,
                          r2.Country AS Country2,
                          r1.River
          FROM   geo_River AS r1
                 INNER JOIN
                 geo_River AS r2
                 ON r2.River = r1.River) AS s
         ON s.Country1 = b.Country1
            AND s.Country2 = b.Country2
GROUP BY c1.Name, c2.Name;

--5
SELECT   c1.Name AS Land1,
         c2.Name AS Land2,
         count(s.River) AS GemeinsameFluesse
FROM     borders AS b
         INNER JOIN
         Country AS c1
         ON c1.Code = b.Country1
         INNER JOIN
         Country AS c2
         ON c2.Code = b.Country2
         LEFT OUTER JOIN
         (SELECT DISTINCT r1.Country AS Country1,
                          r2.Country AS Country2,
                          r1.River
          FROM   geo_River AS r1
                 INNER JOIN
                 geo_River AS r2
                 ON r2.River = r1.River) AS s
         ON s.Country1 = b.Country1
            AND s.Country2 = b.Country2
GROUP BY c1.Name, c2.Name;

--6
SELECT   c.Name AS Land,
         count(n.Nachbar) AS AnzahlNachbarlaender
FROM     Country AS c
         LEFT OUTER JOIN
         (SELECT b.Country1 AS Land,
                 b.Country2 AS Nachbar
          FROM   borders AS b
          UNION
          SELECT b.Country2,
                 b.Country1
          FROM   borders AS b) AS n
         ON n.Land = c.Code
GROUP BY c.Name;

--7
SELECT   TOP 1 WITH TIES c.Name AS Land,
                         sum(b.Length) / c.Area AS GrenzlaengeProFlaeche
FROM     Country AS c
         INNER JOIN
         (SELECT b.Country1 AS Land,
                 b.Length
          FROM   borders AS b
          UNION ALL
          SELECT b.Country2,
                 b.Length
          FROM   borders AS b) AS b
         ON b.Land = c.Code
WHERE    c.Area > 0
GROUP BY c.Name, c.Area
ORDER BY GrenzlaengeProFlaeche DESC;

--8
SELECT c.Name AS Land,
       o.Name AS Organisation
FROM   Country AS c
       INNER JOIN
       isMember AS m
       ON m.Country = c.Code
       INNER JOIN
       Organization AS o
       ON o.Abbreviation = m.Organization
WHERE  NOT EXISTS (SELECT *
                   FROM   (SELECT b.Country1 AS Land,
                                  b.Country2 AS Nachbar
                           FROM   borders AS b
                           UNION
                           SELECT b.Country2,
                                  b.Country1
                           FROM   borders AS b) AS n
                          INNER JOIN
                          isMember AS m2
                          ON m2.Country = n.Nachbar
                             AND m2.Organization = m.Organization
                   WHERE  n.Land = c.Code);

--9
SELECT c.Name AS Land
FROM   Country AS c
WHERE  NOT EXISTS (SELECT *
                   FROM   (SELECT gs.Country AS Land
                           FROM   geo_Sea AS gs
                           UNION
                           SELECT gi.Country AS Land
                           FROM   geo_Island AS gi
                                  INNER JOIN
                                  islandIn AS ii
                                  ON ii.Island = gi.Island
                           WHERE  ii.Sea IS NOT NULL) AS kl
                   WHERE  kl.Land = c.Code)
       AND NOT EXISTS (SELECT *
                       FROM   (SELECT b.Country1 AS Land,
                                      b.Country2 AS Nachbar
                               FROM   borders AS b
                               UNION
                               SELECT b.Country2,
                                      b.Country1
                               FROM   borders AS b) AS n
                              INNER JOIN
                              (SELECT gs.Country AS Land
                               FROM   geo_Sea AS gs
                               UNION
                               SELECT gi.Country AS Land
                               FROM   geo_Island AS gi
                                      INNER JOIN
                                      islandIn AS ii
                                      ON ii.Island = gi.Island
                               WHERE  ii.Sea IS NOT NULL) AS kl
                              ON kl.Land = n.Nachbar
                       WHERE  n.Land = c.Code);