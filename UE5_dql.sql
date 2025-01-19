use president;

-- 1.
select Pres_Name, Birth_Year, State_Born
from President
order by State_Born asc, Birth_Year desc;

-- 2.
select distinct Party
from President;

-- 3.
select State_Name, Year_Entered
from State;

-- 4.
select max(Nr_Children) as Max_Children
from Pres_Marriage;

-- 5.
select distinct Hobby
from Pres_Hobby
order by Hobby asc;

-- 6.
select Pres_Name, Winner_Loser_Indic
from Election
cross join President
where Election.Candidate = President.Pres_Name;

-- 7.
select Pres_Name, isnull(Death_Age + Birth_Year, -1) as Death_Year 
from President;