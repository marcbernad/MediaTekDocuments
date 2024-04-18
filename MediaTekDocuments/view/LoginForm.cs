using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class LoginForm : Form
    {

        private readonly FrmMediatekController controller;

        public LoginForm()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }



        public bool ValiderConnexion(string nom, string motDePasseSaisi)
        {
            List<Utilisateur> utilisateur = controller.GetUtilisateur(nom);
            if (utilisateur.Count > 0 && utilisateur[0].Password == motDePasseSaisi)
            {
                // L'utilisateur est authentifié
                return true;
            }
            // Authentification échouée
            return false;
        }


        private void btnConnexion_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            bool authentification = ValiderConnexion(username, password);

            if (authentification)
            {
                // Si authentifié, ouvrir la fenêtre principale
                FrmMediatek frmMediatek = new FrmMediatek(username);
                frmMediatek.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.");
            }
        }
    }
}
