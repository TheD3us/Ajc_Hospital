using DAO;
using DllAuthentification.Model;
using DllMedicament.Model;
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
        Visites VisiteEnCours = new Visites();

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
            }
            else
            {
                Console.WriteLine($"[Salle {Numero} - {Medecin}] Aucun patient en attente.");
            }
        }

        //Sauvegarder visites en base
        public void SauvegarderVisites(Patient p)
        {
            if(VisiteEnCours.Tarif > 0)
            {
                VisiteEnCours.Tarif += 23;
            }
            else
            {
                VisiteEnCours.Tarif = 23;
            }
            
            if(p!=null)
            {
                VisiteEnCours.IdPatient = p.Id;
                VisiteEnCours.Date = DateTime.Now;
                VisiteEnCours.Medecin = new AuthentificationDao().GetAuthentificationId(Medecin.Nom, Medecin.Metier);
                VisiteEnCours.NumSalle = Numero;

                ListeVisiteEnCours.Add(VisiteEnCours);
                VisiteEnCours = new Visites();
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

        //Historique des visites
        public void VoirHistoriqueVisites()
        {
            List<Visites> ListeHistorique = new List<Visites>();
            int idMedecin =new AuthentificationDao().GetAuthentificationId(Medecin.Nom, Medecin.Metier);
            ListeHistorique = new VisiteDao().SelectVisitesByMedecin(idMedecin);

            foreach(Visites v in ListeHistorique)
            {
                Patient p = new PatientDao().GetPatientById(v.IdPatient);

                Console.WriteLine(p.Nom + " " + p.Prenom + " - " + v.Date + " tarif : "+ v.Tarif);
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
            else
            {
                SauvegarderVisites(current);
                Console.WriteLine($"[Salle {Numero}] Patient {current.Nom} {current.Prenom} sorti.");
                current = null;
            }
            
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

        public void CreerOrdonnance(Authentification auth)
        {
            if(current == null)
            {
                Console.WriteLine("Aucun patient en salle, impossible de créer une ordonnance");
                return;
            }
            else
            {
                MedicamentDao dao = new MedicamentDao();
                var listeMedicaments = dao.GetAllMedicaments();
                List<LigneOrdonnance> ordonnance = new List<LigneOrdonnance>();

                bool continuer = true;
                while (continuer)
                {
                    Console.WriteLine("=== Liste des médicaments ===");
                    foreach (var m in listeMedicaments)
                        Console.WriteLine(m);

                    Console.Write("ID du médicament (0 pour terminer) : ");
                    int id = int.Parse(Console.ReadLine());
                    if (id == 0) break;

                    var med = dao.GetMedicamentById(id);
                    if (med == null)
                    {
                        Console.WriteLine("Médicament introuvable.");
                        continue;
                    }

                    Console.Write($"Quantité (max {med.Quantite}) : ");
                    int qte = int.Parse(Console.ReadLine());
                    if (qte <= 0 || qte > med.Quantite)
                    {
                        Console.WriteLine("Quantité invalide.");
                        continue;
                    }

                    ordonnance.Add(new LigneOrdonnance { Medicament = med, Quantite = qte });

                    // Mise à jour du stock
                    dao.UpdateStock(med.IdMedicaments, med.Quantite - qte);
                }

                // Calcul du prix total
                int totalMedocs = ordonnance.Sum(l => l.PrixTotal);
                Console.WriteLine($"Prix total des médicaments : {totalMedocs} euro");

                string champOrdo = string.Join("/", ordonnance.Select(l => $"{l.Quantite}-{l.Medicament.IdMedicaments}"));
                Console.WriteLine($"Champ ordonnance : {champOrdo}");

                VisiteEnCours.Tarif = totalMedocs;
            } 
        }
    }
}
