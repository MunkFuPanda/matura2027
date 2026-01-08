USE tankstelle;
GO

CREATE OR ALTER PROCEDURE dbo.stpBetrag
(
    @ZapfsaeulenID     INT,
    @LiterMenge        INT,
    @Gesamtbetrag      FLOAT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @VerfuegbareMenge INT =
    (
        SELECT aktMengeL
        FROM Zapfsaeule
        WHERE ZNr = @ZapfsaeulenID
    );

    IF (@VerfuegbareMenge IS NULL)
    BEGIN
        RAISERROR('Ungültige Zapfsäulen-Nummer', 11, 1);
        RETURN;
    END;

    IF (@LiterMenge < 0 OR @LiterMenge > @VerfuegbareMenge)
    BEGIN
        RAISERROR('Menge ist kleiner 0 oder größer als vorhandene Menge', 11, 2);
        RETURN;
    END;

    DECLARE @PreisProLiter FLOAT =
    (
        SELECT Preis
        FROM Tagespreis
        WHERE KName =
        (
            SELECT KName
            FROM Zapfsaeule
            WHERE ZNr = @ZapfsaeulenID
        )
        AND Tagesdatum = CONVERT(DATETIME, '1.10.2009', 104)
    );

    SET @Gesamtbetrag = @LiterMenge * @PreisProLiter;

    DECLARE @NeueVerkaufsNr INT =
    (
        SELECT MAX(VNr) + 1
        FROM Verkauf
    );

    INSERT INTO Verkauf (VNr, MengeL, Verkaufszeitpunkt)
    VALUES (@NeueVerkaufsNr, @LiterMenge, CONVERT(DATETIME, '1.10.2009', 104));

    UPDATE Zapfsaeule
    SET aktMengeL = aktMengeL - @LiterMenge
    WHERE ZNr = @ZapfsaeulenID;
END;
GO


DECLARE @ResultBetrag FLOAT;

EXEC dbo.stpBetrag
     @ZapfsaeulenID = 1,
     @LiterMenge = 999,
     @Gesamtbetrag = @ResultBetrag OUTPUT;

SELECT @ResultBetrag AS Betrag;
GO


CREATE OR ALTER PROCEDURE dbo.stpBestellung
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ZNr,
        aktMengeL,
        maxMengeL,
        maxMengeL - aktMengeL AS FehlendeMenge
    FROM Zapfsaeule
    WHERE aktMengeL < maxMengeL * 0.1;
END;
GO

EXEC dbo.stpBestellung;
