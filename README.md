# Composer API

## General
Composer service - mediator between web application and other parts of the application.
- Access validation - restricts access to web portal only for users from the access list (see below).
- Authentication service - allows users to register and log in to their accounts to get access to data.
- Authorization service - allows to grant access to data for authorized users.
- Search service - provides api to query data using different search criteria.
- Protein annotation service - provides annotation of protein coding transcripts to retrieve information about pfam protein domains of coded sequences.

Composer web api is written in ASP.NET (.NET 5)

## Access
Environment|Address|Port
-----------|-------|----
Host|http://localhost:5002|5002
Docker|http://composer.unite.net|80

## Dependencies
- [SQL](https://github.com/dkfz-unite/unite-environment/tree/main/programs/postgresql) - SQL server with domain data and user identity data.
- [Elasticsearch](https://github.com/dkfz-unite/unite-environment/tree/main/programs/elasticsearch) - Elasticsearch server with indices of domain data.

## Configuration
To configure the application, change environment variables either in docker or in [launchSettings.json](https://github.com/dkfz-unite/unite-composer/blob/main/Unite.Composer.Web/Properties/launchSettings.json) file:
Variable|Description|Default(Local)|Default(Docker)
--------|-----------|--------------|---------------
ASPNETCORE_ENVIRONMENT|ASP.NET environment|Debug|Release
UNITE_ENSEMBL_HOST|Ensembl web api|https://grch37.rest.ensembl.org|https://grch37.rest.ensembl.org
UNITE_UNIPROT_HOST|Uniprot web api|https://www.ebi.ac.uk|https://www.ebi.ac.uk
UNITE_PFAM_HOST|Pfam web api|https://pfam.xfam.org|https://pfam.xfam.org
UNITE_SQL_HOST|SQL server host|localhost|sql.unite.net
UNITE_SQL_PORT|SQL server port|5432|5432
UNITE_SQL_USER|SQL server user||
UNITE_SQL_PASSWORD|SQL server password||
UNITE_ELASTIC_HOST|ES service host|http://localhost:9200|es.unite.net:9200
UNITE_ELASTIC_USER|ES service user||
UNITE_ELASTIC_PASSWORD|ES service password||

## Installation

### Docker Compose
The easies way to install the application is to use docker-compose:
- Environment configuration and installation scripts: https://github.com/dkfz-unite/unite-environment
- Composer api configuration and installation scripts: https://github.com/dkfz-unite/unite-environment/tree/main/applications/unite-composer

### Docker
[Dockerfile](https://github.com/dkfz-unite/unite-composer/blob/main/Dockerfile) is used to build an image of the application.
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
-v [container_data_folder_absolute_path]:/app/data:rw \
-e ASPNETCORE_ENVIRONMENT=Release \
-e UNITE_ELASTIC_HOST=http://es.unite.net:9200 \
-e UNITE_ELASTIC_USER=[elasticsearch_user] \
-e UNITE_ELASTIC_PASSWORD=[elasticsearch_password] \
-e UNITE_SQL_HOST=sql.unite.net \
-e UNITE_SQL_PORT=5432 \
-e UNITE_SQL_USER=[sql_user] \
-e UNITE_SQL_PASSWORD=[sql_password] \
-e UNITE_ENSEMBL_HOST=https://grch37.rest.ensembl.org \
-e UNITE_UNIPROT_HOST=https://www.ebi.ac.uk \
-e UNITE_PFAM_HOST=https://pfam.xfam.org \
-d \
unite.composer:latest
```

## Access List
Access to web portal is limited to users listed in the access list.
To grant access for specific users their emails should be listed in **access-list.txt** file.
The file has to be located in the folder bound to the container as a volume.
The file has the following structure:
```
email-1@mail.com
email-2@mail.com
...
email-n@mail.com
```
