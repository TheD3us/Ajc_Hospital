using DllPatient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

        public class EtatHopital
        {
            public List<Patient> FileAttente { get; set; }
        }

        public void SauvegarderEtat(string cheminFichier)
        {
            try
            {
                var etat = new EtatHopital
                {
                    FileAttente = fileAttente
                };

                using (var fs = new FileStream(cheminFichier, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(EtatHopital));
                    serializer.Serialize(fs, etat);
                }

                Console.WriteLine("[Hopital] État sauvegardé.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde : {ex.Message}");
            }

        }
        public void ChargerEtat(string cheminFichier)
        {
            try
            {
                if (!File.Exists(cheminFichier))
                {
                    Console.WriteLine("[Hopital] Aucun fichier de sauvegarde trouvé. Nouvelle file d’attente.");
                    return;
                }

                using (var fs = new FileStream(cheminFichier, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(EtatHopital));
                    var etat = (EtatHopital)serializer.Deserialize(fs);

                    fileAttente = etat.FileAttente ?? new List<Patient>();
                }

                Console.WriteLine("[Hopital] État restauré depuis le fichier.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement : {ex.Message}");
            }
        }

    }
}
