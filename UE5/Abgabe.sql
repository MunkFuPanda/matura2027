use president;

-- 1.
select pres_name, birth_year, state_born
from president
order by state_born asc, birth_year desc;

-- 2.
select distinct party
from president;

-- 3.
select state_name, year_entered
from state;

-- 4.
select max(nr_children) as max_children
from pres_marriage;

-- 5.
select distinct hobby
from pres_hobby
order by hobby asc;

-- 6.
select pres_name, winner_loser_indic
from election
         cross join president
where election.candidate = president.pres_name;

-- 7.
select pres_name, isnull(death_age + birth_year, -1) as death_year
from president;