using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class ExpirationForm : Form
    {

        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgExpirationAbonnementsListe = new BindingSource();
        private List<ExpirationAbonnements> lesExpirations = new List<ExpirationAbonnements>();

        public ExpirationForm()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Récupère les abonnements sur le point d'expirer
        /// </summary>
        /// <returns>Liste d'id et de dates de fin d'abonnement</returns>
        public List<ExpirationAbonnements> GetExpirationAbonnements()
        {
            DateTime today = DateTime.Today;
            string todayString = today.ToString("yyyy-MM-dd");
            return lesExpirations = controller.GetExpirationAbonnements(todayString);
        }

        /// <summary>
        /// Remplit le datagridview des abonnements sur le point d'expirer
        /// </summary>
        /// <param name="expirations">les abonnements sur le point d'expirer</param>
        private void RemplirReceptionExpirationAbonnementListe(List<ExpirationAbonnements> expirations)
        {

            if (expirations != null)
            {
                
                expirations.Sort((x, y) => x.DateFinAbonnement.CompareTo(y.DateFinAbonnement));

                bdgExpirationAbonnementsListe.DataSource = expirations;
                dgvExpiration.DataSource = bdgExpirationAbonnementsListe;
                dgvExpiration.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvExpiration.Columns["Titre"].DisplayIndex = 0;
                dgvExpiration.Columns["DateFinAbonnement"].DisplayIndex = 1;
            }
            else
            {
                bdgExpirationAbonnementsListe.DataSource = null;
            }
        }

        /// <summary>
        /// Ferme la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFermerFenetre_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Charge la fenêtre en remplissant le datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpirationForm_Load(object sender, EventArgs e)
        {
            lesExpirations = GetExpirationAbonnements();
            Console.WriteLine("*********" + lesExpirations.Count);
            RemplirReceptionExpirationAbonnementListe(lesExpirations);
        }
    }
}
