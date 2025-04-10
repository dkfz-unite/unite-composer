# Specimens API
Allows to retrieve information about specimens and their data.

> [!NOTE]
> API is accessible for authorized users only and requires `JWT` token as `Authorization` header (read more about [Identity Service](https://github.com/dkfz-unite/unite-identity)).  
> Each specimen always belongs to a donor and can contain sequencing data associated with it.


## Specimen Types
Specimens are classified by following types:
- `Material` - donor derived materials.
- `Line` - cell lines.
- `Organoid` - organoids.
- `Xenograft` - xenografts.


## Overview
- get:[api/specimen/{id}](#get-apispecimenid) - get specimen data.
- post:[api/specimen/{id}/drugs](#post-apispecimeniddrugs) - search specimen drugs screening data.
- post:[api/specimen/{id}/genes](#post-apispecimenidgenes) - search specimen genes.
- post:[api/specimen/{id}/variants/{type?}](#post-apispecimenidvariantstype) - search specimen variants.
- post:[api/specimen/{id}/profile](#post-apispecimenidprofile) - load specimen genomic profile.
- post:[api/specimen/{id}/data](#post-apispecimeniddata) - download specimen data.
- post:[api/specimens/{type}](#post-apispecimenstype) - search specimens.
- post:[api/specimens/{type}/stats](#post-apispecimenstypestats) - load specimens statistics.
- post:[api/specimens/{type}/data](#post-apispecimenstypedata) - download specimens data.


## GET: [api/specimen/{id}](http://localhost:5002/api/specimen/1)
Get specimen general data by id.

### Parameters
- `id` - specimen id.

### Resources
- Specimen general data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - specimen not found


## POST: [api/specimen/{id}/drugs](http://localhost:5002/api/specimen/1/drugs)
Search specimen drugs screening data.

### Parameters
- `id` - specimen id.

### Resources
- Array of drugs screening data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - specimen not found


## POST: [api/specimen/{id}/genes](http://localhost:5002/api/specimen/1/genes)
Search for genes either affected by any variant or expressed in the specimen.

### Parameters
- `id` - specimen id.

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "from": 0,
    "size": 20,
    "gene": {
        "symbol": ["EGFR", "TP53"]
    }
}
```

### Resources
- Array of specimen genes.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - specimen not found


## POST: [api/specimen/{id}/variants/{type?}](http://localhost:5002/api/specimen/1/variants/sm)
Search variants of given type appearing in the specimen filtered by search criteria.

### Parameters
- `id` - specimen id.
- `type` - variant [type](#variant-types) or any if not specified.

#### Variant Types
- `SM` - simple mutation.
- `CNV` - copy number variant.
- `SV` - structural variant.

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "from": 0,
    "size": 20,
    "sm": {
        "chromosome": [10, 11],
        "impact": ["High", "Moderate"]
    }
}
```

### Resources
- Array of specimen variants.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - specimen not found


## POST: [api/specimen/{id}/profile](http://localhost:5002/api/specimen/1/profile)
Load specimen genomic profile data.

### Parameters
- `id` - specimen id.

### Body
Genomic range filter [criteria]().

#### application/json
```json
{
    "startChr": 1,
    "start":  1,
    "endChr": 1,
    "end": 100000
}
```

### Resources
- Genomic profile data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - specimen not found


## POST: [api/specimen/{id}/data](http://localhost:5002/api/specimen/1/data)
Download specimen data.

### Parameters
- `id` - specimen id.

### Body
Data download [model](./api-domain-models-download.md).

#### application/json
```json
{
    "donors": true, // include specimen donor data
    "clinical": true, // include specimen donor clinical data
    "treatments": true, // include specimen donor treatments data
    "specimens": true, // include specimen data
    "molecular": true, // include specimen molecular data
    "drugs": true, // include specimen drugs screening data
    "interventions": true, // include specimen interventions data
    "sms": true, // include specimen SMs data
    "smsTranscriptsFull": true // include affected transcripts data in SMs
}
```

### Resources
- `Blob` - archive with requested data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - specimen not found


## POST: [api/specimens/{type}](http://localhost:5002/api/specimens/material)
Search specimens of given type filtered by search criteria.

### Parameters
- `type` - specimen [type](#specimen-types).

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
    "material": {
        "type": "Tumor",
        "tumorType": "Primary",
        "source": "Tissue"
    }
}
```

### Resources
- Array of specimens.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/specimens/{type}/stats](http://localhost:5002/api/specimens/material/stats)
Gather statistics about specimens filtered by search criteria.

### Parameters
- `type` - specimen [type](#specimen-types).

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "donor": {
        "diagnosis": ["Glioblastoma"],
        "age": { "from": 40, "to": 60 }
    },
    "material": {
        "type": "Tumor",
        "tumorType": "Primary",
        "source": "Tissue"
    }
}
```

### Resources
- Specimens [statistics](./api-domain-models-stats.md).

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/specimens/{type}/data](http://localhost:5002/api/specimens/material/data)
Download specimens data filtered by search criteria.

### Parameters
- `type` - specimen [type](#specimen-types).

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
        "material": {
            "type": "Tumor",
            "tumorType": "Primary",
            "source": "Tissue"
        }
    },
    "data": // data to download
    {
        "donors": true,
        "clinical": true,
        "treatments": true,
        "specimens": true,
        "molecular": true,
        "drugs": true,
        "interventions": true,
        "sms": true,
        "smsTranscriptsFull": true
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
