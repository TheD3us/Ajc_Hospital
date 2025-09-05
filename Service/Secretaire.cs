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

        public void NotifierChangementFile(List<Patient> fileAttente)
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

        
    }
}