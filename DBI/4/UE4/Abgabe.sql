create database ueberlagerte_entitaeten;
go
use ueberlagerte_entitaeten;
go

-- DATENBANK-IMPLEMENTIERUNG - MS SQL SERVER
-- Thema: Spieler/Tennis/Fußball
-- Ausprägung 1: partiell | Ausprägung 2: disjunkt
-- Implementierungsvariante: 2

-- 1. ERD mit Attributen
/* schöne AI grafik hier  ;)
┌─────────────────────────────────┐
│         SPIELER                 │
│  (Supertyp)                     │
├─────────────────────────────────┤
│  PNr (PK)                       │
│  Name                           │
│  Geburtsdatum                   │
│  Nationalität                   │
└─────────────────────────────────┘
              │
              │ ISA (partiell, disjunkt)
              │
      ┌───────┴────────┐
      │                │
┌─────▼──────────┐  ┌──▼────────────────┐
│ TENNISSPIELER  │  │ FUSSBALLSPIELER   │
│  (Subtyp)      │  │  (Subtyp)         │
├────────────────┤  ├───────────────────┤
│ PNr (PK, FK)   │  │ PNr (PK, FK)      │
│ Weltrangliste  │  │ Verein            │
│ Turniersiege   │  │ Position          │
└────────────────┘  └───────────────────┘

Notation: 
- ISA-Beziehung mit partieller und disjunkter Spezialisierung
- Jeder Sub-Typ benötigt neben dem Primary Key zwei weitere Attribute
*/

-- 2a. VARIANTE 1A

CREATE TABLE Spieler_1A (
    PNr INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Geburtsdatum DATE,
    Nationalitaet NVARCHAR(50)
);

CREATE TABLE Tennisspieler_1A (
    PNr INT PRIMARY KEY,
    Weltranglistenplatz INT,
    Turniersieg_Anzahl INT,
    CONSTRAINT FK_Tennis_Spieler FOREIGN KEY (PNr) 
        REFERENCES Spieler_1A(PNr) ON DELETE CASCADE
);

CREATE TABLE Fussballspieler_1A (
    PNr INT PRIMARY KEY,
    Verein NVARCHAR(100),
    Position NVARCHAR(50),
    CONSTRAINT FK_Fussball_Spieler FOREIGN KEY (PNr) 
        REFERENCES Spieler_1A(PNr) ON DELETE CASCADE
);


-- 2b. NACH ANGABE ENTWEDER 1B/2/3

CREATE TABLE Tennisspieler_2 (
    PNr INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Geburtsdatum DATE,
    Nationalitaet NVARCHAR(50),
    Weltranglistenplatz INT,
    Turniersieg_Anzahl INT
);

CREATE TABLE Fussballspieler_2 (
    PNr INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Geburtsdatum DATE,
    Nationalitaet NVARCHAR(50),
    Verein NVARCHAR(100),
    Position NVARCHAR(50)
);


-- 2c. DISKUSSION ZU REDUNDANZEN UND SPEICHERBEDARF
-- Variante 1A hat keine Redundanzen und weniger Speicherbedarf, unterstützt
-- partiell gut; Variante 2 ist schneller bei Subtyp-Abfragen aber mit
-- Redundanz der Supertyp-Attribute und kann partiell nicht abbilden.


-- 3a. VARIANTE 1A

INSERT INTO Spieler_1A (PNr, Name, Geburtsdatum, Nationalitaet) VALUES
(1, N'Roger Federer', '1981-08-08', N'Schweiz'),
(2, N'Rafael Nadal', '1986-06-03', N'Spanien'),
(3, N'Novak Djokovic', '1987-05-22', N'Serbien'),
(4, N'Lionel Messi', '1987-06-24', N'Argentinien'),
(5, N'Cristiano Ronaldo', '1985-02-05', N'Portugal'),
(6, N'Robert Lewandowski', '1988-08-21', N'Polen'),
(7, N'Max Mustermann', '1990-01-01', N'Deutschland');

INSERT INTO Tennisspieler_1A (PNr, Weltranglistenplatz, Turniersieg_Anzahl) VALUES
(1, 3, 103),
(2, 2, 92),
(3, 1, 98);

