--create database Imkerei
use Imkerei

CREATE TABLE Imker( ImkerNr INTEGER PRIMARY KEY,
					  Name VARCHAR(100) NOT NULL,
					  GeborenAm DATE);


CREATE TABLE Hilfsarbeiter( stelltAn INTEGER,
						 	  ArbeiterNr INTEGER,
						 	  Name VARCHAR(100) NOT NULL,
						 	  Lohn INTEGER NOT NULL,
						 	  PRIMARY KEY(stelltAn,ArbeiterNr),
						 	  FOREIGN KEY(stelltAn) REFERENCES Imker(ImkerNr));


CREATE TABLE gelerntVon( Meister INTEGER REFERENCES Imker(ImkerNr),
						   Lehrling INTEGER REFERENCES Imker(ImkerNr),
						   PRIMARY KEY(Meister,Lehrling));

CREATE TABLE Bienenstock ( zustaendigFuer INTEGER REFERENCES Imker(ImkerNr),
							 Typ VARCHAR(100),
							 StockNr INTEGER,
							 Honigertrag INTEGER NOT NULL,
							 PRIMARY KEY(Typ,StockNr));

CREATE TABLE Brutnest ( liegtInTyp VARCHAR(100),
						  liegtInStockNr INTEGER,
						  NestNr INTEGER,
						  Groesse INTEGER NOT NULL,
						  PRIMARY KEY(liegtInTyp,liegtInStockNr,NestNr),
						  FOREIGN KEY(liegtInTyp,liegtInStockNr) REFERENCES Bienenstock(Typ,StockNr));

CREATE TABLE Koenigin( lebtInTyp VARCHAR(100) NOT NULL,
						 lebtInStockNr INTEGER NOT NULL,
						 Kennzahl INTEGER PRIMARY KEY,
						 Gattung VARCHAR(100) NOT NULL,
						 FOREIGN KEY(lebtInTyp,lebtInStockNr) REFERENCES Bienenstock(Typ,StockNr));

CREATE TABLE Arbeiterin( arbeitetInTyp VARCHAR(100) NOT NULL,
						   arbeitetInStockNr INTEGER NOT NULL,
						   Kennzahl INTEGER PRIMARY KEY,
						   Gattung VARCHAR(100) NOT NULL,
						   FOREIGN KEY(arbeitetInTyp,arbeitetInStockNr) REFERENCES Bienenstock(Typ,StockNr));

CREATE TABLE Feld( Feldkennzahl INTEGER,
					 Ort VARCHAR(100),
					 Gesamtflaeche INTEGER NOT NULL,
					 PRIMARY KEY(Feldkennzahl,Ort));

CREATE TABLE bestaeubt( Kennzahl INTEGER,
						  Feldkennzahl INTEGER,
						  Ort VARCHAR(100),
						  PRIMARY KEY(Kennzahl,Feldkennzahl,Ort),
						  FOREIGN KEY(Kennzahl) REFERENCES Arbeiterin(Kennzahl),
						  FOREIGN KEY(Feldkennzahl,Ort) REFERENCES Feld(Feldkennzahl,Ort));

CREATE TABLE Landwirtschaftsbetrieb( Betriebsform VARCHAR(100),
									   BetriebsNr INTEGER,
									   Name VARCHAR(100),
									   Haupterzeugnis VARCHAR(100),
									   PRIMARY KEY(Betriebsform,BetriebsNr));

CREATE TABLE verwendetVon( Betriebsform VARCHAR(100),
							 BetriebsNr INTEGER,
							 Feldkennzahl INTEGER,
							 Ort VARCHAR(100),
							 Flaechenanteil NUMERIC NOT NULL,
							 PRIMARY KEY(Betriebsform,BetriebsNr,Feldkennzahl,Ort),
							 FOREIGN KEY(Feldkennzahl,Ort) REFERENCES Feld(Feldkennzahl,Ort),
							 FOREIGN KEY(Betriebsform,BetriebsNr) REFERENCES Landwirtschaftsbetrieb(Betriebsform,BetriebsNr));

CREATE TABLE bezahltFuer( ImkerNr INTEGER,
							Betriebsform VARCHAR(100),
							BetriebsNr INTEGER,
							Feldkennzahl INTEGER,
							Ort VARCHAR(100),
							Betrag INTEGER NOT NULL,
							PRIMARY KEY(ImkerNr,Betriebsform,BetriebsNr,Feldkennzahl,Ort),
							FOREIGN KEY(ImkerNr) REFERENCES Imker(ImkerNr),
							FOREIGN KEY(Betriebsform,BetriebsNr) REFERENCES Landwirtschaftsbetrieb(Betriebsform,BetriebsNr),
							FOREIGN KEY(Feldkennzahl,Ort) REFERENCES Feld(Feldkennzahl,Ort));

go
CREATE VIEW Biene (Kennzahl,Gattung) AS 
	SELECT Kennzahl,Gattung 
	FROM ( 
		SELECT  Koenigin.Kennzahl,Koenigin.Gattung  
		FROM Koenigin 
		UNION 
		SELECT  Arbeiterin.Kennzahl,Arbeiterin.Gattung  
		FROM Arbeiterin ) as tmp;
go