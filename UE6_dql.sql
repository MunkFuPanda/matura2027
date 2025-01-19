use president;

-- 1.
select
    *
from
    president
where Birth_Year = 1946;

-- 2.
select
    *
from
    president
where Death_Age is null;

-- 3.
select
    Pres_Name,
    Death_Age
from
    president
where Death_Age between 50 and 60;

-- 4.
select
    *
from
    president
where Pres_Name like '_e%' and Pres_Name not like 'R%';

-- 5.
select
    Pres_Name,
    Party
from
    president
where Party != 'Republican';

-- 6.
select
    Pres_Name
from
    Pres_Marriage
where Nr_Children = 0;

-- 7.
select
    *
from
    Pres_Hobby as p
where Hobby in ('Painting', 'Swimming') and not exists (select
        *
    from
        Pres_Hobby as p2
    where p2.Pres_Name = p.Pres_Name and p2.Hobby = 'Billards');
