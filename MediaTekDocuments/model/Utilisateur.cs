using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Utilisateur
    {
        public string Id { get; set; }
        public string Nom { get; set; }
        public string Password { get; set; }
        public string IdService { get; set; }

        public Utilisateur(string id, string nom, string password, string idService)
        {
            this.Id = id;
            this.Nom = nom;
            this.Password = password;
            this.IdService = idService;
        }
    }
}
