# Registration API

## Register a client

This endpoint is used to register a client.

## HTTP Request

`GET http://idserver/registration`

The operation is protected by OAUTH2.0. An access token valids for the scope `register_client` must be passed.

## Query parameters

| Parameter       | Description                                   |
| --------------- | --------------------------------------------- |
| redirect_uris   | list of redirect_uris						  |

## Errors

| Error code                                                               | Meaning |
| ------------------------------------------------------------------------ | ------- |
| no parameter in body request	                                           |         |
| the parameter request_uris is missing									   |		 |
| the redirect_uri {0} is not well formed								   |		 |
| the redirect_uri {0} cannot contains fragment							   |		 |
| the parameter logo_uri is not correct									   |		 |
| the parameter tos_uri is not correct									   |		 |
| the parameter jwks_uri is not correct									   |		 |