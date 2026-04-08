USE AirportDB;

-- 1.)

GO
CREATE OR ALTER FUNCTION dbo.udffindData(@startDate VARCHAR(50), @endDate VARCHAR(50))
returns TABLE 
AS
RETURN (
	SELECT
	f.Airline_code
	,Ai.Airline_Name
	,A.Aircraft_code
	,f.Flight_number
	,FlA.Leg_number
	,PA.Passport_number
	,PA.First_name
	,PA.Minit
	,PA.Last_name
	,PA.Air_ticket_number
FROM
	Flight_shedule_date fd
	JOIN flight f ON f.Flight_number = fd.Flight_number
	JOIN Airline Ai ON Ai.Airline_code = f.Airline_code
	JOIN Aircraft A ON A.Airline_code = f.Airline_code
	JOIN Flight_leg_A FlA ON FlA.Flight_number = f.Flight_number
	JOIN Passenger_A PA ON FlA.Leg_number = PA.Leg_number
WHERE fd.Date BETWEEN @startDate AND @endDate
	)
GO

SELECT
	*
FROM
	udffindData('2018-12-21', '2018-12-21')

-- 2.)

GO
CREATE OR ALTER FUNCTION dbo.statusInfo(@status VARCHAR(50), @date VARCHAR(50))
returns TABLE
AS
RETURN (
	SELECT
	FlB.Status
	,FlB.Remark
	,FlB.Arrival_teminal_number
	,FlA.Staff_ID
	,FlA.Leg_number
	,FlA.Flight_number
	,PA.Passport_number
	,concat(PA.First_Name, ' ', PA.Last_name) AS Passenger_name
	,PC.Passenger_catogary
	,PR.Requirement
	,fd.Date
FROM
	Flight_shedule_date fd
	JOIN flight f ON f.Flight_number = fd.Flight_number
	JOIN Airline Ai ON Ai.Airline_code = f.Airline_code
	JOIN Aircraft A ON A.Airline_code = f.Airline_code
	JOIN Flight_leg_A FlA ON FlA.Flight_number = f.Flight_number
	JOIN Passenger_A PA ON FlA.Leg_number = PA.Leg_number
	JOIN Flight_leg_B FlB ON FlB.Arrival_teminal_number = FlA.Arrival_teminal_number
	JOIN Passenger_catogary PC ON PC.Passport_number = PA.Passport_number
	JOIN Passenger_requirements PR ON PR.Passport_number = PA.Passport_number
WHERE fd.Date = @date
	AND flB.Status = @status
	)
GO

SELECT
	*
FROM
	dbo.statusInfo('Canceled', '2018-12-21')	

-- 3.)


GO
CREATE OR ALTER PROCEDURE dbo.stp_passExpire
	@PassportNumber VARCHAR(50)
AS
BEGIN
	IF ((SELECT
		Date_of_Expire
	FROM
		Passenger_A
	WHERE Passport_number = @PassportNumber) < cast(GETDATE() AS DATE))
	BEGIN
		PRINT 'Passengers Passport has been expired'
	END
	ELSE IF EXISTS (SELECT
		Date_of_Expire
	FROM
		Passenger_A
	WHERE Passport_number = @PassportNumber)
	BEGIN
		PRINT 'This Passenger has a valid Passport'
	END
	ELSE
	BEGIN
		PRINT 'This Passport Number does not exist'
	END
END
GO

EXEC dbo.stp_passExpire 'M100123155'
EXEC dbo.stp_passExpire 'M100123464'