using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DllVisites;



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
        

        //Partie Patient



        //Partie Authentification


    }

    
}
