# UMA resource API

This endpoint is used to manage the UMA resources.

An access token valids for the scope `uma_protection` or with a role equals to `administrator` must be passed to the `Authorization` header.

## Add resource

Add a new resource.

### HTTP request

`POST http://umaserver.com/rs/resource_set`

### Query parameters

| Parameter | Description                        |
| --------- | ---------------------------------- |
| name      | resource name                      |
| uri       | link to the resource               |
| type      | type of resource : document, bills |
| icon_uri  | resource image                     |
| scopes    | list of scopes                     |

## Get resource

Get a resource by its identifier.

### HTTP request

`GET http://umaserver.com/rs/resource_set/{id}`

## Delete resource

Delete a resource by its identifier.

### HTTP request

`DELETE http://umaserver.com/rs/resource_set/{id}`

## Search resources

Search resources.

### HTTP request

`POST http://umaserver.com/rs/resource_set/.search`

### Query parameters

| Parameter   | Description                              |
| ----------- | ---------------------------------------- |
| ids         | list of resource identifiers             |
| names       | list of resource names                   |
| types       | list of resource types                   |
| start_index | start index used by the pagination       |
| count       | number of results used by the pagination |

## Get all the resources

Get the identifier of all the UMA resources.

### HTTP request

`GET http://umaserver.com/rs/resource_set`

## Update a resource

Update a resource.

### HTTP request

`PUT http://umaserver.com/rs/resource_set`

### Query parameters

| Parameter | Description                        |
| --------- | ---------------------------------- |
| id        | resource identifier                |
| name      | resource name                      |
| uri       | link to the resource               |
| type      | type of resource : document, bills |
| icon_uri  | resource image                     |
| scopes    | list of scopes                     |

# Errors

The resource API uses the following error codes

| Error code | Meaning        |
| ---------- | -------------- |
| 400        | Bad request    |
| 401        | Not authorized |
| 404        | Not found      |

Following error messages can be returned

| Error messages                                           | Meaning |
| -------------------------------------------------------- | ------- |
| no parameter in body request                             |         |
| the identifier must be specified                         |         |
| the parameter {0} needs to be specified                  |         |
| the url {0} is not well formed                           |         |
| resource set {0} cannot be updated                       |         |
| an error occured while trying to insert the resource set |         |
| resource cannot be found                                 |         |
