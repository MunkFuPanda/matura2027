use president;

-- 1.
select pres_name
from president
         left join election
                   on president.pres_name = election.candidate
where election.candidate is null

select pres_name
from president
except
select candidate
from election

-- 2.

-- falsch
select *
from president
         inner join election on president.pres_name = election.candidate
    and election.election_year <> 1800
where election.election_year = 1789

select pres_name
from president
         left join election
                   on president.pres_name = election.candidate
where election.election_year = 1789
except
select pres_name
from president
         left join election
                   on president.pres_name = election.candidate
where election.election_year = 1800

-- 3.
select election_year
from election
where candidate = 'Trump D J'
union
select election_year
from election
where candidate like 'Bush %'

select distinct election_year
from election
where candidate like 'Bush %'
   or candidate = 'Trump D J'

-- 4.
select election_year
from election
where candidate = 'Adams J'
intersect
select election_year
from election
where candidate like 'Clinton G'

select distinct e1.election_year
from election as e1
         cross join election as e2
where e1.candidate = 'Adams J'
  and e2.candidate = 'Clinton G'
