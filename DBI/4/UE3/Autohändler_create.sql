create database Autohaendler;

use Autohaendler;

drop table if exists haendler_verkauft_automarke
drop table if exists Haendler;
drop table if exists AutoMarke;

/* DDL */
create table Haendler (
	haendler_id int primary key,
	fName text,
	lName text,
	ort text
);

create table AutoMarke (
	autoMarke_id int primary key,
	markeName text,
	gruender text
);

create table haendler_verkauft_automarke (
	anz_verfuebare_modelle int,

	haendler_id int,
	autoMarke_id int,

	foreign key (haendler_id) references Haendler,
	foreign key (autoMarke_id) references AutoMarke,
	primary key (haendler_id, autoMarke_id)
);


/* DML */
insert into Haendler (haendler_id, fName, lName, ort) values 
	(1, 'Hans', 'Peter', 'Buxtehude'),
	(2, 'Günther', 'Mayer', 'Lenzlandback'),
	(3, 'Jürgen', 'Hofer', 'Neu Lenksbach'),
	(4, 'Daniel', 'Simth', 'Markt Randsbach'),
	(5, 'Jens', 'Mitterbacher', 'Neudorf')


insert into AutoMarke (autoMarke_id, markeName, gruender) values
	(1, 'Audi', 'August Horch'),
	(2, 'BMW', 'Karl Rapp, Gustav Otto'),
	(3, 'Mercedes-Benz', 'Karl Benz, Gottlieb Daimler'),
	(4, 'Volkswagen', 'Deutsche Arbeitsfront (unter Leitung von Ferdinand Porsche)'),
	(5, 'Porsche', 'Ferdinand Porsche');


insert into haendler_verkauft_automarke (anz_verfuebare_modelle, haendler_id, autoMarke_id) values
-- Hans Peter 
(8, 1, 1),
(5, 1, 2),
(6, 1, 3),
(7, 1, 4),
(3, 1, 5),

-- Günther Mayer 
(4, 2, 1),
(6, 2, 2),
(5, 2, 3),
(2, 2, 4),

-- Jürgen Hofer
(3, 3, 2),
(7, 3, 4),
(4, 3, 5),

-- Daniel Simth
(5, 4, 1),
(9, 4, 4),

-- Jens Mitterbacher
(2, 5, 3);