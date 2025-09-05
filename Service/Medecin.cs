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

        public void NotifierNouveauPatient(Patient patient)
        {
            if (patient == null)
                Console.WriteLine($"[Médecin {Nom} - Salle {Salle}] Aucun patient en attente.");
            else
                Console.WriteLine($"[Médecin {Nom} - Salle {Salle}] Prochain patient : {patient.Nom} {patient.Prenom}");
        }

    }
}
