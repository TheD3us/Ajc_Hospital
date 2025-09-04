using DllPatient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Medecin : IObserverMedecin
    {
        public string Nom { get; }
        public int Salle { get; }

        public Medecin(string nom, int salle)
        {
            Nom = nom;
            Salle = salle;
        }

<<<<<<< HEAD
        public void NotifierNouveauPatient(Patient patient)
=======
        public void NotifierNouveauPatient(DllPatient.Model.Patient patient)
>>>>>>> 57421f9d3f1a5bc72f4d6fd3783ac4bd730af351
        {
            if (patient == null)
                Console.WriteLine($"[Médecin {Nom} - Salle {Salle}] Aucun patient en attente.");
            else
                Console.WriteLine($"[Médecin {Nom} - Salle {Salle}] Prochain patient : {patient.Nom} {patient.Prenom}");
        }
    }
}
