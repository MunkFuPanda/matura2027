use wichtel;

-- 1.
select
	concat(santa.Vorname, ' ', santa.Nachname) as 'Secret Santa',
	concat(schueler.Vorname, ' ', schueler.Nachname) as 'Schüler',
	Geschenk.Name as Geschenk
from
	Wichtel
	inner join Geschenk
	on Wichtel.GeschenkID = Geschenk.ID
	inner join Schueler as santa
	on Wichtel.VonID = santa.ID
	inner join Schueler as schueler
	on Wichtel.FuerID = schueler.ID

-- 2.
select
	Schueler.Vorname,
	Schueler.Nachname
from
	Wichtel
	inner join Schueler
	on Wichtel.VonID = Schueler.ID
where Wichtel.VonID = Wichtel.FuerID

-- 3.
select
	Schueler.Vorname,
	Schueler.Nachname
from
	Schueler
	left join Wichtel
	on Schueler.ID = Wichtel.FuerID
where Wichtel.FuerID is null

-- 4.
select
	Schueler.Vorname,
	Schueler.Nachname,
	count(Wichtel.VonID) as '# Geschenke'
from
	Schueler
	join Wichtel
	on Schueler.ID = Wichtel.VonID
group by Schueler.Vorname, Schueler.Nachname
having count(Wichtel.VonID) >= 2

-- 5.
select
	Geschenk.Name,
	count(Wichtel.GeschenkID) as 'Anzahl'
from
	Geschenk
	join Wichtel
	on Geschenk.ID = Wichtel.GeschenkID
group by Geschenk.Name

-- 6.
select
	Klasse.Name,
	count(Wichtel.VonID) as '# Geschenke'
from
	Klasse
	join Schueler as schueler
	on Schueler.KlasseID = Klasse.ID
	join Wichtel
	on Schueler.ID = Wichtel.VonID
	join Schueler as schueler2
	on schueler2.ID = Wichtel.FuerID
where schueler2.KlasseID <> Klasse.ID
group by Klasse.Name

-- 7.
select
	top 3
	Geschenk.Name
from
	Geschenk
	join Wichtel
	on Geschenk.ID = Wichtel.GeschenkID
group by Geschenk.Name
order by count(Wichtel.GeschenkID) desc
