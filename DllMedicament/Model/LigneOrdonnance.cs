using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllMedicament.Model
{
    public class LigneOrdonnance
    {
        public Medicament Medicament { get; set; }
        public int Quantite { get; set; }

        public int PrixTotal => Medicament.Prix * Quantite;

        public override string ToString()
        {
            return $"{Quantite} x {Medicament.Nom} = {PrixTotal} euro";
        }
    }
}