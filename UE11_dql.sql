use president;

-- 1.
select
	Pres_Name
from
	President
	left join Election
	on President.Pres_Name = Election.Candidate
where Election.Candidate is null

	select
		Pres_Name
	from
		President
except
	select
		Candidate
	from
		Election

-- 2.

-- falsch
select
	*
from
	President
	inner join Election on President.Pres_Name = Election.Candidate
		and Election.Election_Year <> 1800
where Election.Election_Year = 1789

	select
		Pres_Name
	from
		President
		left join Election
		on President.Pres_Name = Election.Candidate
	where Election.Election_Year = 1789
except
	select
		Pres_Name
	from
		President
		left join Election
		on President.Pres_Name = Election.Candidate
	where Election.Election_Year = 1800

-- 3.
	select
		Election_Year
	from
		Election
	where Candidate = 'Trump D J'
union
	select
		Election_Year
	from
		Election
	where Candidate like 'Bush %'

select
	distinct
	Election_Year
from
	Election
where Candidate like 'Bush %' or Candidate = 'Trump D J'

-- 4.
	select
		Election_Year
	from
		Election
	where Candidate = 'Adams J'
intersect
	select
		Election_Year
	from
		Election
	where Candidate like 'Clinton G'

select
	distinct
	e1.Election_Year
from
	Election as e1
cross join Election as e2
where e1.Candidate = 'Adams J' and e2.Candidate = 'Clinton G'
