USE airport;

GO
CREATE OR ALTER FUNCTION udffindData(
	@startDate DATE,
	@endDate DATE
) returns TABLE
AS RETURN
(
	SELECT
	Airline.Airline_code
	,Airline.Airline_name
	,Aircraft.Aircraft_code
	,Flight.Flight_number
	,Flight_leg_A.Leg_number
	,Passenger_A.Passport_number
	,Passenger_A.First_name
	,Passenger_A.Minit
	,Passenger_A.Last_name
	,Passenger_A.Air_ticket_number
FROM
	Passenger_A
	INNER JOIN Flight_leg_A
	ON Passenger_A.Leg_number = Flight_leg_A.Leg_number
	INNER JOIN Aircraft
	ON Flight_leg_A.Aircraft_code = Aircraft.Aircraft_code
	INNER JOIN Airline
	ON Airline.Airline_code = Aircraft.Airline_code
	INNER JOIN Flight
	ON Flight.Airline_code = Airline.Airline_code
	INNER JOIN Airport
	ON Airport.IATA_code = Airline.IATA_code
WHERE Flight_leg_A.Date_of_flight BETWEEN @startDate AND @endDate
)
GO


SELECT
	*
FROM
	udffindData('2000-01-01', '2026-01-01')

GO
CREATE OR ALTER FUNCTION udfstatusInfo(
	@status VARCHAR(50),
	@date DATE
) returns TABLE
AS RETURN (
	SELECT
	Flight_leg_B.[Status]
	,Flight_leg_B.Remark
	,Flight_leg_B.Arrival_teminal_number
	,Flight_leg_A.Staff_ID
	,Flight_leg_A.Leg_number
	,Flight_leg_A.Flight_number
	,Passenger_A.Passport_number
	,concat(Passenger_A.First_name,Passenger_A.Last_name) AS Passenger_name
	,Passenger_category.Passenger_category
	,Passenger_requirements.Requirement
	,Flight_leg_A.Date_of_flight AS Schedule_date
FROM
	Passenger_A
	INNER JOIN Passenger_requirements
	ON Passenger_A.Passport_number = Passenger_requirements.Passport_number
	INNER JOIN Passenger_category
	ON Passenger_A.Passport_number = Passenger_category.Passport_number
	INNER JOIN Flight_leg_A
	ON Passenger_A.Leg_number = Flight_leg_A.Leg_number
	INNER JOIN Flight_leg_B
	ON Flight_leg_B.Arrival_teminal_number = Flight_leg_A.Arrival_teminal_number

)
GO

SELECT
	*
FROM
	udfstatusInfo('Delayed', '2018-12-06')


GO
CREATE OR ALTER PROCEDURE stp_passExpire(
	@PassportNumber VARCHAR(50)
)
AS
BEGIN
	if not exists (select * from Passenger_A where Passport_number = @PassportNumber)
	BEGIN
		print 'This passport id is not valid!'
		return
	END

	if (select Date_of_Expire from Passenger_A where Passport_number = @PassportNumber) < GETDATE()
	BEGIN
		print 'Passport expired!'
		return
	END
	ELSE
	BEGIN
		print 'Passport valid!'
		return
	END
END


EXEC stp_passExpire 'M100123155'
EXEC stp_passExpire 'M100123464'
