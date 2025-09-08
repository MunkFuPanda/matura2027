--create database books;
use books

drop table if exists borrowings_books;
drop table if exists books;
drop table if exists publishers;
drop table if exists authors;
drop table if exists borrowings;


CREATE TABLE authors (
    author_id INT PRIMARY KEY IDENTITY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    birth_date DATE,
    death_date DATE DEFAULT NULL,
    nationality VARCHAR(50),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME 
);

CREATE TABLE publishers (
    publisher_id INT PRIMARY KEY IDENTITY,
    name VARCHAR(100) NOT NULL,
    founded_date DATE,
    country VARCHAR(50),
    website VARCHAR(255),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME
);

CREATE TABLE books (
    book_id INT PRIMARY KEY IDENTITY,
    title VARCHAR(255) NOT NULL,
	borrowings_count INT DEFAULT NULL,
    publication_year INT,
    genre VARCHAR(50),
    isbn VARCHAR(20) UNIQUE,
    page_count INT,
    publication_date DATE,
    language VARCHAR(50) DEFAULT 'English',
    author_id INT,
    publisher_id INT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME,
    FOREIGN KEY (author_id) REFERENCES authors(author_id),
    FOREIGN KEY (publisher_id) REFERENCES publishers(publisher_id)
);

create table borrowings (
	borrowing_id INT PRIMARY KEY IDENTITY,
	startdate DATETIME,
	enddate DATETIME,
	customer varchar(100)
);

create table borrowings_books (
	borrowing_id INT REFERENCES borrowings,
	book_id INT REFERENCES books,
	primary key (borrowing_id, book_id)
);




INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('J.K.', 'Rowling', '1965-07-31', 'British');
INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('George', 'Orwell', '1903-06-25', 'British');
INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('Jane', 'Austen', '1775-12-16', 'British');
INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('Mark', 'Twain', '1835-11-30', 'American');
INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('Leo', 'Tolstoy', '1828-09-09', 'Russian');
INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('Charles', 'Dickens', '1812-02-07', 'British');
INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('Gabriel', 'García Márquez', '1927-03-06', 'Colombian');
INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('Haruki', 'Murakami', '1949-01-12', 'Japanese');
INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('J.R.R.', 'Tolkien', '1892-01-03', 'British');
INSERT INTO authors (first_name, last_name, birth_date, nationality) VALUES ('Agatha', 'Christie', '1890-09-15', 'British');

INSERT INTO publishers (name, founded_date, country, website) VALUES ('Penguin Random House', '1925-07-01', 'USA', 'https://www.penguinrandomhouse.com');
INSERT INTO publishers (name, founded_date, country, website) VALUES ('HarperCollins', '1817-01-01', 'USA', 'https://www.harpercollins.com');
INSERT INTO publishers (name, founded_date, country, website) VALUES ('Simon & Schuster', '1924-01-02', 'USA', 'https://www.simonandschuster.com');
INSERT INTO publishers (name, founded_date, country, website) VALUES ('Macmillan Publishers', '1843-01-01', 'UK', 'https://us.macmillan.com');
INSERT INTO publishers (name, founded_date, country, website) VALUES ('Hachette Livre', '1826-01-01', 'France', 'https://www.hachette.com');
INSERT INTO publishers (name, founded_date, country, website) VALUES ('Scholastic Corporation', '1920-10-22', 'USA', 'https://www.scholastic.com');
INSERT INTO publishers (name, founded_date, country, website) VALUES ('Bloomsbury', '1986-01-01', 'UK', 'https://www.bloomsbury.com');
INSERT INTO publishers (name, founded_date, country, website) VALUES ('Springer Nature', '1842-05-10', 'Germany', 'https://www.springernature.com');
INSERT INTO publishers (name, founded_date, country, website) VALUES ('Wiley', '1807-01-01', 'USA', 'https://www.wiley.com');
INSERT INTO publishers (name, founded_date, country, website) VALUES ('Pearson', '1844-01-01', 'UK', 'https://www.pearson.com');

INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Harry Potter and the Sorcerer''s Stone', 1997, 'Fantasy', '978-0747532699', 223, '1997-06-26', 'English', 1, 1);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('1984', 1949, 'Dystopian', '978-0451524935', 328, '1949-06-08', 'English', 2, 2);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Pride and Prejudice', 1813, 'Romance', '978-1503290563', 279, '1813-01-28', 'English', 3, 3);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('The Adventures of Tom Sawyer', 1876, 'Adventure', '978-0486400778', 224, '1876-04-30', 'English', 4, 4);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Peace AND War', 1869, 'Historical Fiction', '978-0199232765', 1225, '1869-01-01', 'Russian', 5, 5);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Great Expectations', 1861, 'Classic', '978-0141439563', 505, '1861-08-01', 'English', 6, 1);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('One Hundred Years of Solitude', 1967, 'Magical Realism', '978-0060883287', 417, '1967-06-05', 'Spanish', 7, 6);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Norwegian Wood', 1987, 'Drama', '978-0375704024', 296, '1987-09-04', 'Japanese', 8, 7);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('The Hobbit', 1937, 'Fantasy', '978-0547928227', 310, '1937-09-21', 'English', 9, 8);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Murder on the Orient Express', 1934, 'Mystery', '978-0062073501', 256, '1934-01-01', 'English', 10, 9);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Harry Potter and the Chamber of Secrets', 1998, 'Fantasy', '978-0747538493', 251, '1998-07-02', 'English', 1, 1);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Harry Potter and the Prisoner of Azkaban', 1999, 'Fantasy', '978-0747542155', 317, '1999-07-08', 'English', 1, 1);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Animal Farm', 1945, 'Political Satire', '978-0451526342', 112, '1945-08-17', 'English', 2, 2);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Sense and Sensibility', 1811, 'Romance', '978-1503290310', 374, '1811-10-30', 'English', 3, 3);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('The Prince and the Pauper', 1881, 'Historical Fiction', '978-0486453217', 192, '1881-11-01', 'English', 4, 4);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Anna Karenina', 1877, 'Drama', '978-0143035008', 864, '1877-01-01', 'Russian', 5, 5);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('David Copperfield', 1850, 'Classic', '978-0140439441', 768, '1850-05-01', 'English', 6, 1);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Love in the Time of Cholera', 1985, 'Romance', '978-0307389732', 368, '1985-09-15', 'Spanish', 7, 6);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('Kafka on the Shore', 2002, 'Magical Realism', '978-1400079278', 467, '2002-10-15', 'Japanese', 8, 7);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('The Lord of the Rings: The Fellowship of the Ring', 1954, 'Fantasy', '978-0547928210', 423, '1954-07-29', 'English', 9, 8);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('The Lord of the Rings: The Two Towers', 1954, 'Fantasy', '978-0547928203', 352, '1954-11-11', 'English', 9, 8);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('The Lord of the Rings: The Return of the King', 1955, 'Fantasy', '978-0547928197', 416, '1955-10-20', 'English', 9, 8);
INSERT INTO books (title, publication_year, genre, isbn, page_count, publication_date, language, author_id, publisher_id) VALUES ('The ABC Murders', 1936, 'Mystery', '978-0062073556', 256, '1936-01-06', 'English', 10, 9);

select *
from books;

select *
from authors;

select *
from publishers;