--create database lt
use lt

drop table if exists lt;
drop table if exists l;
drop table if exists t;

create table l (
  lnr    varchar(2) not null primary key
, lname  varchar(6) not null
, rabatt decimal(3) not null
, stadt  varchar(6) not null  check (stadt in ('London', 'Athen', 'Rom', 'Paris'))
);

create table t (
  tnr    varchar(2)    not null primary key
, tname  varchar(8)    not null
, farbe  varchar(5)    not null
, preis  decimal(4, 2) not null
, stadt  varchar(6)    not null check (stadt in ('London', 'Athen', 'Rom', 'Paris'))
);

create table lt (
  lnr    varchar(2) not null references l
, tnr    varchar(2) not null references t
, menge  decimal(4) not null check(menge > 0)
, primary key (lnr, tnr)
);

drop view if exists LTX;
go
create view LTX as
    select LT.*, L.lname, L.stadt
    from LT 
        join L on LT.lnr = L.lnr
go

-- without if exists as drop table removes
-- 1. any indexes 2. rules 3. triggers 4. constraints that exist for the target table
create index idx_lt_lnr
          on lt(lnr);

create index idx_lt_tnr
          on lt(tnr);


insert into l
  (lnr,  lname   , rabatt, stadt   )
values
  ('L1', 'Schmid',     20, 'London')
, ('L2',  'Jonas',     10, 'Paris' )
, ('L3', 'Berger',     30, 'Paris' )
, ('L4',  'Klein',     20, 'London')
, ('L5',   'Adam',     30, 'Athen' );

insert into t
  (tnr , tname     , farbe  , preis, stadt   )
values
  ('T1', 'Mutter'  , 'rot'  ,    12, 'London')
, ('T2', 'Bolzen'  , 'gelb' ,    17, 'Paris' )
, ('T3', 'Schraube', 'blau' ,    17, 'Rom'   )
, ('T4', 'Schraube', 'rot'  ,    14, 'London')
, ('T5', 'Welle'   , 'blau' ,    12, 'Paris' )
, ('T6', 'Zahnrad' , 'rot'  ,    19, 'London');

insert into lt
  (lnr , tnr,  menge)
values
  ('L1', 'T1',   300)
, ('L1', 'T2',   200)
, ('L1', 'T3',   400)
, ('L1', 'T4',   200)
, ('L1', 'T5',   100)
, ('L1', 'T6',   100)
, ('L2', 'T1',   300)
, ('L2', 'T2',   400)
, ('L3', 'T2',   200)
, ('L4', 'T2',   200)
, ('L4', 'T4',   300)
, ('L4', 'T5',   400);

select *
from t;


select *
from l;

select *
from lt;