# OpenId implicit workflow (with session)

## Purpose

The objective of this tutorial is to offer the possibility to your end-users to authenticate against your website with their local or external accounts. The difference with the previous tutorial is the lifecycle of the session is not managed by the website but by the OPENID provider via the [session management mechanism](http://openid.net/specs/openid-connect-session-1_0.html). 

![images\openidGrantTypeImplicitWithSessionWorkflow](images\openidGrantTypeImplicitWithSessionWorkflow.png)



1. The REACT.JS application redirects the user-agent to the authorization endpoint

2. The end-user authenticates with his local or external account. If the authentication is successful then an access token is returned into the query string with a session_state.

3. The REACT.JS application fetches the access token and the session_state from the redirection url. It periodically checks that the session is still active.

## Prerequisistes

* The **idserver** must be deployed

* The OPENID provider must be configured

## Implementation

### Add the OPENID provider

Please refer to this tutorial to create a new OPENID server

### Configure OPENID

Register a new client into the OPENID provider and set the following properties

| Property                 | Value                              |
| ------------------------ | ---------------------------------- |
| ClientId                 | Website                            |
| Grant-Type               | Implicit                           |
| Application Type         | web                                |
| Redirection URL          | http://localhost:64950/callback    |
| Response types           | id_token token                     |
| Post Logout Redirect Url | http://localhost:64950/end_session |

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

Once the authorization url is built then the tab can be opened and the REACT.JS can monitor the URL to get the access token and the session state from the callback url  and store those values into a local / session storage. The code below shows how to get the access token, session_state and validate the nonce and state parameters.

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
            var sessionState = getParameterByName('session_state', href);
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

Once the tokens are received, add an hidden iframe into your application with the target url : 

```textile
http://localhost:60000/check_session
```

When the iframe is loaded then periodically check the session :

```javascript
        var self = this;
        self.handleMessage = function(e) { // This operation is called when a response has been received from the iframe.
          var self = this;
          if (e.data === 'error' || e.data === 'changed') {
            	sessionStorage.removeItem('key'); // Remove the session.
          }
      	};

				window.addEventListener("message", self.handleMessage, false);
        var originUrl = window.location.protocol + "//" + window.location.host;
        self._interval = setInterval(function() { 
            var session = JSON.parse(sessionStorage.getItem('key')); // Get the session from the storage
            var message = "Website ";
            if (session) {
                message += session['session_state'];
            } else {
                session += "tmp";
            }
            
            var win = self._sessionFrame.contentWindow; // Post a message to the hidden frame.
            win.postMessage(message, "http://localhost:60000");
        }, 3*1000);
```




