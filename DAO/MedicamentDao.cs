using DllMedicament.Model;
using GestionDatabase.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class MedicamentDao
    {
        private Db db;

        public MedicamentDao()
        {
            db = new Db();
        }

        public List<Medicament> GetAllMedicaments()
        {
            return db.SelectAllMedicaments();
        }

        public Medicament GetMedicamentById(int id)
        {
            return db.SelectMedicamentById(id);
        }

        public void UpdateStock(int id, int nouvelleQuantite)
        {
            db.UpdateMedicamentStock(id, nouvelleQuantite);
        }
    }
}