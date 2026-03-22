DROP TRIGGER IF EXISTS trg_titles_instead_of;
GO
DELETE FROM TitlesAuthors WHERE ISBN10     LIKE 'TSTBOOK%';
DELETE FROM Titles         WHERE ISBN10     LIKE 'TSTBOOK%';
DELETE FROM Authors        WHERE AuthorCode >= 101;
GO
DROP TRIGGER IF EXISTS trg_titlesauthors_max3;
DROP TRIGGER IF EXISTS trg_titles_price_no_decrease;
DROP TRIGGER IF EXISTS trg_titles_no_delete;
DROP TRIGGER IF EXISTS trg_authors_no_delete;
GO

CREATE OR ALTER TRIGGER trg_authors_no_delete
ON Authors
AFTER DELETE
AS
BEGIN
    IF EXISTS (
        SELECT
        1
    FROM
        TitlesAuthors ta
        JOIN deleted d ON ta.AuthorCode = d.AuthorCode
    )
    BEGIN
        RAISERROR('Autor kann nicht geloescht werden: Es existieren noch verknuepfte Detaildatensaetze in TitlesAuthors.', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;
GO

CREATE OR ALTER TRIGGER trg_titles_no_delete
ON Titles
AFTER DELETE
AS
BEGIN
    IF EXISTS (
        SELECT
        1
    FROM
        TitlesAuthors ta
        JOIN deleted d ON ta.ISBN10 = d.ISBN10
    )
    BEGIN
        RAISERROR('Titel kann nicht geloescht werden: Es existieren noch verknuepfte Detaildatensaetze in TitlesAuthors.', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;
GO

INSERT INTO Authors
    (AuthorCode, AuthorName)
VALUES
    (101 ,'Test Autor A')
    ,(102 ,'Test Autor B')
    ,(103 ,'Test Autor C')
    ,(104 ,'Test Autor D');
GO

INSERT INTO Titles
    (ISBN10, Title, Language, price)
VALUES
    ('TSTBOOK001' ,'Test Buch 1' ,'de' ,20)
    ,('TSTBOOK002' ,'Test Buch 2' ,'de' ,15);
GO

INSERT INTO TitlesAuthors
    (ISBN10, AuthorCode)
VALUES
    ('TSTBOOK001' ,101)
    ,('TSTBOOK001' ,102)
    ,('TSTBOOK001' ,103);
GO

ALTER TABLE TitlesAuthors NOCHECK CONSTRAINT ALL;
GO
DELETE FROM Authors WHERE AuthorCode = 101;
GO
ALTER TABLE TitlesAuthors CHECK CONSTRAINT ALL;
GO
DELETE FROM Authors WHERE AuthorCode = 104;
GO

ALTER TABLE TitlesAuthors NOCHECK CONSTRAINT ALL;
GO
DELETE FROM Titles WHERE ISBN10 = 'TSTBOOK001';
GO
ALTER TABLE TitlesAuthors CHECK CONSTRAINT ALL;
GO

-- Warum kein ROLLBACK nötig?
-- Ein INSTEAD OF trigger ersetzt die Operation, somit werden die Daten gar nicht eingefügt
CREATE OR ALTER TRIGGER trg_titles_instead_of
ON Titles
INSTEAD OF INSERT, DELETE
AS
BEGIN
    RAISERROR('Eintraege in Titles koennen weder eingefuegt noch geloescht werden.', 16, 1);
END;
GO

INSERT INTO Titles
    (ISBN10, Title, Language, price)
VALUES
    ('TSTBOOK003' ,'Gesperrter Titel' ,'de' ,10);
GO
DELETE FROM Titles WHERE ISBN10 = 'TSTBOOK002';
GO

CREATE OR ALTER TRIGGER trg_titles_price_no_decrease
ON Titles
AFTER UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT
        1
    FROM
        inserted i
        JOIN deleted d ON i.ISBN10 = d.ISBN10
    WHERE  i.price < d.price
    )
    BEGIN
        RAISERROR('Der Preis eines Titels darf nicht gesenkt werden.', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;
GO

UPDATE Titles SET price = 10 WHERE ISBN10 = 'TSTBOOK001';
GO
UPDATE Titles SET price = 25 WHERE ISBN10 = 'TSTBOOK001';
GO

CREATE OR ALTER TRIGGER trg_titlesauthors_max3
ON TitlesAuthors
AFTER INSERT
AS
BEGIN
    IF EXISTS (
        SELECT
        ISBN10
    FROM
        TitlesAuthors
    WHERE   ISBN10 IN (SELECT
        ISBN10
    FROM
        inserted)
    GROUP BY ISBN10
    HAVING  COUNT(*) > 3
    )
    BEGIN
        RAISERROR('Einem Titel duerfen maximal 3 Autoren zugeordnet sein.', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;
GO

INSERT INTO TitlesAuthors
    (ISBN10, AuthorCode)
VALUES
    ('TSTBOOK001' ,4);
GO
INSERT INTO TitlesAuthors
    (ISBN10, AuthorCode)
VALUES
    ('TSTBOOK002' ,101);
GO
