using DllAuthentification.Model;
using GestionDatabase.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class AuthentificationDao
    {
        private Db db;

        public AuthentificationDao()
        {
            db = new Db();
        }

        // Authentifie un utilisateur par login et mot de passe.
        // Retourne l’objet Authentification ou null si échec.
        public Authentification Authenticate(string login, string password)
        {
            return db.SelectAuthentificationByLoginPassword(login, password);
        }

        // Récupère tous les utilisateurs (optionnel)
        public List<Authentification> GetAllUsers()
        {
            return db.SelectAllAuthentifications();
        }


        // Récupère un utilisateur par login
        public Authentification GetUserByLogin(string login)
        {
            return db.SelectAuthentificationByLogin(login);
        }

        //Récupère id Auth
        public int GetAuthentificationId(string Nom, int Metier)
        {
            return db.GetAuthentificationId(Nom, Metier);
        }

       
    }
}
