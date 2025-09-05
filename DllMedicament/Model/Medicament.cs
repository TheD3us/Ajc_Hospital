using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllMedicament.Model
{
    public class Medicament
    {
        public int IdMedicaments { get; set; }
        public string Nom { get; set; }
        public int Prix { get; set; }
        public int Quantite { get; set; }

        public Medicament() { }

        public Medicament(int idMedicaments, string nom, int prix, int quantite)
        {
            IdMedicaments = idMedicaments;
            Nom = nom;
            Prix = prix;
            Quantite = quantite;
        }

        public override string ToString()
        {
            return $"{IdMedicaments} - {Nom} ({Prix} euro) - Stock: {Quantite}";
        }
    }
}