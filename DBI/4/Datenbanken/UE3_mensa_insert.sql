insert into lieferant
values(1 ,'Lieferant1' ,'1010' ,'Wien' ,'Wienerstr.123' ,'01/123456')
insert into lieferant
values(2 ,'Lieferant2' ,'1020' ,'Wien' ,'Grazerstr.123' ,'01/78964')
insert into lieferant
values(3 ,'Lieferant3' ,'1030' ,'Wien' ,'Grazerstr.123' ,'01/143215')
go
insert into tag
values('5.11.2007' ,1 ,100)
insert into tag
values('6.11.2007' ,1 ,100)
insert into tag
values('7.11.2007' ,1 ,100)
insert into tag
values('8.11.2007' ,0 ,100)
insert into tag
values('9.11.2007' ,1 ,100)
insert into tag
values('12.11.2007' ,0 ,100)
insert into tag
values('13.11.2007' ,0 ,100)
go
insert into menue
values(11 ,2 ,8)
insert into menue
values(22 ,1 ,6)
insert into menue
values(33 ,2 ,7)
insert into menue
values(44 ,1 ,5)
insert into menue
values(55 ,2 ,6)
insert into menue
values(66 ,1 ,5)
insert into menue
values(77 ,2 ,6)
go

insert into speise
values(100 ,'Nudelsuppe' ,1)
insert into speise
values(101 ,'Gem�sesuppe' ,1)
insert into speise
values(102 ,'Knoblauchcremesuppe' ,1)
insert into speise
values(103 ,'Zucchinicremesuppe' ,1)
insert into speise
values(104 ,'Wiener Schnitzel mit Salzkartoffel' ,2)
insert into speise
values(105 ,'Rindsgulasch mit Bandnudeln' ,2)
insert into speise
values(106 ,'Faschierte Laibchen mit Kartoffelp�rree' ,2)
insert into speise
values(107 ,'Leberk�se mit Spinat' ,2)
insert into speise
values(108 ,'Gem�sestrudel' ,2)
insert into speise
values(109 ,'Pudding' ,3)
insert into speise
values(110 ,'Apfelkuchen' ,3)
insert into speise
values(111 ,'Kakaoroulade' ,3)
insert into speise
values(112 ,'gemischtes Eis' ,3)
insert into speise
values(113 ,'Traubenschnitte' ,3)
insert into speise
values(114 ,'frische Fr�chte' ,3)
go


insert into zutat
values
    (1 ,'Karotten' ,'kg' ,0.9 ,10)
insert into zutat
values
    (2 ,'Fisolen' ,'kg' ,1.2 ,15)
insert into zutat
values
    (3 ,'Karfiol' ,'Stk' ,1.2 ,8)
insert into zutat
values
    (4 ,'Kartoffel' ,'kg' ,1.1 ,150)
insert into zutat
values
    (5 ,'Suppennudeln' ,'kg' ,0.7 ,20)
insert into zutat
values
    (6 ,'Creme fraiche' ,'l' ,0.3 ,10)
insert into zutat
values
    (7 ,'Knoblauch' ,'Stk' ,1.2 ,20)
insert into zutat
values
    (8 ,'Zucchini' ,'kg' ,1.2 ,16)
insert into zutat
values
    (9 ,'Schweinsschnitzel' ,'kg' ,1.2 ,16)
insert into zutat
values
    (10 ,'Faschiertes gemischt' ,'kg' ,1.2 ,16)
insert into zutat
values
    (11 ,'Bandnudeln' ,'kg' ,1.2 ,16)
insert into zutat
values
    (12 ,'Milch' ,'l' ,1.1 ,40)
insert into zutat
values
    (13 ,'�pfel' ,'kg' ,1.2 ,30)
insert into zutat
values
    (14 ,'Eier' ,'Stk' ,0.10 ,80)
insert into zutat
values
    (15 ,'Rindsgulaschfleisch' ,'kg' ,1.8 ,50)
insert into zutat
values
    (16 ,'Zwiebel' ,'kg' ,1.8 ,20)
insert into zutat
values
    (17 ,'Leberk�se' ,'kg' ,1.7 ,20)
insert into zutat
values
    (18 ,'Cremespinat' ,'kg' ,1.5 ,18)
insert into zutat
values
    (19 ,'Zucker' ,'kg' ,0.9 ,90)
insert into zutat
values
    (20 ,'Kakao' ,'kg' ,2.5 ,9)
insert into zutat
values
    (21 ,'Erdbeereis' ,'l' ,6 ,2)
insert into zutat
values
    (22 ,'Schokoladeeis' ,'l' ,2 ,2)
insert into zutat
values
    (23 ,'Vanilleeis' ,'l' ,5 ,2)
insert into zutat
values
    (24 ,'Butter' ,'l' ,6 ,2)
