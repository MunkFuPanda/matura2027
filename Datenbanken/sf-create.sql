/* S c h u l u n g s f i r m a - c r e a t e . s q l MSSQLServer */  

DROP TABLE if exists besucht;
DROP TABLE if exists kveranst;
DROP TABLE if exists geeignet;
DROP TABLE if exists setztvor;
DROP TABLE if exists kurs;
DROP TABLE if exists referent;
DROP TABLE if exists person;

CREATE TABLE person (
  pnr INTEGER PRIMARY KEY,
  fname VARCHAR(16),
  vname VARCHAR(16),
  ort VARCHAR(10),
  land CHAR(3) CHECK (land IN ('A','D','F','GB','I','RUS'))
);

CREATE TABLE referent (
  pnr INTEGER PRIMARY KEY REFERENCES person,
  gebdat DATE,
  seit DATE,
  titel VARCHAR(6),
  CHECK (gebdat<seit)
);

CREATE TABLE kurs (
  knr INTEGER PRIMARY KEY,
  bezeichn CHAR(20),
  tage INTEGER CHECK (tage BETWEEN 1 AND 10),
  preis DECIMAL(7,2)
);

CREATE TABLE setztvor (
  knr INTEGER REFERENCES kurs,
  knrvor INTEGER REFERENCES kurs,
  PRIMARY KEY (knr,knrvor),
  CHECK (knr<>knrvor)
);

CREATE TABLE geeignet (
  knr INTEGER REFERENCES kurs,
  pnr INTEGER REFERENCES referent,
  PRIMARY KEY (knr,pnr)
);

CREATE TABLE kveranst (
  knr INTEGER REFERENCES kurs,
  knrlfnd INTEGER,
  von DATE,
  bis DATE,
  ort VARCHAR(10),
  plaetze INTEGER,
  pnr INTEGER REFERENCES referent,
  -- nicht NOT NULL, da 0 bei (0,1)
  PRIMARY KEY (knr,knrlfnd),
  CHECK (von<=bis)
);

CREATE TABLE besucht (
  knr INTEGER,
  knrlfnd INTEGER,
  pnr INTEGER REFERENCES person,
  bezahlt DATE,
  PRIMARY KEY (knr,knrlfnd,pnr),
  FOREIGN KEY (knr,knrlfnd) REFERENCES kveranst
);
