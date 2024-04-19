using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Serilog;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";

        private static readonly string connectionName = "MediaTekDocuments.Properties.Settings.apiAuthenticationString";

        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour delete
        private const string DELETE = "DELETE";
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("logs/log.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

            String authenticationString;
            try
            {
                authenticationString = GetConnectionStringByName(connectionName);
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        static string GetConnectionStringByName(string name)
        {
            string returnValue = null;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings != null)
                returnValue = settings.ConnectionString;
            return returnValue;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre");
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon");
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public");
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre");
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd");
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue");
            return lesRevues;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire/" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "CreerExemplaire : erreur lors de la conversion de l'objet au format JSON");
            }
            return false; 
        }

        /// <summary>
        /// Retourne les commandes d'un document
        /// </summary>
        /// <param name="idLivreDvd">id du document concerné</param>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandeDocument(string idLivreDvd)
        {
            String jsonIdLivreDvd = convertToJson("id", idLivreDvd);
            Console.WriteLine("********" + uriApi + "commandedocument/" + jsonIdLivreDvd);
            List<CommandeDocument> lesCommandes = TraitementRecup<CommandeDocument>(GET, "commandedocument/" + jsonIdLivreDvd);
            return lesCommandes;
        }

        /// <summary>
        /// ecriture d'une commande de document en base de données
        /// </summary>
        /// <param name="commandedocument">commande à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerCommande(CommandeDocument commandeDocument)
        {
            String jsonCommandeDocument = JsonConvert.SerializeObject(commandeDocument, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new CustomDateTimeConverter() }
            });
            Console.WriteLine("********" + uriApi + "commandedocument/" + jsonCommandeDocument);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<CommandeDocument> listeCommandeDocument = TraitementRecup<CommandeDocument>(POST, "commandedocument/" + jsonCommandeDocument);
                return (listeCommandeDocument != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "CreerCommande : erreur lors de la conversion de l'objet au format JSON");
            }
            return false;
        }

        public bool ModifierCommande(MajCommande commandeDocument, string id)
        {

            String jsonModifCommande = JsonConvert.SerializeObject(commandeDocument, new CustomDateTimeConverter());
            Console.WriteLine("********" + uriApi + "commandedocument/" + id + "/" + jsonModifCommande);
            try
            {
                List<MajCommande> listeCommandeDocument = TraitementRecup<MajCommande>(PUT, "commandedocument/"+ id + "/" + jsonModifCommande);
                return (listeCommandeDocument != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "ModifierCommande : erreur lors de la conversion de l'objet au format JSON");
            }
            return false;
        }

        public bool SupprimerCommande(Commande commande)
        {
            String jsonSupprCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            Console.WriteLine("********" + uriApi + "commande/" + jsonSupprCommande);
            try
            {
                List<Commande> listeCommande = TraitementRecup<Commande>(DELETE, "commande/" + jsonSupprCommande);
                return (listeCommande != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "SupprimerCommande : erreur lors de la conversion de l'objet au format JSON");
            }
            return false;
        }

        public List<Abonnement> GetAbonnementRevue(string idRevue)
        {
            String jsonIdRevue = convertToJson("id", idRevue);
            Console.WriteLine("********" + uriApi + "abonnement/" + jsonIdRevue);

            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "abonnement/" + jsonIdRevue);
            return lesAbonnements;
        }

        public List<ExpirationAbonnements> GetExpirationAbonnements(string today)
        {
            string jsonToday = convertToJson("date", today);
            Console.WriteLine("********" + uriApi + "expirationabonnements/" + jsonToday);

            List<ExpirationAbonnements> lesExpirations = TraitementRecup<ExpirationAbonnements>(GET, "expirationabonnements/" + jsonToday);
            Console.WriteLine("*********" + lesExpirations.Count);
            return lesExpirations;
        }

        public List<Utilisateur> GetUtilisateur(string nom)
        {
            string jsonNom = convertToJson("nom", nom);
            Console.WriteLine("*********" + uriApi + "utilisateur/" + jsonNom);
            List<Utilisateur> utilisateur = TraitementRecup<Utilisateur>(GET, "utilisateur/" + jsonNom);
            return utilisateur;
        }

        public bool CreerAbonnement(Abonnement abonnement)
        {
            String jsonAbonnement = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());
            Console.WriteLine("********" + uriApi + "abonnement/" + jsonAbonnement);
            try
            {    
                List<Abonnement> listeAbonnements = TraitementRecup<Abonnement>(POST, "abonnement/" + jsonAbonnement);
                return (listeAbonnements != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "CreerAbonnement : erreur lors de la conversion de l'objet au format JSON");
            }
            return false;
        }

        public bool SupprimerAbonnement(Abonnement abonnement)
        {
            String jsonSupprAbonnement = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());
            Console.WriteLine("********" + uriApi + "abonnement/" + jsonSupprAbonnement);
            try
            {
                List<Abonnement> listeAbonnement = TraitementRecup<Abonnement>(DELETE, "abonnement/" + jsonSupprAbonnement);
                return (listeAbonnement != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "SupprimerAbonnement : erreur lors de la conversion de l'objet au format JSON");
            }
            return false;
        }


        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message)
        {
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                    Log.Error("Le code 200 est attendu mais le code {Code} a été récupéré avec le message : {Retour}", code, (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
                Log.Fatal(e, "Erreur lors de l'accès à l'API : {Message}", e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

    }
}
