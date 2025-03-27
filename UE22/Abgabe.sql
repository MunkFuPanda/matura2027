use schulungsfirma;

-- 1.
select person.pnr, fname
from person
         inner join dbo.besucht b on person.pnr = b.pnr
         inner join kveranst v on (b.knr = v.knr and v.knrlfnd = b.knrlfnd)
where v.ort = 'Wien'
intersect
select person.pnr, fname
from person
         inner join dbo.besucht b on person.pnr = b.pnr
         inner join kveranst v on (b.knr = v.knr and v.knrlfnd = b.knrlfnd)
where v.ort = 'Paris';

-- 2.

select p.fname, p.vname
from person p
except
select distinct p.fname, p.vname
from kveranst kv
         join person p on p.pnr = kv.pnr
where year(kv.von) in (2013, 2015)

-- 3.

select k1.bezeichn
from kurs k1
where k1.preis > (select max(k2.preis)
                  from kveranst kv1
                           join kurs k2 on k2.knr = kv1.knr
                  where kv1.ort = 'Paris');

-- 4.
select p.pnr, p.fname
from kveranst kv
         join besucht b on b.knr = kv.knr and b.knrlfnd = kv.knrlfnd
         join referent r on r.pnr = kv.pnr
         join person p on p.pnr = b.pnr
where kv.ort = 'Wien'

-- 5.)

begin transaction;
update k
set k.tage = k.tage + 1
from kurs k
where k.tage > 2

update kveranst
set bis = dateadd(day, 1, bis)
from kveranst
         join kurs k on k.knr = kveranst.knr
where k.tage > 2;
commit;

-- 6.

select p.pnr, p.fname
from kveranst kv
         join referent r on r.pnr = kv.pnr
         join person p on p.pnr = r.pnr
where 1 >= (select count(*)
            from setztvor s
            where s.knr = kv.knr);

-- 7.

go
create view ref as
select p.fname, p.vname, count(kv.pnr) anzahl
from kveranst kv
         join person p on p.pnr = kv.pnr
group by p.fname, p.vname
having count(kv.pnr) >= 2;
go
select *
from ref;
go
drop view ref
go