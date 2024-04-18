using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    class CommandeDocumentDisplay
    {
        public string Id { get; set; }
        public DateTime DateCommande { get; set; }
        public int Montant { get; set; }
        public string IdLivreDvd { get; set; }
        public int NbExemplaire { get; set; }
        public string IdEtape { get; set; }
        public string Etape { get; set; }
    }
}
