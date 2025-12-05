# AdminIvoire

Ceci est un petit projet fun pour exposer une api décrivant les différentes subdivisions administratives de la Côte d'Ivoire.

## Prérequis

- [Docker](https://www.docker.com/get-started) (version 20.10 ou supérieure)
- [Docker Compose](https://docs.docker.com/compose/install/) (version 2.0 ou supérieure)
- Un compte Google Maps avec une clé API (pour la fonctionnalité de géocodage)

## Configuration

### 1. Configuration des secrets

L'application utilise Docker secrets pour gérer les données sensibles. Vous devez créer les fichiers de secrets suivants dans le répertoire `secrets/` :

#### Créer les fichiers de secrets

Les fichiers suivants doivent être créés avec vos valeurs réelles :

- `secrets/postgres_user.txt` - Nom d'utilisateur PostgreSQL (par défaut: `adminivoire`)
- `secrets/postgres_password.txt` - Mot de passe PostgreSQL
- `secrets/googlemaps_apikey.txt` - Clé API Google Maps

**Exemple de contenu :**

```bash
# secrets/postgres_user.txt
adminivoire

# secrets/postgres_password.txt
VotreMotDePasseSecurise123!

# secrets/googlemaps_apikey.txt
AIzaSyBvOkBvXxxxxxxxxxxxxxxxxxxxxxxx
```

> **Important :** Les fichiers de secrets sont déjà ignorés par Git (via `.gitignore`). Ne commitez jamais ces fichiers dans le dépôt.

### 2. Variables d'environnement (optionnel)

Vous pouvez créer un fichier `.env` à la racine du projet pour définir des variables d'environnement :

```env
POSTGRES_DB=adminivoire
```

Si `POSTGRES_DB` n'est pas défini, la valeur par défaut `adminivoire` sera utilisée.

## Exécution de l'application

### Première exécution

Pour la première fois, vous devez construire les images Docker :

```bash
docker-compose up --build
```

### Exécutions suivantes

Pour démarrer l'application après la première construction :

```bash
docker-compose up
```

Pour exécuter en arrière-plan (mode détaché) :

```bash
docker-compose up -d
```

### Arrêter l'application

Pour arrêter l'application :

```bash
docker-compose down
```

Pour arrêter et supprimer les volumes (⚠️ cela supprimera les données de la base de données) :

```bash
docker-compose down -v
```

## Accès à l'application

Une fois l'application démarrée, vous pouvez y accéder via :

- **API HTTP** : <http://localhost:8080>
- **API HTTPS** : <https://localhost:8081>
- **Base de données PostgreSQL** : localhost:5432

## Structure des services

L'application est composée de deux services Docker :

1. **adminivoire.webapi** - L'API ASP.NET Core
   - Ports : 8080 (HTTP), 8081 (HTTPS)
   - Lit les secrets pour configurer la connexion à la base de données et la clé API Google Maps

2. **adminivoire.db** - La base de données PostgreSQL
   - Port : 5432
   - Utilise les secrets pour l'authentification

## Dépannage

### Vérifier que les secrets sont correctement configurés

Assurez-vous que les fichiers dans `secrets/` existent et contiennent les bonnes valeurs :

```bash
# Windows PowerShell
Get-Content secrets\postgres_password.txt
Get-Content secrets\postgres_user.txt
Get-Content secrets\googlemaps_apikey.txt

# Linux/Mac
cat secrets/postgres_password.txt
cat secrets/postgres_user.txt
cat secrets/googlemaps_apikey.txt
```

### Vérifier les logs

Pour voir les logs des services :

```bash
# Tous les services
docker-compose logs

# Un service spécifique
docker-compose logs adminivoire.webapi
docker-compose logs adminivoire.db

# Suivre les logs en temps réel
docker-compose logs -f
```

### Reconstruire l'application

Si vous modifiez le code ou la configuration Docker, reconstruisez les images :

```bash
docker-compose up --build --force-recreate
```

### Problèmes de connexion à la base de données

Si l'application ne peut pas se connecter à la base de données :

1. Vérifiez que le service `adminivoire.db` est démarré : `docker-compose ps`
2. Vérifiez les logs de la base de données : `docker-compose logs adminivoire.db`
3. Vérifiez que les secrets `postgres_user.txt` et `postgres_password.txt` sont corrects
4. Vérifiez que la variable `POSTGRES_DB` correspond au nom de la base dans la chaîne de connexion

## Développement

Pour le développement local sans Docker, consultez la documentation du projet pour les instructions spécifiques.

## Sécurité

- ⚠️ **Ne commitez jamais les fichiers de secrets** dans le dépôt Git
- ⚠️ Utilisez des mots de passe forts pour la base de données en production
- ⚠️ Protégez votre clé API Google Maps et limitez ses restrictions dans la console Google Cloud
