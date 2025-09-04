using DllPatient.Model;
using ProjetHopital.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDao
{
    public class PatientDao
    {
        private Db db;

        public PatientDao()
        {
            db = new Db();
        }

        // récupérer tous les patients
        public List<Patient> GetAllPatients()
        {
            return db.SelectAllPatients();
        }

        // Récupérer un patient par ID
        public Patient GetPatientById(int id)
        {
            return db.SelectPatientById(id);
        }

        // Vérifier si un patient existe déjà
        public bool PatientExists(int id)
        {
            return GetPatientById(id) != null;
        }

        // Ajouter un nouveau patient
        public void AddPatient(Patient patient)
        {
            db.InsertPatient(patient);
        }

        // Mettre à jour un patient existant
        public void UpdatePatient(Patient patient)
        {
            db.UpdatePatient(patient);
        }
    }
}