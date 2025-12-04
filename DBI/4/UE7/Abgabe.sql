USE lagerverwaltung;
-- Anlieferung
DROP PROCEDURE IF EXISTS dbo.stpanlieferung
SELECT *
FROM lager
GO CREATE
   OR ALTER PROCEDURE dbo.stpanlieferung @anr INT,
   @datum DATETIME,
   @stueck INT AS
   BEGIN DECLARE @currentlnr INT = 1;
       DECLARE @currentlfnd INT = 0;
       DECLARE @firsttime INT = 0;
       DECLARE @maxkap INT = 0 DECLARE @howmuchnow INT = 0 WHILE ( @currentlnr < 5 )
       BEGIN
           SET @currentlfnd = ( SELECT MAX(lfndnr) FROM lieferung WHERE lnr = @currentlnr ) + 1
           SET @maxkap = ( SELECT stueckkap FROM lager WHERE lnr = @currentlnr )
           SET @howmuchnow = ( SELECT SUM(li.stueck)
                               FROM lieferung li
                                    JOIN lager la ON la.lnr = li.lnr
                               WHERE la.lnr = @currentlnr ) DECLARE @add INT = 0 IF ( @maxkap > @howmuchnow + @stueck ) BEGIN SET @add = @stueck END
           ELSE
                BEGIN IF ( @maxkap - @howmuchnow < @stueck ) BEGIN SET @add = @maxkap - @howmuchnow END
                    ELSE BEGIN SET @add = @stueck END
                END
           SET @stueck = @stueck - @add
           INSERT INTO LIEFERUNG(lnr, lfndnr, anr, datum, stueck)
           VALUES ( @currentlnr, @currentlfnd, @anr, @datum, @stueck ) IF ( @stueck <= 0 ) BEGIN BREAK END
           SET @currentlnr += 1
       END IF ( @stueck > 0 ) BEGIN PRINT 'error' END
       ELSE BEGIN ( SELECT lnr, stueck FROM lieferung WHERE anr = @anr ) END
   END
GO EXEC dbo.stpanlieferung 3,
   '2012-01-01 00:00:00.000',
   400
SELECT *
FROM lieferung;
-- Entnahme
     
-- LagerLoeschen
GO CREATE
   OR ALTER PROCEDURE dbo.stplagerloeschen @lnr INT AS
   BEGIN
       DELETE FROM lieferung
       WHERE lnr = @lnr
       DELETE FROM lager
       WHERE lnr = @lnr
   END
GO
-- Bestand
GO CREATE
   OR ALTER PROCEDURE dbo.stpbestand AS
   BEGIN
       SET nocount ON DECLARE @lnr INT;
                      DECLARE @lfndnr INT;
                      DECLARE @anr INT;
                      DECLARE @sum INT = 0;
                      DECLARE @bezeichnung VARCHAR ( 30 );
                      DECLARE @ortaktuell VARCHAR ( 30 );
                      DECLARE @ort VARCHAR ( 30 );
                      DECLARE @datum DATETIME;
                      DECLARE @stueck INT;
                      DECLARE @ergebnis TABLE ( bezeichnung VARCHAR ( 50 ), ort VARCHAR ( 50 ), datum DATE, stueck INT );
                      DECLARE crs_lieferung CURSOR FOR
       SELECT lnr,
              lfndnr
       FROM lieferung
       ORDER BY lnr,
                anr OPEN crs_lieferung;
                FETCH crs_lieferung
       INTO @lnr,
            @lfndnr;
            WHILE @@fetch_status = 0
            BEGIN FETCH crs_lieferung
                INTO @lnr,
                     @lfndnr IF ( @anr != ( SELECT anr FROM lieferung WHERE lnr = @lnr AND lfndnr = @lfndnr ) ) BEGIN INSERT INTO @ergebnis VALUES ( 'Summe', '', NULL, @sum ) SET @sum = 0; END IF ( @ortaktuell = ( SELECT ort FROM lager WHERE lnr = @lnr ) ) BEGIN SET @ort = '' END
                ELSE
                     BEGIN
                         SET @ort = ( SELECT ort FROM lager WHERE lnr = @lnr )
                         SET @ortaktuell = @ort
                     END
                SET @anr = ( SELECT anr FROM lieferung WHERE lnr = @lnr AND lfndnr = @lfndnr );
                SET @bezeichnung = ( SELECT bezeichnung FROM artikel WHERE anr = @anr );
                SET @datum = ( SELECT datum FROM lieferung WHERE lnr = @lnr AND lfndnr = @lfndnr )
                SET @stueck = ( SELECT stueck FROM lieferung WHERE lnr = @lnr AND lfndnr = @lfndnr )
                SET @sum += @stueck
                INSERT INTO @ergebnis
                VALUES ( @bezeichnung, @ort, ( SELECT CONVERT(VARCHAR( 10 ), @datum, 104) ), @stueck )
            END
       SELECT *
       FROM @ergebnis;
            CLOSE crs_lieferung;
            DEALLOCATE crs_lieferung;
   END
GO
GO EXEC dbo.stpbestand;
-- Lagerbestand
GO CREATE
   OR ALTER PROCEDURE dbo.stplagerbestand @lnr INT AS
   BEGIN
       SET nocount ON
       SELECT *
       FROM lager
       WHERE lnr = @lnr;
       SELECT a.bezeichnung,
              SUM(stueck) 'Lagerbestand'
       FROM lieferung l
            JOIN artikel a ON a.anr = l.anr
       WHERE lnr = @lnr
       GROUP BY a.bezeichnung;
   END
GO EXEC dbo.stplagerbestand 1;