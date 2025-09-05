using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DllVisites;
using GestionDatabase.Db;

namespace DAO
{
    public class VisiteDao
    {
        private Db db;

        public VisiteDao()
        {
            db = new Db();
        }

        public List<Visites> selectAllVisite()
        {
            return db.selectAllVisites();
        }

        public void insertVisite(int IdPatient, DateTime Date, int Medecin, int NumSalle, double Tarif)
        {
            db.InsertVisite(IdPatient, Date, Medecin, NumSalle, Tarif);
        }

        public void deleteVisite(int Id)
        {
            db.DeleteVisite(Id);
        }

        public void updateVisite(int IdPatient, DateTime Date, int Medecin, int NumSalle, double Tarif, int Id)
        {
            db.UpdateVisite(IdPatient, Date, Medecin, NumSalle, Tarif, Id);
        }

        public List<Visites> SelectVisitesByMedecin(int IdMedecin)
        {
            return db.SelectVisitesByMedecin(IdMedecin);
        }


    }
}
