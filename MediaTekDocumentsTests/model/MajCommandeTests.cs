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
    public class MajCommandeTests
    {
        private const string id = "01";
        private const string idLivreDvd = "00005";
        private const int nbExemplaire = 13;
        private const string idEtape = "1";
        private static readonly MajCommande commande = new MajCommande(id, idLivreDvd, nbExemplaire, idEtape);


        [TestMethod()]
        public void MajCommandeTest()
        {
            Assert.AreEqual(id, commande.id, "devrait réussir : id valorisé");
            Assert.AreEqual(idLivreDvd, commande.idLivreDvd, "devrait réussir : idLivreDvd valorisé");
            Assert.AreEqual(nbExemplaire, commande.nbExemplaire, "devrait réussir : nbExemplaire valorisé");
            Assert.AreEqual(idEtape, commande.idEtape, "devrait réussir : idEtape valorisé");

        }
    }
}