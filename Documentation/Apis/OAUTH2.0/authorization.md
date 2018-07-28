# Authorization API

## Receive authorization code

This endpoint is used to get an authorization code.

## HTTP Request

`GET http://idserver/authorization`

## Query parameters

| Parameter     | Description                                   |
| ------------- | --------------------------------------------- |
| scope         | a list of scopes for example : openid profile |
| response_type | code / token / id_token                       |
| client_id     | the client identifier                         |
| redirect_uri  | the redirection uri                           |
| state         |                                               |
| response_mode |                                               |
| nonce         |                                               |
| display       |                                               |
| prompt        | none / login / consent / select_account       |
| max_age       |                                               |
| ui_locales    |                                               |
| id_token_hint |                                               |
| login_hint    |                                               |
| claims        |                                               |
| acr_values    |                                               |
| request       |                                               |

## Errors

| Error code                                                               | Meaning |
| ------------------------------------------------------------------------ | ------- |
| the parameter scope is missing                                           |         |
| the parameter client_id is missing                                       |         |
| the parameter redirect_uri is missing                                    |         |
| the parameter response_type is missing                                   |         |
| the parameter nonce is missing                                           |         |
| at least one response_type parameter is not supported                    |         |
| Based on the RFC-3986 the redirection-uri is not well formed             |         |
| the client id parameter {0} doesn't exist or is not valid                |         |
| the redirect url {0} doesn't exist or is not valid                       |         |
| the client '{0}' doesn't support the response type: '{1}'                |         |
| the scopes {0} are not allowed or invali                                 |         |
| the client {0} requires PKCE                                             |         |
| the user needs to be authenticated                                       |         |
| the user needs to give his consent                                       |         |
| the id_token_hint parameter is not a valid token                         |         |
| the id_token_hint parameter cannot be decrypted                          |         |
| the identity token doesnt contain simple identity server in the audience |         |
| the current authenticated user doesn't match with the identity token     |         |
