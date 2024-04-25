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
    public class UtilisateurTests
    {

        private const string id = "02";
        private const string nom = "alicedupont";
        private const string password = "password123";
        private const string idService = "2";
        private static readonly Utilisateur utilisateur = new Utilisateur(id, nom, password, idService);

        [TestMethod()]
        public void UtilisateurTest()
        {
            Assert.AreEqual(id, utilisateur.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(nom, utilisateur.Nom, "devrait réussir : nom valorisé");
            Assert.AreEqual(password, utilisateur.Password, "devrait réussir : password valorisé");
            Assert.AreEqual(idService, utilisateur.IdService, "devrait réussir : idService valorisé");
        }
    }
}