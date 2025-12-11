use mensa
go
drop table if exists serviert
drop table if exists Speise_besteht_aus
drop table if exists Menue_besteht_aus
drop table if exists liefert
drop table if exists bestellposition
drop table if exists bestellung
drop table if exists tag
drop table if exists speise
drop table if exists zutat
drop table if exists menue
drop table if exists lieferant
go

create table speise
(
	SpeiseNr    int         primary key
	,Bezeichnung varchar(60)
	,Typ         int
	--1=Vorspeise,2=Hauptspeise,3=Nachspeise
)
go

create table zutat
(
	ZutatenNr   int          primary key
	,Bezeichnung varchar(35)
	,Einheit     char(3)
	--  kg,l,Stk usw. 
	,Preis       decimal(8,2)
	,aktBestand  int
)
go

create table menue
(
	MenueNr   int          primary key
	,Kategorie tinyint
	--es gibt nur  2 Kategorien
	,Preis     decimal(8,2)
)
go

create table lieferant
(
	LiefNr  int         primary key
	,Name    varchar(35)
	,Plz     char(6)
	,Ort     varchar(40)
	,Strasse varchar(30)
	,TelNr   varchar(20)
)
go

create table tag
(
	aktTag    datetime primary key
	,Ferientag bit
	--0=Ferientag, 1=Schultag
	,Anzahl    int
)
go

create table bestellung
(
	BestellNr    int      primary key
	,Bestelldatum datetime
	,Lieferdatum  datetime
)
go

create table bestellposition
(
	BestellNr   int          references bestellung
	,PositionsNr int
	,primary key (BestellNr,PositionsNr)
	,Menge       int
	,Preis       decimal(8,2),
)
go

create table Speise_besteht_aus
(
	SpeiseNr  int          references Speise
	,Zutatennr int          references Zutat
	,primary key(SpeiseNr,Zutatennr)
	,Menge     decimal(8,2)
)
go

create table Menue_besteht_aus
(
	SpeiseNr int references Speise
	,MenueNr  int references Menue
	,primary key(SpeiseNr,MenueNr)
)
go

create table serviert
(
	MenueNr int      references Menue
	,aktTag  datetime references Tag
	,primary key(MenueNr,aktTag)
)
go

create table liefert
(
	ZutatenNr int references Zutat
	,LiefNr    int references Lieferant
	,primary key(ZutatenNr,LiefNr),
)
go