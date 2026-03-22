--use master;
--DROP DATABASE Feuerwehr;
--CREATE DATABASE Feuerwehr;

USE Feuerwehr;

ALTER TABLE team DROP CONSTRAINT team_person_FK;
DROP TABLE IF EXISTS is_troop_member; 
DROP TABLE IF EXISTS has_participated; 
DROP TABLE IF EXISTS person; 
DROP TABLE IF EXISTS team;
DROP TABLE IF EXISTS pers_rank; 
DROP TABLE IF EXISTS competitve_troop; 
DROP TABLE IF EXISTS competition;
GO

CREATE TABLE competition
(
    Competition_id INTEGER NOT NULL PRIMARY KEY,
    organizer VARCHAR(34),
    location VARCHAR(50),
    category_name VARCHAR(30),
    start_time DATETIME,
    end_time DATETIME
);
GO

CREATE TABLE competitve_troop
(
    troop_id INTEGER NOT NULL PRIMARY KEY,
    foundation_date DATE,
    category VARCHAR(30),
    bonus INTEGER
);
GO

CREATE TABLE pers_rank
(
    rank_header VARCHAR(30) NOT NULL PRIMARY KEY,
    rank_base_content VARCHAR(30),
    pers_rank_rank_header VARCHAR(30) REFERENCES pers_rank(rank_header),
    salary NUMERIC(10,2)
);
GO

/* team zuerst ohne FK zu person wegen Zykluliernder Beziehung */
CREATE TABLE team
(
    team_id INTEGER NOT NULL PRIMARY KEY,
    team_nickname VARCHAR(30),
    pnr INTEGER
);
GO

CREATE TABLE person
(
    pnr INTEGER NOT NULL PRIMARY KEY,
    first_name VARCHAR(30),
    last_name VARCHAR(30),
    Birthdate DATE,
    phone VARCHAR(40),
    rankdate DATE,
    rank_header VARCHAR(30) NOT NULL REFERENCES pers_rank(rank_header),
    team_id INTEGER NOT NULL REFERENCES team(team_id)
);
GO

ALTER TABLE team
ADD CONSTRAINT team_person_FK FOREIGN KEY (pnr) REFERENCES person (pnr);
GO

CREATE TABLE has_participated
(
    troop_id INTEGER NOT NULL REFERENCES competitve_troop(troop_id),
    Competition_id INTEGER NOT NULL REFERENCES competition(Competition_id),
    placing INT NULL,  -- 1=Sieger, 2=Zweiter, 3=Dritter, sonst NULL/andere Zahl
    PRIMARY KEY (troop_id, Competition_id)
);
GO

CREATE TABLE is_troop_member
(
    troop_id INTEGER NOT NULL REFERENCES competitve_troop(troop_id),
    pnr INTEGER NOT NULL REFERENCES person(pnr),
    job_description VARCHAR(40),
    PRIMARY KEY (troop_id, pnr)
);
GO

INSERT INTO pers_rank (rank_header, rank_base_content, pers_rank_rank_header, salary) VALUES
('Feuerwehrmann', 'FM', NULL, 1800.00),
('Oberfeuerwehrmann', 'OFM', 'Feuerwehrmann', 1950.00),
('Hauptfeuerwehrmann', 'HFM', 'Oberfeuerwehrmann', 2100.00),
('L�schmeister', 'LM', 'Hauptfeuerwehrmann', 2400.00),
('Oberl�schmeister', 'OLM', 'L�schmeister', 2600.00),
('Hauptl�schmeister', 'HLM', 'Oberl�schmeister', 2800.00),
('Brandmeister', 'BM', 'Hauptl�schmeister', 3100.00),
('Oberbrandmeister', 'OBM', 'Brandmeister', 3400.00),
('Hauptbrandmeister', 'HBM', 'Oberbrandmeister', 3700.00),
('Brandinspektor', 'BI', 'Hauptbrandmeister', 4100.00),
('Abschnittsbrandinspektor', 'ABI', 'Brandinspektor', 4500.00);
GO

INSERT INTO team (team_id, team_nickname, pnr) VALUES
(1, 'Tank Wien 1', NULL),
(2, 'Pumpe Wien 1', NULL),
(3, 'R�st Wien 1', NULL),
(4, 'Atemschutz Wien 1', NULL),
(5, 'KDO Wien 1', NULL),
(6, 'Pumpe N� 3', NULL);
GO

