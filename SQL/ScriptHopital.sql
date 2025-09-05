CREATE TABLE Patients(
id INT PRIMARY KEY, 
nom VARCHAR(50)NOT NULL,
prenom VARCHAR(50)NOT NULL,
age INT NOT NULL,
adresse VARCHAR(50),
telephone VARCHAR(50)
);

CREATE TABLE Authentification(
id INT IDENTITY PRIMARY KEY,
login VARCHAR(50)NOT NULL UNIQUE,
password VARCHAR(50)NOT NULL,
nom VARCHAR(50)NOT NULL,
metier INT NOT NULL 
);

CREATE TABLE Visites(
id INT IDENTITY PRIMARY KEY,
idpatient INT NOT NULL,
date DATETIME NOT NULL DEFAULT GETDATE(),
medecin INT NOT NULL,
num_salle INT NOT NULL,
tarif FLOAT(10) NOT NULL DEFAULT 23,
FOREIGN KEY (idpatient)REFERENCES Patients(id),
FOREIGN KEY (medecin)REFERENCES Authentification(id)
);


INSERT INTO Patients (id, nom, prenom, age, adresse, telephone) VALUES
(1, 'Durant', 'Blard', 45, '12 rue de la lampisterie', '0601020304'),
(2, 'Jean', 'Paul', 50, '5 avenue Victor Hugo', '0605060708'),
(3, 'Martin', 'Sophie', 32, '8 place de la Republique', '0611223344');


-- metier : 0 = secrétaire, 1 = médecin salle 1, 2 = médecin salle 2
INSERT INTO Authentification (login, password, nom, metier) VALUES
('sec1', 'pass123', 'Marie', 0),
('med1', 'pass123', 'Dupont', 1),
('med2', 'pass123', 'Dupond', 2);


INSERT INTO Visites (idpatient, date, medecin, num_salle, tarif) VALUES
(1, '2025-09-01', 2, 1, 23),
(2, '2025-09-02', 3, 2, 23),
(3, '2025-09-03', 2, 1, 23);



​INSERT INTO Authentification (login, password, nom, metier) VALUES
('admin1', 'pass123', 'Premier', -1),
('admin2', 'pass123', 'Secondaire', -1);

CREATE TABLE Medicaments (
    idMedicaments INT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prix INT NOT NULL,
    quantite INT NOT NULL
);

INSERT INTO Medicaments (idMedicaments, nom, prix, quantite) VALUES
(1, 'Paracétamol 500mg', 2, 100),
(2, 'Ibuprofène 200mg', 3, 80),
(3, 'Amoxicilline 500mg', 5, 50);