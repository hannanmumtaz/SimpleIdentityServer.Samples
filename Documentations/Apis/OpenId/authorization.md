# Authorization API

## Receive authorization code

This endpoint is used to get an authorization code

## HTTP Request

`GET http://idserver/authorization`

## Query parameters

| Parameter     | Description                             |
| ------------- | --------------------------------------- |
| scope         |                                         |
| response_type | code / token / id_token                 |
| client_id     |                                         |
| redirect_uri  |                                         |
| state         |                                         |
| response_mode |                                         |
| nonce         |                                         |
| display       |                                         |
| prompt        | none / login / consent / select_account |
| max_age       |                                         |
| ui_locales    |                                         |
| id_token_hint |                                         |
| login_hint    |                                         |
| claims        |                                         |
| acr_values    |                                         |
| request       |                                         |

## Errors

| Error code                                                   | Meaning |
| ------------------------------------------------------------ | ------- |
| the parameter scope is missing                               |         |
| the parameter client_id is missing                           |         |
| the parameter redirect_uri is missing                        |         |
| the parameter response_type is missing                       |         |
| at least one response_type parameter is not supported        |         |
| Based on the RFC-3986 the redirection-uri is not well formed |         |
| the client id parameter {0} doesn't exist or is not valid    |         |
| the redirect url {0} doesn't exist or is not valid           |         |
|                                                              |         |
|                                                              |         |
|                                                              |         |
