use president;


-- 1.
select
    AVG(election_count) as avg_wahlen
from
    (
    select
        Candidate,
        COUNT(*) as election_count
    from
        Election
    group by
      Candidate
  ) as election_count;


-- 2.
select
    Candidate,
    COUNT(*) as election_count
from
    Election
group by
  Candidate
having
  COUNT(*) > (
    select
    AVG(election_count)
from
    (
        select
        Candidate,
        COUNT(*) as election_count
    from
        Election
    group by
          Candidate
      ) as election_count
  );


-- 3.
select
    Pres_Name,
    (
    select
        COUNT(*)
    from
        Pres_Marriage as PM
    where
      PM.Pres_Name = P.Pres_Name
  ) as wifes_count
from
    President P;


-- 4.
select
    P.Pres_Name,
    PH.Hobby
from
    President P,
    (
    select
        distinct
        Hobby
    from
        Pres_Hobby
  ) PH;