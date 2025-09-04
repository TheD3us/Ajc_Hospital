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
using DllAuthentification.Model;

namespace GestionDatabase.Db
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
            

            sqlCo.Open();
            sqlCmd.CommandText = "select idpatient, date, medecin, num_salle, tarif "+
                                 "  from visite                                     ";
            var reader = sqlCmd.ExecuteReader();
            while (reader.Read())
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

            sqlCo.Open();
            sqlCmd.CommandText = "insert into visites (idpatient, date, medecin, num_salle, tarif)    " +
                                 "             values (@IdPatient, @Date, @Medecin, @NumSalle, @Tarif)";
            sqlCmd.Parameters.Clear();
            sqlCmd.Parameters.AddWithValue("@IdPatient", IdPatient);
            sqlCmd.Parameters.AddWithValue("@Date", Date);
            sqlCmd.Parameters.AddWithValue("@Medecin", Medecin);
            sqlCmd.Parameters.AddWithValue("@NumSalle", NumSalle);
            sqlCmd.Parameters.AddWithValue("@Tarif", Tarif);
            sqlCmd.ExecuteNonQuery();
            sqlCo.Close();
        }

        public void DeleteVisite(int Id)
        {

            sqlCo.Open();
            sqlCmd.CommandText = "delete from visites where id = " + Id;
            sqlCmd.ExecuteNonQuery();
            sqlCo.Close();
        }

        public void UpdateVisite(int IdPatient, DateTime Date, int Medecin, int NumSalle, double Tarif, int Id)
        {

            sqlCo.Open();
            sqlCmd.CommandText = "update visites set idpatient = @Idpatient, date = @Date ," +
                                 "medecin = @Medecin, num_salle = @NumSalle, tarif = @Tarif" +
                                 " where id = @Id                                          ";
            sqlCmd.Parameters.Clear();
            sqlCmd.Parameters.AddWithValue("@IdPatient", IdPatient);
            sqlCmd.Parameters.AddWithValue("@Date", Date);
            sqlCmd.Parameters.AddWithValue("@Medecin", Medecin);
            sqlCmd.Parameters.AddWithValue("@NumSalle", NumSalle);
            sqlCmd.Parameters.AddWithValue("@Tarif", Tarif);
            sqlCmd.Parameters.AddWithValue("@Id", Id);
            sqlCmd.ExecuteNonQuery();
            sqlCo.Close();

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
            sqlCmd.CommandText = "INSERT INTO patients (id, nom, prenom, age, adresse, telephone) VALUES (@id, @nom, @prenom, @age, @adresse, @telephone)";

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
        // Authentifie un utilisateur par login et mot de passe.
        // Retourne l’objet Authentification ou null si échec.
        public Authentification SelectAuthentificationByLoginPassword(string login, string password)
        {
            sqlCmd.CommandText = @"
                                   SELECT login, password, nom, metier
                                   FROM Authentification
                                   WHERE login = @login AND password = @password";

            sqlCmd.Parameters.Clear();
            sqlCmd.Parameters.AddWithValue("@login", login);
            sqlCmd.Parameters.AddWithValue("@password", password);

            sqlCo.Open();
            var reader = sqlCmd.ExecuteReader();

            Authentification auth = null;

            if (reader.Read())
            {
                auth = new Authentification
                {
                    Login = reader["login"].ToString(),
                    Password = reader["password"].ToString(),
                    Nom = reader["nom"].ToString(),
                    Metier = Convert.ToInt32(reader["metier"])
                };
            }

            sqlCo.Close();
            return auth;
        }

        // Récupère tous les utilisateurs.
        public List<Authentification> SelectAllAuthentifications()
        {
            var result = new List<Authentification>();

            sqlCmd.CommandText = @"
                                   SELECT login, password, nom, metier
                                   FROM Authentification";

            sqlCmd.Parameters.Clear();

            sqlCo.Open();
            var reader = sqlCmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Authentification
                {
                    Login = reader["login"].ToString(),
                    Password = reader["password"].ToString(),
                    Nom = reader["nom"].ToString(),
                    Metier = Convert.ToInt32(reader["metier"])
                });
            }

            sqlCo.Close();
            return result;
        }

        public int GetAuthentificationId(string Nom, int Metier)
        {
            sqlCmd.CommandText = @"
                                   select id
                                   from Authentification
                                   where nom = @Nom and metier = @Metier ";
            sqlCmd.Parameters.Clear();
            sqlCmd.Parameters.AddWithValue("@Nom", Nom);
            sqlCmd.Parameters.AddWithValue("@Metier", Metier);

            sqlCo.Open();

            var reader = sqlCmd.ExecuteReader();

            if (reader.Read())
            {
                return Convert.ToInt32(reader["id"].ToString());
            }
            else
            {
                return 0;
            }
        }

        // Récupère un utilisateur par login.
        public Authentification SelectAuthentificationByLogin(string login)
        {
            sqlCmd.CommandText = @"
                                   SELECT login, password, nom, metier
                                   FROM Authentification
                                   WHERE login = @login";

            sqlCmd.Parameters.Clear();
            sqlCmd.Parameters.AddWithValue("@login", login);

            sqlCo.Open();
            var reader = sqlCmd.ExecuteReader();

            Authentification auth = null;

            if (reader.Read())
            {
                auth = new Authentification
                {
                    Login = reader["login"].ToString(),
                    Password = reader["password"].ToString(),
                    Nom = reader["nom"].ToString(),
                    Metier = Convert.ToInt32(reader["metier"])
                };
            }

            sqlCo.Close();
            return auth;
        }
    }
}


