# Genes API
Allows to retrieve information about genes and their data.

> [!NOTE]
> API is accessible for authorized users only and requires `JWT` token as `Authorization` header (read more about [Identity Service](https://github.com/dkfz-unite/unite-identity)).  
> Genes appear in the system either as expression leves wit BulkRnaSeq data or as affected by any variant durgin the variant calling process.


## Overview
- get:[api/gene/{id}](#get-apigeneid) - get gene data.
- post:[api/gene/{id}/donors](#post-apigeneiddonors) - search gene donors.
- post:[api/gene/{id}/variants/{type?}](#post-apigeneidvariantstype) - search gene variants.
- get:[api/gene/{id}/translations](#get-apigeneidtranslations) - load gene translations.
- post:[api/gene/{id}/data](#post-apigeneiddata) - download gene data.
- post:[api/genes](#post-apigenes) - search genes.
- post:[api/genes/stats](#post-apigenesstats) - load genes statistics.
- post:[api/genes/data](#post-apigenesdata) - download genes data.


## GET: [api/gene/{id}](http://localhost:5002/api/gene/1)
Get gene general data by id.

### Parameters
- `id` - gene id.

### Resources
- Gene general data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - gene not found


## POST: [api/gene/{id}/donors](http://localhost:5002/api/gene/1/donors)
Search for donors having gene either expressed or affected by variants in any specimen filtered by search criteria.

### Parameters
- `id` - gene id.

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
- Array of gene donors.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - gene not found


## POST: [api/gene/{id}/variants/{type?}](http://localhost:5002/api/gene/1/variants/sm)
Search for variants of given type affecting the gene.

### Parameters
- `id` - gene id.
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
- Array of gene variants.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - gene not found


## GET: [api/gene/{id}/translations](http://localhost:5002/api/gene/1/translations)
Load gene translations.

### Parameters
- `id` - gene id.

### Resources
- Array of gene translations.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - gene not found


## POST: [api/gene/{id}/data](http://localhost:5002/api/gene/1/data)
Download gene data.

### Parameters
- `id` - gene id.

### Body
Data download [model](./api-domain-models-download.md).

```json
{
    "donors": true, // include gene expressed or affected donors data
    "clinical": true, // include gene expressed or affected donors clinical data
    "sms": true, // include gene affecting SMs data
    "smsTranscriptsFull": true, // include affected transcripts data in SMs
    "cnvs": true, // include gene affecting CNVs data
    "cnvsTranscriptsFull": true, // include affected transcripts data in CNVs
    "svs": true, // include gene affecting SVs data
    "svsTranscriptsFull": true, // include affected transcripts data in SVs
    "geneExp": true // include gene expression data
}
```

### Resources
- `Blob` - archive with requested data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - gene not found


## POST: [api/genes](http://localhost:5002/api/genes)
Search genes filtered by search criteria.

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
    },
    "sm": {
        "chromosome": [10, 11],
        "impact": ["High", "Moderate"]
    }
}
```

### Resources
- Array of genes.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/genes/stats](http://localhost:5002/api/genes/stats)
Gather statistics about genes filtered by search criteria.

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
    },
    "sm": {
        "chromosome": [10, 11],
        "impact": ["High", "Moderate"]
    }
}
```

### Resources
- Genes [statistics](./api-domain-models-stats.md).

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/genes/data](http://localhost:5002/api/genes/data)
Download data of genes filtered by search criteria.

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
        "gene": {
            "symbol": ["TP53", "EGFR"]
        },
        "sm": {
            "chromosome": [10, 11],
            "impact": ["High", "Moderate"]
        }
    },
    "data": // data to download
    {
        "donors": true,
        "clinical": true,
        "sms": true,
        "smsTranscriptsFull": true,
        "cnvs": true,
        "cnvsTranscriptsFull": true,
        "svs": true,
        "svsTranscriptsFull": true,
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
