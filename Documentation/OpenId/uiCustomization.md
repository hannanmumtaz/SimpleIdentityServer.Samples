# OPENID UI customization

The OPENID UI can easily be customized thanks to the behavior of the Razor view engine. By default it is trying to get the view from the folder `Views\*\*.cshtml`.
If the file doesn't exist then the engine retrieves the view from the DLL. In SimpleIdServer default views have been embedded in the UI components.

In the current version `3.0.0.3` there are four nuget packages who contains embedded views :

| Nuget package                                   | Description                                                                                             |
| ----------------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| SimpleIdentityServer.Authenticate.LoginPassword | Supports login & password authentication                                                                |
| SimpleIdentityServer.Authenticate.SMS           | Supports passwordless authentication via SMS                                                            |
| SimpleIdentityServer.Shell                      | Contains the UI shell / template and all the views needed by the OPENID server for example : `consents` |
| SimpleIdentityServer.UserManagement             | Allows a user to manage his account                                                                     |

## Implementation

Follow the steps below to customize the UI

* Open your OPENID project and create a new folder `Areas`
* In this folder add the different Areas used by the UI component installed in your project

| Nuget package                                   | Area                 |
| ----------------------------------------------- | -------------------- |
| SimpleIdentityServer.Authenticate.LoginPassword | pwd\Views            |
| SimpleIdentityServer.Authenticate.SMS           | sms\Views            |
| SimpleIdentityServer.Shell                      | Shell\Views          |
| SimpleIdentityServer.UserManagement             | UserManagement\Views |

* In each areas get the views from git and add them into the corresponding folder

| Nuget package                                   | Git                                                                                                                                                                                |
| ----------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| SimpleIdentityServer.Authenticate.LoginPassword | [source](https://github.com/thabart/SimpleIdentityServer/tree/master/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Authenticate.LoginPassword/Areas/pwd/Views) |
| SimpleIdentityServer.Authenticate.SMS           | [source](https://github.com/thabart/SimpleIdentityServer/tree/master/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Authenticate.SMS/Areas/sms/Views)           |
| SimpleIdentityServer.Shell                      | [source](https://github.com/thabart/SimpleIdentityServer/tree/master/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Shell/Areas/Shell/Views)                    |
| SimpleIdentityServer.UserManagement             | [source](https://github.com/thabart/SimpleIdentityServer/tree/master/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.UserManagement/Areas/UserManagement/Views)  |

## Result

run the sample application please follow the steps below :

1. Fetch the [sample projects](https://github.com/thabart/SimpleIdentityServer.Samples.git).

2. Open the folder /SimpleIdentityServer.Samples/OpenId/CustomOpenidUi and execute the command **launch.cmd**.
