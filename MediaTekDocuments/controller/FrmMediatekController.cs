using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }

        /// <summary>
        /// Récupère les commande d'un document dans la bdd
        /// </summary>
        /// <param name="idDocument">Id du document concerné</param>
        /// <returns>Liste de commandes</returns>
        public List<CommandeDocument> GetCommandeDocument(string idDocument)
        {
            return access.GetCommandeDocument(idDocument);
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            return access.GetExemplairesRevue(idDocument);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="commande">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommande(CommandeDocument commandeDocument)
        {
            return access.CreerCommande(commandeDocument);
        }

        /// <summary>
        /// Modifie l'état d'une commande dans la bdd
        /// </summary>
        /// <param name="commandeDocument">L'objet MajCommande concerné</param>
        /// <param name="id">L'id de la commande</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierCommande(MajCommande commandeDocument, string id)
        {
            return access.ModifierCommande(commandeDocument, id);
        }

        /// <summary>
        /// Supprime une commande de la bdd
        /// </summary>
        /// <param name="commande">L'objet Commande concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerCommande(Commande commande)
        {
            return access.SupprimerCommande(commande);
        }

        /// <summary>
        /// Récupère les abonnements de la revue sélectionnée
        /// </summary>
        /// <param name="id">Id de la revue sélectionnée</param>
        /// <returns>liste d'abonnements</returns>
        public List<Abonnement> GetAbonnementRevue(string id)
        {
            return access.GetAbonnementRevue(id);
        }

        /// <summary>
        /// Crée un abonnement dans la bdd
        /// </summary>
        /// <param name="abonnement">L'objet Abonnement concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            return access.CreerAbonnement(abonnement);
        }

        /// <summary>
        /// Supprime un abonnement de la bdd
        /// </summary>
        /// <param name="abonnement">L'objet Abonnement concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerAbonnement(Abonnement abonnement)
        {
            return access.SupprimerAbonnement(abonnement);
        }

        /// <summary>
        /// Récupère les abonnements expirés dans les 30 jours dans la bdd
        /// </summary>
        /// <param name="today">Date du jour</param>
        /// <returns>Liste d'id et date de fin d'abonnement</returns>
        public List<ExpirationAbonnements> GetExpirationAbonnements(string today)
        {
            return access.GetExpirationAbonnements(today);
        }

        /// <summary>
        /// Récupère un utilisateur dans la bdd
        /// </summary>
        /// <param name="nom">Nom de l'utilisateur</param>
        /// <returns>Un utilisateur</returns>
        public List<Utilisateur> GetUtilisateur(string nom)
        {
            return access.GetUtilisateur(nom);
        }
    }
}
