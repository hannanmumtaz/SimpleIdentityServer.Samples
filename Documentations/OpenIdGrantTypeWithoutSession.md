# OpenId implicit workflow (without session)

## Purpose

The objective of this tutorial is to offer the possibility to your end-users to authenticate against your website with their local or external accounts. The grant-type used it the OAUTH2.0 [implicit](https://tools.ietf.org/html/rfc6749#section-4.2).

![images\openidGrantTypeImplicitWithoutSessionWorkflow](images\openidGrantTypeImplicitWithoutSessionWorkflow.png)

1. The REACT.JS application redirects the user-agent to the authorization endpoint.

2. The end-user authenticates with his local or external account. If the authentication is successful then an access token is returned into the query string.

3. The REACT.JS application fetches the access token from the redirection url and use the web storage API to store the session. 

4. The session lifetime is managed by the REACT.JS application. If the access token is expired then the session is removed from the (local / session) storage.

## Prerequisites

* The **idserver** database must be deployed

* The OPENID provider must be configured

## Implementation

### Add the OPENID server

Please refer to this tutorial to create a new OPENID server.

### Configure OPENID

Register a new client into the OPENID provider and set the following properties

| Property         | Value                           |
| ---------------- | ------------------------------- |
| ClientId         | Website                         |
| Grant-Type       | Implicit                        |
| Application type | web                             |
| Redirect URL     | http://localhost:64950/callback |
| Response types   | id_token token                  |

### Create website

#### REACT.JS

##### Authenticate the end-user

Create a new login page into your REACT.JS application and add a new button **external authentication**. When this button is clicked by the end-user then a new tab is opened and the authorization web page is displayed.

Before going further, the authorization url must be built by the REACT.JS application. Execute an HTTP GET request to get the well-known configuration. The base authorization url can be fetched from the **authorization_endpoint** key.

```textile
Request URL : http://localhost:60000/.well-known/openid-configuration
Request Method : GET
```

Append the following queries to your authorization url : 

| Query         | Value                           |
| ------------- | ------------------------------- |
| scope         | role profile                    |
| state         | <generate a random value>       |
| redirect_uri  | http://localhost:64950/callback |
| response_type | id_token token                  |
| ClientId      | Website                         |
| nonce         | <generate a random value>       |
| response_mode | query                           |

At the end the authorization url should look like to something like this : 

```textile
http://localhost:60000/authorization?scope=openid role profile&state=75BCNvRlEGHpQRCT&redirect_uri=http://localhost:64950/callback&response_type=id_token token&client_id=Website&nonce=nonce&response_mode=query
```

Once the authoriztion url is built then the tab can be opened and the REACT.JS can monitor the url to get the access token from the callback url and store this value into a local / session storage. The code below shows how to get the access token and validate the nonce and state parameters.    

```javascript
        const clientId = 'Website';
        const callbackUrl = 'http://localhost:64950/callback';
        const stateValue = '75BCNvRlEGHpQRCT';
        const nonceValue = 'nonce';
        var self = this;
        var getParameterByName = function (name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        };
        var url = "http://localhost:60000/authorization?scope=openid role profile&state="+stateValue+"&redirect_uri="
            + callbackUrl
            + "&response_type=id_token token&client_id=" + clientId + "&nonce=" + nonceValue +"&response_mode=query";var w = window.open(url, '_blank'); // Build the authorization url.
        var interval = setInterval(function () {
            if (w.closed) {
                clearInterval(interval);
                return;
            }

            var href = w.location.href;
            var accessToken = getParameterByName('access_token', href);
            var idToken = getParameterByName('id_token', href);
            var state = getParameterByName('state', href);
            if (!idToken && !accessToken) {
                return;
            }

            if (state !== stateValue) { // Check the state
                return;
            }

            var payload = JSON.parse(window.atob(idToken.split('.')[1]));
            if (payload.nonce !== nonceValue) { // Check the nonce.
                return;
            }
          // The end-user is authenticated.
          clearInterval(interval);
        });
```

##### Check the session

Once the tokens are received, use the web storage API to store them. Use the code below to check the session validity :

```javascript
import moment from 'moment';

                    ...code...

                    var self = this;
                    this._interval = setInterval(function () {
                user = UserStore.getUser(); // Get token stored in the (session / local) storage
                if (!user['access_token']) {
                    clearInterval(self._interval);
                    return;
                }

                var accessToken = user['access_token'];
                var accessTokenPayload = JSON.parse(window.atob(accessToken.split('.')[1])); // retrieve the user information.
                var expirationTime = moment.unix(accessTokenPayload['exp']); // Get the expiration time.
                var now = moment();
                if (expirationTime < now) { // Check the validity of the access token.
                    clearInterval(self._interval);
                }
            }, 3*1000);
```

## Result

To run the sample application please follow the steps below :

1. Fetch the  [sample projects](https://github.com/thabart/SimpleIdentityServer.Samples.git).

2. Open the folder /SimpleIdentityServer.Samples/Migrations/<database> corresponding to the database engine (SQLSERVER, SQLITE, POSTGRE) you're using. By default the database used is **idserver**, if you're using a different one then open the **appsetting.json** and update the connectionString.

3. Launch the command **dotnet run -f net461 / netcoreapp2.0**. At the end of the execution the database will be migrated and the tables will be populated.

4. Before starting the OPENID server ensure that the environment variable **SID_MODULE** exists and its value is set to a directory.

5. Open the folder /SimpleIdentityServer.Samples/WebsiteAuthentication and execute the command **launch.cmd**.

In a browser open the url **http://localhost:64950** and click on the button **EXTERNAL AUTHENTICATION WITHOUT SESSION**

![images\openidImplicitWithoutSessionResult](images\openidImplicitWithoutSessionResult.png)
