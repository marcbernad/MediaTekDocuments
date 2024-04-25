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
    public class RevueTests
    {
        private const string periodicite = "mensuel";
        private const int delaiMiseADispo = 20;
        private const string id = "00005";
        private const string titre = "Test";
        private const string image = "";
        private const string idGenre = "3";
        private const string genre = "Livre";
        private const string idPublic = "123";
        private const string Public = "Adulte";
        private const string idRayon = "2";
        private const string rayon = "Polar";
        private static readonly Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, Public, idRayon, rayon, periodicite, delaiMiseADispo);

        [TestMethod()]
        public void RevueTest()
        {

            Assert.AreEqual(periodicite, revue.Periodicite, "devrait réussir : periodicite valorisé");
            Assert.AreEqual(delaiMiseADispo, revue.DelaiMiseADispo, "devrait réussir : delaiMiseADispo valorisé");
            Assert.AreEqual(id, revue.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, revue.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, revue.Image, "devrait réussir : image valorisé");
            Assert.AreEqual(idGenre, revue.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, revue.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, revue.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(Public, revue.Public, "devrait réussir : Public valorisé");
            Assert.AreEqual(idRayon, revue.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, revue.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}