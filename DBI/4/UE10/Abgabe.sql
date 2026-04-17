USE lagerverwaltung;
	/*
Stored Function Lagerwert (@anr int)
R�ckgabewert: Lagerwert (d.h. Preis * Anzahl) des gesuchten Artikels (Parameterwert)
*/
GO

CREATE
	OR

ALTER FUNCTION Lagerwert (@anr INT)
RETURNS DECIMAL
AS
BEGIN
	DECLARE @totalvalue DECIMAL

	SET @totalvalue = (
			SELECT sum(l.stueck)
			FROM lieferung l
			WHERE l.anr = @anr
			) * (
			SELECT preis
			FROM artikel
			WHERE anr = @anr
			)

	RETURN @totalvalue
END
GO

SELECT dbo.Lagerwert(1)
	/*
Stored Function LetzterMonatstag (@datum datetime)
R�ckgabewert: letzter Tag im Monat des gesuchten Datums (Parameterwert)
*/
GO

CREATE
	OR

ALTER FUNCTION LetzterMonatstag (@datum DATETIME)
RETURNS DATE
AS
BEGIN
	DECLARE @Tag DATE

	SET @Tag = eomonth(@datum)

	RETURN @Tag
END
GO

SELECT dbo.LetzterMonatstag('1.1.2020')
	/*
Stored Function Artikelliste (@lnr int)
R�ckgabewert: eine Liste aller Artikel (mit Bezeichnung und St�ckanzahl) des
gesuchten Lagers (Parameterwert)
*/
GO

CREATE
	OR

ALTER FUNCTION Artikelliste (@lnr INT)
RETURNS TABLE
AS
RETURN (
		SELECT a.bezeichnung
			,sum(l.stueck) AS 'St�ckanzahl'
		FROM lieferung l
		JOIN artikel a ON a.anr = l.anr
		WHERE l.lnr = @lnr
		GROUP BY a.bezeichnung
		)
GO

SELECT *
FROM dbo.Artikelliste(1)
	/*
Stored Function Uebersicht( (@artbezeichnung varchar(20))
R�ckgabewert: Liste aller Lager (LNR, Ort) mit der aktuellen St�ckzahl des gesuchten
Artikels (Parameterwert) und der freien Kapazit�t des Lagers
*/
GO

CREATE
	OR

ALTER FUNCTION Uebersicht (@artbezeichnung VARCHAR(20))
RETURNS TABLE
AS
RETURN (
		SELECT ll.lnr
			,ll.ort
			,sum(l.stueck) AS 'St�ckanzahl'
			,(
				SELECT (lll.stueckkap - sum(l.stueck))
				FROM lieferung l
				JOIN lager lll ON ll.lnr = l.lnr
				WHERE lll.lnr = ll.lnr
				GROUP BY lll.stueckkap
				) AS 'freie Kapazit�t'
		FROM artikel a
		JOIN lieferung l ON l.anr = a.anr
		JOIN lager ll ON ll.lnr = l.lnr
		WHERE a.bezeichnung = @artbezeichnung
		GROUP BY ll.lnr
			,ll.ort
			,ll.stueckkap
		)
GO

SELECT *
FROM dbo.Uebersicht('Artikel1')