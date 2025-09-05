using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllAuthentification.Model
{
    public class Authentification
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nom { get; set; }
        public int Metier { get; set; } // 0 = secrétaire, 1 = médecin salle 1, 2 = médecin salle 2, 3 admin

        public Authentification() { }

        public Authentification(string login, string password, string nom, int metier)
        {
            Login = login;
            Password = password;
            Nom = nom;
            Metier = metier;
        }

        public override string ToString()
        {
            return $"{Nom} ({Login}) - Métier={Metier}";
        }
    }
}
