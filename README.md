### BoostCsharp

![Capture d'écran 2025-06-14 120306](https://github.com/user-attachments/assets/38e62efd-bb86-4f6b-865b-76d1e860a14f)

Connexion à la base de données PostgreSQL
=========================================

Cette application utilise PostgreSQL comme système de gestion de base de données.

Chaîne de connexion
-------------------

La connexion est définie dans le fichier DataBase.cs :

connectionString = "Host=localhost;Port=5432;Database=csharp;Username=postgres;Password=123";

- Host      : localhost (base de données en local)
- Port      : 5432 (port par défaut de PostgreSQL)
- Database  : csharp (nom de la base de données utilisée)
- Username  : postgres
- Password  : 123

Remarque
--------

Assurez-vous que la base de données "csharp" existe dans PostgreSQL et que l’utilisateur "postgres" a les droits nécessaires pour s’y connecter.
