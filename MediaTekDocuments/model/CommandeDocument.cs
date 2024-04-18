using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class CommandeDocument : Commande
    {
        public string IdLivreDvd { get; set; }
        public int NbExemplaire { get; set; }
        public string IdEtape { get; set; }
        public string Etape { get; set; }

        public CommandeDocument(string id, DateTime dateCommande, int montant,
                                 string idLivreDvd, int nbExemplaire, string idEtape)
                                : base(id, dateCommande, montant)
        {

            this.IdLivreDvd = idLivreDvd;
            this.NbExemplaire = nbExemplaire;
            this.IdEtape = idEtape;
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;

        }
    }
}
