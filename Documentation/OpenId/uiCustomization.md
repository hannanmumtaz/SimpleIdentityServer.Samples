In SimpleIdentityServer, some default Razor views are embedded with the UI components. Besides, the UI can be customized by adding custom views to the Areas folder.

# SimpleIdentityServer default UI
Starting from version `3.0.0.3` there are four nuget packages which contain embedded views :

| Nuget package                                   | Description                                                                                             |
| ----------------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| SimpleIdentityServer.Authenticate.LoginPassword | Supports login & password authentication                                                                |
| SimpleIdentityServer.Authenticate.SMS           | Supports passwordless authentication via SMS                                                            |
| SimpleIdentityServer.Shell                      | Contains the UI shell / template and all the views needed by the OPENID server for example : `consents` |
| SimpleIdentityServer.UserManagement             | Allows a user to manage his account                                                                     |


## UI customization

In order to customize the UI, we need to execute the following actions:

* Open the SimpleIdentityServer project and create a new folder `Areas`
* In this folder create a folder for each UI component to be customized:

| Nuget package                                   | Area                 |
| ----------------------------------------------- | -------------------- |
| SimpleIdentityServer.Authenticate.LoginPassword | pwd\Views            |
| SimpleIdentityServer.Authenticate.SMS           | sms\Views            |
| SimpleIdentityServer.Shell                      | Shell\Views          |
| SimpleIdentityServer.UserManagement             | UserManagement\Views |

* Retrieve the corresponding views from SimpleIdentityServer's repository and copy the views to the corresponding folder

| Nuget package                                   | Git                                                                                                                                                                                |
| ----------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| SimpleIdentityServer.Authenticate.LoginPassword | [source](https://github.com/thabart/SimpleIdentityServer/tree/master/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Authenticate.LoginPassword/Areas/pwd/Views) |
| SimpleIdentityServer.Authenticate.SMS           | [source](https://github.com/thabart/SimpleIdentityServer/tree/master/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Authenticate.SMS/Areas/sms/Views)           |
| SimpleIdentityServer.Shell                      | [source](https://github.com/thabart/SimpleIdentityServer/tree/master/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Shell/Areas/Shell/Views)                    |
| SimpleIdentityServer.UserManagement             | [source](https://github.com/thabart/SimpleIdentityServer/tree/master/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.UserManagement/Areas/UserManagement/Views)  |

## Sample project

run the sample application please follow the steps below :

1. Fetch the [sample projects](https://github.com/thabart/SimpleIdentityServer.Samples.git).

2. Open the folder /SimpleIdentityServer.Samples/OpenId/CustomOpenidUi and execute the command **launch.cmd**.
