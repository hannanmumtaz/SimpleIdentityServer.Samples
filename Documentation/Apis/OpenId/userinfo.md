# UserInformation API

## Receive user information

This endpoint is used to get the user information.

## HTTP Request

``GET or POST http://idserver.com/userinfo``

### HTTP Header

| Parameter     | Value                        |
| ------------- | ---------------------------- |
| Target        | http://idserver.com/userinfo |
| Method        | POST or GET                  |
| Authorization | Bearer <Access token>        |

# Errors

| Error messages                   | Meaning |
| -------------------------------- | ------- |
| not a valid resource owner token |         |
| the token is not valid           |         |
