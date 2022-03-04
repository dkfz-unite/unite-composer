# Identity API

## GET: [api/identity/accessibility](http://localhost:5002/api/identity/accessibility)

Checks if email address is in access list.

**Parameters**

- `email` - email address (e.g. test1@email.com)

**Response**

- `200` - email address is in access list
- `404` - email address is not in access list


## POST: [api/identity/signup](http://localhost:5002/api/identity/signup)

Registers new user.

**Boby** (_application/json_)

```json
{
    "email": "test1@email.com",
    "password": "L0ngPa55word_",
    "passwordRepeat": "L0ngPa55word_"
}
```

**Password requirements**

- Minimum length is 8 characters
- Should have at least one letter
- Should have at least one number
- Passwords should match


## POST: [api/identity/signin](http://localhost:5002/api/identity/signin)

Logs in registered user.

**Body** (_application/json_)

```json
{
    "email": "test1@email.com",
    "password": "L0ngPa55word_"
}
```

**Response cookies**

Successful request will set two cookies:
- `unite_session` - session token
- `unite_token` - authentication token (http only)


## POST: [api/identity/signout](http://localhost:5002/api/identity/signout)

Logs out logged in user.

**Request cookies**

Request cookies have to be set to values, received at signin
- `unite_session` - session token
- `unite_token` - authentication token (http only)
