using DAO;
using DllAuthentification.Model;
using DllPatient.Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MenuHopital
{
    class Program
    {

        static void Main(string[] args)
        {
            MenuDepart();
        }

        public static void MenuDepart()
        {
            string login;
            string mdp;
            Authentification auth;

            Console.WriteLine("#######Hopital Saint Mikael ##########");
            Console.WriteLine("Bienvenue à l'hopital, veuillez vous connecter");
            Console.Write("login :");
            login = Console.ReadLine();
            Console.Write("Mot de passe :");
            mdp = Console.ReadLine();
            auth = new AuthentificationDao().Authenticate(login, mdp);
            if(auth == null)
            {
                Console.WriteLine("Mauvais login / mdp");
                MenuDepart();
            }
            else
            {
                switch(auth.Metier)
                {
                    case 0:
                        MenuSecretaire(auth);
                        break;

                    case 1 : case 2:
                        MenuMedecin(auth, null);
                        break;

                    case -1:
                        MenuAdmin(auth);
                        break;

                    default:
                        Console.WriteLine("Metier inconnu --- Deconnexion");
                        auth = null;
                        MenuDepart();
                        break;
                }
            }

        }

        public static void MenuSecretaire(Authentification auth)
        {
            short choix;
            Hospital H = Hospital.Hopital();

            Console.WriteLine("Bonjour " + auth.Nom);
            Console.WriteLine("-------------------Menu Secretaire-------------------");
            Console.WriteLine("1 - Reception patient");
            Console.WriteLine("2 - Afficher file d'attente");
            Console.WriteLine("3 - Afficher prochain patient");
            Console.WriteLine("4 - Deconnexion");

            choix = Convert.ToInt16(Console.ReadLine());
            switch(choix)
            {
                case 1:
                    AjoutPatient(auth);
                    break;
                case 2:
                    H.AfficherFile();
                    MenuSecretaire(auth);
                    break;
                case 3:
                    H.ProchainPatient();
                    MenuSecretaire(auth);
                    break;
                case 4:
                    Console.WriteLine("Au revoir " + auth.Nom);
                    auth = null;
                    MenuDepart();
                    break;
                default:
                    Console.WriteLine("Choix erroné, veuillez recommencer");
                    MenuSecretaire(auth);
                    break;
            }

        }

        public static void AjoutPatient(Authentification auth)
        {
            Hospital H = Hospital.Hopital();
            int NumSecu;
            bool YesNo;
            Console.WriteLine("Veuillez saisir le numéro de sécurité sociale du patient");
            Console.WriteLine("0 - Menu Précedent");
            NumSecu = Convert.ToInt32(Console.ReadLine());
            if (NumSecu == 0)
            {
                MenuSecretaire(auth);
            }
            Patient patient = new PatientDao().GetPatientById(NumSecu);
            if (patient == null)
            {
                Console.WriteLine("Le numéro {patient.id} ne correspond à aucun patient");
                Console.WriteLine("Voulez vous créer un nouveau patient ? Y / N");
                YesNo = (Console.ReadLine().ToUpper() == "Y");
                if (YesNo)
                {
                    patient = new Patient();
                    Console.Write("Nom : "); patient.Nom = Console.ReadLine();
                    Console.Write("Prénom : "); patient.Prenom = Console.ReadLine();
                    Console.Write("Age : "); patient.Age = int.Parse(Console.ReadLine());
                    Console.Write("Adresse : "); patient.Adresse = Console.ReadLine();
                    Console.Write("Téléphone : "); patient.Telephone = Console.ReadLine();
                    patient.Id = NumSecu;
                    new PatientDao().AddPatient(patient);
                    Console.WriteLine("Patient ajouté en base.");
                    H.AjouterPatient(patient);
                    MenuSecretaire(auth);
                }
                else
                {
                    AjoutPatient(auth);
                }
            }
            else
            {
                H.AjouterPatient(patient);
                MenuSecretaire(auth);
            }
        }

        public static void MenuMedecin(Authentification auth, Salle salleAttribuee)
        {
            if (salleAttribuee == null)
            {
                salleAttribuee = new Salle(auth.Metier, auth);
            }
            
            Hospital H = Hospital.Hopital();
            Console.WriteLine("Bonjour Dr. " + auth.Nom);
            Console.WriteLine("-------------------Menu Médecin-------------------");
            Console.WriteLine("1 - Afficher file d’attente");
            Console.WriteLine("2 - Afficher prochain patient");
            Console.WriteLine("3 - Faire entrer patient suivant");
            Console.WriteLine("4 - Libérer Salle");
            Console.WriteLine("5 - Sauvegarder visites");
            Console.WriteLine("6 - Deconnexion");
            Console.Write("Choix : ");
            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    H.AfficherFile();
                    MenuMedecin(auth, salleAttribuee);
                    break;
                case "2":
                    salleAttribuee.AfficherProchainPatient();
                    MenuMedecin(auth, salleAttribuee);
                    break;
                case "3":
                    salleAttribuee.AssignerProchainPatient();
                    MenuMedecin(auth, salleAttribuee);
                    break;
                case "4":
                    salleAttribuee.LibererSalle();
                    MenuMedecin(auth, salleAttribuee);
                    break;
                case "5":
                    salleAttribuee.SauvegarderVisites(null);
                    MenuMedecin(auth, salleAttribuee);
                    break;
                case "6":
                    MenuDepart();
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }

        public static void MenuAdmin(Authentification auth)
        {
            Hospital H = Hospital.Hopital();

            Console.WriteLine("Bonjour Admin " + auth.Nom);
            Console.WriteLine("-------------------Menu Admin-------------------");
            Console.WriteLine("1 - Gérer les Patients");
            Console.WriteLine("2 - Gérer les secrétaires et médecins");
            Console.WriteLine("3 - Déconnexion");
            Console.WriteLine("Choix:");
            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    MenuGestionPatients(auth);
                    break;
                case "2":
                    MenuGestionPersonnel(auth);
                    break;
                case "3":
                    MenuDepart();
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }


        private static void MenuGestionPatients(Authentification auth)
        {
            Console.WriteLine("---- Gestion des Patients ----");
            Console.WriteLine("1 - Supprimer un patient");
            Console.WriteLine("2 - Modifier un patient");
            Console.WriteLine("3 - Ajouter un patient");
            Console.WriteLine("4 - Retour");
            Console.Write("Choix: ");
            var choix = Console.ReadLine();

            var patientDao = new PatientDao();

            switch (choix)
            {
                case "1": 
                    Console.Write("ID du patient à supprimer: ");
                    int idSup = int.Parse(Console.ReadLine());
                    patientDao.DeletePatient(idSup);
                    Console.WriteLine("Patient supprimé.");
                    break;

                case "2":
                    Console.Write("ID du patient à modifier: ");
                    int idModif = int.Parse(Console.ReadLine());
                    var patientModif = patientDao.GetPatientById(idModif);
                    if (patientModif == null)
                    {
                        Console.WriteLine("Patient introuvable.");
                        break;
                    }
                    Console.Write("Nom (" + patientModif.Nom + "): ");
                    patientModif.Nom = Console.ReadLine();
                    Console.Write("Prénom (" + patientModif.Prenom + "): ");
                    patientModif.Prenom = Console.ReadLine();
                    Console.Write("Âge (" + patientModif.Age + "): ");
                    patientModif.Age = int.Parse(Console.ReadLine());
                    Console.Write("Adresse (" + patientModif.Adresse + "): ");
                    patientModif.Adresse = Console.ReadLine();
                    Console.Write("Téléphone (" + patientModif.Telephone + "): ");
                    patientModif.Telephone = Console.ReadLine();
                    patientDao.UpdatePatient(patientModif);
                    Console.WriteLine("Patient mis à jour.");
                    break;

                case "3": 
                    var nouveau = new Patient();
                    Console.Write("ID (Numéro sécu): ");
                    nouveau.Id = int.Parse(Console.ReadLine());
                    Console.Write("Nom: ");
                    nouveau.Nom = Console.ReadLine();
                    Console.Write("Prénom: ");
                    nouveau.Prenom = Console.ReadLine();
                    Console.Write("Âge: ");
                    nouveau.Age = int.Parse(Console.ReadLine());
                    Console.Write("Adresse: ");
                    nouveau.Adresse = Console.ReadLine();
                    Console.Write("Téléphone: ");
                    nouveau.Telephone = Console.ReadLine();
                    patientDao.AddPatient(nouveau);
                    Console.WriteLine("Patient ajouté.");
                    break;

                case "4":
                    MenuAdmin(auth);
                    return;
            }

            MenuGestionPatients(auth);
        }

        private static void MenuGestionPersonnel(Authentification auth)
        {
            Console.WriteLine("---- Gestion du Personnel ----");
            Console.WriteLine("1 - Créer un médecin");
            Console.WriteLine("2 - Créer une secrétaire");
            Console.WriteLine("3 - Supprimer un médecin");
            Console.WriteLine("4 - Supprimer une secrétaire");
            Console.WriteLine("5 - Retour");
            Console.Write("Choix: ");
            var choix = Console.ReadLine();

            var authDao = new AuthentificationDao();

            switch (choix)
            {
                case "1":
                    CreerUtilisateur(authDao, metier: 1);
                    break;
                case "2":
                    CreerUtilisateur(authDao, metier: 0);
                    break;
                case "3":
                    SupprimerUtilisateur(authDao, metier: 1);
                    break;
                case "4":
                    SupprimerUtilisateur(authDao, metier: 0);
                    break;
                case "5":
                    MenuAdmin(auth);
                    return;
            }

            MenuGestionPersonnel(auth);
        }

        private static void CreerUtilisateur(AuthentificationDao dao, int metier)
        {
            var nouvelUtilisateur = new Authentification();
            Console.Write("Nom: ");
            nouvelUtilisateur.Nom = Console.ReadLine();
            Console.Write("Login: ");
            nouvelUtilisateur.Login = Console.ReadLine();
            Console.Write("Mot de passe: ");
            nouvelUtilisateur.Password = Console.ReadLine();
            nouvelUtilisateur.Metier = metier;
            dao.AddUtilisateur(nouvelUtilisateur);
            Console.WriteLine((metier == 1 ? "Médecin" : "Secrétaire") + " créé avec succès.");
        }

        private static void SupprimerUtilisateur(AuthentificationDao dao, int metier)
        {
            Console.Write("ID de l'utilisateur à supprimer: ");
            int id = int.Parse(Console.ReadLine());
            dao.DeleteUtilisateur(id);
            Console.WriteLine((metier == 1 ? "Médecin" : "Secrétaire") + " supprimé.");
        }
        

    }
}
