using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllVisites
{
    public class Visites
    {
        int idpatient;
        DateTime date;
        int medecin;
        int numSalle;
        double tarif;

        public Visites()
        { }

        public Visites(int IdPatient, DateTime Date, int Medecin, int NumSalle, double Tarif)
        {
            this.idpatient = IdPatient;
            this.date = Date;
            this.medecin = Medecin;
            this.numSalle = NumSalle;
            this.tarif = Tarif;
        }

        public int IdPatient
        {
            get { return this.idpatient; }
            set { this.idpatient = value; }
        }

        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        public int Medecin
        {
            get { return this.medecin; }
            set { this.medecin = value; }
        }

        public int NumSalle
        {
            get { return this.numSalle; }
            set { this.numSalle = value; }
        }

        public double Tarif
        {
            get { return this.tarif; }
            set { this.tarif = value; }
        }
    }
}
