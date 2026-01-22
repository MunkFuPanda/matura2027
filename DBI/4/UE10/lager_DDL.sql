use lagerverwaltung
go

drop table lieferung
drop table artikel
drop table lager

go

create table artikel(
   anr int not null primary key,
   bezeichnung varchar(30) not null,
   preis decimal(12,2)
)
go

create table lager(
   lnr int not null primary key,
   ort varchar(30),
   stueckkap int
)
go

create table lieferung(
   lnr int not null references lager,
   lfndNr int,
   anr int not null references artikel,
   datum datetime,
   stueck  int not null,
   primary key (lnr,lfndNr)
)

insert into artikel values(1,'Artikel1',123.80)
insert into artikel values(2,'Artikel2',99.90)
insert into artikel values(3,'Artikel3',100.78)
go

insert into lager values(1,'Halle A',100)
insert into lager values(2,'Halle B',200)
insert into lager values(3,'Halle C',300)
insert into lager values(4,'Halle D',100)
go

insert into lieferung values(1,1,1,'1.1.2020',20)
insert into lieferung values(1,2,1,'2.1.2020',30)
insert into lieferung values(1,3,2,'1.2.2020',40)
insert into lieferung values(2,1,1,'2.1.2020',20)
insert into lieferung values(2,2,1,'3.1.2020',10)
insert into lieferung values(1,4,1,'1.1.2020',20)
insert into lieferung values(1,5,1,'2.1.2020',30)
insert into lieferung values(1,6,2,'1.2.2020',40)
insert into lieferung values(2,3,1,'2.1.2020',20)
insert into lieferung values(2,4,3,'3.1.2020',10)
insert into lieferung values(1,7,1,'1.1.2020',20)
insert into lieferung values(1,8,1,'2.1.2020',30)
insert into lieferung values(1,9,2,'1.2.2020',40)
insert into lieferung values(2,5,1,'2.1.2020',20)
insert into lieferung values(2,6,3,'3.1.2020',10)
go

select * from lieferung

