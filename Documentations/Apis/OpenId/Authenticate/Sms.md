# SMS API

## Send phone number

This endpoint is used to send a confirmation code to a phone number

## HTTP request

```POST http://idserver.com/code```

## Query Parameters

| Parameter    | Description  |
| ------------ | ------------ |
| phone_number | Phone number |

# Errors

The SMS API uses the following error codes :

| Error code | Meaning               |
| ---------- | --------------------- |
| 400        | Bad request           |
| 500        | Internal server error |

Following error messages can be returned :

- parameter phone_number is missing

- the twilio account is not properly configured

- the confirmation code cannot be saved

- unhandled exception occured please contact the administrator
