using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class CommandeDocumentTests
    {
        private const string id = "1";
        private static DateTime dateCommande = new DateTime(2024, 4, 15);
        private const int montant = 45;
        private const string idLivreDvd = "20001";
        private const int nbExemplaire = 15;
        private const string idEtape = "2";
        private static readonly CommandeDocument commande = new CommandeDocument(id, dateCommande, montant, idLivreDvd, nbExemplaire, idEtape);

        [TestMethod()]
        public void CommandeDocumentTest()
        {
            Assert.AreEqual(id, commande.Id, "Devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, commande.DateCommande, "Devrait réussir : dateCommande valorisé");
            Assert.AreEqual(montant, commande.Montant, "Devrait réussir : montant valorisé");
            Assert.AreEqual(idLivreDvd, commande.IdLivreDvd, "Devrait réussir : idLivreDvd valorisé");
            Assert.AreEqual(nbExemplaire, commande.NbExemplaire, "Devrait réussir : nbExemplaire valorisé");
            Assert.AreEqual(idEtape, commande.IdEtape, "Devrait réussir : idEtape valorisé");
        }
    }
}