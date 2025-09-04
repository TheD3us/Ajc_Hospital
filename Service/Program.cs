using DllPatient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            testService();
        }

        static void testService()
        {
            var salle1 = new Salle(1, "Dr. Dupont");
            var salle2 = new Salle(2, "Dr. Martin");

            Hospital.Hopital.AjouterPatient(new Patient(1, "Durand", "Paul", 20, "0203040506", "18 rue Dupond"));
            Hospital.Hopital.AjouterPatient(new Patient(2, "Lefevre", "Sophie", 20, "0203040506", "18 rue Dupond"));
            Hospital.Hopital.AjouterPatient(new Patient(3, "Bernard", "Luc", 20, "0203040506", "18 rue Dupond"));

            bool continuer = true;
            while (continuer)
            {
                Console.WriteLine("\n=== MENU MÉDECIN ===");
                Console.WriteLine("1. Afficher file d’attente");
                Console.WriteLine("2. Afficher prochain patient");
                Console.WriteLine("3. Faire entrer patient (Salle 1)");
                Console.WriteLine("4. Faire entrer patient (Salle 2)");
                Console.WriteLine("5. Libérer Salle 1");
                Console.WriteLine("6. Libérer Salle 2");
                Console.WriteLine("0. Quitter");
                Console.Write("Choix : ");
                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        Hospital.Hopital.AfficherFile();
                        break;
                    case "2":
                        salle1.AfficherProchainPatient();
                        break;
                    case "3":
                        salle1.AssignerProchainPatient();
                        break;
                    case "4":
                        salle2.AssignerProchainPatient();
                        break;
                    case "5":
                        salle1.LibererSalle();
                        break;
                    case "6":
                        salle2.LibererSalle();
                        break;
                    case "0":
                        continuer = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }

            }
        }
    }
}
