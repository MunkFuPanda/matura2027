USE kfz;
GO

-- 1)
DROP TRIGGER IF EXISTS dbo.trg_protocol;
GO

CREATE OR ALTER TRIGGER dbo.trg_protocol
ON Fahrzeug
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Protokoll
        (Tabelle, Aktion, Beschreibung, BenutzerName)
    SELECT
        'Fahrzeug'
        ,'UPDATE'
        ,'Status geändert von ' + ISNULL(d.Status, '(leer)') +
        ' auf '                + ISNULL(i.Status, '(leer)') +
        ' (Kennzeichen: '      + ISNULL(i.Kennzeichen, '(leer)') + ')'
        ,SYSTEM_USER
    FROM
        INSERTED i
        JOIN DELETED d ON i.FahrzeugID = d.FahrzeugID
    WHERE ISNULL(i.Status, '') <> ISNULL(d.Status, '');
END
GO

-- 2)
DROP TRIGGER IF EXISTS dbo.trg_vermietung_status;
GO

CREATE OR ALTER TRIGGER dbo.trg_vermietung_status
ON Vermietung
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM
            INSERTED i
            JOIN Fahrzeug f ON f.FahrzeugID = i.FahrzeugID
        WHERE f.Status <> 'Verfügbar'
    )
    BEGIN
        RAISERROR('Fahrzeug ist nicht verfügbar und kann nicht vermietet werden.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    UPDATE Fahrzeug
    SET    Status = 'Vermietet'
    FROM
        Fahrzeug f
        JOIN INSERTED i ON f.FahrzeugID = i.FahrzeugID;
END
GO

-- 3)
DROP TRIGGER IF EXISTS dbo.trg_fahrzeug_delete;
GO

CREATE OR ALTER TRIGGER dbo.trg_fahrzeug_delete
ON Fahrzeug
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO Protokoll
            (Tabelle, Aktion, Beschreibung, BenutzerName)
        SELECT
            'Fahrzeug'
            ,'DELETE'
            ,'Löschen verhindert – offene Vermietungen vorhanden' +
            ' (FahrzeugID: ' + CAST(d.FahrzeugID AS VARCHAR(10)) +
            ', Kennzeichen: ' + ISNULL(d.Kennzeichen, '(leer)') + ')'
            ,SYSTEM_USER
        FROM
            DELETED d
        WHERE EXISTS (
            SELECT 1
            FROM   Vermietung v
            WHERE  v.FahrzeugID    = d.FahrzeugID
                AND v.Abgeschlossen = 0
        );

        UPDATE Fahrzeug
        SET    Status = 'Gesperrt'
        FROM
            Fahrzeug f
            JOIN DELETED d ON f.FahrzeugID = d.FahrzeugID
        WHERE EXISTS (
            SELECT 1
            FROM   Vermietung v
            WHERE  v.FahrzeugID    = d.FahrzeugID
                AND v.Abgeschlossen = 0
        );

        IF EXISTS (
            SELECT 1
            FROM   DELETED d
            WHERE EXISTS (
                SELECT 1
                FROM   Vermietung v
                WHERE  v.FahrzeugID    = d.FahrzeugID
                    AND v.Abgeschlossen = 0
            )
        )
            RAISERROR('Löschen nicht möglich – offene Vermietungen vorhanden.', 10, 1);

        DELETE FROM Fahrzeug
        WHERE FahrzeugID IN (
            SELECT d.FahrzeugID
            FROM   DELETED d
            WHERE  NOT EXISTS (
                SELECT 1
                FROM   Vermietung v
                WHERE  v.FahrzeugID    = d.FahrzeugID
                    AND v.Abgeschlossen = 0
            )
        );
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- tests 1)

UPDATE Fahrzeug SET Status = 'Gewartet' WHERE FahrzeugID = 1;
SELECT TOP 1 *
FROM   Protokoll
ORDER BY ProtokollID DESC;
GO

