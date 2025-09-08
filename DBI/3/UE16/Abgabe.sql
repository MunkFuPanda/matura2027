use president;

-- 1.
select avg(election_count) as avg_wahlen
from (select candidate,
             count(*) as election_count
      from election
      group by candidate) as election_count;


-- 2.
select candidate,
       count(*) as election_count
from election
group by candidate
having count(*) > (select avg(election_count)
                   from (select candidate,
                                count(*) as election_count
                         from election
                         group by candidate) as election_count);


-- 3.
select pres_name,
       (select count(*)
        from pres_marriage as pm
        where pm.pres_name = p.pres_name) as wifes_count
from president p;


-- 4.
select p.pres_name,
       ph.hobby
from president p,
     (select distinct hobby
      from pres_hobby) ph;