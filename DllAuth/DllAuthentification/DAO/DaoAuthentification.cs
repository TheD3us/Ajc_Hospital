using DllAuthentification.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllAuthentification.DAO
{
    public class DAOAuthentification
    {
        private readonly string _connectionString;

        public DAOAuthentification(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Authentifie un utilisateur par login et password.
        // Retourne l’objet Authentification ou null si échec.
        public Authentification Authenticate(string login, string password)
        {
            const string sql = @"
                SELECT login, password, nom, metier
                FROM Authentification
                WHERE login = @login AND password = @password";

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    // Variables temporaires pour éviter injection SQL
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", password);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;

                        return new Authentification
                        {
                            Login = reader["login"].ToString(),
                            Password = reader["password"].ToString(),
                            Nom = reader["nom"].ToString(),
                            Metier = Convert.ToInt32(reader["metier"])
                        };
                    }
                }
            }
        }
    }
}
