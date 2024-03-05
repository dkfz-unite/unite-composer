# Variants API
Allows to retrieve information about variants and their data.

> [!NOTE]
> API is accessible for authorized users only and requires `JWT` token as `Authorization` header (read more about [Identity Service](https://github.com/dkfz-unite/unite-identity)).


## Variant Types
Variants are classified by following types:
- `SSM` - simple somatic mutation.
- `CNV` - copy number variant.
- `SV` - structural variant.


## Overview
- get:[api/variant/{id}](#get-apivariantid) - get variant data.
- post:[api/variant/{id}/donors](#post-apivariantiddonors) - search variant donors.
- get:[api/variant/{id}/translations](#get-apivariantidtranslations) - load variant translations.
- post:[api/variant/{id}/data](#post-apivariantiddata) - download variant data.
- post:[api/variants/{type}](#post-apivariantstype) - search variants.
- post:[api/variants/{type}/stats](#post-apivariantstypestats) - load variants statistics.
- post:[api/variants/{type}/data](#post-apivariantstypedata) - download variants data.


## GET: [api/variant/{id}](http://localhost:5002/api/variant/1)
Get variant general data by id.

### Parameters
- `id` - variant id.

### Resources
- Variant general data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - variant not found


## POST: [api/variant/{id}/donors](http://localhost:5002/api/variant/1/donors)
Search variant donors filtered by search criteria.

### Parameters
- `id` - variant id.

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "from": 0,
    "size": 20,
    "donor": {
        "diagnosis": ["Glioblastoma"],
        "age": { "from": 40, "to": 60 }
    }
}
```

### Resources
- Array of variant donors.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - variant not found


## GET: [api/variant/{id}/translations](http://localhost:5002/api/variant/1/translations)
Load translations of a gene affected by the variant (available for SSMs only).

### Parameters
- `id` - variant id.

### Resources
- Array of translations.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - variant not found


## POST: [api/variant/{id}/data](http://localhost:5002/api/variant/1/data)
Download variant data.

### Parameters
- `id` - variant id.

### Body
Data download [model](./api-domain-models-download.md).

#### application/json
```json
{
    "donors": true, // include variant affected donors data
    "clinical": true, // include variant affected donors clinical data
    "treatments": true, // include variant affected donors treatments data
    "specimens": true, // include variant affected specimens data
    "ssms": true, // include SSMs data
    "ssmsTranscriptsFull": true // include affected transcripts data in SSMs
}
```

### Resources
- `Blob` - archive with requested data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - variant not found


## POST: [api/variants/{type}](http://localhost:5002/api/variants/ssm)
Search variants of given type filtered by search criteria.

### Parameters
- `type` - variant [type](#variant-types).

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "from": 0,
    "size": 20,
    "donor": {
        "diagnosis": ["Glioblastoma"],
        "age": { "from": 40, "to": 60 }
    },
    "ssm": {
        "chromosome": [10, 11],
        "impact": ["High", "Moderate"]
    }
}
```

### Resources
- Array of variants.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/variants/{type}/stats](http://localhost:5002/api/variants/ssm/stats)
Gather statistics about variants of given type filtered by search criteria.

### Parameters
- `type` - variant [type](#variant-types).

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "donor": {
        "diagnosis": ["Glioblastoma"],
        "age": { "from": 40, "to": 60 }
    },
    "ssm": {
        "chromosome": [10, 11],
        "impact": ["High", "Moderate"]
    }
}
```

### Resources
- Variants [statistics](./api-domain-models-stats.md).

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/variants/{type}/data](http://localhost:5002/api/variants/ssm/data)
Download data of variants of given type filtered by search criteria.

### Parameters
- `type` - variant [type](#variant-types).

### Body
Data download [model](./api-domain-models-download.md) and search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "criteria": // search criteria
    {
        "donor": {
            "diagnosis": ["Glioblastoma"],
            "age": { "from": 40, "to": 60 }
        },
        "ssm": {
            "chromosome": [10, 11],
            "impact": ["High", "Moderate"]
        }
    },
    "data": {
        "donors": true,
        "clinical": true,
        "treatments": true,
        "specimens": true,
        "ssms": true,
        "ssmsTranscriptsFull": true
    }
}
```

### Resources
- `Blob` - archive with requested data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
