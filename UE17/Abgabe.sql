use school;

go
drop view if exists vstudent;
go

-- 1.
go
create view vstudent
as
select p.personid    as studentid,
       p.firstname,
       p.lastname,
       avg(sg.grade) as avggrade
from person p
         left join studentgrade sg on p.personid = sg.studentid
where p.discriminator = 'Student'
group by p.personid, p.firstname, p.lastname;
go


-- 2a.
select firstname,
       lastname,
       title
from vstudent
         inner join courseinstructor
                    on vstudent.studentid = courseinstructor.personid
         inner join course
                    on courseinstructor.courseid = course.courseid
where studentid = personid;

-- 2b.
select firstname,
       lastname,
       title
from person
         inner join courseinstructor
                    on person.personid = courseinstructor.personid
         inner join course
                    on courseinstructor.courseid = course.courseid
where discriminator = 'Student';

-- 3a.
select firstname, lastname
from vstudent
where avggrade > (select avg(avggrade)
                  from vstudent);

-- 3b.
with localstudentview as (select firstname,
                                 lastname,
                                 avg(grade) as avggrade
                          from person
                                   left join studentgrade sg on personid = studentid
                          where discriminator = 'Student'
                          group by personid, firstname, lastname)
select firstname, lastname
from localstudentview
where avggrade > (select avg(avggrade) from localstudentview);

-- 4a.
select department.name,
       count(*) as coursecount
from department
         inner join dbo.course c on department.departmentid = c.departmentid
         inner join onlinecourse oc on c.courseid = oc.courseid
where oc.courseid is null
group by department.departmentid, department.name;

select department.name,
       coalesce(oc.coursecount, 0)  as onlinecourses,
       coalesce(oc2.coursecount, 0) as onsitecourses
from department
         left join (select c.departmentid as ocid, count(onlinecourse.courseid) as coursecount
                    from onlinecourse
                             inner join dbo.course c on onlinecourse.courseid = c.courseid
                    group by c.departmentid) as oc on ocid = department.departmentid
         left join (select c.departmentid as osid, count(onsitecourse.courseid) as coursecount
                    from onsitecourse
                             inner join dbo.course c on onsitecourse.courseid = c.courseid
                    group by c.departmentid) as oc2 on osid = department.departmentid

-- 4b.
select department.name,
       (select count(*)
        from course
                 inner join onlinecourse on course.courseid = onlinecourse.courseid
        where departmentid = department.departmentid) as onlinecourses,
       (select count(*)
        from course
                 inner join onsitecourse on course.courseid = onsitecourse.courseid
        where departmentid = department.departmentid) as onsitecourses
from department

-- 4c.
-- der name ist nicht unique, kann also zu problemen führen, deswegen von mir über die departmentid abgeleitet