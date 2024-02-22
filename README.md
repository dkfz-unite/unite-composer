# Composer API

## General
Composer service - mediator between web application and other parts of the application.
- [Data domain service](./Docs/api-domain.md) - provides api search domain data.
- [Data analysis service](https://github.com/dkfz-unite/unite-analysis) - provides api to perform different analyses on the data.

## Access
Environment|Address|Port
-----------|-------|----
Host|http://localhost:5002|5002
Docker|http://composer.unite.net|80

## Dependencies
- [SQL](https://github.com/dkfz-unite/unite-environment/tree/main/programs/postgresql) - SQL server with domain data and user identity data.
- [Elasticsearch](https://github.com/dkfz-unite/unite-environment/tree/main/programs/elasticsearch) - Elasticsearch server with indices of domain data.

## Configuration
To configure the application, change environment variables in either docker or in [launchSettings.json](Unite.Composer.Web/Properties/launchSettings.json) file (if running locally):

- `ASPNETCORE_ENVIRONMENT` - ASP.NET environment (`Release`).
- `UNITE_API_KEY` - API key for decription of JWT token and user authorization.
- `UNITE_ELASTIC_HOST` - Elasticsearch service host (`es.unite.net:9200`).
- `UNITE_ELASTIC_USER` - Elasticsearch service user.
- `UNITE_ELASTIC_PASSWORD` - Elasticsearch service password.
- `UNITE_SQL_HOST` - SQL server host (`sql.unite.net`).
- `UNITE_SQL_PORT` - SQL server port (`5432`).
- `UNITE_SQL_USER` - SQL server user.
- `UNITE_SQL_PASSWORD` - SQL server password.
- `UNITE_ENSEMBL_HOST` - Ensembl web api (`http://data.ensembl.unite.net`).

> [!NOTE]
> For local development purposes we recommend to use **default** values.

## Installation

### Docker Compose
The easiest way to install the application is to use docker-compose:
- Environment configuration and installation scripts: https://github.com/dkfz-unite/unite-environment
- Composer api configuration and installation scripts: https://github.com/dkfz-unite/unite-environment/tree/main/applications/unite-composer

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
-e UNITE_SQL_HOST=sql.unite.net \
-e UNITE_SQL_PORT=5432 \
-e UNITE_SQL_USER=[sql_user] \
-e UNITE_SQL_PASSWORD=[sql_password] \
-e UNITE_ENSEMBL_HOST=http://data.ensembl.unite.net \
-e ASPNETCORE_ENVIRONMENT=Release \
-d \
unite.composer:latest
```
