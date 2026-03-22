use AirportDB;

-- 1.)

go
create or alter function dbo.udffindData(@startDate varchar(50), @endDate varchar(50))
returns table 
as
return (
	select f.Airline_code, Ai.Airline_Name, A.Aircraft_code, f.Flight_number, FlA.Leg_number, PA.Passport_number, PA.First_name, PA.Minit, PA.Last_name, PA.Air_ticket_number from Flight_shedule_date fd
		join flight f on f.Flight_number = fd.Flight_number
		join Airline Ai on Ai.Airline_code = f.Airline_code
		join Aircraft A on A.Airline_code = f.Airline_code
		join Flight_leg_A FlA on FlA.Flight_number = f.Flight_number
		join Passenger_A PA on FlA.Leg_number = PA.Leg_number
		where fd.Date between @startDate and @endDate
	)
go

select * from udffindData('2018-12-21', '2018-12-21')

-- 2.)

go
create or alter function dbo.statusInfo(@status varchar(50), @date varchar(50))
returns table
as
return (
	select FlB.Status, FlB.Remark, FlB.Arrival_teminal_number, FlA.Staff_ID, FlA.Leg_number, FlA.Flight_number, PA.Passport_number, 
	concat(PA.First_Name, ' ', PA.Last_name) as Passenger_name, PC.Passenger_catogary, PR.Requirement, fd.Date from Flight_shedule_date fd
		join flight f on f.Flight_number = fd.Flight_number
		join Airline Ai on Ai.Airline_code = f.Airline_code
		join Aircraft A on A.Airline_code = f.Airline_code
		join Flight_leg_A FlA on FlA.Flight_number = f.Flight_number
		join Passenger_A PA on FlA.Leg_number = PA.Leg_number
		join Flight_leg_B FlB on FlB.Arrival_teminal_number = FlA.Arrival_teminal_number
		join Passenger_catogary PC on PC.Passport_number = PA.Passport_number
		join Passenger_requirements PR on PR.Passport_number = PA.Passport_number
		where fd.Date = @date
		and flB.Status = @status
	)
go

select * from dbo.statusInfo('Canceled', '2018-12-21')	

-- 3.)


go
create or alter procedure dbo.stp_passExpire
@PassportNumber varchar(50)
as
begin 
	if ((select Date_of_Expire from Passenger_A where Passport_number = @PassportNumber) < cast(GETDATE() as date))
	begin
		print 'Passengers Passport has been expired'
	end
	else if exists (select Date_of_Expire from Passenger_A where Passport_number = @PassportNumber)
	begin
		print 'This Passenger has a valid Passport'
	end
	else
	begin
		print 'This Passport Number does not exist'
	end
end
go

exec dbo.stp_passExpire 'M100123155'
exec dbo.stp_passExpire 'M100123464'