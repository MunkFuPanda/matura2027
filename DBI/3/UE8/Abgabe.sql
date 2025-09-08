use president;

-- 1.
select mar_year
from pres_marriage
group by mar_year
having count(*) = 2;

-- 2.
select pres_name,
       count(hobby)
from pres_hobby
group by pres_name;

-- 3.
select pres_name,
       sum(nr_children) as children
from pres_marriage
group by pres_name
having sum(nr_children) >= 10;

-- 4.
select state_born,
       min(death_age)              as min_death_age,
       avg(death_age)              as avg_death_age,
       max(death_age)              as max_death_age,
       avg(birth_year + death_age) as avg_death_year
from president
group by state_born;

-- 5.
select party,
       count(*) as presidents
from president
where birth_year > 1850
group by party
having count(*) > 8;

-- 6.
select party,
       count(*) as presidents
from president
group by party;

-- 7.
select pres_name,
       sum(nr_children) as children
from pres_marriage
group by pres_name;