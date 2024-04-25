# MediatekDocuments
Cette application permet de gérer les documents (livres, DVD, revues) d'une médiathèque. Elle a été codée en C# sous Visual Studio 2019. C'est une application de bureau, prévue d'être installée sur plusieurs postes accédant à la même base de données.<br>
L'application exploite une API REST pour accéder à la BDD MySQL. Des explications sont données plus loin, ainsi que le lien de récupération.
## Présentation
L'application permet de consulter les informations des documents, valider la parution d'une revue, gérer les commandes de livres ou dvd et les abonnements aux revues.<br>
![img1](https://github.com/marcbernad/MediaTekDocuments/assets/115026928/973ab71c-23db-455a-94d2-3963934f36f5)

<br>L'application ne comporte qu'une seule fenêtre divisée en plusieurs onglets.

## Connexion
L'application nécessite de se connecter avec ses identifiants. Selon le service d'affection de l'utilisateur, différents droits s'appliquent.<br>
![img2](https://github.com/marcbernad/MediaTekDocuments/assets/115026928/3612d19c-34c7-42a5-bd32-776c7359c816)<br>

## Rappel expiration des abonnements
Pour les salariés du service administratif qui s'occupe des commandes et des abonnements, un rappel des abonnements bientôt expirés s'affiche après connexion.<br>
![img3](https://github.com/marcbernad/MediaTekDocuments/assets/115026928/2cbbd3d2-db58-44bf-88fc-a212d4be86da)<br>

## Les différents onglets
### Onglet 1 : Livres
Cet onglet présente la liste des livres, triée par défaut sur le titre.<br>
La liste comporte les informations suivantes : titre, auteur, collection, genre, public, rayon.
![img3](https://github.com/CNED-SLAM/MediaTekDocuments/assets/100127886/e3f31979-cf24-416d-afb1-a588356e8966)
#### Recherches
<strong>Par le titre :</strong> Il est possible de rechercher un ou plusieurs livres par le titre. La saisie dans la zone de recherche se fait en autocomplétions sans tenir compte de la casse. Seuls les livres concernés apparaissent dans la liste.<br>
<strong>Par le numéro :</strong> il est possible de saisir un numéro et, en cliquant sur "Rechercher", seul le livre concerné apparait dans la liste (ou un message d'erreur si le livre n'est pas trouvé, avec la liste remplie à nouveau).
#### Filtres
Il est possible d'appliquer un filtre (un seul à la fois) sur une de ces 3 catégories : genre, public, rayon.<br>
Un combo par catégorie permet de sélectionner un item. Seuls les livres correspondant à l'item sélectionné, apparaissent dans la liste (par exemple, en choisissant le genre "Policier", seuls les livres de genre "Policier" apparaissent).<br>
Le fait de sélectionner un autre filtre ou de faire une recherche, annule le filtre actuel.<br>
Il est possible aussi d'annuler le filtre en cliquant sur une des croix.
#### Tris
Le fait de cliquer sur le titre d'une des colonnes de la liste des livres, permet de trier la liste par rapport à la colonne choisie.
#### Affichage des informations détaillées
Si la liste des livres contient des éléments, par défaut il y en a toujours un de sélectionné. Il est aussi possible de sélectionner une ligne (donc un livre) en cliquant n'importe où sur la ligne.<br>
La partie basse de la fenêtre affiche les informations détaillées du livre sélectionné (numéro de document, code ISBN, titre, auteur(e), collection, genre, public, rayon, chemin de l'image) ainsi que l'image.
### Onglet 2 : DVD
Cet onglet présente la liste des DVD, triée par titre.<br>
La liste comporte les informations suivantes : titre, durée, réalisateur, genre, public, rayon.<br>
Le fonctionnement est identique à l'onglet des livres.<br>
La seule différence réside dans certaines informations détaillées, spécifiques aux DVD : durée (à la place de ISBN), réalisateur (à la place de l'auteur), synopsis (à la place de collection).
### Onglet 3 : Revues
Cet onglet présente la liste des revues, triées par titre.<br>
La liste comporte les informations suivantes : titre, périodicité, délai mise à dispo, genre, public, rayon.<br>
Le fonctionnement est identique à l'onglet des livres.<br>
La seule différence réside dans certaines informations détaillées, spécifiques aux revues : périodicité (à la place de l'auteur), délai mise à dispo (à la place de collection).
### Onglet 4 : Parutions des revues
Cet onglet permet d'enregistrer la réception de nouvelles parutions d'une revue.<br>
Il se décompose en 2 parties (groupbox).
#### Partie "Recherche revue"
Cette partie permet, à partir de la saisie d'un numéro de revue (puis en cliquant sur le bouton "Rechercher"), d'afficher toutes les informations de la revue (comme dans l'onglet précédent), ainsi que son image principale en petit, avec en plus la liste des parutions déjà reçues (numéro, date achat, chemin photo). Sur la sélection d'une ligne dans la liste des parutions, la photo de la parution correspondante s'affiche à droite.<br>
Dès qu'un numéro de revue est reconnu et ses informations affichées, la seconde partie ("Nouvelle parution réceptionnée pour cette revue") devient accessible.<br>
Si une modification est apportée au numéro de la revue, toutes les zones sont réinitialisées et la seconde partie est rendue inaccessible, tant que le bouton "Rechercher" n'est pas utilisé.
#### Partie "Nouvelle parution réceptionnée pour cette revue"
Cette partie n'est accessible que si une revue a bien été trouvée dans la première partie.<br>
Il est possible alors de réceptionner une nouvelle parution en saisissant son numéro, en sélectionnant une date (date du jour proposée par défaut) et en cherchant l'image correspondante (optionnel) qui doit alors s'afficher à droite.<br>
Le clic sur "Valider la réception" va permettre d'ajouter un tuple dans la table Exemplaire de la BDD. La parution correspondante apparaitra alors automatiquement dans la liste des parutions et les zones de la partie "Nouvelle parution réceptionnée pour cette revue" seront réinitialisées.<br>
Si le numéro de la parution existe déjà, il n’est pas ajouté et un message est affiché.<br>
![img4](https://github.com/CNED-SLAM/MediaTekDocuments/assets/100127886/225e10f2-406a-4b5e-bfa9-368d45456056)<br>

### Onglet 5 : Commandes Livres
#### Recherche
La recherche se fait par le numéro de document.
#### Informations détaillées
Les informations détaillées sont identiques à celles de l'onglet Livre.
#### Gestion de la commande
Les commandes en cours ou passées sont affichées.<br>
Il est possible de modifier le statut de la commande ou la supprimer si elle n'est pas encore livrée.
#### Nouvelle commande
Il est possible d'ajouter une nouvelle commande en saisissant les informations de commande.<br><br>
![img5](https://github.com/marcbernad/MediaTekDocuments/assets/115026928/412facd5-c611-41d6-9b8d-e98bd6c763d9)

### Onglet 6 : Commandes DVD
L'onglet est identique à l'onglet Commandes Livres, à l'exception des informations détaillées adaptées aux dvd.
### Onglet 7 : Gestion Revues
#### Recherche
La recherche se fait par le numéro de revue.
#### Informations détaillées
Les informations détaillées sont identiques à celles de l'onglet Revues.
#### Gestion des abonnements
Les abonnements en cours ou passées sont affichés.<br>
Il est possible de supprimer l'abonnement si aucun numéro n'est paru durant la période d'abonnement.
#### Nouvel abonnement ou renouvellement
Il est possible d'ajouter un nouvel abonnement en saisissant les informations d'abonnement.<br>
![img6](https://github.com/marcbernad/MediaTekDocuments/assets/115026928/7f47b4c4-8d2c-4f5f-9047-6dd02a5d2790)

## La base de données
La base de données 'mediatek86 ' est au format MySQL.<br>
Voici sa structure :<br>
![img7](https://github.com/marcbernad/MediaTekDocuments/assets/115026928/b1a2bd3a-ab2d-4e8e-b1a4-18f5c0bd0518)

<br>On distingue les documents "génériques" (ce sont les entités Document, Revue, Livres-DVD, Livre et DVD) des documents "physiques" qui sont les exemplaires de livres ou de DVD, ou bien les numéros d’une revue ou d’un journal.<br>
Chaque exemplaire est numéroté à l’intérieur du document correspondant, et a donc un identifiant relatif. Cet identifiant est réel : ce n'est pas un numéro automatique. <br>
Un exemplaire est caractérisé par :<br>
. un état d’usure, les différents états étant mémorisés dans la table Etat ;<br>
. sa date d’achat ou de parution dans le cas d’une revue ;<br>
. un lien vers le fichier contenant sa photo de couverture de l'exemplaire, renseigné uniquement pour les exemplaires des revues, donc les parutions (chemin complet) ;
<br>
Un document a un titre (titre de livre, titre de DVD ou titre de la revue), concerne une catégorie de public, possède un genre et est entreposé dans un rayon défini. Les genres, les catégories de public et les rayons sont gérés dans la base de données. Un document possède aussi une image dont le chemin complet est mémorisé. Même les revues peuvent avoir une image générique, en plus des photos liées à chaque exemplaire (parution).<br>
Une revue est un document, d’où le lien de spécialisation entre les 2 entités. Une revue est donc identifiée par son numéro de document. Elle a une périodicité (quotidien, hebdomadaire, etc.) et un délai de mise à disposition (temps pendant lequel chaque exemplaire est laissé en consultation). Chaque parution (exemplaire) d'une revue n'est disponible qu'en un seul "exemplaire".<br>
Un livre a aussi pour identifiant son numéro de document, possède un code ISBN, un auteur et peut faire partie d’une collection. Les auteurs et les collections ne sont pas gérés dans des tables séparées (ce sont de simples champs textes dans la table Livre).<br>
De même, un DVD est aussi identifié par son numéro de document, et possède un synopsis, un réalisateur et une durée. Les réalisateurs ne sont pas gérés dans une table séparée (c’est un simple champ texte dans la table DVD).
Enfin, 3 tables permettent de mémoriser les données concernant les commandes de livres ou DVD et les abonnements. Une commande est effectuée à une date pour un certain montant. Un abonnement est une commande qui a pour propriété complémentaire la date de fin de l’abonnement : il concerne une revue.  Une commande de livre ou DVD a comme caractéristique le nombre d’exemplaires commandé et concerne donc un livre ou un DVD.<br>
Un utilisateur possède un identifiant et possède un nom, un password (mot de passe) et un identifiant le reliant à son service.<br>
<br>
## L'API REST
L'accès à la BDD se fait à travers une API REST protégée par une authentification basique.<br>
Le code de l'API se trouve ici :<br>
https://github.com/marcbernad/ApiMediatek<br>
avec toutes les explications pour l'utiliser (dans le readme).
## Installation de l'application
Télécharger le fichier setup.msi dans le dépôt, le lancer et suivre les indications d'installation.
