--create database Schulungsfirma;
use Schulungsfirma;

drop table if exists besucht,
geeignet,
kveranst,
referent,
setztvor,
person,
kurs;

create table kurs (
	knr int primary key,
	bezeichn varchar(30),
	tage int check(
		tage >= 1
		and tage <= 10
	),
	preis float
);

create table person (
	pnr int primary key,
	fname varchar(20),
	vname varchar(20),
	ort varchar(50),
	land varchar(30)
);

create table setztvor (
	knr int not null references kurs(knr),
	knrvor int not null references kurs(knr),
	primary key(knr, knrvor)
);

create table referent (
	pnr int references person(pnr) primary key,
	gebdat date,
	seit date,
	titel varchar(20)
);

create table kveranst (
	knr int not null references kurs(knr),
	knrlfnd int not null,
	von date,
	bis date,
	ort varchar(50),
	plaetze int check(plaetze > 0),
	pnr int references referent(pnr),
	primary key(knr, knrlfnd),
	check(von < bis)
);

create table geeignet (
	knr int not null references kurs(knr),
	pnr int not null references referent(pnr),
	primary key(knr, pnr)
);

create table besucht (
	knr int not null,
	knrlfnd int not null,
	pnr int not null references person,
	bezahlt date,
	foreign key (knr, knrlfnd) references kveranst(knr, knrlfnd)
);