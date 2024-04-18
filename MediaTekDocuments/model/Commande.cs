using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
     public class Commande
     {
        public string Id { get; set; }
        public DateTime DateCommande { get; set; }
        public int Montant { get; set; }

        public Commande()
        {
        }

        public Commande(string id, DateTime dateCommande, int montant)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
     }
}
