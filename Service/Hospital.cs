using DllPatient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Hospital
    {
        // singleton
        static Hospital hopital;

        private List<Patient> fileAttente = new List<Patient>();

        private List<IObserverMedecin> medecins = new List<IObserverMedecin>();
        private List<IObserverSecretaire> secretaires = new List<IObserverSecretaire>();

        protected Hospital() { }

        public static Hospital Hopital()
        {
            if (hopital == null)
            {
                hopital = new Hospital();
            }
            return hopital;

        }

        public void AjouterPatient(Patient p)
        {
            fileAttente.Add(p);
            Console.WriteLine($"Patient {p.Nom} {p.Prenom} ajouté à la file.");

            NotifierMedecins();
            NotifierSecretaires();
        }

        public void AfficherFile()
        {
            if (fileAttente.Count == 0)
            {
                Console.WriteLine("La file d’attente est vide.");
                return;
            }

            Console.WriteLine("File d’attente actuelle :");
            foreach (var p in fileAttente)
            {
                Console.WriteLine($" - {p.Id} : {p.Nom} {p.Prenom}, {p.Age} ans, Tel={p.Telephone}, Adresse={p.Adresse}");
            }
        }

        public Patient EntrerProchainPatient()
        {

            if (fileAttente.Count == 0) return null;

            var patient = fileAttente[0];
            fileAttente.RemoveAt(0);

            Console.WriteLine($"Le patient {patient.Nom} {patient.Prenom} est entré en salle.");

            NotifierMedecins();
            NotifierSecretaires();

            return patient;
        }

        public Patient ProchainPatient()
        {
            if (fileAttente.Count == 0)
            {
                Console.WriteLine("Aucun patient dans la file d’attente.");
                return null;
            }

            // Récupérer le premier patient de la file
            DllPatient.Model.Patient prochain = fileAttente[0];

            Console.WriteLine($"Prochain patient : {prochain.Id} - {prochain.Nom} {prochain.Prenom}, {prochain.Age} ans");

            // Retourner le patient (sans le retirer de la liste)
            return prochain;
        }

        public void AddMedecin(IObserverMedecin medecin)
        {
            medecins.Add(medecin);
        }

        public void AddSecretaire(IObserverSecretaire secretaire)
        {
            secretaires.Add(secretaire);
        }

        private void NotifierMedecins()
        {
            var prochain = ProchainPatient();
            foreach (var m in medecins)
                m.NotifierNouveauPatient(prochain);
        }

        private void NotifierSecretaires()
        {
            foreach (var s in secretaires)
                s.NotifierChangementFile(new List<DllPatient.Model.Patient>(fileAttente));
        }
    }
}
