USE mondial;
-- 1a
SELECT organization.name AS organization,
       SUM(country.population) AS population
FROM organization
     LEFT JOIN ismember ON ismember.organization = organization.abbreviation
     LEFT JOIN country ON ismember.country = country.code
GROUP BY organization.name
ORDER BY population DESC
-- 1b
SELECT country.name AS country,
       COUNT(DISTINCT ismember.organization) AS numberoforganizations
FROM country
     LEFT JOIN ismember ON ismember.country = country.code
GROUP BY country.name
HAVING COUNT(DISTINCT ismember.organization) > 60
ORDER BY numberoforganizations DESC
-- 1c
SELECT country.name AS country,
       COUNT(DISTINCT ismember.organization) AS numberoforganizations
FROM country
     LEFT JOIN ismember ON ismember.country = country.code
WHERE country.area > 500000
GROUP BY country.name
HAVING COUNT(DISTINCT ismember.organization) > 60
ORDER BY numberoforganizations DESC
-- 1d
SELECT DISTINCT country.name AS country
FROM country
     LEFT JOIN ismember ON ismember.country = country.code
WHERE ismember.organization IN ( SELECT abbreviation
                                 FROM organization AS o2
                                      INNER JOIN ismember AS im2 ON im2.organization = o2.abbreviation
                                 WHERE im2.country = 'D' )
-- 1e
SELECT name
FROM country
WHERE NOT EXISTS ( SELECT organization
                   FROM ismember m_and
                   WHERE m_and.country = 'AND'
                         AND NOT EXISTS ( SELECT organization
                                          FROM ismember m_other
                                          WHERE m_other.country = country.code
                                                AND m_other.organization = m_and.organization ) );
-- 1f
SELECT organization
FROM ismember
GROUP BY organization
HAVING COUNT(DISTINCT country) = ( SELECT COUNT(*) FROM country );
-- 2a
       
-- abfrage falsch, weil bei einer nicht gruppierten spalte kein aggregat steht
SELECT TOP 1 MAX(area),
       code
FROM country
GROUP BY code
ORDER BY MAX(area) DESC;
-- 2b
SELECT population,
       capital
FROM country;
-- macht kein sinn, da nur die Bevölkerung des Landes und die Hauptstadt gegeben sind
     
-- 3
-- für abfrage mit having siehe 1b
SELECT country,
       numberoforganizations
FROM ( SELECT country.name AS country,
              COUNT(DISTINCT ismember.organization) AS numberoforganizations
       FROM country
            LEFT JOIN ismember ON ismember.country = country.code
       GROUP BY country.name ) AS orgcounts
WHERE numberoforganizations > 60
ORDER BY numberoforganizations DESC;