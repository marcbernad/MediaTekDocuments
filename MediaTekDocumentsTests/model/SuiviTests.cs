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
    public class SuiviTests
    {
        private const int idSuivi = 1;
        private const string etape = "en cours";
        private static readonly Suivi suivi = new Suivi(idSuivi, etape);

        [TestMethod()]
        public void SuiviTest()
        {
            Assert.AreEqual(idSuivi, suivi.IdSuivi, "devrait réussir : idSuivi valorisé");
            Assert.AreEqual(etape, suivi.Etape, "devrait réussir : etape valorisé");
        }
    }
}