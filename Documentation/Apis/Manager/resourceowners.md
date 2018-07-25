# Manager Resource Owners API

This endpoint is used to manage the resource owners.

An access token valids for the scope `manager` or with a claim `role` equals to `administrator` must be passed to the request.

## Search resource owners

### HTTP request

`POST http://manageidserver.com/resource_owners/.search`

#### Query parameters

| Parameter   | Description                  |
| ----------- | ---------------------------- |
| subjects    | List of subjects             |
| start_index | Start index                  |
| count       | Number of records            |
| order       | Ascending / Descending order |

The `order` parameter contains the following properties

| Property | Description               |
| -------- | ------------------------- |
| target   | update_datetime    etc... |
| order    | 1 = desc, 0 = asc         |

#### Errors

| Error code | Meaning        |
| ---------- | -------------- |
| 401        | Not authorized |

## Update password

### HTTP request

`PUT http://manageidserver.com/resource_owners/password`

### Query parameters

| Parameter | Description                      |
| --------- | -------------------------------- |
| login     | resource owner's login / subject |
| claims    | list of claims                   |

### Errors

| Error code | Meaning        |
| ---------- | -------------- |
| 401        | Not authorized |

List of errors returned by the operation

| Error messages                       | Description |
| ------------------------------------ | ----------- |
| no parameter in body request         |             |
| the parameter login is missing       |             |
| the parameter password is missing    |             |
| the resource owner {0} doesn't exist |             |

## Update claims

### HTTP request

`PUT http://manageidserver.com/resource_owners/claims`

### Query parameters

| Parameter | Description                      |
| --------- | -------------------------------- |
| login     | resource owner's login / subject |
| password  |                                  |

### Errors

| Error code | Meaning        |
| ---------- | -------------- |
| 401        | Not authorized |

List of errors returned by the operation

| Error messages                       | Description |
| ------------------------------------ | ----------- |
| no parameter in body request         |             |
| the parameter login is missing       |             |
| the resource owner {0} doesn't exist |             |

## Get a resource owner

### HTTP request

`GET http://manageidserver.com/resource_owners/{id}`

### Errors

| Error code | Meaning        |
| ---------- | -------------- |
| 401        | Not authorized |

List of errors returned by the operation

| Error messages                       | Description |
| ------------------------------------ | ----------- |
| the id parameter must be specified   |             |
| the resource owner {0} doesn't exist |             |

## Get all the resource owners

### HTTP request

`GET http://manageidserver.com/resource_owners`

### Errors

| Error code | Meaning        |
| ---------- | -------------- |
| 401        | Not authorized |

## Add a resource owner

### HTTP request

`POST http://manageidserver.com/resource_owners`

### Query parameters

| Parameter | Description                      |
| --------- | -------------------------------- |
| login     | resource owner's login / subject |
| password  |                                  |

### Errors

| Error code | Meaning        |
| ---------- | -------------- |
| 401        | Not authorized |

List of errors returned by the operation

| Error messages                                            | Description |
| --------------------------------------------------------- | ----------- |
| no parameter in body request                              |             |
| the parameter login is missing                            |             |
| the parameter password is missing                         |             |
| a resource owner with the same credentials already exists |             |

## Delete a resource owner

### HTTP request

`DELETE http://manageidserver.com/resource_owners/{id}`

### Errors

| Error code | Meaning        |
| ---------- | -------------- |
| 401        | Not authorized |

List of errors returned by the operation

| Error messages                       | Description |
| ------------------------------------ | ----------- |
| no parameter in body request         |             |
| the resource owner {0} doesn't exist |             |
