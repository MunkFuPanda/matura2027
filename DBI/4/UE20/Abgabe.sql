-- Datenbank erstellen
IF EXISTS (SELECT
    name
FROM
    sys.databases
WHERE name = 'RAID5')
    DROP DATABASE RAID5;
GO
CREATE DATABASE RAID5;
GO
USE RAID5;
GO

-- 1) Tabelle DiskArray erstellen
CREATE TABLE DiskArray
(
    BlockNo    INT     PRIMARY KEY
    ,Disk1      TINYINT
    ,Disk2      TINYINT
    ,Disk3      TINYINT
    ,ParityDisk TINYINT
);
GO

-- 2) Datenbefüllung mit ASCII()
--    Disk1: "Wichtig"  Disk2: "Firmen"  Disk3: "Daten"
--    Leere Blöcke = Leerzeichen (ASCII 32)
--    ParityDisk erstmal NULL
INSERT INTO DiskArray
    (BlockNo, Disk1, Disk2, Disk3, ParityDisk)
VALUES
    (1 ,ASCII('W') ,ASCII('F') ,ASCII('D') ,NULL)
    ,(2 ,ASCII('i') ,ASCII('i') ,ASCII('a') ,NULL)
    ,(3 ,ASCII('c') ,ASCII('r') ,ASCII('t') ,NULL)
    ,(4 ,ASCII('h') ,ASCII('m') ,ASCII('e') ,NULL)
    ,(5 ,ASCII('t') ,ASCII('e') ,ASCII('n') ,NULL)
    ,(6 ,ASCII('i') ,ASCII('n') ,ASCII(' ') ,NULL)
    ,(7 ,ASCII('g') ,ASCII(' ') ,ASCII(' ') ,NULL);
GO

-- 3) ParityDisk mit XOR befüllen (^ = XOR in SQL)
UPDATE DiskArray
SET ParityDisk = Disk1 ^ Disk2 ^ Disk3;
GO

-- 4) Inhalt anzeigen – Nutzdaten als Zeichen mit CHAR()
SELECT
    BlockNo                  AS BlockNo
    ,Disk1                    AS Disk1
    ,CHAR(Disk1)              AS Disk1Char
    ,Disk2                    AS Disk2
    ,CHAR(Disk2)              AS Disk2Char
    ,Disk3                    AS Disk3
    ,CHAR(Disk3)              AS Disk3Char
    ,ParityDisk               AS ParityDisk
FROM
    DiskArray;
GO

-- 5) Disk1 zerstören – alle Werte auf NULL setzen
UPDATE DiskArray
SET Disk1 = NULL;
GO

SELECT
    BlockNo                  AS BlockNo
    ,Disk1                    AS Disk1
    ,Disk2                    AS Disk2
    ,CHAR(Disk2)              AS Disk2Char
    ,Disk3                    AS Disk3
    ,CHAR(Disk3)              AS Disk3Char
    ,ParityDisk               AS ParityDisk
FROM
    DiskArray;
GO

-- 6) Disk1 durch XOR wiederherstellen: Disk1 = Disk2 ^ Disk3 ^ ParityDisk
UPDATE DiskArray
SET Disk1 = Disk2 ^ Disk3 ^ ParityDisk;
GO

SELECT
    BlockNo                  AS BlockNo
    ,Disk1                    AS Disk1
    ,CHAR(Disk1)              AS Disk1Char
    ,Disk2                    AS Disk2
    ,CHAR(Disk2)              AS Disk2Char
    ,Disk3                    AS Disk3
    ,CHAR(Disk3)              AS Disk3Char
    ,ParityDisk               AS ParityDisk
FROM
    DiskArray;
GO

-- 7) Beweis: Es ist egal, welche Disk zerstört wird
--    Disk2 zerstören und wiederherstellen
UPDATE DiskArray
SET Disk2 = NULL;
GO

SELECT
    BlockNo                  AS BlockNo
    ,Disk1                    AS Disk1
    ,CHAR(Disk1)              AS Disk1Char
    ,Disk2                    AS Disk2
    ,Disk3                    AS Disk3
    ,CHAR(Disk3)              AS Disk3Char
    ,ParityDisk               AS ParityDisk
FROM
    DiskArray;
GO

-- Disk2 = Disk1 ^ Disk3 ^ ParityDisk
UPDATE DiskArray
SET Disk2 = Disk1 ^ Disk3 ^ ParityDisk;
GO

SELECT
    BlockNo                  AS BlockNo
    ,Disk1                    AS Disk1
    ,CHAR(Disk1)              AS Disk1Char
    ,Disk2                    AS Disk2
    ,CHAR(Disk2)              AS Disk2Char
    ,Disk3                    AS Disk3
    ,CHAR(Disk3)              AS Disk3Char
    ,ParityDisk               AS ParityDisk
FROM
    DiskArray;
GO

-- 8) Disk3 zerstören und wiederherstellen
UPDATE DiskArray
SET Disk3 = NULL;
GO

SELECT
    BlockNo                  AS BlockNo
    ,Disk1                    AS Disk1
    ,CHAR(Disk1)              AS Disk1Char
    ,Disk2                    AS Disk2
    ,CHAR(Disk2)              AS Disk2Char
    ,Disk3                    AS Disk3
    ,ParityDisk               AS ParityDisk
FROM
    DiskArray;
GO

-- Disk3 = Disk1 ^ Disk2 ^ ParityDisk
UPDATE DiskArray
SET Disk3 = Disk1 ^ Disk2 ^ ParityDisk;
GO

SELECT
    BlockNo                  AS BlockNo
    ,Disk1                    AS Disk1
    ,CHAR(Disk1)              AS Disk1Char
    ,Disk2                    AS Disk2
    ,CHAR(Disk2)              AS Disk2Char
    ,Disk3                    AS Disk3
    ,CHAR(Disk3)              AS Disk3Char
    ,ParityDisk               AS ParityDisk
FROM
    DiskArray;
GO

-- 9) ParityDisk zerstören und wiederherstellen
UPDATE DiskArray
SET ParityDisk = NULL;
GO

SELECT
    BlockNo                  AS BlockNo
    ,Disk1                    AS Disk1
    ,CHAR(Disk1)              AS Disk1Char
    ,Disk2                    AS Disk2
    ,CHAR(Disk2)              AS Disk2Char
    ,Disk3                    AS Disk3
    ,CHAR(Disk3)              AS Disk3Char
    ,ParityDisk               AS ParityDisk
FROM
    DiskArray;
GO

-- ParityDisk = Disk1 ^ Disk2 ^ Disk3
UPDATE DiskArray
SET ParityDisk = Disk1 ^ Disk2 ^ Disk3;
GO

SELECT
    BlockNo                  AS BlockNo
    ,Disk1                    AS Disk1
    ,CHAR(Disk1)              AS Disk1Char
    ,Disk2                    AS Disk2
    ,CHAR(Disk2)              AS Disk2Char
    ,Disk3                    AS Disk3
    ,CHAR(Disk3)              AS Disk3Char
    ,ParityDisk               AS ParityDisk
FROM
    DiskArray;
GO
