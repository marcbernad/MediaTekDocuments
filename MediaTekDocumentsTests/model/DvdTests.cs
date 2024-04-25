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
    public class DvdTests
    {
        private const int duree = 120;
        private const string realisateur = "Wes Anderson";
        private const string synopsis = "Synopsis du film";
        private const string id = "00005";
        private const string titre = "Test";
        private const string image = "";
        private const string idGenre = "3";
        private const string genre = "Livre";
        private const string idPublic = "123";
        private const string Public = "Adulte";
        private const string idRayon = "2";
        private const string rayon = "Polar";
        private static readonly Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis,
             idGenre, genre, idPublic, Public, idRayon, rayon);

        [TestMethod()]
        public void DvdTest()
        {

            Assert.AreEqual(duree, dvd.Duree, "devrait réussir : duree valorisé");
            Assert.AreEqual(realisateur, dvd.Realisateur, "devrait réussir : realisateur valorisé");
            Assert.AreEqual(synopsis, dvd.Synopsis, "devrait réussir : synopsis valorisé");
            Assert.AreEqual(id, dvd.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, dvd.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, dvd.Image, "devrait réussir : image valorisé");
            Assert.AreEqual(idGenre, dvd.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, dvd.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, dvd.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(Public, dvd.Public, "devrait réussir : Public valorisé");
            Assert.AreEqual(idRayon, dvd.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, dvd.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}