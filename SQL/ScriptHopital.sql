zu%8BwjW​CREATE TABLE Patients (
  id INT PRIMARY KEY,
  nom VARCHAR(50),
  prenom VARCHAR(50),
  age INT,
  adresse VARCHAR(100),
  telephone VARCHAR(15)
);

CREATE TABLE Visites (
  id INT IDENTITY PRIMARY KEY,
  idpatient INT FOREIGN KEY REFERENCES Patients(id),
  date DATETIME,
  medecin VARCHAR(50),
  num_salle INT,
  tarif DECIMAL(5,2)
);

CREATE TABLE Authentification (
  id INT PRIMARY KEY,
  login VARCHAR(50),
  password VARCHAR(50),
  nom VARCHAR(50),
  metier INT 
);