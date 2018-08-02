# Token API

## Receive token

This endpoint is used to get an access / identity token

## HTTP request

``POST http://idserver.com/token``

### Password grant-type

#### Query parameters

| Parameter  | Description                                      |
| ---------- | ------------------------------------------------ |
| grant_type | password                                         |
| username   | login or phone number                            |
| password   | password or confirmation code                    |
| scope      | List of scopes for example : openid role profile |

*TODO : authenticate clients*

### Client credentials grant-type

#### Query parameters

| Parameter  | Description                            |
| ---------- | -------------------------------------- |
| grant_type | client_credentials                     |
| scope      | List of scopes for example : api1 api2 |

*TODO : Authenticate clients*

### Refresh token grant-type

#### Query parameters

| Parameter     | Description   |
| ------------- | ------------- |
| grant_type    | refresh_token |
| refresh_token |               |
|               |               |

*TODO : Authenticate clients*

### Authorization code grant-type

#### Query parameters

| Parameter    | Description        |
| ------------ | ------------------ |
| grant_type   | authorization_code |
| redirect_uri |                    |
| code         |                    |

*TODO : Authenticate clients*

### UMA ticket grant-type

| Parameter  | Description                                    |
| ---------- | ---------------------------------------------- |
| grant_type | uma_ticket                                     |
| ticket     | ticket retrieved from the permission API       |

*TODO : Authenticate clients*

# Revoke token

This endpoint is used to revoke a token

## HTTP Request

`POST http://idserver.com/token/revoke`

## Query parameters

| Parameter       | Description             |
| --------------- | ----------------------- |
| token           | access or refresh token |
| token_type_hint | refresh or access       |


The token API uses the following error codes 

| Error code | Meaning               |
| ---------- | --------------------- |
| 400        | Bad request           |
| 500        | Internal server error |

Following error messages can be returned

| Error messages                                               | Meaning |
| ------------------------------------------------------------ | ------- |
| the parameter grant_type is missing                          |         |
| the parameter username is missing                            |         |
| the parameter password is missing                            |         |
| the parameter scope is missing                               |         |
| the parameter refresh_token is missing                       |         |
| the parameter code is missing                                |         |
| the parameter token is missing                               |         |
| the client doesn't exist                                     |         |
| the client cannot be authenticated with secret basic         |         |
| the client cannot be authenticated with secret post          |         |
| the client cannot be authenticated with TLS                  |         |
| the client {0} doesn't have a shared secret                  |         |
| the client assertion is not a JWE token                      |         |
| the jwe token cannot be decrypted                            |         |
| the client assertion is not a JWS token                      |         |
| the JWS payload cannot be extracted                          |         |
| the client id passed in JWT is not correct                   |         |
| the audience passed in JWT is not correct                    |         |
| the received JWT has expired                                 |         |
| resource owner credentials are not valid                     |         |
| the scopes {0} are not allowed or invalid                    |         |
| the client {0} doesn't support the grant type {1}            |         |
| the client '{0}' doesn't support the response type: '{1}'    |         |
| the refresh token is not valid                               |         |
| the refresh token can be used only by the same issuer        |         |
| Based on the RFC-3986 the redirection-uri is not well formed |         |
| the authorization code is not correct                        |         |
| no parameter in body request                                 |         |
| the token has not been issued for the given client id '{0}'  |         |
| the token doesn't exist                                      |         |
