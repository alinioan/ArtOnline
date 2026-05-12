# ArtOnline - the online art galery
---

ArtOnline is a web application that allows users to upload and share their artworks.

---
Prerequisites:
The project uses .NET 10.

To start the backend, first install docker and docker compose from https://docs.docker.com/engine/install/ and enter the command below to launch the Postgresql database while in the Deployment folder:

```sh
docker-compose -f .\docker-compose.yml -p mobylab-app-db up -d
```

To work with the database migrations in .NET install the dotnet-ef tool by using the following command:

```sh
dotnet tool install --global dotnet-ef --version 10.*
```

To create a new migration use the following command and replace migration_name with the name of your new migration, usually the first migration is called "InitialCreate":

```sh
dotnet ef migrations add <migration_name> --context WebAppDatabaseContext --project .\MobyLabWebProgramming.Database --startup-project .\MobyLabWebProgramming.Api
```

Example for the first migration:

```sh
dotnet ef migrations add InitialCreate --context WebAppDatabaseContext --project .\MobyLabWebProgramming.Database --startup-project .\MobyLabWebProgramming.Api
```

The project has a worker service that will initialize the database with a first admin user. To log in the default user is "admin@default.com" with password "default".