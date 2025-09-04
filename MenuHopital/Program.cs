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
    }
}
