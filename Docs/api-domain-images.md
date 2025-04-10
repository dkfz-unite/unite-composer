# Images API
Allows to retrieve information about images and their data.

> [!NOTE]
> API is accessible for authorized users only and requires `JWT` token as `Authorization` header (read more about [Identity Service](https://github.com/dkfz-unite/unite-identity)).  
> Each image always belongs to a donor and is accociated with donor derived materials.


## Image Types
Images are classified by following types:
- `MR` - magnetic resonance image.


## Overview
- get:[api/image/{id}](#get-apiimageid) - get image data.
- post:[api/image/{id}/data](#post-apiimageiddata) - download image data.
- post:[api/images/{type}](#post-apiimagestype) - search images by type.
- post:[api/images/{type}/stats](#post-apiimagestypestats) - load images statistics.
- post:[api/images/{type}/data](#post-apiimagestypedata) - download images data.


## GET: [api/image/{id}](http://localhost:5002/api/image/1)
Get image general data by id.

### Parameters
- `id` - image id.

### Resources
- Image general data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - image not found


## POST: [api/image/{id}/data](http://localhost:5002/api/image/1/data)
Download image data.

### Parameters
- `id` - image id.

### Body
Data download [model](./api-domain-models-download.md).

```jsonc
{
    "donors": true, // include image donor data
    "clinical": true, // include image donor clinical data
    "treatments": true, // include image donor treatments data
    "mrs": true, // include MR image data
    "specimens": true, // include image donor derived materials data
    "sms": true, // include image donor derived materials SMs data
    "smsTranscriptsFull": true, // include affected transcripts data in SMs
    "geneExp": true, // include image donor derived materials bulk gene expressions data
}
```

### Resources
- `Blob` - archive with requested data.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - image not found


## POST: [api/images/{type}](http://localhost:5002/api/images/mr)
Search images of given type filtered by search criteria.

### Parameters
- `type` - image [type](#image-types).

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
    "mr": {
        "wholeTumor": { "from": 200, "to": 300 }
    }
}
```

### Resources
- Array of images.

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/images/{type}/stats](http://localhost:5002/api/images/mr/stats)
Gather statistics about images filtered by search criteria.

### Parameters
- `type` - image [type](#image-types).

### Body
Search [criteria](https://github.com/dkfz-unite/unite-indices/blob/main/Docs/search-criteria.md).

#### application/json
```json
{
    "donor": {
        "project": ["TCGA"],
        "diagnosis": ["Glioblastoma"],
        "age": { "from": 40, "to": 60 }
    },
    "mr": {
        "wholeTumor": { "from": 200, "to": 300 }
    }
}
```

### Resources
- Images [statistics](./api-domain-models-stats.md).

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: [api/images/{type}/data](http://localhost:5002/api/images/mr/data)
Download data for images filtered by search criteria.

### Parameters
- `type` - image [type](#image-types).

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
        "mr": {
            "wholeTumor": { "from": 200, "to": 300 }
        }
    },
    "data": // data to download
    {
        "donors": true,
        "clinical": true,
        "treatments": true,
        "mrs": true,
        "specimens": true,
        "sms": true,
        "smsTranscriptsFull": true,
        "geneExp": true,
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
