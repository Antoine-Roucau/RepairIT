# RepairIT

Application de gestion pour RepairIT, société de réparation et de vente d'ordinateurs.
L'application est découpée en micro-services ASP.NET Core avec un front Blazor.

## Version 1

- Création de la solution et des projets (front et APIs)
- CRUD des ordinateurs (`RepairIT.CatalogService`)
- Création des utilisateurs (`RepairIT.UserService`)
- EF Core avec SQLite et initialisation de la base
- Tests unitaires (`tests/RepairIT.Tests`)
- Injection des dépendances, Logging et Configuration

## Lancement

- Tous les projets : `start-all.bat`
- Tests : `dotnet test`

## Architecture

| Projet                           | Rôle                                   |
|----------------------------------|----------------------------------------|
| RepairIT.Client                  | Front Blazor (port 5000)               |
| RepairIT.Gateway                 | Point d'entrée unique vers les APIs    |
| RepairIT.AuthentificationService | Authentification (Keycloak)            |
| RepairIT.UserService             | Utilisateurs                           |
| RepairIT.CatalogService          | Ordinateurs                            |
| RepairIT.CustomerService         | Clients (V2)                           |
| RepairIT.RepairService           | Réparations (V2)                       |
| RepairIT.SalesService            | Ventes (V3)                            |
| RepairIT.Shared                  | Classes partagées (requêtes, DTO, enums) |
| RepairIT.Tests                   | Tests unitaires                        |

Chaque service est organisé en dossiers : `Entite` (tables), `DB` (contexte, initialisation, migrations),
`Services` (logique métier) et `Controllers` (routes HTTP).

## Modèles de base de données

### catalog.db – Ordinateurs

| Colonne       | Type          | Contrainte                     |
|---------------|---------------|--------------------------------|
| Id            | int           | clé primaire                   |
| Marque        | texte (100)   | obligatoire                    |
| Modele        | texte (100)   | obligatoire                    |
| Processeur    | texte (100)   | obligatoire                    |
| RamGo         | int           |                                |
| StockageGo    | int           |                                |
| Type          | texte         | Portable, Fixe ou ToutEnUn     |
| Prix          | decimal(10,2) |                                |
| QuantiteStock | int           |                                |
| DateAjout     | date          | remplie à la création          |

### user.db – Utilisateurs

| Colonne        | Type        | Contrainte                          |
|----------------|-------------|-------------------------------------|
| Id             | int         | clé primaire                        |
| KeycloakUserId | texte (64)  | `non-synchronise` par défaut        |
| Nom            | texte (100) | obligatoire                         |
| Prenom         | texte (100) | obligatoire                         |
| Email          | texte (200) | obligatoire, unique                 |
| Role           | texte       | Gestionnaire ou Administrateur      |
| EstActif       | bool        | vrai à la création                  |

Pas de mot de passe en base : l'authentification sera gérée par Keycloak.

## Choix techniques

- **Une base par micro-service** : chaque service reste indépendant des autres.
- **SQLite** pour la V1, comme demandé. La chaîne de connexion est dans `appsettings.json`,
  ce qui facilitera le passage à PostgreSQL en V2.
- **Initialisation de la base** : au démarrage, chaque service applique les migrations EF Core puis
  ajoute des données de départ si la table est vide.
- **Enums enregistrés en texte** (en base et dans le JSON) : plus lisible qu'un nombre.
- **DTO et requêtes dans `RepairIT.Shared`** : l'API ne renvoie pas directement les entités, et les
  requêtes contiennent la validation des champs (`[Required]`, `[StringLength]`…).
- **Keycloak non géré en V1** : un nouvel utilisateur reçoit la valeur par défaut `non-synchronise`
  pour `KeycloakUserId`.
- **Email unique** : un index unique en base, et le service renvoie une erreur 409 si l'email existe déjà.
- **Injection des dépendances** : le contexte EF Core et les services sont enregistrés dans `Program.cs`
  (`AddDbContext`, `AddScoped`). Les contrôleurs reçoivent une interface (`IOrdinateurService`,
  `IUtilisateurService`), ce qui permet de la remplacer par un faux service dans les tests.
- **Logging** : `ILogger` est injecté dans les services. `LogInformation` pour les actions réussies,
  `LogWarning` pour les échecs (élément introuvable, email déjà utilisé).
- **Configuration** : `IConfiguration` lit dans `appsettings.json` la chaîne de connexion et la valeur
  Keycloak par défaut.
- **Tests** : xUnit et Moq, avec une base EF Core en mémoire différente pour chaque test.
  Sont testés le service des ordinateurs, le service des utilisateurs et le contrôleur des utilisateurs.