INSERT INTO Fussballspieler_1A (PNr, Verein, Position) VALUES
(4, N'Inter Miami', N'Sturm'),
(5, N'Al-Nassr', N'Sturm'),
(6, N'FC Barcelona', N'Sturm');


-- 3b. NACH ANGABE ENTWEDER 1B/2/3

INSERT INTO Tennisspieler_2 (PNr, Name, Geburtsdatum, Nationalitaet, 
                              Weltranglistenplatz, Turniersieg_Anzahl) VALUES
(1, N'Roger Federer', '1981-08-08', N'Schweiz', 3, 103),
(2, N'Rafael Nadal', '1986-06-03', N'Spanien', 2, 92),
(3, N'Novak Djokovic', '1987-05-22', N'Serbien', 1, 98);

INSERT INTO Fussballspieler_2 (PNr, Name, Geburtsdatum, Nationalitaet, 
                                Verein, Position) VALUES
(4, N'Lionel Messi', '1987-06-24', N'Argentinien', N'Inter Miami', N'Sturm'),
(5, N'Cristiano Ronaldo', '1985-02-05', N'Portugal', N'Al-Nassr', N'Sturm'),
(6, N'Robert Lewandowski', '1988-08-21', N'Polen', N'FC Barcelona', N'Sturm');


-- 4a. Alle Informationen zu einem Subtyp

-- i. FÜR VARIANTE 1A
SELECT 
    s.PNr,
    s.Name,
    s.Geburtsdatum,
    s.Nationalitaet,
    t.Weltranglistenplatz,
    t.Turniersieg_Anzahl
FROM Spieler_1A s
INNER JOIN Tennisspieler_1A t ON s.PNr = t.PNr
ORDER BY t.Weltranglistenplatz;


-- ii. NACH ANGABE ENTWEDER 1B/2/3
SELECT 
    PNr,
    Name,
    Geburtsdatum,
    Nationalitaet,
    Weltranglistenplatz,
    Turniersieg_Anzahl
FROM Tennisspieler_2
ORDER BY Weltranglistenplatz;


-- iii. DISKUSSION ZU PERFORMANCE/KOMPLEXITÄT
-- Variante 2 ist schneller weil kein JOIN nötig ist, Variante 1A braucht JOIN
-- aber ist flexibler für übergreifende Abfragen.


-- 4b. Alle Infos aller Entitäten

-- i. FÜR VARIANTE 1A
SELECT 
    s.PNr,
    s.Name,
    s.Geburtsdatum,
    s.Nationalitaet,
    t.Weltranglistenplatz,
    t.Turniersieg_Anzahl,
    f.Verein,
    f.Position,
    CASE 
        WHEN t.PNr IS NOT NULL THEN 'Tennisspieler'
        WHEN f.PNr IS NOT NULL THEN 'Fußballspieler'
        ELSE 'Nur Spieler'
    END AS Spielertyp
FROM Spieler_1A s
LEFT JOIN Tennisspieler_1A t ON s.PNr = t.PNr
LEFT JOIN Fussballspieler_1A f ON s.PNr = f.PNr
ORDER BY s.Name;


-- ii. NACH ANGABE ENTWEDER 1B/2/3
SELECT 
    PNr,
    Name,
    Geburtsdatum,
    Nationalitaet,
    Weltranglistenplatz,
    Turniersieg_Anzahl,
    NULL AS Verein,
    NULL AS Position,
    'Tennisspieler' AS Spielertyp
FROM Tennisspieler_2

UNION ALL

SELECT 
    PNr,
    Name,
    Geburtsdatum,
    Nationalitaet,
    NULL AS Weltranglistenplatz,
    NULL AS Turniersieg_Anzahl,
    Verein,
    Position,
    'Fußballspieler' AS Spielertyp
FROM Fussballspieler_2

ORDER BY Name;


-- iii. DISKUSSION ZU PERFORMANCE/KOMPLEXITÄT
-- Variante 1A ist besser weil sie auch Spieler ohne Subtyp zeigt (partiell)
-- und strukturell klarer ist, Variante 2 braucht UNION und kann partiell nicht
-- abbilden.