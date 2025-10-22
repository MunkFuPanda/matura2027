use Autohaendler;

/* Variante 1 */
select *
from Haendler h
where not exists (
	select *
	from AutoMarke am
	where not exists (
		select * 
		from haendler_verkauft_automarke hva
		where (hva.autoMarke_id = am.autoMarke_id) and (hva.haendler_id = h.haendler_id)
	)
)


/* Variante 2 */
select *
from Haendler h
where not exists ( 
	(
		select am.autoMarke_id
		from AutoMarke am
	)
	except
	(
		select hva.autoMarke_id
		from haendler_verkauft_automarke hva
		where hva.haendler_id = h.haendler_id
	)
)


/* Variante 3 */
select h.haendler_id
from Haendler h
	join haendler_verkauft_automarke hva on h.haendler_id = hva.haendler_id
	join AutoMarke am on hva.autoMarke_id = am.autoMarke_id
group by h.haendler_id
having count(*) = (
	select count(*)
	from AutoMarke
)


/* Variante 4*/
(
	select h.haendler_id
	from Haendler h
)
except
(
	select inner_erg.haendler_id
	from (
		(
			select inner_h.haendler_id, am.autoMarke_id
			from Haendler inner_h, AutoMarke am
		)
		except
		(
			select hva.haendler_id, hva.autoMarke_id
			from haendler_verkauft_automarke hva
		)
	) inner_erg
)
