USE fluege;

-- 1.
GO
CREATE OR ALTER FUNCTION Flugminuten (@FzNr INT)
RETURNS INT
AS
BEGIN
	RETURN (
			SELECT sum(datediff(s, startzeit, landezeit))
			FROM fliegt
			WHERE fznr = @FzNr
			)
END
GO

SELECT dbo.Flugminuten(1);
SELECT dbo.Flugminuten(2);
SELECT dbo.Flugminuten(3);

-- 2.
GO
CREATE OR ALTER FUNCTION FluegeProTyp ()
RETURNS TABLE
AS
RETURN (
		SELECT pnname
			,pvname
			,ftype.bezeichnung
			,count(fliegt.fnr) AS anzahl
		FROM pilot
		INNER JOIN fliegt ON fliegt.pnr = pilot.pnr
		INNER JOIN flugzeug ON fliegt.fznr = flugzeug.fznr
		INNER JOIN ftype ON ftype.tnr = flugzeug.tnr
		GROUP BY pnname
			,pvname
			,ftype.bezeichnung
		)
GO

SELECT *
FROM dbo.FluegeProTyp();

--3.
GO
CREATE OR ALTER FUNCTION Flugaufkommen (@ANr INTEGER = - 1)
RETURNS TABLE
AS
RETURN (
		SELECT Bezeichnung
			,(
				SELECT count(*)
				FROM flug
				WHERE splatz = fhnr
				) AS starting
			,(
				SELECT count(*)
				FROM flug
				WHERE lplatz = fhnr
				) AS landing
		FROM flughafen
		WHERE @ANr = - 1
			OR @ANr = flughafen.fhnr
		)
GO

SELECT * from Flugaufkommen(default)
SELECT * from Flugaufkommen(2)