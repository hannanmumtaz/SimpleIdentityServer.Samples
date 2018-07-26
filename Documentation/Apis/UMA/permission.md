# Permission API

This endpoint is used to get a permission ticket.

An access token valids for the scope `uma_protection` or with a role equals to `administrator` must be passed to the `Authorization` header.

## Get a permission ticket

Get a permission ticket for **one resource** and a list of scopes.

### HTTP request

`POST http://umaserver.com/perm`

### Query parameters

| Parameter       | Description                    |
| --------------- | ------------------------------ |
| resource_set_id | identifier of the resource set |
| scopes          | list of scopes                 |

## Get a permission ticket (bulk)

Get a permission tickets for one or more resources.

### HTTP request

`POST http://umaserver.com/perm/bulk`

### Query parameters

An array of JSON objects must be passed into the HTTP body request. 

Each object contains the following parameters :

| Parameter       | Description                    |
| --------------- | ------------------------------ |
| resource_set_id | identifier of the resource set |
| scopes          | list of scopes                 |

# Errors

The permission API uses the following error codes

| Error code | Meaning        |
| ---------- | -------------- |
| 400        | Bad request    |
| 401        | Not authorized |

Following error messages can be returned

| Error messages                          | Meaning |
| --------------------------------------- | ------- |
| no parameter in body request            |         |
| the client_id cannot be extracted       |         |
| the parameter {0} needs to be specified |         |
| resource set {0} doesn't exist          |         |
| one or more scopes are not valid        |         |
| the ticket cannot be inserted           |         |
