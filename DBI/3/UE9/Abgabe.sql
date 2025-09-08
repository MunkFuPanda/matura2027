use schulungsfirma;

-- 1.
select kveranst.knr,
       kurs.bezeichn,
       knrlfnd,
       von
from kveranst
         join kurs
              on kurs.knr = kveranst.knr
where pnr is null;

-- 2.
select kveranst.knr,
       kurs.bezeichn,
       kveranst.knrlfnd,
       von
from kveranst
         join kurs
              on kurs.knr = kveranst.knr
         join besucht
              on kveranst.knr = besucht.knr and kveranst.knrlfnd = besucht.knrlfnd
where kveranst.pnr is not null
  and besucht.pnr is not null
group by kveranst.knr, kurs.bezeichn, kveranst.knrlfnd, kveranst.von;

-- 3.
select kurs.bezeichn,
       von
from kveranst
         join kurs
              on kveranst.knr = kurs.knr
         join referent
              on kveranst.pnr = referent.pnr
where referent.pnr is not null;

-- 4.
select person.vname,
       person.fname
from person
         join besucht b
              on person.pnr = b.pnr
         join besucht b1
              on person.pnr = b1.pnr
where b.knr = 1
  and b1.knr = 5;

-- 5.
select kurs.bezeichn,
       kveranst.von,
       kveranst.bis,
       kveranst.knrlfnd,
       round(kurs.preis / kurs.tage, 2) as 'durchschnittlicher Tagespreis'
from kveranst
         join kurs
              on kveranst.knr = kurs.knr
         join referent
              on referent.pnr = kveranst.pnr
where round(kurs.preis / kurs.tage, 2) between 610 and 690;

-- 6.
select vname,
       fname,
       count(besucht.pnr) as '# Kveranst besucht'
from person
         join besucht
              on besucht.pnr = person.pnr
group by vname, fname
having count(besucht.pnr) > 1;

-- 7.
select referent.pnr,
       count(kveranst.knr) as '# Kverant gehalten'
from referent
         join kveranst
              on kveranst.pnr = referent.pnr
group by referent.pnr
having count(kveranst.knr) > 0
order by count(kveranst.knr) desc;

-- 8.
select person.vname,
       person.fname,
       count(kveranst.knr) as '# Kveranst gehalten'
from referent
         join kveranst
              on kveranst.pnr = referent.pnr
         join person
              on person.pnr = referent.pnr
group by person.vname, person.fname
having count(kveranst.knr) > 1
order by person.vname, person.fname desc;