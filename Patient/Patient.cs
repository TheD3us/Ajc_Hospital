using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient
{
    public class Patient
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Age { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }

        public Patient() { }

        public Patient(int id, string nom, string prenom, int age, string telephone, string adresse)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Age = age;
            Telephone = telephone;
            Adresse = adresse;
        }
    }
}
