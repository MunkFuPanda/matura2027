use school;

-- 1.
select person.firstname, person.lastname
from person
where discriminator = 'Student'
  and not exists (select *
                  from course
                  where not exists(select *
                                   from studentgrade
                                   where course.courseid = studentgrade.courseid
                                     and studentgrade.studentid = person.personid))

-- 2.
select course.title, count(*) as studentcount
from course
         inner join studentgrade
                    on course.courseid = studentgrade.courseid
group by course.courseid, course.title
order by count(*);

-- 3.
select course.title
from course
where courseid not in (select courseid
                       from onsitecourse
                       union
                       select courseid
                       from onlinecourse)

-- 4a.
select firstname, lastname
from person
         left join studentgrade
                   on person.personid = studentgrade.studentid
where studentgrade.studentid is null

-- 4b.
select firstname, lastname
from person
where not exists (select *
                  from studentgrade
                  where studentgrade.studentid = person.personid);

-- 5.

-- ich sehe keinen Sinn hinter dieser View, da sie mir nicht erlaubt Rückschlüsse zu ziehen, wer welche kurse hält.
with all_courses as (select d.name                                              as departmentname,
       count(c.courseid)                                   as anzahl_alle_kurse,
       count(case when oc.courseid is not null then 1 end) as anzahl_online_courses,
       count(case when os.courseid is not null then 1 end)     as anzahl_onsite_courses
from department d
         inner join course c on d.departmentid = c.departmentid
         left join onlinecourse oc on c.courseid = oc.courseid
         left join onsitecourse os on c.courseid = os.courseid
group by d.name) select * from all_courses

-- 5c.
-- jede Spalte muss einen namen haben
-- nein, da mir die Anzahl nichts bringt, um Rückschlüsse zu ziehen
