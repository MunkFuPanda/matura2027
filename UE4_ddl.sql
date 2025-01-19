-- create database fahrzeugverwaltung;
use fahrzeugverwaltung;

drop table if exists  Buchungen, Hotels, Rueckfluege, Hinfluege, Reiseziele, Kunden, Fluege;

create table Fluege
(
    FNr char(5) primary key,
)

create table Kunden
(
    KNr int primary key,
    Name varchar(30) not null,
    Vorname varchar(30) not null,
    Adresse varchar(30),
    Ort varchar(30),
    unique (Name, Vorname, Adresse, Ort),
)

create table Reiseziele
(
    RZNr int primary key,
    Reiseziel varchar(30) unique not null,
)

create table Hinfluege
(
    BNr int,
    FNr char(5) references Fluege(FNr),
    HFDat date not null,
    HFZeit time not null,
    primary key(BNr, FNr),
)

create table Rueckfluege
(
    BNr int,
    FNr char(5) references Fluege(FNr),
    RFDat date not null,
    RFZeit time not null,
    primary key(BNr, FNr),
)

create table Hotels
(
    HNr int primary key,
    Hotel varchar(30) not null check(Hotel in ('Hilton', 'Royal', 'Central', 'Aloha', 'Pallas', 'Perle', 'Tropica', 'Mango')),
    RZNr int references Reiseziele(RZNr) not null,
)

create table Buchungen
(
    BNr int primary key,
    Buchungsdatum date,
    Preis float check(Preis between 50 and 5000),
    Personen int check(Personen > 0),
    KNr int references Kunden(KNr) not null,
    HNr int references Hotels(HNr) not null,
)

insert into Fluege
    (FNr)
values
    ('AF210'),
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

insert into Kunden
    (KNr, Name, Vorname, Adresse, Ort)
values
    (1, 'Meier', 'Max', 'Feldweg 5', 'Buckten'),
    (2, 'Müller', 'Hugo', 'Saturnweg 7', 'Laufen'),
    (3, 'Müller', 'Hugo', 'Flühstr. 12', 'Reinach'),
    (4, 'Schmid', 'Beat', 'Hauptstr. 13', 'Aesch'),
    (5, 'Steffen', 'Felix', 'Heuboden 2', 'Pratteln');

insert into Reiseziele
    (RZNr, Reiseziel)
values
    (1, 'Birmingham'),
    (2, 'Caracas'),
    (3, 'Frankfurt'),
    (4, 'Hawaii'),
    (5, 'Ibiza'),
    (6, 'Rio'),
    (7, 'St. Domingo');

insert into Hinfluege
    (BNr, FNr, HFDat, HFZeit)
values
    (1, 'SR220', '2008-03-12', '07:15:00'),
    (2, 'BA212', '2008-04-23', '08:20:00'),
    (3, 'SR420', '2008-04-23', '09:30:00'),
    (4, 'SR212', '2008-05-25', '12:40:00'),
    (5, 'BA123', '2008-03-12', '12:10:00'),
    (5, 'AF320', '2008-05-24', '08:15:00'),
    (5, 'AF512', '2008-05-24', '17:20:00'),
    (6, 'AV555', '2008-04-12', '10:00:00'),
    (6, 'VI113', '2008-04-12', '22:30:00');

insert into Rueckfluege
    (BNr, FNr, RFDat, RFZeit)
values
    (1, 'BA321', '2008-03-15', '12:10:00'),
    (2, 'SR212', '2008-04-28', '12:30:00'),
    (4, 'DA110', '2008-04-24', '21:10:00'),
    (5, 'AF210', '2008-06-04', '09:30:00'),
    (5, 'AF212', '2008-06-04', '18:20:00'),
    (5, 'CR101', '2008-08-05', '07:20:00')

insert into Hotels
    (HNr, Hotel, RZNr)
values
    (1, 'Aloha', 4),
    (2, 'Central', 2),
    (3, 'Hilton', 6),
    (4, 'Pallas', 6),
    (5, 'Perle', 5),
    (6, 'Mango', 5),
    (7, 'Royal', 1),
    (8, 'Royal', 7),
    (9, 'Tropica', 3);

insert into Buchungen
    (BNr, Buchungsdatum, Preis, Personen, KNr, HNr)
values
    (1, '2007-12-12', 2450, 2, 2, 3),
    (2, '2007-12-22', 450, 1, 1, 7),
    (3, '2008-01-01', 4450, 3, 4, 1),
    (4, '2008-01-04', 840, 4, 2, 9),
    (5, '2008-01-15', 1820, 1, 5, 8),
    (6, '2008-02-01', 2400, 2, 3, 2);