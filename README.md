# Composer API

Shared architecture, development workflows, and agent instructions:
[UNITE project documentation](https://github.com/dkfz-unite/.github/blob/main/README.md).

## General
Composer exposes indexed-data search/access, saved dataset persistence, and
administrative submission review to the portal.

- [Data domain service](./Docs/api-domain.md) - provides api search domain data.
- [unite-analysis](https://github.com/dkfz-unite/unite-analysis) supplies the separate analysis API used by the portal.

## Access
Environment|Address|Port
-----------|-------|----
Host|http://localhost:5002|5002
Docker|http://composer.unite.net|80

## Dependencies
- [PostgreSQL](https://github.com/dkfz-unite/unite-env/tree/main/programs/postgresql) - domain data and processing tasks.
- [MongoDB](https://github.com/dkfz-unite/unite-env/tree/main/programs/mongodb) - saved datasets and staged submission access for review.
- [Elasticsearch](https://github.com/dkfz-unite/unite-env/tree/main/programs/elasticsearch) - indexed domain data and search.
- [Ensembl Data](https://github.com/dkfz-unite/unite-ensembl-data) - reference-data HTTP client.

### Library dependencies

The main library references `Unite.Data.Context`, `Unite.Indices.Search`,
`Unite.Cache`, and `Unite.Essentials.Tsv`. The download project also references
the SQL and TSV packages. Search depends on the separate `Unite.Indices.Context`
and `Unite.Indices` packages. These are libraries, not additional HTTP APIs.
Current versions and commented sibling project references are in the `.csproj`
files. HTTP clients include Ensembl, Identity, and donors-feed clients.

## Configuration
To configure the application, change environment variables in either docker or in [launchSettings.json](Unite.Composer.Web/Properties/launchSettings.json) file (if running locally):

- `ASPNETCORE_ENVIRONMENT` - ASP.NET environment (`Release`).
- `UNITE_API_KEY` - shared key for JWT signature validation; authorization uses claims and policies.
- `UNITE_ELASTIC_HOST` - Elasticsearch service host (`http://es.unite.net:9200`).
- `UNITE_ELASTIC_USER` - Elasticsearch service user.
- `UNITE_ELASTIC_PASSWORD` - Elasticsearch service password.
- `UNITE_SQL_HOST` - SQL server host (`pgsql.unite.net`).
- `UNITE_SQL_PORT` - SQL server port (`5432`).
- `UNITE_SQL_USER` - SQL server user.
- `UNITE_SQL_PASSWORD` - SQL server password.
- `UNITE_MONGO_HOST` - MongoDB host (`mongo.unite.net`).
- `UNITE_MONGO_PORT` - MongoDB port (`27017`).
- `UNITE_MONGO_USER` - MongoDB user.
- `UNITE_MONGO_PASSWORD` - MongoDB password.
- `UNITE_ENSEMBL_HOST` - Ensembl web api (`http://data.ensembl.unite.net`).
- `UNITE_GENOME_BUILD` - genome build of the installation, `GRCh37` (default) or `GRCh38`; selects the Ensembl database used for protein annotations.
- `UNITE_IDENTITY_HOST` - Identity service host, used for project users (`http://identity.unite.net`).
- `UNITE_FEED_DONORS_HOST` - Donors feed host, used for project reindexing (`http://donors.feed.unite.net`).

> [!NOTE]
> For local development purposes we recommend to use **default** values.

## Installation

### Docker Compose
The easiest way to install the application is to use docker-compose:
- [Environment configuration and installation scripts](https://github.com/dkfz-unite/unite-env)
- [Composer configuration and installation scripts](https://github.com/dkfz-unite/unite-env/tree/main/applications/unite-composer)

### Docker
The image of the service is available in our [registry](https://github.com/dkfz-unite/unite-composer/pkgs/container/unite-composer).

[Dockerfile](Dockerfile) is used to build an image of the application.
To build an image run the following command:
```
docker build -t unite.composer:latest .
```

All application components should run in the same docker network.
To create common docker network if not yet available run the following command:
```bash
docker network create unite
```

To run application in docker run the following command:
```bash
docker run \
--name unite.composer \
--restart unless-stopped \
--net unite \
--net-alias composer.unite.net \
-p 127.0.0.1:5002:80 \
-e UNITE_API_KEY=[api_key] \
-e UNITE_ELASTIC_HOST=http://es.unite.net:9200 \
-e UNITE_ELASTIC_USER=[elasticsearch_user] \
-e UNITE_ELASTIC_PASSWORD=[elasticsearch_password] \
-e UNITE_SQL_HOST=pgsql.unite.net \
-e UNITE_SQL_PORT=5432 \
-e UNITE_SQL_USER=[sql_user] \
-e UNITE_SQL_PASSWORD=[sql_password] \
-e UNITE_MONGO_HOST=mongo.unite.net \
-e UNITE_MONGO_PORT=27017 \
-e UNITE_MONGO_USER=[mongo_user] \
-e UNITE_MONGO_PASSWORD=[mongo_password] \
-e UNITE_ENSEMBL_HOST=http://data.ensembl.unite.net \
-e UNITE_IDENTITY_HOST=http://identity.unite.net \
-e UNITE_FEED_DONORS_HOST=http://donors.feed.unite.net \
-e ASPNETCORE_ENVIRONMENT=Release \
-d \
unite.composer:latest
```
