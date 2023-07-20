# Composer API

## General
Composer service - mediator between web application and other parts of the application.
- Search service - provides api to query data using different search criteria.
- Protein annotation service - provides annotation of protein coding transcripts to retrieve information about pfam protein domains of coded sequences.

Composer web api is written in ASP.NET (.NET 6)

## Access
Environment|Address|Port
-----------|-------|----
Host|http://localhost:5002|5002
Docker|http://composer.unite.net|80

## Dependencies
- [SQL](https://github.com/dkfz-unite/unite-environment/tree/main/programs/postgresql) - SQL server with domain data and user identity data.
- [Elasticsearch](https://github.com/dkfz-unite/unite-environment/tree/main/programs/elasticsearch) - Elasticsearch server with indices of domain data.

## Configuration
To configure the application, change environment variables either in docker or in [launchSettings.json](/Unite.Composer.Web/Properties/launchSettings.json) file:
Variable|Description|Default(Local)|Default(Docker)
--------|-----------|--------------|---------------
ASPNETCORE_ENVIRONMENT|ASP.NET environment|Debug|Release
UNITE_API_KEY|Unite 32 bit api key||
UNITE_ENSEMBL_HOST|Ensembl web api|http://localhost:5202|http://ensembl.unite.net
UNITE_SQL_HOST|SQL server host|localhost|sql.unite.net
UNITE_SQL_PORT|SQL server port|5432|5432
UNITE_SQL_USER|SQL server user||
UNITE_SQL_PASSWORD|SQL server password||
UNITE_ELASTIC_HOST|ES service host|http://localhost:9200|http://es.unite.net:9200
UNITE_ELASTIC_USER|ES service user||
UNITE_ELASTIC_PASSWORD|ES service password||

## Installation

### Docker Compose
The easiest way to install the application is to use docker-compose:
- Environment configuration and installation scripts: https://github.com/dkfz-unite/unite-environment
- Composer api configuration and installation scripts: https://github.com/dkfz-unite/unite-environment/tree/main/applications/unite-composer

### Docker
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
-e UNITE_API_KEY=[32bit_api_key] \
-e UNITE_ELASTIC_HOST=http://es.unite.net:9200 \
-e UNITE_ELASTIC_USER=[elasticsearch_user] \
-e UNITE_ELASTIC_PASSWORD=[elasticsearch_password] \
-e UNITE_SQL_HOST=sql.unite.net \
-e UNITE_SQL_PORT=5432 \
-e UNITE_SQL_USER=[sql_user] \
-e UNITE_SQL_PASSWORD=[sql_password] \
-e UNITE_ENSEMBL_HOST=http://ensembl.unite.net \
-e ASPNETCORE_ENVIRONMENT=Release \
-d \
unite.composer:latest
```
