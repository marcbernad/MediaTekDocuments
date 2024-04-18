using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class ExpirationAbonnements
    {
        public string Titre {get; set;}
        public DateTime DateFinAbonnement { get; set; }

        public ExpirationAbonnements(DateTime dateFinAbonnement, string titre)
        {
            this.Titre = titre;
            this.DateFinAbonnement = dateFinAbonnement;
        }
    }
}
