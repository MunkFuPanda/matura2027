use president;

-- 1.
select
	Administration.Pres_Name,
	count(Vice_Pres_Name) as VpCount
from
	Administration
	inner join Admin_PR_VP
	on Administration.Admin_NR = Admin_PR_VP.Admin_NR
group by Administration.Pres_Name

select
	Administration.Pres_Name,
	count(Vice_Pres_Name) as VpCount
from
	Administration
	left join Admin_PR_VP
	on Administration.Admin_NR = Admin_PR_VP.Admin_NR
group by Administration.Pres_Name

-- kein unterschied

-- 2.
select
	President.Pres_Name
from
	President
	left join Pres_Marriage
	on President.Pres_Name = Pres_Marriage.Pres_Name
where Pres_Marriage.Pres_Name is null;

select
	President.Pres_Name
from
	Pres_Marriage
	right join President
	on President.Pres_Name = Pres_Marriage.Pres_Name
where Pres_Marriage.Pres_Name is null;

-- kein unterschied

-- 3.
select
	President.Pres_Name,
	Pres_Marriage.Mar_Year
from
	President
	inner join Pres_Marriage
	on President.Pres_Name = Pres_Marriage.Pres_Name
where (Pres_Marriage.Mar_Year between 1939 and 1945) and President.Party = 'Republican'

-- 4.
select
	President.Pres_Name,
	count(Pres_Marriage.Pres_Name) as '# Marriage'
from
	President
	left join Pres_Marriage
	on President.Pres_Name = Pres_Marriage.Pres_Name
group by President.Pres_Name
order by count(Pres_Marriage.Pres_Name) desc;

-- 5.
select
	State.State_Name
from
	State
	left join President
	on State.State_Name = President.State_Born
where President.State_Born is null
order by State.State_Name

-- 6.
select
	President.Pres_Name,
	count(Pres_Marriage.Pres_Name) as '# Marriage'
from
	President
	inner join Pres_Marriage
	on President.Pres_Name = Pres_Marriage.Pres_Name
group by President.Pres_Name

-- 7.
select
	President.Pres_Name,
	count(Pres_Marriage.Pres_Name) as '# Marriage'
from
	President
	left join Election
	on President.Pres_Name = Election.Candidate
	inner join Pres_Marriage
	on President.Pres_Name = Pres_Marriage.Pres_Name
where Election.Candidate is null
group by President.Pres_Name
