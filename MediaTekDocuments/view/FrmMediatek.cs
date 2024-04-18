using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.ComponentModel;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        private string _username;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek(string username)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            _username = username;
            ConfigureAccessRights();
        }

        private void ConfigureAccessRights()
        {
            List<Utilisateur> utilisateur = controller.GetUtilisateur(_username);
            string serviceType = utilisateur[0].IdService;

            switch (serviceType)
            {
                case "1":
                    
                    break;
                case "2":
                    MessageBox.Show("Vous n'avez pas les droits suffisants pour accéder à cette application.");
                    Application.Exit();
                    break;
                case "3":
                    tabOngletsApplication.TabPages.Remove(tabCommandesLivres);
                    tabOngletsApplication.TabPages.Remove(tabCommandesDvd);
                    tabOngletsApplication.TabPages.Remove(tabAbonnementRevues);
                    break;
            }
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion


        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();


        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Parutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

       

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocument = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocument);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region Commande Documents
        private readonly BindingSource bdgCommandesListe = new BindingSource();
        private List<CommandeDocument> lesCommmandes = new List<CommandeDocument>();

        private bool isFirstLoad = true;

        #region Commandes Livres

        private void tabCommandesLivre_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
        }

        /// <summary>
        /// Recherche les infos du livre sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheNumLivre_Click(object sender, EventArgs e)
        {
            if (!txtLivreNumRecherche.Text.Equals(""))
            {

                Livre livre = lesLivres.Find(x => x.Id.Equals(txtLivreNumRecherche.Text));
                if (livre != null)
                {
                    AfficheLivreCommandeInfos(livre);
                    string idLivreDvd = txtLivreNumRecherche.Text;
                    lesCommmandes = controller.GetCommandeDocument(idLivreDvd);
                    isFirstLoad = true;
                    RemplirReceptionCommandesLivresListe(lesCommmandes);
                    
                }
                else
                {
                    MessageBox.Show("numéro introuvable");

                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir un numéro");
            }
        }



        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivreCommandeInfos(Livre livre)
        {
            txtLivreAuteur.Text = livre.Auteur;
            txtLivreCollection.Text = livre.Collection;
            txtLivreImage.Text = livre.Image;
            txtLivreIsbn.Text = livre.Isbn;
            txtLivreNum.Text = livre.Id;
            txtLivreGenre.Text = livre.Genre;
            txtLivrePublic.Text = livre.Public;
            txtLivreRayon.Text = livre.Rayon;
            txtLivreTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivreImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivreImage.Image = null;
            }
        }

        

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name = "commandes" > liste des commandes</param>
        private void RemplirReceptionCommandesLivresListe(List<CommandeDocument> commandes)
        {

            if (commandes != null)
            {
                if (isFirstLoad)
                {
                    commandes.Sort((x, y) => y.DateCommande.CompareTo(x.DateCommande));
                    isFirstLoad = false;
                }

                bdgCommandesListe.DataSource = commandes;
                dgvCommandeLivre.DataSource = bdgCommandesListe;
                dgvCommandeLivre.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCommandeLivre.Columns["idLivreDvd"].Visible = false;
                dgvCommandeLivre.Columns["nbExemplaire"].DisplayIndex = 1;
                dgvCommandeLivre.Columns["idEtape"].Visible = false;
                dgvCommandeLivre.Columns["etape"].DisplayIndex = 3;
                dgvCommandeLivre.Columns["id"].Visible = false;
                dgvCommandeLivre.Columns["dateCommande"].DisplayIndex = 0;
                dgvCommandeLivre.Columns["montant"].DisplayIndex = 2;
            }
            else
            {
                bdgCommandesListe.DataSource = null;
            }
        }


        private void btnEnregistrerCommandeLivres_Click(object sender, EventArgs e)
        {
            if(!txtNbExemplaireCommandeLivres.Text.Equals("") && !txtMontantCommandeLivres.Text.Equals("") && !txtNumCommandeLivre.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txtLivreNumRecherche.Text));
                string idLivreDvd = livre.Id;
                string id = txtNumCommandeLivre.Text;
                int nbExemplaire = int.Parse(txtNbExemplaireCommandeLivres.Text);
                DateTime dateCommande = DateTime.Today;
                int montant = int.Parse(txtMontantCommandeLivres.Text);
                string idEtape = "1";
                CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, idLivreDvd, nbExemplaire, idEtape);
                if (controller.CreerCommande(commandeDocument))
                {
                    AfficheLivreCommandeInfos(livre);
                    lesCommmandes = controller.GetCommandeDocument(idLivreDvd);
                    RemplirReceptionCommandesLivresListe(lesCommmandes);
                    viderNouvelleCommandeLivre();
                }
                else
                {
                    MessageBox.Show("commande impossible", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être renseignés", "Information");
            }
        }

        private void btnModifierCommandeLivre_Click(object sender, EventArgs e)
        {
            if (dgvCommandeLivre.SelectedRows.Count > 0)
            {
                // Obtenez la commande sélectionnée
                CommandeDocument commande = (CommandeDocument)dgvCommandeLivre.SelectedRows[0].DataBoundItem;

                // Récupérez l'idEtape à partir de modifIdEtape()
                string newIdEtape = modifIdEtapeLivre();

                if ((commande.IdEtape == "3" || commande.IdEtape == "4") && int.Parse(newIdEtape) < 3
                    || (int.Parse(commande.IdEtape) < 3 && newIdEtape == "4"))
                {
                    MessageBox.Show("Changement d'étape non autorisé.");
                    return;
                }

                MajCommande majCommandeDocument = new MajCommande(commande.Id, commande.IdLivreDvd, commande.NbExemplaire, newIdEtape);

                controller.ModifierCommande(majCommandeDocument, commande.Id);
                Livre livre = lesLivres.Find(x => x.Id.Equals(txtLivreNumRecherche.Text));
                string idLivreDvd = livre.Id;
                lesCommmandes = controller.GetCommandeDocument(idLivreDvd);
                RemplirReceptionCommandesLivresListe(lesCommmandes);

            }
        }

        

        private string modifIdEtapeLivre()
        {
            switch (cbbEtapeCommandeLivre.Text)
            {
                case "En cours":
                    return "1";
                case "Relancée":
                    return "2";
                case "Livrée":
                    return "3";
                case "Réglée":
                    return "4";
                default:
                    return null;
            }
        }

        private void btnSupprimerCommandeLivre_Click(object sender, EventArgs e)
        {
            if (dgvCommandeLivre.SelectedRows.Count > 0)
            {
                // Obtenez la commande sélectionnée
                CommandeDocument commande = (CommandeDocument)dgvCommandeLivre.SelectedRows[0].DataBoundItem;
                if (commande.IdEtape != "3" && commande.IdEtape != "4")
                {
                    // Demandez une confirmation à l'utilisateur
                    DialogResult dialogResult = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette commande ?", "Confirmation de suppression", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Commande supprCommande = new Commande(commande.Id, commande.DateCommande, commande.Montant);
                        controller.SupprimerCommande(supprCommande);

                        Livre livre = lesLivres.Find(x => x.Id.Equals(txtLivreNumRecherche.Text));
                        string idLivreDvd = livre.Id;
                        lesCommmandes = controller.GetCommandeDocument(idLivreDvd);
                        RemplirReceptionCommandesLivresListe(lesCommmandes);
                    }
                }
                else
                {
                    MessageBox.Show("Une commande livrée ne peut pas être supprimée.");
                }
                 
            }
        }
        private void viderNouvelleCommandeLivre()
        {
            txtNbExemplaireCommandeLivres.Text = "";
            txtMontantCommandeLivres.Text = "";
            txtNumCommandeLivre.Text = "";
            isFirstLoad = true;
        }

        // Ajoutez cette variable à votre classe
        private Dictionary<string, SortOrder> columnSortOrder = new Dictionary<string, SortOrder>();

        private void dgvCommandeLivre_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandeLivre.Columns[e.ColumnIndex].HeaderText;
            triColonneCommande(titreColonne);
        }

        private void triColonneCommande(string titreColonne)
        {
            if (!columnSortOrder.ContainsKey(titreColonne))
            {
                columnSortOrder[titreColonne] = SortOrder.Ascending;
            }
            else
            {
                // Inversez l'ordre de tri
                columnSortOrder[titreColonne] = columnSortOrder[titreColonne] == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }

            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "DateCommande":
                    sortedList = columnSortOrder[titreColonne] == SortOrder.Ascending ?
                        lesCommmandes.OrderBy(o => o.DateCommande).ToList() :
                        lesCommmandes.OrderByDescending(o => o.DateCommande).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = columnSortOrder[titreColonne] == SortOrder.Ascending ?
                        lesCommmandes.OrderBy(o => o.NbExemplaire).ToList() :
                        lesCommmandes.OrderByDescending(o => o.NbExemplaire).ToList();
                    break;
                case "Montant":
                    sortedList = columnSortOrder[titreColonne] == SortOrder.Ascending ?
                        lesCommmandes.OrderBy(o => o.Montant).ToList() :
                        lesCommmandes.OrderByDescending(o => o.Montant).ToList();
                    break;
                case "Etape":
                    sortedList = columnSortOrder[titreColonne] == SortOrder.Ascending ?
                        lesCommmandes.OrderBy(o => o.IdEtape).ToList() :
                        lesCommmandes.OrderByDescending(o => o.IdEtape).ToList();
                    break;
            }
            RemplirReceptionCommandesLivresListe(sortedList);
        }

        #endregion Commandes Livres

        #region Commandes DVD

        private void tabCommandesDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
        }

        private void btnRechercheNumDvd_Click(object sender, EventArgs e)
        {
            Console.Write(txtDvdNumRecherche.Text);
            if (!txtDvdNumRecherche.Text.Equals(""))
            {
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txtDvdNumRecherche.Text));
                if (dvd != null)
                {
                    AfficheDvdCommandeInfos(dvd);
                    string idLivreDvd = txtDvdNumRecherche.Text;
                    lesCommmandes = controller.GetCommandeDocument(idLivreDvd);
                    isFirstLoad = true;
                    RemplirReceptionCommandesDvdListe(lesCommmandes);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");

                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir un numéro");
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdCommandeInfos(Dvd dvd)
        {
            txtDvdRealisateur.Text = dvd.Realisateur;
            txtDvdNum.Text = dvd.Id;
            txtDvdTitre.Text = dvd.Titre;
            txtDvdDuree.Text = dvd.Duree.ToString();
            txtDvdSynopsis.Text = dvd.Synopsis;
            txtDvdGenre.Text = dvd.Genre;
            txtDvdPublic.Text = dvd.Public;
            txtDvdRayon.Text = dvd.Rayon;
            txtDvdImage.Text = dvd.Image;
            string image = dvd.Image;
            try
            {
                pcbDvdCommandeImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdCommandeImage.Image = null;
            }
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name = "commandes" > liste des commandes</param>
        private void RemplirReceptionCommandesDvdListe(List<CommandeDocument> commandes)
        {

            if (commandes != null)
            {
                if (isFirstLoad)
                {
                    commandes.Sort((x, y) => y.DateCommande.CompareTo(x.DateCommande));
                    isFirstLoad = false;
                }

                commandes.Sort((x, y) => y.DateCommande.CompareTo(x.DateCommande));

                bdgCommandesListe.DataSource = commandes;
                dgvCommandeDvd.DataSource = bdgCommandesListe;
                dgvCommandeDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCommandeDvd.Columns["idLivreDvd"].Visible = false;
                dgvCommandeDvd.Columns["nbExemplaire"].DisplayIndex = 1;
                dgvCommandeDvd.Columns["idEtape"].Visible = false;
                dgvCommandeDvd.Columns["etape"].DisplayIndex = 3;
                dgvCommandeDvd.Columns["id"].Visible = false;
                dgvCommandeDvd.Columns["dateCommande"].DisplayIndex = 0;
                dgvCommandeDvd.Columns["montant"].DisplayIndex = 2;
            }
            else
            {
                bdgCommandesListe.DataSource = null;
            }
        }

        private void dgvCommandeDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandeDvd.Columns[e.ColumnIndex].HeaderText;
            triColonneCommande(titreColonne);
        }

        private void btnEnregistrerCommandeDvd_Click(object sender, EventArgs e)
        {
            if (!txtNbExemplaireCommandeDvd.Text.Equals("") && !txtMontantCommandeDvd.Text.Equals("") && !txtNumCommandeDvd.Text.Equals(""))
            {
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txtDvdNumRecherche.Text)); ;
                string idLivreDvd = dvd.Id;
                string id = txtNumCommandeDvd.Text;
                int nbExemplaire = int.Parse(txtNbExemplaireCommandeDvd.Text);
                DateTime dateCommande = DateTime.Today;
                int montant = int.Parse(txtMontantCommandeDvd.Text);
                string idEtape = "1";
                CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, idLivreDvd, nbExemplaire, idEtape);
                if (controller.CreerCommande(commandeDocument))
                {
                    AfficheDvdCommandeInfos(dvd);
                    lesCommmandes = controller.GetCommandeDocument(idLivreDvd);
                    RemplirReceptionCommandesDvdListe(lesCommmandes);
                    viderNouvelleCommandeDvd();
                }
                else
                {
                    MessageBox.Show("commande impossible", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être renseignés", "Information");
            }
        }

        private void btnModifierCommandeDvd_Click(object sender, EventArgs e)
        {
            if (dgvCommandeDvd.SelectedRows.Count > 0)
            {
                // Obtenez la commande sélectionnée
                CommandeDocument commande = (CommandeDocument)dgvCommandeDvd.SelectedRows[0].DataBoundItem;

                // Récupérez l'idEtape à partir de modifIdEtape()
                string newIdEtape = modifIdEtapeDvd();

                if ((commande.IdEtape == "3" || commande.IdEtape == "4") && int.Parse(newIdEtape) < 3
                    || (int.Parse(commande.IdEtape) < 3 && newIdEtape == "4"))
                {
                    MessageBox.Show("Changement d'étape non autorisé.");
                    return;
                }

                MajCommande majCommandeDocument = new MajCommande(commande.Id, commande.IdLivreDvd, commande.NbExemplaire, newIdEtape);

                controller.ModifierCommande(majCommandeDocument, commande.Id);
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txtDvdNumRecherche.Text));
                string idLivreDvd = dvd.Id;
                lesCommmandes = controller.GetCommandeDocument(idLivreDvd);
                RemplirReceptionCommandesDvdListe(lesCommmandes);

            }
        }

        private void btnSupprimerCommandeDvd_Click(object sender, EventArgs e)
        {
            if (dgvCommandeDvd.SelectedRows.Count > 0)
            {
                // Obtenez la commande sélectionnée
                CommandeDocument commande = (CommandeDocument)dgvCommandeDvd.SelectedRows[0].DataBoundItem;
                if (commande.IdEtape != "3" && commande.IdEtape != "4")
                {
                    // Demandez une confirmation à l'utilisateur
                    DialogResult dialogResult = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette commande ?", "Confirmation de suppression", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Commande supprCommande = new Commande(commande.Id, commande.DateCommande, commande.Montant);
                        controller.SupprimerCommande(supprCommande);

                        Dvd dvd = lesDvd.Find(x => x.Id.Equals(txtDvdNumRecherche.Text));
                        string idLivreDvd = dvd.Id;
                        lesCommmandes = controller.GetCommandeDocument(idLivreDvd);
                        RemplirReceptionCommandesLivresListe(lesCommmandes);
                    }
                }
                else
                {
                    MessageBox.Show("Une commande livrée ne peut pas être supprimée.");
                }
                
            }
        }



        private string modifIdEtapeDvd()
        {
            switch (cbbEtapeCommandeDvd.Text)
            {
                case "En cours":
                    return "1";
                case "Relancée":
                    return "2";
                case "Livrée":
                    return "3";
                case "Réglée":
                    return "4";
                default:
                    return null;
            }
        }

        private void viderNouvelleCommandeDvd()
        {
            txtNbExemplaireCommandeDvd.Text = "";
            txtMontantCommandeDvd.Text = "";
            txtNumCommandeDvd.Text = "";
        }

        #endregion Commandes DVD


        #endregion Commande Documents

        #region Abonnement Revue

        private readonly BindingSource bdgAbonnementsListe = new BindingSource();
        private List<Abonnement> lesAbonnements = new List<Abonnement>();

        private void tabAbonnementRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
        }

        private void AfficheRevuesAbonnementInfos(Revue revue)
        {
            txtRevueNum.Text = revue.Id;
            txtRevueTitre.Text = revue.Titre;
            txtRevuePeriodicite.Text = revue.Periodicite;
            txtRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txtRevueGenre.Text = revue.Genre;
            txtRevuePublic.Text = revue.Public;
            txtRevueRayon.Text = revue.Rayon;
            txtRevueImage.Text = revue.Image;
            string image = revue.Image;
            try
            {
                pcbRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevueImage.Image = null;
            }
        }

        private void btnRechercheNumRevue_Click(object sender, EventArgs e)
        {
            if (!txtNumRechercheRevue.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txtNumRechercheRevue.Text));
                if (revue != null)
                {
                    AfficheRevuesAbonnementInfos(revue);
                    string idRevue = txtNumRechercheRevue.Text;
                    lesAbonnements = controller.GetAbonnementRevue(idRevue);
                    isFirstLoad = true;
                    RemplirReceptionAbonnementRevueListe(lesAbonnements);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");

                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir un numéro");
            }
        }

        private void RemplirReceptionAbonnementRevueListe(List<Abonnement> abonnements)
        {

            if (abonnements != null)
            {
                if (isFirstLoad)
                {
                    abonnements.Sort((x, y) => y.DateCommande.CompareTo(x.DateCommande));
                    isFirstLoad = false;
                }

                bdgAbonnementsListe.DataSource = abonnements;
                dgvAbonnementRevue.DataSource = bdgAbonnementsListe;
                dgvAbonnementRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvAbonnementRevue.Columns["Id"].Visible = false;
                dgvAbonnementRevue.Columns["DateFinAbonnement"].DisplayIndex = 2;
                dgvAbonnementRevue.Columns["IdRevue"].Visible = false;
                dgvAbonnementRevue.Columns["DateCommande"].DisplayIndex = 0;
                dgvAbonnementRevue.Columns["Montant"].DisplayIndex = 1;
            }
            else
            {
                bdgAbonnementsListe.DataSource = null;
            }
        }

        private void triColonneAbonnement(string titreColonne)
        {
            if (!columnSortOrder.ContainsKey(titreColonne))
            {
                columnSortOrder[titreColonne] = SortOrder.Ascending;
            }
            else
            {
                // Inversez l'ordre de tri
                columnSortOrder[titreColonne] = columnSortOrder[titreColonne] == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }

            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "dateCommande":
                    sortedList = columnSortOrder[titreColonne] == SortOrder.Ascending ?
                        lesAbonnements.OrderBy(o => o.DateCommande).ToList() :
                        lesAbonnements.OrderByDescending(o => o.DateCommande).ToList();
                    break;
                case "montant":
                    sortedList = columnSortOrder[titreColonne] == SortOrder.Ascending ?
                        lesAbonnements.OrderBy(o => o.Montant).ToList() :
                        lesAbonnements.OrderByDescending(o => o.Montant).ToList();
                    break;
                case "dateFinAbonnement":
                    sortedList = columnSortOrder[titreColonne] == SortOrder.Ascending ?
                        lesAbonnements.OrderBy(o => o.DateFinAbonnement).ToList() :
                        lesAbonnements.OrderByDescending(o => o.DateFinAbonnement).ToList();
                    break;
            }
            RemplirReceptionAbonnementRevueListe(sortedList);
        }

        private void dgvAbonnementRevue_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvAbonnementRevue.Columns[e.ColumnIndex].HeaderText;
            triColonneAbonnement(titreColonne);
        }

        private void btnEnregistrerAbonnement_Click(object sender, EventArgs e)
        {
            if (!txtMontantAbonnement.Text.Equals("") && !dtpRevueFinAbonnement.Value.Equals(null) && !txtNumCommandeAbonnement.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txtNumRechercheRevue.Text));
                string idRevue = revue.Id;
                string id = txtNumCommandeAbonnement.Text;
                DateTime dateCommande = DateTime.Today;
                int montant = int.Parse(txtMontantAbonnement.Text);
                DateTime dateFinAbonnement = dtpRevueFinAbonnement.Value;
                Abonnement abonnement = new Abonnement(id, dateFinAbonnement, idRevue, dateCommande, montant);
                if (controller.CreerAbonnement(abonnement))
                {
                    AfficheRevuesAbonnementInfos(revue);
                    lesAbonnements = controller.GetAbonnementRevue(idRevue);
                    RemplirReceptionAbonnementRevueListe(lesAbonnements);
                    viderNouvelAbonnement();
                }
                else
                {
                    MessageBox.Show("commande impossible", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être renseignés", "Information");
            }
        }

        private void viderNouvelAbonnement()
        {
            dtpRevueFinAbonnement.Value = DateTime.Today;
            txtMontantAbonnement.Text = "";
            txtNumCommandeAbonnement.Text = "";
        }

        private void btnSupprimerAbonnement_Click(object sender, EventArgs e)
        {
            
            if (dgvAbonnementRevue.SelectedRows.Count > 0)
            {
                Abonnement abonnementSelectionne = (Abonnement)dgvAbonnementRevue.SelectedRows[0].DataBoundItem;
                List<Exemplaire> exemplairesRevue = controller.GetExemplairesRevue(abonnementSelectionne.IdRevue);
                bool exemplaireParu = exemplairesRevue.Any(ex => ex.Id == abonnementSelectionne.IdRevue && ParutionDansAbonnement(abonnementSelectionne.DateCommande, abonnementSelectionne.DateFinAbonnement, ex.DateAchat));

                if (!exemplaireParu)
                {
                    DialogResult dialogResult = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette commande ?", "Confirmation de suppression", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        controller.SupprimerAbonnement(abonnementSelectionne);
                        Abonnement abonnement = lesAbonnements.Find(x => x.IdRevue.Equals(txtNumRechercheRevue.Text));
                        string idRevue = abonnement.IdRevue;
                        lesAbonnements = controller.GetAbonnementRevue(idRevue);
                        RemplirReceptionAbonnementRevueListe(lesAbonnements);
                    }
                    
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer l'abonnement : un ou plusieurs exemplaires sont parus pendant la période d'abonnement.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un abonnement à supprimer.");
            }
        }

        public static bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }

        


        #endregion Abonnement Revues


        private void FrmMediatek_Load(object sender, EventArgs e)
        {
            List<Utilisateur> utilisateur = controller.GetUtilisateur(_username);
            string serviceType = utilisateur[0].IdService;
            if(serviceType == "1")
            {
                ExpirationForm expirationForm = new ExpirationForm();
                expirationForm.ShowDialog();
            }
        }

        private void FrmMediatek_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
