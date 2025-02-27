/* S c h u l u n g s f i r m a - i n s e r t . s q l MSSQLServer */

DELETE FROM besucht;
DELETE FROM kveranst;
DELETE FROM geeignet;
DELETE FROM setztvor;
DELETE FROM kurs;
DELETE FROM referent;
DELETE FROM person;
  
SET DATEFORMAT dmy;

INSERT INTO person VALUES (101,'Bach','Johann Sebastian',  'Leipzig', 'D');
INSERT INTO person VALUES (102,'Haendel','Georg Friedrich','London',  'GB');
INSERT INTO person VALUES (103,'Haydn','Joseph',           'Wien',    'A');
INSERT INTO person VALUES (104,'Mozart','Wolfgang Amadeus','Salzburg','A');
INSERT INTO person VALUES (105,'Beethoven','Ludwig van',   'Wien',    'A');
INSERT INTO person VALUES (106,'Schubert','Franz',         'Wien',    'A');
INSERT INTO person VALUES (107,'Berlioz','Hector',         'Paris',   'F');
INSERT INTO person VALUES (108,'Liszt','Franz',            'Wien',    'A');
INSERT INTO person VALUES (109,'Wagner','Richard',         'Muenchen','D');
INSERT INTO person VALUES (110,'Verdi','Giuseppe',         'Busseto', 'I');
INSERT INTO person VALUES (111,'Bruckner','Anton',         'Linz',    'A');
INSERT INTO person VALUES (112,'Brahms','Johannes',        'Wien',    'A');
INSERT INTO person VALUES (113,'Bizet','Georges',          'Paris',   'F');
INSERT INTO person VALUES (114,'Tschaikowskij','Peter',    'Moskau',  'RUS');
INSERT INTO person VALUES (115,'Puccini','Giacomo',        'Mailand', 'I');
INSERT INTO person VALUES (116,'Strauss','Richard',        'Muenchen','D');
INSERT INTO person VALUES (117,'Schoenberg','Arnold',      'Wien',    'A');

INSERT INTO referent VALUES (101,'21.03.1935','01.01.1980',NULL);
INSERT INTO referent VALUES (103,'01.04.1932','01.01.1991',NULL);
INSERT INTO referent VALUES (104,'27.01.1956','01.07.1985',NULL);
INSERT INTO referent VALUES (111,'04.09.1924','01.07.1990','Mag');
INSERT INTO referent VALUES (114,'25.04.1940','01.07.1980',NULL);
INSERT INTO referent VALUES (116,'11.06.1964','01.01.1994','Dr');

INSERT INTO kurs VALUES (1,'Notenkunde',       2,1400.00);
INSERT INTO kurs VALUES (2,'Harmonielehre',    3,2000.00);
INSERT INTO kurs VALUES (3,'Rhythmik',         1, 700.00);
INSERT INTO kurs VALUES (4,'Instrumentenkunde',2,1500.00);
INSERT INTO kurs VALUES (5,'Dirigieren',       3,1900.00);
INSERT INTO kurs VALUES (6,'Musikgeschichte',  2,1400.00);
INSERT INTO kurs VALUES (7,'Komposition',      4,3000.00);

INSERT INTO setztvor VALUES (2,1);
INSERT INTO setztvor VALUES (3,1);
INSERT INTO setztvor VALUES (5,2);
INSERT INTO setztvor VALUES (5,3);
INSERT INTO setztvor VALUES (5,4);
INSERT INTO setztvor VALUES (7,5);
INSERT INTO setztvor VALUES (7,6);

INSERT INTO geeignet VALUES (1,103);
INSERT INTO geeignet VALUES (1,114);
INSERT INTO geeignet VALUES (2,104);
INSERT INTO geeignet VALUES (2,111);
INSERT INTO geeignet VALUES (3,103);
INSERT INTO geeignet VALUES (4,104);
INSERT INTO geeignet VALUES (5,101);
INSERT INTO geeignet VALUES (5,114);
INSERT INTO geeignet VALUES (6,111);
INSERT INTO geeignet VALUES (7,103);
INSERT INTO geeignet VALUES (7,116);

INSERT INTO kveranst VALUES (1,1,'07.04.2013','08.04.2013','Wien',    3,103);
INSERT INTO kveranst VALUES (1,2,'23.06.2014','24.06.2014','Moskau',  4,114);
INSERT INTO kveranst VALUES (1,3,'10.04.2015','11.04.2015','Paris',   3,NULL);
INSERT INTO kveranst VALUES (2,1,'09.10.2013','11.10.2013','Wien',    4,104);
INSERT INTO kveranst VALUES (3,1,'17.11.2013','17.11.2013','Moskau',  3,103);
INSERT INTO kveranst VALUES (4,1,'12.01.2014','13.01.2014','Wien',    3,116);
INSERT INTO kveranst VALUES (4,2,'28.03.2014','29.03.2014','Wien',    4,104);
INSERT INTO kveranst VALUES (5,1,'18.05.2014','20.05.2014','Paris',   3,101);
INSERT INTO kveranst VALUES (5,2,'23.09.2014','26.09.2014','Wien',    2,101);
INSERT INTO kveranst VALUES (5,3,'30.03.2015','01.04.2015','Rom',     3,NULL);
INSERT INTO kveranst VALUES (7,1,'09.03.2015','13.03.2015','Wien',    5,103);
INSERT INTO kveranst VALUES (7,2,'14.09.2015','18.09.2015','Muenchen',4,116);

INSERT INTO besucht VALUES (1,1,108,'01.05.2013');
INSERT INTO besucht VALUES (1,1,109,NULL);
INSERT INTO besucht VALUES (1,1,114,NULL);
INSERT INTO besucht VALUES (1,2,110,'01.07.2014');
INSERT INTO besucht VALUES (1,2,112,'03.07.2014');
INSERT INTO besucht VALUES (1,2,113,'20.07.2014');
INSERT INTO besucht VALUES (1,2,116,NULL);
INSERT INTO besucht VALUES (1,3,110,NULL);
INSERT INTO besucht VALUES (2,1,105,'15.10.2013');
INSERT INTO besucht VALUES (2,1,109,'03.11.2013');
INSERT INTO besucht VALUES (2,1,112,'28.10.2013');
INSERT INTO besucht VALUES (2,1,116,NULL);
INSERT INTO besucht VALUES (3,1,101,NULL);
INSERT INTO besucht VALUES (3,1,109,NULL);
INSERT INTO besucht VALUES (3,1,117,'20.11.2013');
INSERT INTO besucht VALUES (4,1,102,'20.01.2014');
INSERT INTO besucht VALUES (4,1,107,'01.02.2014');
INSERT INTO besucht VALUES (4,1,111,NULL);
INSERT INTO besucht VALUES (4,2,106,'07.04.2014');
INSERT INTO besucht VALUES (4,2,109,'15.04.2014');
INSERT INTO besucht VALUES (5,1,103,NULL);
INSERT INTO besucht VALUES (5,1,109,'07.06.2014');
INSERT INTO besucht VALUES (5,2,115,'07.10.2014');
INSERT INTO besucht VALUES (5,2,116,NULL);
INSERT INTO besucht VALUES (7,1,109,'20.03.2015');
INSERT INTO besucht VALUES (7,1,113,NULL);
INSERT INTO besucht VALUES (7,1,117,'08.04.2015');

SELECT * FROM person;
SELECT * FROM referent;
SELECT * FROM kurs;
SELECT * FROM setztvor;
SELECT * FROM geeignet;
SELECT * FROM kveranst;
SELECT * FROM besucht;
