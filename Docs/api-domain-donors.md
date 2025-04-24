# Donors API
Allows to retrieve information about donors and their data.

> [!NOTE]
> API is accessible for authorized users only and requires `JWT` token as `Authorization` header (read more about [Identity Service](https://github.com/dkfz-unite/unite-identity)).


## Overview
- get:[api/donor/{id}](#get-apidonorid) - get donor data.
- post:[api/donor/{id}/images/{type?}](#post-apidonoridimagestype) - search donor images.
- post:[api/donor/{id}/specimens/{type?}](#post-apidonoridspecimenstype) - search donor specimens.
- post:[api/donor/{id}/data](#post-apidonoriddata) - download donor data.
- post:[api/donors](#post-apidonors) - search donors.
- post:[api/donors/stats](#post-apidonorsstats) - load donors statistics.
- post:[api/donors/data](#post-apidonorsdata) - download donors data.


## GET: [api/donor/{id}](http://localhost:5002/api/donor/1)
Get donor general data by id.

### Parameters
- `id` - donor id.

### Resources
- Donor general data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - donor not found


## POST: [api/donor/{id}/images/{type?}](http://localhost:5002/api/donor/1/images/mr)
Search donor images of given type filtered by search criteria.

### Parameters
- `id` - donor id.
- `type` - image [type](#image-types) or any if not specified.

#### Image Types
- `MR` - magnetic resonance image.

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "from": 0,
    "size": 20,
    "mr": {
        "wholeTumor": { "from": 200, "to": 300 }
    }
}
```

### Resources
- Array of donor images.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - donor not found


## POST: [api/donor/{id}/specimens/{type?}](http://localhost:5002/api/donor/1/specimens/material)
Search donor specimens of given type filtered by search criteria.

### Parameters
- `id` - donor id.
- `type` - specimen [type](#specimen-types) or any if not specified.

#### Specimen Types
- `Material` - donor derived material.
- `Line` - cell line.
- `Organoid` - organoid.
- `Xenograft` - xenograft.

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "from": 0,
    "size": 20,
    "material": {
        "type": "Tumor",
        "tumorType": "Primary",
        "source": "Tissue"
    }
}
```

### Resources
- Array of donor specimens.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - donor not found


## POST: [api/donor/{id}/data](http://localhost:5002/api/donor/1/data)
Download donor data.

### Parameters
- `id` - donor id.

### Body
Data download [model](./api-domain-models-download.md).

```json
{
    "donors": true, // include donor data
    "clinical": true, // include donor clinical data
    "treatments": true, // include donor treatments data
    "specimens": true, // include donor specimens data
    "sms": true, // include donor specimens SMs data
    "smsTranscriptsFull": true, // include affected transcripts data in SMs,
    "geneExp": true // include donor bulk gene expressions data
}
```

### Resources
- `Blob` - archive with requested data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - donor not found

## POST: [api/donors](http://localhost:5002/api/donors)
Search donors filtered by search criteria.

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
    "gene": {
        "symbol": ["TP53", "EGFR"]
    }
}
```

### Resources
- Array of donors.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/donors/stats](http://localhost:5002/api/donors/stats)
Gather statistics about donors filtered by search criteria.

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "donor": {
        "diagnosis": ["Glioblastoma"],
        "age": { "from": 40, "to": 60 }
    },
    "gene": {
        "symbol": ["TP53", "EGFR"]
    }
}
```

### Resources
- Donors [statistics](./api-domain-models-stats.md).

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/donors/data](http://localhost:5002/api/donors/data)
Download data of donors filtered by search criteria.

### Body
Data download [model](./api-domain-models-download.md) and search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```jsonc
{
    "criteria": // search criteria
    {
        "donor": {
            "diagnosis": ["Glioblastoma"],
            "age": { "from": 40, "to": 60 }
        },
        "gene": {
            "symbol": ["TP53", "EGFR"]
        }
    },
    "data": // data to download
    {
        "donors": true,
        "clinical": true,
        "treatments": true,
        "specimens": true,
        "sms": true,
        "smsTranscriptsFull": true,
        "geneExp": true
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
