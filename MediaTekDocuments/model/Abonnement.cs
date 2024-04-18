using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Abonnement : Commande
    {
        public DateTime DateFinAbonnement { get; set; }
        public string IdRevue { get; set; }


        public Abonnement()
        {

        }

        public Abonnement(string id, DateTime dateFinAbonnement, string idRevue, DateTime dateCommande, int montant): base(id, dateCommande, montant)
        {
            this.Id = id;
            this.DateFinAbonnement = dateFinAbonnement;
            this.IdRevue = idRevue;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }

        public Abonnement(string id, DateTime dateFinAbonnement, string idRevue)
        {
            this.Id = id;
            this.DateFinAbonnement = dateFinAbonnement;
            this.IdRevue = idRevue;
        }
    }
}
