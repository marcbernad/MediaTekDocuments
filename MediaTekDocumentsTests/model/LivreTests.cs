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
    public class LivreTests
    {
        private const string isbn = "111";
        private const string auteur = "st ex";
        private const string collection = "collection";
        private const string id = "00005";
        private const string titre = "Test";
        private const string image = "";
        private const string idGenre = "3";
        private const string genre = "Livre";
        private const string idPublic = "123";
        private const string Public = "Adulte";
        private const string idRayon = "2";
        private const string rayon = "Polar";
        private static readonly Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, Public, idRayon, rayon);

        [TestMethod()]
        public void LivreTest()
        {
            Assert.AreEqual(isbn, livre.Isbn, "devrait réussir : duree valorisé");
            Assert.AreEqual(auteur, livre.Auteur, "devrait réussir : realisateur valorisé");
            Assert.AreEqual(collection, livre.Collection, "devrait réussir : synopsis valorisé");
            Assert.AreEqual(id, livre.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, livre.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, livre.Image, "devrait réussir : image valorisé");
            Assert.AreEqual(idGenre, livre.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, livre.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, livre.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(Public, livre.Public, "devrait réussir : Public valorisé");
            Assert.AreEqual(idRayon, livre.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, livre.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}