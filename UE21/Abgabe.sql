use school;

-- 1a.
select title
from course
where (select count(*)
       from studentgrade sg
       where sg.courseid = course.courseid) >= 6;

-- 1b.
select title
from course
where courseid in (select sg.courseid
                   from studentgrade sg
                   group by sg.courseid
                   having count(*) >= 6);

-- 2.
select title
from course
         inner join onlinecourse
                    on course.courseid = onlinecourse.courseid
intersect
select title
from course
         inner join onsitecourse
                    on course.courseid = onsitecourse.courseid

-- 3.
with avg_grades as (select personid, avg(grade) as avg_grade
                    from person
                             inner join studentgrade
                                        on person.personid = studentgrade.studentid
                    group by person.personid
                    having avg(grade) is not null)
select personid, avg_grade
from avg_grades
where avg_grade < (select avg(avg_grade)
                   from avg_grades);

-- 4.
with ci_per_dep as (select department.departmentid, count(ci.courseid) as ci_count
                    from department
                             inner join course c on department.departmentid = c.departmentid
                             inner join courseinstructor ci on c.courseid = ci.courseid
                    group by department.departmentid)
select name, ci_count
from ci_per_dep
         inner join department
                    on ci_per_dep.departmentid = department.departmentid
where ci_count >= 2;

-- 5.
select d.name as departmentname, count(distinct ci.personid) as ci_count
from department d
         join course c on d.departmentid = c.departmentid
         join courseinstructor ci on c.courseid = ci.courseid
group by d.name
having count(distinct ci.personid) >= 2;
