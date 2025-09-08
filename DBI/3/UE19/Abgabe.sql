use books;

-- 1.
-- haben wir noch nicht gelernt??
select case
           when b.publication_year between 1800 and 1899 then '1800-1899'
           when b.publication_year between 1900 and 1999 then '1900-1999'
           when b.publication_year > 2000 then '>2000'
           end               as zeitraum,
       coalesce(count(*), 0) as summe_der_entlehnten_buecher
from borrowings_books bb
         inner join books b
                    on bb.book_id = b.book_id
where b.publication_year between 1800 and 1899
   or b.publication_year between 1900 and 1999
   or b.publication_year > 2000
group by case
             when b.publication_year between 1800 and 1899 then '1800-1899'
             when b.publication_year between 1900 and 1999 then '1900-1999'
             when b.publication_year > 2000 then '>2000'
             end;

-- 2.
select datediff(day, startdate, enddate)   as tage,
       datediff(month, startdate, enddate) as monate,
       datediff(year, startdate, enddate)  as jahre
from borrowings
order by tage desc;

-- 3.
select br.startdate                                        as startdatum,
       br.enddate                                          as enddatum,
       coalesce(string_agg(b.title, ', '), 'Keine Bücher') as liste_der_titel
from borrowings br
         left join borrowings_books bb on br.borrowing_id = bb.borrowing_id
         left join books b on bb.book_id = b.book_id
group by br.startdate, br.enddate;

-- 4.
select title,
       publication_date,
       genre,
       substring(isbn, charindex('-', coalesce(isbn, '')) + 1, len(isbn)) as isbn,
       page_count,
       language,
       (case
            when publication_year <= 1899 then 'Historisch'
            else 'Neuzeit'
           end)                                                           as zeit
from books