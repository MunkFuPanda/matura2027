use president;

-- 1.
select
    Pres_Marriage.Pres_Name,
    Pres_Marriage.Mar_Year
from
    Pres_Marriage
    inner join
    President
    on Pres_Marriage.Pres_Name = President.Pres_Name
where Mar_Year between 1939 and 1945 and President.Party = 'Republican';

-- 2.
select
    Pres_Marriage.Spouse_Name
from
    Pres_Marriage
    inner join Pres_Hobby
    on Pres_Marriage.Pres_Name = Pres_Hobby.Pres_Name
where Pres_Hobby.Hobby = 'Poker';

-- 3.
select
    President.Pres_Name,
    President.Party
from
    President
    inner join
    Pres_Marriage
    on President.Pres_Name = Pres_Marriage.Pres_Name
where Pres_Marriage.Nr_Children = 0;

-- 4.
select
    President.Pres_Name,
    President.Party
from
    President
    inner join
    Pres_Hobby
    on President.Pres_Name = Pres_Hobby.Pres_Name
where Pres_Hobby.Hobby = 'Fishing';

-- 5.
select
    distinct
    Pres_Hobby.Hobby
from
    Pres_Hobby
    inner join
    President
    on Pres_Hobby.Pres_Name = President.Pres_Name
where President.Party = 'Republican'
order by Hobby asc;

-- 6.
select
    President.Pres_Name,
    President.Party,
    Pres_Marriage.Mar_Year - President.Birth_Year as age
from
    President
    inner join
    Pres_Marriage
    on President.Pres_Name = Pres_Marriage.Pres_Name
where Pres_Marriage.Pr_Age < 25
    or President.Party = 'Republican' order by Pres_Name asc;