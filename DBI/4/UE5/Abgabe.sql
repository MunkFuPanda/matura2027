--a)  Welche Spalten enthält die Tabelle L?
SELECT column_name
FROM information_schema.columns
WHERE table_name = 'L';
--b)  Welche Tabellen / Views / Tabellen oder Views enthalten eine Spalte STADT?
SELECT table_name
FROM information_schema.columns
WHERE column_name = 'STADT';
--c)  Wieviele Spalten enthält die Tabelle LT?
SELECT COUNT(column_name)
FROM information_schema.columns
WHERE table_name = 'LT';
--d)  Wieviele Spalten haben die einzelnen Tabellen / Views?
SELECT table_name,
       COUNT(column_name) AS spaltenanzahl
FROM information_schema.columns
GROUP BY table_name;
--e)  Wieviele Fremdschlüssel sind in der Tabelle LT enthalten?
SELECT COUNT(*)
FROM information_schema.key_column_usage
WHERE table_name = 'LT'
      AND constraint_name LIKE 'FK_%';
--f) Welche Tabellen haben einen zusammengesetzten Primärschlüssel?
SELECT tc.table_name
FROM information_schema.table_constraints tc
     JOIN information_schema.key_column_usage kcu ON tc.constraint_name = kcu.constraint_name
WHERE tc.constraint_type = 'PRIMARY KEY'
GROUP BY tc.table_name
HAVING COUNT(kcu.column_name) > 1;
--g)  Welche Tabellen haben einen zusammengesetzten Fremdschlüssel?
SELECT tc.table_name
FROM information_schema.table_constraints tc
     JOIN information_schema.key_column_usage kcu ON tc.constraint_name = kcu.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY'
GROUP BY tc.table_name
HAVING COUNT(kcu.column_name) > 1;
--h)  Welche Tabellen enthalten keinen Unique-Constraint?
SELECT table_name
FROM information_schema.tables
WHERE table_type = 'BASE TABLE'
      AND table_name NOT IN ( SELECT DISTINCT table_name
                              FROM information_schema.table_constraints
                              WHERE constraint_type = 'UNIQUE' );
--i) Welche Tabellen kommen in der View LTX vor?
SELECT table_name
FROM information_schema.view_table_usage
WHERE view_name = 'LTX';
--j) In welchen Views kommt die Tabelle L vor?
SELECT view_name
FROM information_schema.view_table_usage
WHERE table_name = 'L';
--k) In welchen Views kommen die Tabellen L und T vor?
SELECT view_name
FROM information_schema.view_table_usage
WHERE table_name IN ( 'L', 'T' )
GROUP BY view_name
HAVING COUNT(DISTINCT table_name) = 2;
--l) In welchen / wievielen Tabellen kommt eine Spalte mit dem Datentyp DECIMAL vor?
SELECT table_name,
       COUNT(column_name) AS anzahl_decimal_spalten
FROM information_schema.columns
WHERE data_type = 'decimal'
GROUP BY table_name;
--m) Welche Spalten (Tabellen- und Spaltenname) dürfen nicht NULL sein?
SELECT table_name,
       column_name
FROM information_schema.columns
WHERE is_nullable = 'NO';
--n)  In welchen Check-Constraints kommt die Spalte STADT vor?
SELECT tc.constraint_name,
       tc.table_name
FROM information_schema.table_constraints tc
     JOIN information_schema.check_constraints cc ON tc.constraint_name = cc.constraint_name
     JOIN information_schema.constraint_column_usage ccu ON tc.constraint_name = ccu.constraint_name
WHERE ccu.column_name = 'STADT'
      AND tc.constraint_type = 'CHECK';
--o)  Welche Tabellen werden von einem Fremdschlüssel referenziert?
SELECT DISTINCT ccu.table_name AS referenzierte_tabelle
FROM information_schema.table_constraints tc
     JOIN information_schema.key_column_usage kcu ON tc.constraint_name = kcu.constraint_name
     JOIN information_schema.constraint_column_usage ccu ON tc.constraint_name = ccu.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY';