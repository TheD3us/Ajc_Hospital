using DAO;
using DllAuthentification.Model;
using DllPatient.Model;
using DllVisites;
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
        public Authentification Medecin { get; }
        private Patient current;
        Hospital H = Hospital.Hopital();
        List<Visites> ListeVisiteEnCours = new List<Visites>();

        public Salle(int numero, Authentification medecin)
        {
            Numero = numero;
            Medecin = medecin;
        }

        // Fait entrer le prochain patient de la file dans cette salle
        public void AssignerProchainPatient()
        {
            this.LibererSalle();

            var p = H.EntrerProchainPatient();
            if (p != null)
            {
                current = p;
                Console.WriteLine($"[Salle {Numero} - {Medecin}] Nouveau patient : {p.Nom} {p.Prenom}");
                SauvegarderVisites(p);
            }
            else
            {
                Console.WriteLine($"[Salle {Numero} - {Medecin}] Aucun patient en attente.");
            }
        }

        //Sauvegarder visites en base
        public void SauvegarderVisites(Patient p)
        {
            if(p!=null)
            {
                Visites nouvelleVisite = new Visites(p.Id, DateTime.Now, new AuthentificationDao().GetAuthentificationId(Medecin.Nom, Medecin.Metier),
                                        Numero, 23);
                ListeVisiteEnCours.Add(nouvelleVisite);
            }
            
            if (ListeVisiteEnCours.Count >= 5 || p == null)
            {
                foreach (Visites v in ListeVisiteEnCours)
                {
                    new VisiteDao().insertVisite(v.IdPatient, v.Date, v.Medecin, v.NumSalle, v.Tarif);
                }
                ListeVisiteEnCours.Clear();
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
