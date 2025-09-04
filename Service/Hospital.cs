using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Hospital
    {
        // singleton
        private static Hospital hopital = null;

        private List<Patient.Patient> fileAttente = new List<Patient.Patient>();

        private List<IObserverMedecin> medecins = new List<IObserverMedecin>();
        private List<IObserverSecretaire> secretaires = new List<IObserverSecretaire>();

        private Hospital() { }

        public static Hospital Hopital
        {
            get
            {
                if (hopital == null)
                {
                    hopital = new Hospital();
                }
                return hopital;
            }
        }

        public void AjouterPatient(Patient.Patient p) { }

        //public Patient.Patient EntrerProchainPatient() { }

        //public Patient.Patient ProchainPatient() { }

        public void AddMedecin(IObserverMedecin medecin) { }

        public void AddSecretaire(IObserverSecretaire secretaire) { }

        private void NotifierMedecins() { }

        private void NotifierSecretaires() { }
    }
}
