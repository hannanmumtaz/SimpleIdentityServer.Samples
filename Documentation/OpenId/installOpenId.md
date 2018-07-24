# Getting started with the OPENID server

The OpenId server can run in two different modes :

1. **Offline**: The units are manually installed and configured by the developer.

2. **Online**: The units are automatically retrieved from the Nuget feeds and dynamically loaded during the runtime.

The OpenId server is made of several components :

**Components**:

| Component       | Description                                                                                            |
| --------------- | ------------------------------------------------------------------------------------------------------ |
| OAuthStorage    | Store the access token, authorization code or confirmation code into an InMemory or Redis storage.     |
| OAuthRepository | Store the clients, resource owners or claims into a database (SQLSERVER, POSTGRE, INMEMORY or SQLITE)  |
| Bus             | When an event is raised then a message is pushed into the queue. Those events can be handled later ... |
| UI              | UI components : authentication, user management and the shell                                          |

## Offline usage

TODO

## Online usage

TODO