insert into zutat
values
    (25 ,'Staubzucker' ,'l' ,2 ,2)
insert into zutat
values
    (26 ,'Mehl' ,'l' ,5 ,2)
insert into zutat
values
    (27 ,'Weintrauben' ,'kg' ,3.9 ,2)

go

insert into Speise_besteht_aus
values(100 ,5 ,0.6)
insert into Speise_besteht_aus
values(101 ,1 ,0.5)
insert into Speise_besteht_aus
values(101 ,2 ,0.5)
insert into Speise_besteht_aus
values(101 ,3 ,1)
insert into Speise_besteht_aus
values(101 ,4 ,1)
insert into Speise_besteht_aus
values(102 ,6 ,0.25)
insert into Speise_besteht_aus
values(102 ,7 ,1)
insert into Speise_besteht_aus
values(103 ,8 ,0.5)
insert into Speise_besteht_aus
values(103 ,6 ,0.25)
insert into Speise_besteht_aus
values(104 ,9 ,4)
insert into Speise_besteht_aus
values(104 ,4 ,0.6)
insert into Speise_besteht_aus
values(105 ,16 ,1)
insert into Speise_besteht_aus
values(105 ,15 ,1)
insert into Speise_besteht_aus
values(105 ,14 ,1)
insert into Speise_besteht_aus
values(106 ,10 ,1)
insert into Speise_besteht_aus
values(106 ,14 ,2)
insert into Speise_besteht_aus
values(106 ,16 ,0.5)
insert into Speise_besteht_aus
values(106 ,7 ,0.5)
insert into Speise_besteht_aus
values(107 ,17 ,0.8)
insert into Speise_besteht_aus
values(107 ,18 ,0.8)
insert into Speise_besteht_aus
values(108 ,2 ,0.3)
insert into Speise_besteht_aus
values(108 ,1 ,0.3)
insert into Speise_besteht_aus
values(108 ,4 ,0.3)
insert into Speise_besteht_aus
values(108 ,8 ,0.3)
insert into Speise_besteht_aus
values(109 ,12 ,1)
insert into Speise_besteht_aus
values(109 ,19 ,0.4)
insert into Speise_besteht_aus
values(110 ,13 ,1)
insert into Speise_besteht_aus
values(111 ,14 ,5)
insert into Speise_besteht_aus
values(111 ,19 ,0.3)
insert into Speise_besteht_aus
values(111 ,20 ,0.1)
insert into Speise_besteht_aus
values(112 ,21 ,0.2)
insert into Speise_besteht_aus
values(112 ,22 ,0.2)
insert into Speise_besteht_aus
values(112 ,23 ,0.2)
insert into Speise_besteht_aus
values(113 ,24 ,0.14)
insert into Speise_besteht_aus
values(113 ,14 ,5)
insert into Speise_besteht_aus
values(113 ,25 ,0.14)
insert into Speise_besteht_aus
values(113 ,26 ,0.14)
insert into Speise_besteht_aus
values(113 ,27 ,1)
go

insert into Menue_besteht_aus
values(100 ,11)
insert into Menue_besteht_aus
values(104 ,11)
insert into Menue_besteht_aus
values(110 ,11)
insert into Menue_besteht_aus
values(101 ,22)
insert into Menue_besteht_aus
values(105 ,22)
insert into Menue_besteht_aus
values(110 ,22)
insert into Menue_besteht_aus
values(103 ,33)
insert into Menue_besteht_aus
values(106 ,33)
insert into Menue_besteht_aus
values(109 ,33)
insert into Menue_besteht_aus
values(102 ,44)
insert into Menue_besteht_aus
values(107 ,44)
insert into Menue_besteht_aus
values(111 ,44)
insert into Menue_besteht_aus
values(102 ,55)
insert into Menue_besteht_aus
values(108 ,55)
insert into Menue_besteht_aus
values(109 ,55)
insert into Menue_besteht_aus
values(102 ,66)
insert into Menue_besteht_aus
values(104 ,66)
insert into Menue_besteht_aus
values(113 ,66)
insert into Menue_besteht_aus
values(102 ,77)
insert into Menue_besteht_aus
values(105 ,77)
insert into Menue_besteht_aus
values(114 ,77)
go
set dateformat dmy
insert into serviert
values
    (22 ,'5.11.2007')
insert into serviert
values
    (11 ,'5.11.2007')
insert into serviert
values
    (44 ,'6.11.2007')
insert into serviert
values
    (33 ,'6.11.2007')
insert into serviert
values
    (66 ,'7.11.2007')
insert into serviert
values
    (55 ,'7.11.2007')
insert into serviert
values
    (77 ,'8.11.2007')
insert into serviert
values
    (11 ,'8.11.2007')


