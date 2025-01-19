use president;

-- 1.
select
    Mar_Year
from
    Pres_Marriage
group by Mar_Year
having count(*) = 2;

-- 2.
select
    Pres_Name,
    count(Hobby)
from
    Pres_Hobby
group by Pres_Name;

-- 3.
select
    Pres_Name,
    sum(Nr_Children) as Children
from
    Pres_Marriage
group by Pres_Name
having sum(Nr_Children) >= 10;

-- 4.
select
    State_Born,
    min(Death_age) as Min_Death_Age,
    avg(Death_Age) as Avg_Death_Age,
    max(Death_Age) as Max_Death_Age,
    avg(Birth_Year + Death_Age) as Avg_Death_Year
from
    President
group by State_Born;

-- 5.
select
    Party,
    count(*) as Presidents
from
    President
where Birth_Year > 1850
group by Party
having count(*) > 8;

-- 6.
select
    Party,
    count(*) as Presidents
from
    President
group by Party;

-- 7.
select
    Pres_Name,
    sum(Nr_Children) as Children
from
    Pres_Marriage
group by Pres_Name;