--Password alex123
INSERT INTO User (id, firstname, lastname, username, password, salary, age) VALUES (-1, 'Alex','Knr', 'alex123','$2a$04$4vwa/ugGbBVDvbWaKUVZBuJbjyQyj6tqntjSmG8q.hi97.xSdhj/2', 3456, 33);
INSERT INTO User (id, firstname, lastname, username, password, salary, age)  VALUES (-2, 'Tom', 'Asr', 'tom234', '$2a$04$QED4arFwM1AtQWkR3JkQx.hXxeAk/G45NiRd3Q4ElgZdzGYCYKZOW', 7823, 23);
INSERT INTO User (id, firstname, lastname, username, password, salary, age)  VALUES (-3, 'Adam', 'Psr', 'adam', '$2a$04$WeT1SvJaGjmvQj34QG8VgO9qdXecKOYKEDZtCPeeIBSTxxEhazNla', 4234, 45);

INSERT INTO Product (productid, imagename, price, productName, validfrom, validuntil) VALUES (-1, 'SoftwareSuper.img', 20.2, 'SofwareSuper', '2022-10-10', '2022-10-12');
INSERT INTO Product (productid, imagename, price, productName, validfrom, validuntil) VALUES (-2, 'WeySoft.img', 31.99, 'WeySoft', '2022-10-1', '2022-10-2');
INSERT INTO Product (productid, imagename, price, productName, validfrom, validuntil) VALUES (-3, 'TestSoft.img', 25.09, 'TestSoft', '2022-10-3', '2022-10-4');
--Tipps: 1) Vergib ids mit negativen Zahlen
--       2) Datum im Format '2022-10-10'
