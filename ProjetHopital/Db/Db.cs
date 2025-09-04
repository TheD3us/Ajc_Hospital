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

        private void sqlCo_InfoMessage(object sender, System.Data.SqlClient.SqlInfoMessageEventArgs e)
        {

        }

        //Partie Visite
        public List<Visites> selectAllVisites()
        {
            List<Visites> ListeVisite = new List<Visites>();
            var reader = sqlCmd.ExecuteReader();

            sqlCo.Open();
            sqlCmd.CommandText = "select idpatient, date, medecin, num_salle, tarif "+
                                 "  from visite                                     ";

            while(reader.Read())
            {
                Visites v = new Visites(Convert.ToInt32(reader["idpatient"].ToString()), 
                                        Convert.ToDateTime(reader["date"].ToString()), 
                                        Convert.ToInt32(reader["medecin"].ToString()),
                                        Convert.ToInt32(reader["num_salle"].ToString()),
                                        Convert.ToDouble(reader["tarif"].ToString())
                                        );
                ListeVisite.Add(v);
            }

            sqlCo.Close();
            return ListeVisite;
        }

        public void InsertVisite(int IdPatient, DateTime Date, int Medecin, int NumSalle, double Tarif)
        {
            var reader = sqlCmd.ExecuteReader();

            sqlCo.Open();
            sqlCmd.CommandText = "insert into visites (idpatient, date, medecin, num_salle, tarif) " +
                                 "             values ("+IdPatient+","+Date+","+Medecin+",         " +
                                  +NumSalle + "," + Tarif + ")                                     ";
            sqlCmd.ExecuteNonQuery();
            sqlCo.Close();
        }

        public void DeleteVisite(int Id)
        {
            var reader = sqlCmd.ExecuteReader();

            sqlCo.Open();
            sqlCmd.CommandText = "delete from visites where id = " + Id;
            sqlCmd.ExecuteNonQuery();
            sqlCo.Close();
        }

        public void UpdateVisite(int IdPatient, DateTime Date, int Medecin, int NumSalle, double Tarif, int Id)
        {
            var reader = sqlCmd.ExecuteReader();

            sqlCo.Open();
            sqlCmd.CommandText = "update visites set idpatient = " + IdPatient + ", date = " + Date +
                                 ", medecin = " + Medecin + ", num_salle = " + NumSalle + ", tarif = " +
                                 Tarif + " where id = " + Id;
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

        public void InsertPatient(Patient patient)
        {
            sqlCmd.CommandText = "INSERT INTO patients (nom, prenom, age, adresse, telephone) VALUES (@nom, @prenom, @age, @adresse, @telephone)";

            sqlCmd.Parameters.Clear();
            sqlCmd.Parameters.AddWithValue("@nom", patient.Nom);
            sqlCmd.Parameters.AddWithValue("@prenom", patient.Prenom);
            sqlCmd.Parameters.AddWithValue("@age", patient.Age);
            sqlCmd.Parameters.AddWithValue("@adresse", patient.Adresse);
            sqlCmd.Parameters.AddWithValue("@telephone", patient.Telephone);

            sqlCo.Open();
            sqlCmd.ExecuteNonQuery();
            sqlCo.Close();
        }

        public void UpdatePatient(Patient patient)
        {
            sqlCmd.CommandText = "UPDATE patients SET nom=@nom, prenom=@prenom, age=@age, adresse=@adresse, telephone=@telephone WHERE id=@id";
            sqlCmd.Parameters.Clear();
            sqlCmd.Parameters.AddWithValue("@id", patient.Id);
            sqlCmd.Parameters.AddWithValue("@nom", patient.Nom);
            sqlCmd.Parameters.AddWithValue("@prenom", patient.Prenom);
            sqlCmd.Parameters.AddWithValue("@age", patient.Age);
            sqlCmd.Parameters.AddWithValue("@adresse", patient.Adresse);
            sqlCmd.Parameters.AddWithValue("@telephone", patient.Telephone);

            sqlCo.Open();
            sqlCmd.ExecuteNonQuery();
            sqlCo.Close();
        }


        //Partie Authentification


    }

    
}
