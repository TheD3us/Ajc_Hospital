using DllPatient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Salle
    {
        public int Numero { get; }
        public string Medecin { get; }
        private Patient current;
        Hospital H = Hospital.Hopital();

        public Salle(int numero, string medecin)
        {
            Numero = numero;
            Medecin = medecin;
        }

        // Fait entrer le prochain patient de la file dans cette salle
        public void AssignerProchainPatient()
        {
            if (current != null)
            {
                Console.WriteLine($"[Salle {Numero}] Patient déjà en cours : {current.Nom} {current.Prenom}");
                return;
            }

            var p = H.EntrerProchainPatient();
            if (p != null)
            {
                current = p;
                Console.WriteLine($"[Salle {Numero} - {Medecin}] Nouveau patient : {p.Nom} {p.Prenom}");
            }
            else
            {
                Console.WriteLine($"[Salle {Numero} - {Medecin}] Aucun patient en attente.");
            }
        }

        // Libère la salle et permet au prochain patient d'entrer
        public void LibererSalle()
        {
            if (current == null)
            {
                Console.WriteLine($"[Salle {Numero}] Aucun patient à libérer.");
                return;
            }

            Console.WriteLine($"[Salle {Numero}] Patient {current.Nom} {current.Prenom} sorti.");
            current = null;
        }

        // Affiche la file d’attente actuelle depuis l’hôpital
        public void AfficherFile()
        {
            H.AfficherFile();
        }

        // Affiche le prochain patient sans le retirer de la file
        public void AfficherProchainPatient()
        {
            var prochain = H.ProchainPatient();
            if (prochain != null)
            {
                Console.WriteLine($"[Salle {Numero}] Prochain patient : {prochain.Nom} {prochain.Prenom}");
            }
        }
    }
}
