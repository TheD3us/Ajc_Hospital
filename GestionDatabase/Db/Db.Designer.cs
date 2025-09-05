namespace GestionDatabase.Db
{
    partial class Db
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.sqlCo = new System.Data.SqlClient.SqlConnection();
            this.sqlCmd = new System.Data.SqlClient.SqlCommand();
            // 
            // sqlCo
            // 
            this.sqlCo.ConnectionString = "Data Source=DESKTOP-AD02GFS;Initial Catalog=ajc_hospital;Integrated Security=True" +
    "";
            this.sqlCo.FireInfoMessageEventOnUserErrors = false;
            this.sqlCo.InfoMessage += new System.Data.SqlClient.SqlInfoMessageEventHandler(this.sqlCo_InfoMessage);
            // 
            // sqlCmd
            // 
            this.sqlCmd.Connection = this.sqlCo;

        }

        #endregion

        private System.Data.SqlClient.SqlConnection sqlCo;
        private System.Data.SqlClient.SqlCommand sqlCmd;
    }
}
