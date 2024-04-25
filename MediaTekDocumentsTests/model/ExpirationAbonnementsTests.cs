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
    public class ExpirationAbonnementsTests
    {

        private const string titre = "Revue";
        private static DateTime dateFinAbonnement = new DateTime(2024, 4, 15);
        private static readonly ExpirationAbonnements expirAbo = new ExpirationAbonnements(dateFinAbonnement, titre);


        [TestMethod()]
        public void ExpirationAbonnementsTest()
        {
            Assert.AreEqual(titre, expirAbo.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(dateFinAbonnement, expirAbo.DateFinAbonnement, "Devrait réussir : dateFinAbonnement valorisé");
        }
    }
}