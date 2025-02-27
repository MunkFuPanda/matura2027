use books;

-- 1.
-- cleanup before
delete
from borrowings;

insert into borrowings (startdate, enddate, customer)
values (current_timestamp, current_timestamp + day(7), 'John Doe'),
       (current_timestamp, current_timestamp + day(14), 'John Doe'),
       (current_timestamp, current_timestamp + day(21), 'Jane Doe'),
       (current_timestamp, current_timestamp + day(28), 'Jane Doe'),
       (current_timestamp, current_timestamp + day(35), 'Steve Doe'),
       (current_timestamp, '2025-03-01', 'Steve Doe'),
       (current_timestamp, '2025-03-02', 'Mike Doe'),
       (current_timestamp, '2025-03-03', 'Mike Doe'),
       (current_timestamp, '2025-03-04', 'Jenny Doe'),
       (current_timestamp, '2025-03-05', 'Jenny Doe')
;

-- 2.
insert into borrowings_books (borrowing_id, book_id)
values (11, 1),
       (11, 2),
       (12, 3),
       (12, 4),
       (12, 5),
       (13, 6),
       (14, 7),
       (14, 8),
       (14, 9),
       (14, 10),
       (15, 11),
       (15, 12),
       (16, 13),
       (16, 14),
       (16, 15),
       (16, 16),
       (16, 17),
       (17, 18),
       (17, 19),
       (17, 20),
       (18, 21),
       (18, 22),
       (19, 23),
       (20, 1),
       (20, 4),
       (20, 7);

-- 3.
update books
set title = 'War and Peace'
where book_id = 5;

-- 4.
update books
set borrowings_count = (select count(*) from borrowings_books where book_id = books.book_id),
    updated_at       = current_timestamp;

-- borrowings_count verstößt gegen die integrität der Datenbank

-- 5.
-- alle datensätze in welcher tabelle??

delete
from borrowings_books
where book_id in (select books.book_id
                  from books
                  where books.publisher_id in (select publishers.publisher_id from publishers where name like '%u%'));

delete
from books
where books.publisher_id in (select publishers.publisher_id from publishers where name like '%u%');

delete
from publishers
where name like '%u%';
