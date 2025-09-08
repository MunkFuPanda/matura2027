--create database Buchladen;
use Buchladen;

Drop Table if exists Buchbestellung;
Drop Table if exists Bestellung;
Drop Table if exists Buch; 
Drop Table if exists Kunde;

CREATE TABLE Kunde (
    KundenID INT PRIMARY KEY,
    Name VARCHAR(50),
    Email VARCHAR(100),
    Ort VARCHAR(40)
);

CREATE TABLE Buch (
    BuchID INT PRIMARY KEY,
    Titel VARCHAR(100),
    Autor VARCHAR(60),
    Preis DECIMAL(10,2),
    Veröffentlichungsjahr INT
);

CREATE TABLE Bestellung (
    BestellungID INT PRIMARY KEY,
    KundenID INT,
    Bestelldatum DATE,
    FOREIGN KEY (KundenID) REFERENCES Kunde(KundenID)
);

CREATE TABLE Buchbestellung (
    BuchbestellungID INT PRIMARY KEY,
    BestellungID INT,
    BuchID INT,
    Anzahl INT,
    FOREIGN KEY (BestellungID) REFERENCES Bestellung(BestellungID),
    FOREIGN KEY (BuchID) REFERENCES Buch(BuchID)
);

INSERT INTO Kunde (KundenID, Name, Ort, Email) VALUES
(1, 'Anna Schmidt', 'Berlin', 'anna.schmidt@example.com'),
(2, 'Bernd Müller', 'München', 'bernd.mueller@example.com'),
(3, 'Christine Bauer', 'Berlin', 'christine.bauer@example.com'),
(4, 'David Koch', 'Frankfurt', 'david.koch@example.com'),
(5, 'Erika Mustermann', 'Hamburg', 'erika.mustermann@example.com'),
(6, 'Frank Weber', 'Stuttgart', 'frank.weber@example.com'),
(7, 'Greta Lorenz', 'München', 'greta.lorenz@example.com'),
(8, 'Heiko Klein', 'Dortmund', 'heiko.klein@example.com'),
(9, 'Ingrid Fischer', 'Essen', 'ingrid.fischer@example.com'),
(10, 'Jens Vogel', 'Bremen', 'jens.vogel@example.com');

INSERT INTO Buch (BuchID, Titel, Autor, Preis, Veröffentlichungsjahr) VALUES
(1, 'Die unendliche Geschichte', 'Michael Ende', 18.00, 1979),
(2, 'Momo', 'Michael Ende', 12.00, 1973),
(3, 'Der Prozess', 'Franz Kafka', 15.00, 1925),
(4, 'Das Schloss', 'Franz Kafka', 22.00, 1926),
(5, 'Demian', 'Hermann Hesse', 9.00, 1919),
(6, 'Steppenwolf', 'Hermann Hesse', 7.50, 1927),
(7, 'Der Zauberberg', 'Thomas Mann', 25.00, 1924),
(8, 'Buddenbrooks', 'Thomas Mann', 14.50, 1901),
(9, 'Die Blechtrommel', 'Günter Grass', 19.90, 1959),
(10, 'Im Westen nichts Neues', 'Erich Maria Remarque', 13.00, 1928),
(11, 'Die Physiker', 'Friedrich Dürenmatt', 9.90, 1962);

INSERT INTO Bestellung (BestellungID, KundenID, Bestelldatum) VALUES
(1, 1, '2023-10-01'),
(2, 2, '2023-10-02'),
(3, 3, '2023-10-03'),
(4, 4, '2023-10-04'),
(5, 5, '2023-10-05'),
(6, 6, '2023-10-06'),
(7, 7, '2023-10-07'),
(8, 8, '2023-10-08'),
(9, 9, '2023-10-09'),
(10, 10, '2023-10-10');

INSERT INTO Buchbestellung (BuchbestellungID, BestellungID, BuchID, Anzahl) VALUES
(1, 1, 1, 1),
(2, 1, 2, 2),
(3, 2, 2, 1),
(4, 2, 3, 1),
(5, 3, 4, 1),
(6, 3, 5, 2),
(7, 4, 6, 1),
(8, 4, 7, 1),
(9, 5, 8, 1),
(10, 5, 9, 3),
(11, 6, 10, 2),
(12, 7, 1, 1),
(13, 7, 3, 1),
(14, 8, 2, 2),
(15, 8, 4, 2),
(16, 9, 5, 3),
(17, 9, 7, 1),
(18, 10, 6, 1);


  
