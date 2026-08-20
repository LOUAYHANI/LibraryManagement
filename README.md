# Library Management

API REST de gestion des emprunts pour une petite bibliothèque, développée en .NET 8.

Le projet couvre la gestion du catalogue, des adhérents, des emprunts et des retours, ainsi que les règles de quota et de pénalités de retard.

## Fonctionnalités

- Ajouter un livre avec un ou plusieurs exemplaires physiques
- Ajouter un adhérent Standard ou Student
- Emprunter un exemplaire disponible
- Appliquer les règles d'emprunt :
  - Standard : 3 emprunts actifs maximum, durée de 21 jours
  - Student : 5 emprunts actifs maximum, durée de 28 jours
- Retourner un emprunt
- Calculer le nombre de jours de retard
- Calculer les pénalités :
  - 0,20 € par jour de retard
  - plafond de 10 € par emprunt
- Consulter le montant courant des pénalités d'un adhérent

## Architecture

La solution est organisée en quatre projets principaux :

- `LibraryManagement.Domain`
  - entités et règles métier
- `LibraryManagement.Application`
  - cas d'usage et abstractions nécessaires à l'application
- `LibraryManagement.Infrastructure`
  - persistence avec Entity Framework Core et SQLite
- `LibraryManagement.Api`
  - API REST, controllers, configuration et gestion des erreurs HTTP

Les dépendances sont orientées vers le domaine :

API
 |
 v
Application
 |
 v
Domain

Infrastructure
 |
 +--> Application
 +--> Domain
 
 ## Persistence

La persistence utilise Entity Framework Core avec SQLite.

Les migrations EF Core sont appliquées au démarrage de l'API afin de faciliter l'exécution du projet.

## Lancer le projet

Prérequis :

- .NET 8 SDK

Depuis la racine du projet :
dotnet restore
dotnet run --project src/LibraryManagement.Api

## Quelques choix techniques

Un Book peut avoir plusieurs BookCopy, ce qui permet de gérer séparément chaque exemplaire physique.

La persistence est abstraite avec des repositories et un IUnitOfWork.

La gestion des erreurs HTTP est centralisée avec IExceptionHandler.