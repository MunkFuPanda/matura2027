-- Delete existing test data
DELETE FROM PRODUCT WHERE ID < 0;

-- Insert cosmetic products for Radiant Skin shop
INSERT INTO PRODUCT(id, name, description, imageName, price, validfrom, validto)
 VALUES (-1, 'Feuchtigkeitscreme', 'Intensive Feuchtigkeitspflege für trockene Haut mit Hyaluronsäure', 'cream.jpg', 24.99, '2024-01-01', '2099-12-31');

INSERT INTO PRODUCT(id, name, description, imageName, price, validfrom, validto)
 VALUES (-2, 'Gesichtsserum', 'Anti-Aging Serum mit Vitamin C und E für strahlende Haut', 'serum.jpg', 39.99, '2024-01-01', '2099-12-31');

INSERT INTO PRODUCT(id, name, description, imageName, price, validfrom, validto)
 VALUES (-3, 'Reinigungsgel', 'Sanftes Reinigungsgel für alle Hauttypen', 'cleanser.jpg', 15.99, '2024-01-01', '2099-12-31');

INSERT INTO PRODUCT(id, name, description, imageName, price, validfrom, validto)
 VALUES (-4, 'Tagescreme LSF 30', 'Tagesschutz mit UV-Filter für gesunde Haut', 'daycream.jpg', 29.99, '2024-01-01', '2099-12-31');

INSERT INTO PRODUCT(id, name, description, imageName, price, validfrom, validto)
 VALUES (-5, 'Nachtcreme', 'Regenerierende Nachtpflege mit Retinol', 'nightcream.jpg', 34.99, '2024-01-01', '2099-12-31');

INSERT INTO PRODUCT(id, name, description, imageName, price, validfrom, validto)
 VALUES (-6, 'Peeling-Maske', 'Porentiefe Reinigung mit natürlichen Enzymen', 'mask.jpg', 19.99, '2024-01-01', '2099-12-31');

INSERT INTO PRODUCT(id, name, description, imageName, price, validfrom, validto)
 VALUES (-7, 'Augencreme', 'Reduziert dunkle Ringe und Schwellungen', 'eyecream.jpg', 27.99, '2024-01-01', '2099-12-31');

INSERT INTO PRODUCT(id, name, description, imageName, price, validfrom, validto)
 VALUES (-8, 'Gesichtswasser', 'Tonisierendes Gesichtswasser mit Rosenwasser', 'toner.jpg', 12.99, '2024-01-01', '2099-12-31');