INSERT INTO person (pnr, first_name, last_name, Birthdate, phone, rankdate, rank_header, team_id) VALUES
(1101, 'Thomas',  'Gruber',   '1985-03-12', '0664123401', '2010-06-01', 'Brandmeister', 1),
(1102, 'Michael', 'Steiner',  '1990-07-22', '0664123402', '2015-05-01', 'L�schmeister', 1),
(1103, 'Andreas', 'Hofer',    '1995-01-10', '0664123403', '2018-03-15', 'Oberfeuerwehrmann', 2),
(1104, 'Lukas',   'Wagner',   '2000-11-05', '0664123404', '2020-09-01', 'Feuerwehrmann', 2),
(1105, 'Martin',  'Bauer',    '1982-04-18', '0664123405', '2008-02-01', 'Hauptbrandmeister', 3),
(1106, 'Daniel',  'Moser',    '1988-09-14', '0664123406', '2012-04-01', 'Oberbrandmeister', 3),
(1107, 'Florian', 'Leitner',  '1992-02-27', '0664123407', '2016-08-01', 'Hauptl�schmeister', 4),
(1108, 'Simon',   'Fuchs',    '1997-06-03', '0664123408', '2019-10-01', 'L�schmeister', 4),
(1109, 'Patrick', 'Huber',    '1999-12-20', '0664123409', '2021-05-01', 'Hauptfeuerwehrmann', 1),
(1110, 'Julian',  'Schmid',   '2001-01-09', '0664123410', '2022-06-01', 'Feuerwehrmann', 2),

(1111, 'Sebastian','Pichler', '1987-05-30', '0664123411', '2011-03-01', 'Brandinspektor', 5),
(1112, 'Gerald',   'Koller',  '1993-08-19', '0664123412', '2017-07-01', 'Oberl�schmeister', 5),
(1113, 'Markus',   'Novak',   '1984-10-02', '0664123413', '2009-11-01', 'Abschnittsbrandinspektor', 5),

(1114, 'Benjamin','Mayr',     '1996-04-07', '0664123414', '2018-09-01', 'Hauptfeuerwehrmann', 6),
(1115, 'David',   'Eder',     '1998-03-16', '0664123415', '2019-04-01', 'Oberfeuerwehrmann', 6),
(1116, 'Tobias',  'Auer',     '1991-01-25', '0664123416', '2014-01-01', 'Oberbrandmeister', 6),
(1117, 'Philipp', 'Kraus',    '1989-07-11', '0664123417', '2013-05-01', 'Hauptl�schmeister', 6),

(1118, 'Johannes','Haas',     '1994-09-09', '0664123418', '2017-02-01', 'L�schmeister', 1),
(1119, 'Maximilian','Seidl',  '2002-02-14', '0664123419', '2023-01-01', 'Feuerwehrmann', 3),
(1120, 'Alexander','Brunner', '1997-11-28', '0664123420', '2020-02-01', 'Hauptfeuerwehrmann', 4),

(1121, 'Felix',   'Hartl',    '1990-12-01', '0664123421', '2015-10-01', 'Oberl�schmeister', 2),
(1122, 'Matthias','Dietrich', '1986-06-06', '0664123422', '2010-01-01', 'Brandmeister', 3),
(1123, 'Nico',    'Lang',     '1999-05-05', '0664123423', '2021-09-01', 'Oberfeuerwehrmann', 4),
(1124, 'Raphael', 'Winter',   '1995-02-02', '0664123424', '2018-01-01', 'Hauptfeuerwehrmann', 1),
(1125, 'Kevin',   'K�nig',    '2000-08-08', '0664123425', '2022-03-01', 'Feuerwehrmann', 6);
GO

/* ----------------------------
   TEAMLEITER setzen (jetzt existieren Personen, FK ist erf�llbar)
   ---------------------------- */
UPDATE team SET pnr = 1101 WHERE team_id = 1;  -- Tank
UPDATE team SET pnr = 1103 WHERE team_id = 2;  -- Pumpe
UPDATE team SET pnr = 1105 WHERE team_id = 3;  -- R�st
UPDATE team SET pnr = 1107 WHERE team_id = 4;  -- Atemschutz
UPDATE team SET pnr = 1113 WHERE team_id = 5;  -- KDO
UPDATE team SET pnr = 1116 WHERE team_id = 6;  -- N�
GO

INSERT INTO competition (Competition_id, organizer, location, category_name, start_time, end_time) VALUES
(5101, '�BFV', 'Wien', 'Bronze A', '2025-06-01 09:00:00', '2025-06-01 18:00:00'),
(5102, 'LFV N�', 'St. P�lten', 'Silber A', '2025-07-05 08:30:00', '2025-07-05 17:30:00');
GO

INSERT INTO competitve_troop (troop_id, foundation_date, category, bonus) VALUES
(6101, '2012-01-01', 'Aktiv', 10),
(6102, '2018-01-01', 'Aktiv', 5);
GO

INSERT INTO has_participated (troop_id, Competition_id, placing) VALUES
(6101, 5101, 1),
(6102, 5102, 2);
GO

INSERT INTO is_troop_member (troop_id, pnr, job_description) VALUES
(6101, 1101, 'Gruppenkommandant'),
(6101, 1102, 'Maschinist'),
(6101, 1109, 'Angriffstrupp'),
(6101, 1124, 'Wassertrupp'),
(6101, 1118, 'Schlauchtrupp'),

(6102, 1116, 'Gruppenkommandant'),
(6102, 1114, 'Maschinist'),
(6102, 1115, 'Angriffstrupp'),
(6102, 1125, 'Wassertrupp'),
(6102, 1117, 'Schlauchtrupp');
GO


