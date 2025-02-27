-- create database fahrzeugverwaltung;
use fahrzeugverwaltung;

drop table if exists buchungen, hotels, rueckfluege, hinfluege, reiseziele, kunden, fluege;

create table fluege
(
    fnr char(5) primary key,
)

create table kunden
(
    knr     int primary key,
    name    varchar(30) not null,
    vorname varchar(30) not null,
    adresse varchar(30),
    ort     varchar(30),
    unique (name, vorname, adresse, ort),
)

create table reiseziele
(
    rznr      int primary key,
    reiseziel varchar(30) unique not null,
)

create table hinfluege
(
    bnr    int,
    fnr    char(5) references fluege (fnr),
    hfdat  date not null,
    hfzeit time not null,
    primary key (bnr, fnr),
)

create table rueckfluege
(
    bnr    int,
    fnr    char(5) references fluege (fnr),
    rfdat  date not null,
    rfzeit time not null,
    primary key (bnr, fnr),
)

create table hotels
(
    hnr   int primary key,
    hotel varchar(30) not null check (hotel in
                                      ('Hilton', 'Royal', 'Central', 'Aloha', 'Pallas',
                                       'Perle', 'Tropica', 'Mango')),
    rznr  int         not null references reiseziele (rznr),
)

create table buchungen
(
    bnr           int primary key,
    buchungsdatum date,
    preis         float check (preis between 50 and 5000),
    personen      int check (personen > 0),
    knr           int not null references kunden (knr),
    hnr           int not null references hotels (hnr),
)

insert into fluege
    (fnr)
values ('AF210'),
       ('AF212'),
       ('AF320'),
       ('AF512'),
       ('AV555'),
       ('BA123'),
       ('BA212'),
       ('BA321'),
       ('CR101'),
       ('DA110'),
       ('SR212'),
       ('SR220'),
       ('SR420'),
       ('VI113')

insert into kunden
    (knr, name, vorname, adresse, ort)
values (1, 'Meier', 'Max', 'Feldweg 5', 'Buckten'),
       (2, 'Müller', 'Hugo', 'Saturnweg 7', 'Laufen'),
       (3, 'Müller', 'Hugo', 'Flühstr. 12', 'Reinach'),
       (4, 'Schmid', 'Beat', 'Hauptstr. 13', 'Aesch'),
       (5, 'Steffen', 'Felix', 'Heuboden 2', 'Pratteln');

insert into reiseziele
    (rznr, reiseziel)
values (1, 'Birmingham'),
       (2, 'Caracas'),
       (3, 'Frankfurt'),
       (4, 'Hawaii'),
       (5, 'Ibiza'),
       (6, 'Rio'),
       (7, 'St. Domingo');

insert into hinfluege
    (bnr, fnr, hfdat, hfzeit)
values (1, 'SR220', '2008-03-12', '07:15:00'),
       (2, 'BA212', '2008-04-23', '08:20:00'),
       (3, 'SR420', '2008-04-23', '09:30:00'),
       (4, 'SR212', '2008-05-25', '12:40:00'),
       (5, 'BA123', '2008-03-12', '12:10:00'),
       (5, 'AF320', '2008-05-24', '08:15:00'),
       (5, 'AF512', '2008-05-24', '17:20:00'),
       (6, 'AV555', '2008-04-12', '10:00:00'),
       (6, 'VI113', '2008-04-12', '22:30:00');

insert into rueckfluege
    (bnr, fnr, rfdat, rfzeit)
values (1, 'BA321', '2008-03-15', '12:10:00'),
       (2, 'SR212', '2008-04-28', '12:30:00'),
       (4, 'DA110', '2008-04-24', '21:10:00'),
       (5, 'AF210', '2008-06-04', '09:30:00'),
       (5, 'AF212', '2008-06-04', '18:20:00'),
       (5, 'CR101', '2008-08-05', '07:20:00')

insert into hotels
    (hnr, hotel, rznr)
values (1, 'Aloha', 4),
       (2, 'Central', 2),
       (3, 'Hilton', 6),
       (4, 'Pallas', 6),
       (5, 'Perle', 5),
       (6, 'Mango', 5),
       (7, 'Royal', 1),
       (8, 'Royal', 7),
       (9, 'Tropica', 3);

insert into buchungen
    (bnr, buchungsdatum, preis, personen, knr, hnr)
values (1, '2007-12-12', 2450, 2, 2, 3),
       (2, '2007-12-22', 450, 1, 1, 7),
       (3, '2008-01-01', 4450, 3, 4, 1),
       (4, '2008-01-04', 840, 4, 2, 9),
       (5, '2008-01-15', 1820, 1, 5, 8),
       (6, '2008-02-01', 2400, 2, 3, 2);