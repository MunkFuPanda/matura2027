USE Feuerwehr;
GO

-- =====================================================================
-- Aufgabe 1: Skalarwert Funktion udf_bonus
-- =====================================================================
-- Berechnet den Gesamtbonus einer Person anhand ihrer Wettkampf-
-- platzierungen:  1. Platz = 100%, 2. Platz = 50%, 3. Platz = 25%
-- =====================================================================
CREATE OR ALTER FUNCTION udf_bonus(@person_id INT)
RETURNS DECIMAL(10, 2)
AS
BEGIN
    RETURN (
        SELECT
        COALESCE(SUM(
                CASE hp.placing
                    WHEN 1 THEN ct.bonus * 1.00
                    WHEN 2 THEN ct.bonus * 0.50
                    WHEN 3 THEN ct.bonus * 0.25
                    ELSE         0.00
                END
            ), 0.00)
    FROM
        is_troop_member  itm
        JOIN competitve_troop  ct ON itm.troop_id = ct.troop_id
        JOIN has_participated  hp ON ct.troop_id  = hp.troop_id
    WHERE
            itm.pnr = @person_id
    );
END
GO

-- Test: Person 1101 ist in Truppe 6101 (1. Platz, Bonus 10) -> erwartet: 10.00
SELECT
    dbo.udf_bonus(1101) AS bonus_person_1101;
-- Test: Person 1116 ist in Truppe 6102 (2. Platz, Bonus 5)  -> erwartet: 2.50
SELECT
    dbo.udf_bonus(1116) AS bonus_person_1116;
-- Test: Person ohne Wettkampfteilnahme                       -> erwartet: 0.00
SELECT
    dbo.udf_bonus(1113) AS bonus_person_ohne_wettkampf;
GO

-- =====================================================================
-- Aufgabe 2: Table-valued Funktion udf_hierachie_dienstgrad
-- =====================================================================
-- Gibt alle dem uebergebenen Dienstgrad uebergeordneten Dienstgrade
-- aus, inklusive Gehalt und Gehaltsunterschied zum naechst niedrigeren
-- Dienstgrad (0 wenn kein niedrigerer existiert).
-- =====================================================================
CREATE OR ALTER FUNCTION udf_hierachie_dienstgrad(@rank_header VARCHAR(30))
RETURNS TABLE
AS
RETURN (
    WITH
    rang_hierarchie
    AS
    (
        -- Anker: direkt uebergeordneter Dienstgrad
                    SELECT
                r.rank_header
                ,r.rank_base_content
                ,r.pers_rank_rank_header
                ,r.salary
            FROM
                pers_rank r
            WHERE
            r.pers_rank_rank_header = @rank_header

        UNION ALL

            -- Rekursion: weitere uebergeordnete Dienstgrade
            SELECT
                r.rank_header
                ,r.rank_base_content
                ,r.pers_rank_rank_header
                ,r.salary
            FROM
                pers_rank       r
                JOIN rang_hierarchie rh ON r.pers_rank_rank_header = rh.rank_header
    )
    SELECT
    rh.rank_header                                  AS rank_id
    ,rh.rank_base_content                            AS beschreibung
    ,rh.salary                                       AS gehalt
    ,COALESCE(rh.salary - pr.salary, 0)              AS gehaltsunterschied
FROM
    rang_hierarchie  rh
    LEFT JOIN pers_rank pr ON rh.pers_rank_rank_header = pr.rank_header
);
GO

-- Test: alle Dienstgrade ueber Hauptfeuerwehrmann
SELECT
    *
FROM
    dbo.udf_hierachie_dienstgrad('Hauptfeuerwehrmann');
GO

-- =====================================================================
-- Aufgabe 3: Stored Procedure stp_erhoehe_dienstgrad
-- =====================================================================
-- Stuft alle Personen um einen Dienstgrad hoch, die sich seit mindestens
-- @jahre Jahren in ihrem aktuellen Dienstgrad befinden.
-- Personen im hoechsten Dienstgrad bleiben unveraendert.
-- Negativer/Null-Parameter wird mit Fehlermeldung abgebrochen.
-- =====================================================================
CREATE OR ALTER PROCEDURE stp_erhoehe_dienstgrad
    @jahre INT
AS
BEGIN
    -- Parameterpruefung
    IF @jahre <= 0
    BEGIN
        RAISERROR('Der Parameter @jahre muss ein positiver ganzzahliger Wert sein.', 16, 1);
        RETURN;
    END;

    DECLARE @aufgestiegen TABLE (
        pnr              INT
        ,first_name       VARCHAR(30)
        ,last_name        VARCHAR(30)
        ,alter_dienstgrad VARCHAR(30)
        ,neuer_dienstgrad VARCHAR(30)
    );

    -- Hochstufung: JOIN auf naechst hoeherem Dienstgrad
    -- (jener Rang, dessen pers_rank_rank_header = aktueller Rang der Person)
    UPDATE p
    SET
        p.rank_header = naechster_rang.rank_header,
        p.rankdate    = GETDATE()
    OUTPUT
        inserted.pnr,
        inserted.first_name,
        inserted.last_name,
        deleted.rank_header,
        inserted.rank_header
    INTO @aufgestiegen
    FROM
        person      p
        JOIN pers_rank aktueller_rang ON p.rank_header                    = aktueller_rang.rank_header
        JOIN pers_rank naechster_rang ON naechster_rang.pers_rank_rank_header = aktueller_rang.rank_header
    WHERE
        DATEDIFF(YEAR, p.rankdate, GETDATE()) >= @jahre;

    -- Ergebnis ausgeben
    SELECT
        a.first_name        AS vorname
        ,a.last_name         AS nachname
        ,a.alter_dienstgrad  AS alter_dienstgrad
        ,a.neuer_dienstgrad  AS neuer_dienstgrad
    FROM
        @aufgestiegen a;
END;
GO

-- Test
BEGIN TRANSACTION
EXEC stp_erhoehe_dienstgrad @jahre = 5;
ROLLBACK
GO
