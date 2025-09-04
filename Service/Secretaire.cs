using DllPatient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Secretaire : IObserverSecretaire
    {
        public string Nom { get; }

        public Secretaire(string nom)
        {
            Nom = nom;
        }

<<<<<<< HEAD
        public void NotifierChangementFile(List<Patient> fileAttente)
=======
        public void NotifierChangementFile(List<DllPatient.Model.Patient> fileAttente)
>>>>>>> 57421f9d3f1a5bc72f4d6fd3783ac4bd730af351
        {
            Console.WriteLine($"\n[Secrétaire {Nom}] Mise à jour de la file :");
            if (fileAttente.Count == 0)
            {
                Console.WriteLine(" - Aucun patient en attente.");
            }
            else
            {
                foreach (var patient in fileAttente)
                {
                    Console.WriteLine($" - {patient.Id}: {patient.Nom} {patient.Prenom}, {patient.Age} ans");
                }
            }
        }

        //public void LogArriveePatient(Patient.Patient patient)
        //{
        //    string chemin = "patients.txt";
        //    string ligne = $"{patient.Id}\t{DateTime.Now:dd/MM/yyyy HH:mm}";
        //    File.AppendAllLines(chemin, new[] { ligne });
        //    Console.WriteLine($"[Secrétaire {Nom}] Arrivée enregistrée dans {chemin} : {ligne}");
        //}
    }
}
