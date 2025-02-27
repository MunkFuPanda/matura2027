use president;

-- 1.
select pres_marriage.pres_name,
       pres_marriage.mar_year
from pres_marriage
         inner join
     president
     on pres_marriage.pres_name = president.pres_name
where mar_year between 1939 and 1945
  and president.party = 'Republican';

-- 2.
select pres_marriage.spouse_name
from pres_marriage
         inner join pres_hobby
                    on pres_marriage.pres_name = pres_hobby.pres_name
where pres_hobby.hobby = 'Poker';

-- 3.
select president.pres_name,
       president.party
from president
         inner join
     pres_marriage
     on president.pres_name = pres_marriage.pres_name
where pres_marriage.nr_children = 0;

-- 4.
select president.pres_name,
       president.party
from president
         inner join
     pres_hobby
     on president.pres_name = pres_hobby.pres_name
where pres_hobby.hobby = 'Fishing';

-- 5.
select distinct pres_hobby.hobby
from pres_hobby
         inner join
     president
     on pres_hobby.pres_name = president.pres_name
where president.party = 'Republican'
order by hobby asc;

-- 6.
select president.pres_name,
       president.party,
       pres_marriage.mar_year - president.birth_year as age
from president
         inner join
     pres_marriage
     on president.pres_name = pres_marriage.pres_name
where pres_marriage.pr_age < 25
   or president.party = 'Republican'
order by pres_name asc;