DECLARE @cnt_vor INT = (SELECT COUNT(*) FROM Protokoll);
UPDATE Fahrzeug SET Status = 'Gewartet' WHERE FahrzeugID = 1;
DECLARE @cnt_nach INT = (SELECT COUNT(*) FROM Protokoll);
IF @cnt_nach = @cnt_vor
    PRINT 'passt, kein protocol eintrag bei gleichem status';
ELSE
    PRINT 'fail, protocol eintrag sollte nicht sein';
GO

DECLARE @cnt_vor INT = (SELECT COUNT(*) FROM Protokoll);
UPDATE Fahrzeug SET KmStand = 16000 WHERE FahrzeugID = 1;
DECLARE @cnt_nach INT = (SELECT COUNT(*) FROM Protokoll);
IF @cnt_nach = @cnt_vor
    PRINT 'passt, kein protocol eintrag wenn status gleich';
ELSE
    PRINT 'fail, protocol trotz gleicher status';
GO

-- tests 2)

UPDATE Fahrzeug SET Status = 'Verfügbar' WHERE FahrzeugID = 1;
INSERT INTO Vermietung
    (FahrzeugID, KundeID, DatumVon, DatumBis, Gesamtpreis, Abgeschlossen)
VALUES
    (1, 2, '2026-05-01', '2026-05-05', 200.00, 0);
SELECT Status FROM Fahrzeug WHERE FahrzeugID = 1;
GO

BEGIN TRY
    INSERT INTO Vermietung
        (FahrzeugID, KundeID, DatumVon, DatumBis, Gesamtpreis, Abgeschlossen)
    VALUES
        (1, 3, '2026-06-01', '2026-06-05', 150.00, 0);
    PRINT 'fail: insert hätte failen sollen';
END TRY
BEGIN CATCH
    PRINT 'passt, fehler abgefangen: ' + ERROR_MESSAGE();
END CATCH
GO

BEGIN TRY
    INSERT INTO Vermietung
        (FahrzeugID, KundeID, DatumVon, DatumBis, Gesamtpreis, Abgeschlossen)
    VALUES
        (5, 1, '2026-07-01', '2026-07-05', 100.00, 0);
    PRINT 'fail: insert hätte failen sollen';
END TRY
BEGIN CATCH
    PRINT 'passt, fehler abgefangen: ' + ERROR_MESSAGE();
END CATCH
GO

-- tests 3)

DELETE FROM Fahrzeug WHERE FahrzeugID = 2;
SELECT FahrzeugID, Status FROM Fahrzeug WHERE FahrzeugID = 2;
SELECT TOP 1 *
FROM   Protokoll
WHERE  Aktion = 'DELETE'
ORDER BY ProtokollID DESC;
GO

INSERT INTO Fahrzeug
    (Kennzeichen, Marke, Modell, Baujahr, KmStand, Status)
VALUES
    ('WN-TEST1', 'Test', 'Testmodell', 2024, 0, 'Verfügbar');
DECLARE @testID INT = SCOPE_IDENTITY();
DELETE FROM Fahrzeug WHERE FahrzeugID = @testID;
IF NOT EXISTS (SELECT 1 FROM Fahrzeug WHERE FahrzeugID = @testID)
    PRINT 'passt, fahrzeug gelöscht';
ELSE
    PRINT 'fail, fahrzeug noch da';
GO

INSERT INTO Fahrzeug
    (Kennzeichen, Marke, Modell, Baujahr, KmStand, Status)
VALUES
    ('WN-TEST2', 'Test2', 'Testmodell2', 2024, 0, 'Verfügbar');
DECLARE @testID2 INT = SCOPE_IDENTITY();
DELETE FROM Fahrzeug WHERE FahrzeugID IN (2, @testID2);
SELECT FahrzeugID, Kennzeichen, Status FROM Fahrzeug WHERE FahrzeugID = 2;
IF NOT EXISTS (SELECT 1 FROM Fahrzeug WHERE FahrzeugID = @testID2)
    PRINT 'passt, wn-test2 gelöscht';
ELSE
    PRINT 'fail, wn-test2 noch da';
GO

SELECT * FROM Fahrzeug;
SELECT * FROM Protokoll;
GO
