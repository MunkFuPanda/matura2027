use tankstelle;

Drop Table if exists Verkauf
Drop Table if exists Zapfsaeule
Drop Table if exists Tagespreis
Drop Table if exists Kraftstoff
GO

Create Table Kraftstoff(
Name varchar(20) Primary Key,
Oktanzahl int)

Create Table Tagespreis(
Tagesdatum Date,
Preis decimal(6,4),
KName varchar(20) references Kraftstoff 
Primary Key(Tagesdatum, KName)
)

Create Table Zapfsaeule (
ZNr int Primary Key, 
Typ varchar(10),
Hersteller varchar(20),
Selbstbedienung bit, 
aktMengeL int, 
maxMengeL int, 
KName varchar(20) References Kraftstoff)

Create Table Verkauf(
VNr int Primary Key, 
MengeL int, 
VerkaufsZeitpunkt DateTime, 
ZNr int References Zapfsaeule)
Go



set dateformat dmy
INSERT INTO Kraftstoff VALUES ('Benzin',90);
INSERT INTO Kraftstoff VALUES ('Benzin95',95 );
INSERT INTO Kraftstoff VALUES ('Super',100 );
INSERT INTO Kraftstoff VALUES ('Super+',110);
INSERT INTO Kraftstoff VALUES ('Diesel', 0);

INSERT INTO Tagespreis VALUES ('1.10.2009', 0.987, 'Benzin');
INSERT INTO Tagespreis VALUES ('31.10.2009', 0.997, 'Benzin');
INSERT INTO Tagespreis VALUES ('1.10.2009', 0.999, 'Benzin95');
INSERT INTO Tagespreis VALUES ('2.10.2009', 1.001, 'Super');
INSERT INTO Tagespreis VALUES ('2.10.2009', 1.005, 'Super+');
INSERT INTO Tagespreis VALUES ('1.10.2009', 0.975, 'Diesel');
INSERT INTO Tagespreis VALUES ('31.10.2009', 0.985, 'Diesel');

INSERT INTO Zapfsaeule VALUES (1,'PKW','TankiZapf',0,1000,2000,'Benzin');
INSERT INTO Zapfsaeule VALUES (2,'LKW','TankiZapf',1,10000,20000,'Benzin');
INSERT INTO Zapfsaeule VALUES (3,'PKW','TankiZapf',0,1000,2000,'Diesel');
INSERT INTO Zapfsaeule VALUES (4,'LKW','TankiZapf',1,10000,20000,'Diesel');


INSERT INTO Verkauf VALUES (1,40,'3.10.2015',1);
INSERT INTO Verkauf VALUES (2,50,'3.10.2015',1);
INSERT INTO Verkauf VALUES (3,80,'3.10.2015',2);
INSERT INTO Verkauf VALUES (4,40,'3.10.2015',3);
INSERT INTO Verkauf VALUES (5,45,'3.10.2015',3);
INSERT INTO Verkauf VALUES (6,120,'3.10.2015',4);

INSERT INTO Verkauf VALUES (7,40,'4.10.2015',1);
INSERT INTO Verkauf VALUES (8,50,'4.10.2015',1);
INSERT INTO Verkauf VALUES (9,80,'4.10.2015',2);
INSERT INTO Verkauf VALUES (14,40,'5.10.2015',3);
INSERT INTO Verkauf VALUES (15,45,'5.10.2015',3);
INSERT INTO Verkauf VALUES (16,120,'5.10.2015',4);
GO







