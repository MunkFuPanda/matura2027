--create database ltp;

use ltp;

drop table if exists LTP;
drop table if exists P;
drop table if exists T;
drop table if exists L;

create table L (
	LNR varchar(10) primary key,
	LNAME varchar(30) not null,
	RABATT int default 0 check(RABATT between 0 and 100),
	STADT varchar(20) not null,
);

create table T (
	TNR varchar(10) primary key,
	TNAME varchar(30) not null,
	FARBE varchar(20),
	PREIS int not null default 0 check(PREIS > 0),
	STADT varchar(20),
);

create table P (
	PNR varchar(10) primary key,
	PNAME varchar(30) not null,
	STADT varchar (20) not null,
);

create table LTP (
	LNR varchar(10) not null foreign key references L(LNR),
	TNR varchar(10) not null foreign key references T(TNR),
	PNR varchar(10) not null foreign key references P(PNR),
	MENGE int not null check(MENGE > 0),
	primary key (LNR, TNR, PNR)
);