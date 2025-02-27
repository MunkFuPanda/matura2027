use president;

-- 1.
select administration.pres_name,
       count(vice_pres_name) as vpcount
from administration
         inner join admin_pr_vp
                    on administration.admin_nr = admin_pr_vp.admin_nr
group by administration.pres_name

select administration.pres_name,
       count(vice_pres_name) as vpcount
from administration
         left join admin_pr_vp
                   on administration.admin_nr = admin_pr_vp.admin_nr
group by administration.pres_name

-- kein unterschied

-- 2.
select president.pres_name
from president
         left join pres_marriage
                   on president.pres_name = pres_marriage.pres_name
where pres_marriage.pres_name is null;

select president.pres_name
from pres_marriage
         right join president
                    on president.pres_name = pres_marriage.pres_name
where pres_marriage.pres_name is null;

-- kein unterschied

-- 3.
select president.pres_name,
       pres_marriage.mar_year
from president
         inner join pres_marriage
                    on president.pres_name = pres_marriage.pres_name
where (pres_marriage.mar_year between 1939 and 1945)
  and president.party = 'Republican'

-- 4.
select president.pres_name,
       count(pres_marriage.pres_name) as '# Marriage'
from president
         left join pres_marriage
                   on president.pres_name = pres_marriage.pres_name
group by president.pres_name
order by count(pres_marriage.pres_name) desc;

-- 5.
select state.state_name
from state
         left join president
                   on state.state_name = president.state_born
where president.state_born is null
order by state.state_name

-- 6.
select president.pres_name,
       count(pres_marriage.pres_name) as '# Marriage'
from president
         inner join pres_marriage
                    on president.pres_name = pres_marriage.pres_name
group by president.pres_name

-- 7.
select president.pres_name,
       count(pres_marriage.pres_name) as '# Marriage'
from president
         left join election
                   on president.pres_name = election.candidate
         inner join pres_marriage
                    on president.pres_name = pres_marriage.pres_name
where election.candidate is null
group by president.pres_name
