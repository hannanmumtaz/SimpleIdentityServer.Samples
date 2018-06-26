# Getting started with the OPENID server

The SID OPENID server can run in two different modes:

1. **Offline**: The developer manually install the nuget packages and configure them.

2. **Online**: The SimpleIdentityServer units are automatically retrieved the first time the application is launched.



**Components**:

| Component       | Description                                                                                         |
| --------------- | --------------------------------------------------------------------------------------------------- |
| OAuthStorage    | Store the access token, authorization code or confirmation code into the inmemory or redis storage  |
| OAuthRepository | Store the clients, resource owners etc ... into a database (SQLSERVER, POSTGRE, INMEMORY or SQLITE) |
| EventsHandler   | Handle the events raised by the OPENID server                                                       |
| UI              | Different UI components of the OPENID server : authentication, user management and the shell        |

## Offline usage

TODO

## Online usage

TODO
