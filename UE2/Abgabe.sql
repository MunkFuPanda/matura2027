--create database ltp;

use ltp;

drop table if exists ltp;
drop table if exists p;
drop table if exists t;
drop table if exists l;

create table l
(
    lnr    varchar(10) primary key,
    lname  varchar(30) not null,
    rabatt int default 0 check (rabatt between 0 and 100),
    stadt  varchar(20) not null,
);

create table t
(
    tnr   varchar(10) primary key,
    tname varchar(30) not null,
    farbe varchar(20),
    preis int         not null default 0 check (preis > 0),
    stadt varchar(20),
);

create table p
(
    pnr   varchar(10) primary key,
    pname varchar(30) not null,
    stadt varchar(20) not null,
);

create table ltp
(
    lnr   varchar(10) not null foreign key references l (lnr),
    tnr   varchar(10) not null foreign key references t (tnr),
    pnr   varchar(10) not null foreign key references p (pnr),
    menge int         not null check (menge > 0),
    primary key (lnr, tnr, pnr)
);