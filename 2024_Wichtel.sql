-- create database wichtel;
use wichtel;

drop table if exists Wichtel;
drop table if exists Geschenk;
drop table if exists Schueler;
drop table if exists Klasse;

create table Klasse
(
	ID integer identity(1,1) primary key,
	Name varchar(10)
);

create table Schueler
(
	ID integer identity(1,1) primary key,
	Vorname varchar(100),
	Nachname varchar(100),
	KlasseID integer references Klasse(ID)
);

create table Geschenk
(
	ID integer identity(1,1) primary key,
	Name varchar(100)
);

create table Wichtel
(
	VonID integer references Schueler(ID),
	FuerID integer references Schueler(ID),
	GeschenkID integer references Geschenk(ID)
		primary key (VonID, FuerID, GeschenkID)
);

insert into Klasse
	(Name)
values
	('3AHIF'),
	('3BHIF'),
	('3CHIF');

insert into Schueler
	(Nachname, Vorname, KlasseID)
values
	('Ackermann', 'Bernd', 1),
	('Demeter', 'Aron', 1),
	('Eitzenberger', 'Paul', 1),
	('Kappler', 'David', 1),
	('Kienast', 'Franziska', 1),
	('Kosche', 'Johannes', 1),
	('Kroh', 'Nicolas', 1),
	('Machacsek', 'Tim', 1),
	('Mayrhofer', 'Tobias', 1),
	('Monza', 'Fabian', 1),
	('Nyikos', 'Philip', 1),
	('Pallier', 'Nikos', 1),
	('Reithofer', 'Viktor', 1),
	('Salibi', 'Eli', 1),
	('Scheucher', 'Tobias', 1),
	('Schöll', 'Valerie', 1),
	('Uzunovic', 'Marcel', 1),
	('Vollnhofer', 'Markus', 1),
	('Wimmer', 'Andreas', 1),
	('Wolf', 'Martin', 1),
	('Balutsch', 'Aria', 2),
	('Bleier', 'Stefan', 2),
	('Böck', 'Maximilian', 2),
	('Daravoina', 'Dan', 2),
	('Dosek', 'Gabriel', 2),
	('Edris', 'Khaled', 2),
	('Fenz', 'Tim', 2),
	('Fuchs', 'Tobias', 2),
	('Gugumuck', 'Alexander', 2),
	('Hortschitz', 'Moritz', 2),
	('Lampl', 'Sebastian', 2),
	('Leibl', 'Justin', 2),
	('Nedal', 'Julian', 2),
	('Paugger', 'Raffael', 2),
	('Rauhofer', 'Fabian', 2),
	('Sanz', 'Lukas', 2),
	('Schratt', 'Simon', 2),
	('Sedlak', 'Felix', 2),
	('Stolz', 'Nico', 2),
	('Wagner-Baumgartner', 'Niklas', 2),
	('Wiedemann', 'Raphael', 2),
	('Birnbaumer', 'Tobias', 3),
	('Brajovic', 'Jovan', 3),
	('David', 'Constantin', 3),
	('Heigl', 'Thomas', 3),
	('Hodgkin', 'Finley', 3),
	('Kuntsche', 'Philias', 3),
	('Lottenbach', 'Nico', 3),
	('Munkhbat', 'Sebastian', 3),
	('Pavelka', 'Michael', 3),
	('Penzinger', 'Tristan', 3),
	('Pernecker', 'Peter', 3),
	('Pilgram', 'Noah', 3),
	('Sallmannshofer', 'Gregor', 3),
	('Schebesta', 'Kilian', 3),
	('Schmidtbauer', 'Alina', 3),
	('Spitzer', 'Markus', 3),
	('Stastny', 'Markus', 3),
	('Steinwendter', 'Felix', 3),
	('Strobl', 'Lennard', 3),
	('Wallisch', 'Niklas', 3);





insert into Geschenk
	(Name)
values
	('Socken'),
	('Krawatte'),
	('Poker Koffer'),
	('Poker Karten'),
	('Netflix für 1 Monat'),
	('Disney+ für 1 Monat'),
	('Demon Slayer Band 15'),
	('GTA 6'),
	('Groot Figur'),
	('Boo Hoo Figur'),
	('Programmieren für Dummies'),
	('Datenbanken für Dummies'),
	('Yoshi Figur'),
	('Fifa 23'),
	('Princess Peach Showtime!'),
	('Pokemon Sticker'),
	('Codenames');

select
	*
from
	Klasse;
select
	*
from
	Schueler;
select
	*
from
	Geschenk;
select
	*
from
	Wichtel;

-- mit dem nachfolgenden Befehl wird eine zufällige Zahl im Bereich von 1 - 75 erstellt
-- rand(): liefert eine zufällige Zahl zwischen 0 und 0,99
-- *100: liefert eine zufällige Zahl zwischen 0 und 99
-- % 75: brauchen nur zufällige Zahlen zwischen 0 und 74
-- +1: liefert eine zufällige Zahlen zwischen 1 und 75
select
	(cast((rand()*100) as int)) % 75 + 1;

-- ein kleiner Ausblick in den 4. Jahrgang
-- mit dem nachfolgenden Code können Sie mehrere Inserts innerhalb einer Schleife hinzufügen
declare @i smallint;
declare @max smallint;

set @i = 1;
set @max = 76;

while @i < @max
begin
	insert into Wichtel
		(VonID, FuerID, GeschenkID)
	values
		((cast((rand()*100) as int)) % 75 + 1, (cast((rand()*100) as int)) % 75 + 1, (cast((rand()*100) as int)) % 16 + 1);
	-- TODO: fügen Sie hier ein insert ein, um zufällig die vonID, fuerID und die geschenkID einzufügen
	set @i = @i + 1
end;
go
