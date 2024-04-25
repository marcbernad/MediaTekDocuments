using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.view.Tests
{
    [TestClass()]
    public class FrmMediatekTests
    {

        [TestMethod()]
        public void ParutionDansAbonnementTest()
        {
            bool result1 = FrmMediatek.ParutionDansAbonnement(
                dateCommande: new DateTime(2024, 4, 1),
                dateFinAbonnement: new DateTime(2024, 4, 15),
                dateParution: new DateTime(2024, 4, 10)
            );
            Assert.IsTrue(result1);

            // Test avec dateParution avant dateCommande
            bool result2 = FrmMediatek.ParutionDansAbonnement(
                dateCommande: new DateTime(2024, 4, 15),
                dateFinAbonnement: new DateTime(2024, 4, 30),
                dateParution: new DateTime(2024, 4, 10)
            );
            Assert.IsFalse(result2);

            // Test avec dateParution après dateFinAbonnement
            bool result3 = FrmMediatek.ParutionDansAbonnement(
                dateCommande: new DateTime(2024, 4, 1),
                dateFinAbonnement: new DateTime(2024, 4, 15),
                dateParution: new DateTime(2024, 4, 20)
            );
            Assert.IsFalse(result3);

            // Test avec dateParution égale à dateCommande
            bool result4 = FrmMediatek.ParutionDansAbonnement(
                dateCommande: new DateTime(2024, 4, 1),
                dateFinAbonnement: new DateTime(2024, 4, 15),
                dateParution: new DateTime(2024, 4, 1)
            );
            Assert.IsTrue(result4);

            // Test avec dateParution égale à dateFinAbonnement
            bool result5 = FrmMediatek.ParutionDansAbonnement(
                dateCommande: new DateTime(2024, 4, 1),
                dateFinAbonnement: new DateTime(2024, 4, 15),
                dateParution: new DateTime(2024, 4, 15)
            );
            Assert.IsTrue(result5);
        }
    }
}