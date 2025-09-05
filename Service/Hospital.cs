using DAO;
using DllAuthentification.Model;
using DllMedicament.Model;
using DllPatient.Model;
using DllVisites;
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
            LogArriveePatient(p);
            NotifierMedecins();
            NotifierSecretaires();
        }

        //Ajout d'une trace dans fichier texte
        public void LogArriveePatient(Patient patient)
        {
            string chemin = "../../../patients.txt";
            string ligne = $"{patient.Id}\t{DateTime.Now:dd/MM/yyyy HH:mm}";
            File.AppendAllLines(chemin, new[] { ligne });
            Console.WriteLine($"Arrivée enregistrée dans {chemin} : {ligne}");
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

        //Modifier Patient
        public void ModifierPatientParSecretaire()
        {
            string Telephone=null;
            string Adresse=null;
            int Id;
            int choix=-1;
            Console.WriteLine("Veuillez entrer le numéro de sécurité sociale du patient à modifier");
            Console.WriteLine("0 - revenir au menu précédent");
            Id = Convert.ToInt32(Console.ReadLine());
            if (Id != 0 && new PatientDao().PatientExists(Id))
            {
                while(choix != 0)
                {
                    Console.WriteLine("Vous souhaitez modifier :");
                    Console.WriteLine("1 - Le téléphone");
                    Console.WriteLine("2 - L'adresse");
                    Console.WriteLine("0 - Quitter et valider");
                    choix = Convert.ToInt32(Console.ReadLine());
                    switch(choix)
                    {
                        case 1:
                            Console.WriteLine("Nouveau téléphone :");
                            Telephone = Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine("Nouvelle adresse :");
                            Adresse = Console.ReadLine();
                            break;
                        case 0:
                            break;
                        default:
                            Console.WriteLine("Choix erroné");
                            break;
                    }
                }
                Console.WriteLine("Modifications effectuées");
                new PatientDao().UpdatePatientParSecretaire(Telephone, Adresse, Id);
            }
            if (Id != 0 && !new PatientDao().PatientExists(Id))
            {
                Console.WriteLine("Ce patient est absent de la base");
            }
        }

        //HistoriquePatient
        public void VoirHistoriqueVisitesPatient()
        {
            int IdPatient;
            List<Visites> ListeHistorique = new List<Visites>();
            Console.WriteLine("Choisir le patient dont vous voulez voir l'historique");
            Console.WriteLine("0 - Menu précedent");
            IdPatient = Convert.ToInt32(Console.ReadLine());
            if (IdPatient != 0 && new PatientDao().PatientExists(IdPatient))
            {
                ListeHistorique = new VisiteDao().SelectVisitesByPatient(IdPatient);
                foreach (Visites v in ListeHistorique)
                {
                    Patient p = new PatientDao().GetPatientById(v.IdPatient);

                    Console.WriteLine(p.Nom + " " + p.Prenom + " - " + v.Date + " tarif : " + v.Tarif);
                }
            }
            if (IdPatient != 0 && !new PatientDao().PatientExists(IdPatient))
            {
                Console.WriteLine("Ce patient est absent de la base");
            }


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

        public void ListeVisiteParDate()
        {
            Console.WriteLine("Veuillez saisir le numéro de sécurité sociale du patient");
            Console.WriteLine("0 - Menu Précedent");
            int NumSecu = Convert.ToInt32(Console.ReadLine());
            Patient patient = new PatientDao().GetPatientById(NumSecu);
            if (NumSecu != 0)
            {
                if (patient == null)
                {
                    Console.WriteLine("Le numéro {patient.id} ne correspond à aucun patient");
                }
                else
                {
                    List<Visites> listDate = new VisiteDao().GetVisitesByPatientOrderByDate(NumSecu);
                    foreach (var v in listDate)
                        Console.WriteLine($"{v.Date} - Médecin {v.Medecin} - Salle {v.NumSalle} - Tarif {v.Tarif}");
                }
            }
            
        }

        public void ListeVisiteParMedecin()
        {
            Console.WriteLine("Veuillez saisir le numéro de sécurité sociale du patient");
            Console.WriteLine("0 - Menu Précedent");
            int NumSecu = Convert.ToInt32(Console.ReadLine());
            if (NumSecu != 0)
            {
                Patient patient = new PatientDao().GetPatientById(NumSecu);
                if (patient == null)
                {
                    Console.WriteLine("Le numéro {patient.id} ne correspond à aucun patient");
                }
                else
                {
                    List<Visites> listMedecin = new VisiteDao().GetVisitesByPatientOrderByMedecin(NumSecu);
                    foreach (var v in listMedecin)
                        Console.WriteLine($"Médecin {v.Medecin} - {v.Date} - Salle {v.NumSalle} - Tarif {v.Tarif}");
                }
            }
        }

        public void NombreTotalDeVisite()
        {
            Console.WriteLine("Veuillez saisir le numéro de sécurité sociale du patient");
            Console.WriteLine("0 - Menu Précedent");
            int NumSecu = Convert.ToInt32(Console.ReadLine());
            if (NumSecu != 0)
            {
                Patient patient = new PatientDao().GetPatientById(NumSecu);
                if (patient == null)
                {
                    Console.WriteLine("Le numéro {patient.id} ne correspond à aucun patient");
                }
                else
                {
                    int total = new VisiteDao().CountVisitesByPatient(NumSecu);
                    Console.WriteLine($"Total visites : {total}");
                }
            }
        }

        public void NombreVisiteEntreDeuxDate()
        {
            Console.WriteLine("Veuillez saisir le numéro de sécurité sociale du patient");
            Console.WriteLine("0 - Menu Précedent");
            int NumSecu = Convert.ToInt32(Console.ReadLine());
            if (NumSecu != 0)
            {
                Patient patient = new PatientDao().GetPatientById(NumSecu);
                if (patient == null)
                {
                    Console.WriteLine("Le numéro {patient.id} ne correspond à aucun patient");
                }
                else
                {
                    Console.Write("Date début (yyyy-MM-dd) : ");
                    DateTime d1 = DateTime.Parse(Console.ReadLine());
                    Console.Write("Date fin (yyyy-MM-dd) : ");
                    DateTime d2 = DateTime.Parse(Console.ReadLine());
                    int count = new VisiteDao().CountVisitesByPatientBetweenDates(NumSecu, d1, d2);
                    Console.WriteLine($"Visites entre {d1} et {d2} : {count}");
                }
            }
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
