using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DllVisites;
using DllPatient;
using DllPatient.Model;

namespace ProjetHopital.Db
{
    public partial class Db : Component
    {
        public Db()
        {
            InitializeComponent();
        }

        public Db(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        //Partie Visite
        public List<Visites> selectAllVisites()
        {
            List<Visites> ListeVisite = new List<Visites>();
            var reader = sqlCmd.ExecuteReader();

            sqlCo.Open();
            sqlCmd.CommandText = "select idpatient, date, medecin, num-salle, tarif "+
                                 "  from visite                                     ";

            while(reader.Read())
            {
                Visites v = new Visites(Convert.ToInt32(reader["idpatient"].ToString()), 
                                        Convert.ToDateTime(reader["date"].ToString()), 
                                        Convert.ToInt32(reader["medecin"].ToString()),
                                        Convert.ToInt32(reader["num-salle"].ToString()),
                                        Convert.ToDouble(reader["tarif"].ToString())
                                        );
                ListeVisite.Add(v);
            }

            sqlCo.Close();
            return ListeVisite;
        }

        private void sqlCo_InfoMessage(object sender, System.Data.SqlClient.SqlInfoMessageEventArgs e)
        {

        }


        //Partie Patient
        public List<Patient> SelectAllPatients()
        {
            List<Patient> patients = new List<Patient>();
            sqlCmd.CommandText = "SELECT id, nom, prenom, age, adresse, telephone FROM patients";

            sqlCo.Open();
            var reader = sqlCmd.ExecuteReader();

            while (reader.Read())
            {
                Patient p = new Patient(
                    Convert.ToInt32(reader["id"]),
                    reader["nom"].ToString(),
                    reader["prenom"].ToString(),
                    Convert.ToInt32(reader["age"]),
                    reader["telephone"].ToString(),
                    reader["adresse"].ToString()
                );
                patients.Add(p);
            }
            sqlCo.Close();
            return patients;

        }

        public Patient SelectPatientById(int id)
        {
            sqlCmd.CommandText = "SELECT id, nom, prenom, age, adresse, telephone FROM patients WHERE id=@id";
            sqlCmd.Parameters.Clear();
            sqlCmd.Parameters.AddWithValue("@id", id);

            sqlCo.Open();
            var reader = sqlCmd.ExecuteReader();

            Patient patient = null;

            if (reader.Read())
            {
                patient = new Patient(
                    Convert.ToInt32(reader["id"]),
                    reader["nom"].ToString(),
                    reader["prenom"].ToString(),
                    Convert.ToInt32(reader["age"]),
                    reader["telephone"].ToString(),
                    reader["adresse"].ToString()
                    );
            }
            sqlCo.Close();
            return patient;
        }


        //Partie Authentification


    }

    
}
