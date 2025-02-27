use president;

-- 1.
select *
from president
where birth_year = 1946;

-- 2.
select *
from president
where death_age is null;

-- 3.
select pres_name,
       death_age
from president
where death_age between 50 and 60;

-- 4.
select *
from president
where pres_name like '_e%'
  and pres_name not like 'R%';

-- 5.
select pres_name,
       party
from president
where party != 'Republican';

-- 6.
select pres_name
from pres_marriage
where nr_children = 0;

-- 7.
select *
from pres_hobby as p
where hobby in ('Painting', 'Swimming')
  and not exists (select *
                  from pres_hobby as p2
                  where p2.pres_name = p.pres_name
                    and p2.hobby = 'Billards');
