# Policy API

This endpoint is used to manage the UMA authorization policy.

An access token valids for the scope `uma_protection` or with a role equals to `administrator` must be passed to the `Authorization` header.

## Add policy

Add an authorization policy to an existing UMA resource.

### HTTP request

`POST http://umaserver.com/policies`

### Query parameters

| Parameter        | Description                    |
| ---------------- | ------------------------------ |
| resource_set_ids | list of resource identifiers   |
| rules            | list of rules                  |

The JSON object `rule` contains the following properties

| Parameter        | Description                                     |
| ---------------- | ----------------------------------------------- |
| clients          | list of client identifiers                      |
| scopes           | list of scopes                                  |
| claims           | list of claims                                  |
| consent_needed   | consent is needed                               |
| provider         | Well known configuration of the OPENID provider |

The JSON object `claim` contains the following properties 

| Parameter     | Description                     |
| ------------- | ------------------------------- |
| type          | claim type                      |
| value         | claim value                     |

## Get policy

Get an authorization policy by its identifier.

### HTTP request

`GET http://umaserver.com/policies/{policy_id}`

## Update policy

Update an existing authorization policy.

### HTTP request

`PUT http://umaserver.com/policies`

### Query parameters

| Parameter        | Description                    |
| ---------------- | ------------------------------ |
| id			   | policy identifier              |
| rules            | list of rules                  |

## Add resource to authorization policy

Add resource to authorization policy.

### HTTP request

`POST http://umaserver.com/policies/{policy_id}/resources`

### Query parameters

| Parameter        | Description                    |
| ---------------- | ------------------------------ |
| resources		   | list of resource identifiers   |

## Delete resource from authorization policy

Remove the resource from the authorization policy.

### HTTP request

`DELETE http://umaserver.com/policies/{policy_id}/resources/{resource_id}`

# Errors

The permission API uses the following error codes

| Error code | Meaning        |
| ---------- | -------------- |
| 400        | Bad request    |
| 401        | Not authorized |
| 404        | Not found      |

Following error messages can be returned

| Error messages                                             | Meaning |
| ---------------------------------------------------------- | ------- |
| no parameter in body request                               |         |
| the identifier must be specified                           |         |
| the resource_id must be specified                          |         |
| the parameter {0} needs to be specified                    |		   |
| resource set {0} doesn't exist                             |         |
| one or more scopes don't belong to a resource set          |         |
| the authorization policy cannot be inserted                |         |
| policy cannot be found									 |		   |
