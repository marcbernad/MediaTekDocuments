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
    public class AbonnementTests
    {

        private const string id = "1";
        private static DateTime dateFinAbonnement = new DateTime(2024, 4, 25);
        private const string idRevue = "10002";
        private static DateTime dateCommande = new DateTime(2024, 4, 15);
        private const int montant = 45;
        private static readonly Abonnement abonnement = new Abonnement(id, dateFinAbonnement, idRevue, dateCommande, montant);
        private static readonly Abonnement abonnement2 = new Abonnement(id, dateFinAbonnement, idRevue);

        [TestMethod()]
        public void AbonnementTest1()
        {
            Assert.AreEqual(id, abonnement.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateFinAbonnement, abonnement.DateFinAbonnement, "devrait réussir : dateFinAbonnement valorisé");
            Assert.AreEqual(idRevue, abonnement.IdRevue, "devrait réussir : idRevue valorisé");
            Assert.AreEqual(dateCommande, abonnement.DateCommande, "devrait réussir : dateCommande valorisé");
            Assert.AreEqual(montant, abonnement.Montant, "devrait réussir : montant valorisé");
        }

        [TestMethod()]
        public void AbonnementTest2()
        {
            Assert.AreEqual(id, abonnement2.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateFinAbonnement, abonnement2.DateFinAbonnement, "devrait réussir : dateFinAbonnement valorisé");
            Assert.AreEqual(idRevue, abonnement2.IdRevue, "devrait réussir : idRevue valorisé");
        }
    }
}