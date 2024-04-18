using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class MajCommande
    {
        public string id { get; set; }
        public string idLivreDvd { get; set; }
        public int nbExemplaire { get; set; }
        public string idEtape { get; set; }

        public MajCommande(string id, string idLivreDvd, int nbExemplaire, string idEtape)

        {
            this.id = id;
            this.idLivreDvd = idLivreDvd;
            this.nbExemplaire = nbExemplaire;
            this.idEtape = idEtape;

        }
    }
}
