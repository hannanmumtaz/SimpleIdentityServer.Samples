# Introspect API

## Introspect an access or refresh token

This endpoint is used to introspect an access or refresh token.

## HTTP Request

`POST http://idserver/introspect`

## Query parameters

| Parameter       | Description             |
| --------------- | ----------------------- |
| token           | access or refresh token |
| token_type_hint |                         |

*TODO : authenticate clients*

# Errors

The introspect API uses the following error codes

| Error code | Meaning               |
| ---------- | --------------------- |
| 400        | Bad request           |
| 500        | Internal server error |

Following error messages can be returned

| Error code                     | Meaning |
| ------------------------------ | ------- |
| no parameter in body request   |         |
| the parameter token is missing |         |
| the token is not valid         |         |
| the token doesn't exist        |         |
