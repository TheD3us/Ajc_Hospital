using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Hospital
    {
        // singleton
        private static Hospital hopital = null;

        private List<DllPatient.Model.Patient> fileAttente = new List<DllPatient.Model.Patient>();

        private List<IObserverMedecin> medecins = new List<IObserverMedecin>();
        private List<IObserverSecretaire> secretaires = new List<IObserverSecretaire>();

        private Hospital() { }

        public static Hospital Hopital
        {
            get
            {
                if (hopital == null)
                {
                    hopital = new Hospital();
                }
                return hopital;
            }
        }

        public void AjouterPatient(DllPatient.Model.Patient p)
        {
            fileAttente.Add(p);
            Console.WriteLine($"Patient {p.Nom} {p.Prenom} ajouté à la file.");

            NotifierMedecins();
            NotifierSecretaires();
        }

        public DllPatient.Model.Patient EntrerProchainPatient()
        {

            if (fileAttente.Count == 0) return null;

            var patient = fileAttente[0];
            fileAttente.RemoveAt(0);

            Console.WriteLine($"Le patient {patient.Nom} {patient.Prenom} est entré en salle.");

            NotifierMedecins();
            NotifierSecretaires();

            return patient;
        }

        public DllPatient.Model.Patient ProchainPatient()
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

