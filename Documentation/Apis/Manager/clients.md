# Client API

This endpoint is used to manage the clients.

An access token valids for the scope `manager` or with a claim `role` equals to `administrator` must be passed to the request.

## Add client

Add a new client.

### HTTP request

`POST http://manageidserver.com/clients`

#### Query parameters

| Parameter      				  | Description                   															                     |
| ------------------------------- | ------------------------------------------------------------------------------------------------------------ |
| client_name    				  | client name                   																			     |
| logo_uri       				  | logo uri                      																			     |
| client_uri     				  | home page                     																			     |
| application_type				  | application type (native / web)																				 |
| subject_type					  | type of subject																								 |
| token_endpoint_auth_method      | client authentication method for the token endpoint 														 |
| sector_identifier_uri			  | sector identifier uri																						 |
| token_endpoint_auth_signing_alg | token endpoint authentication signing algorithm     														 |
| default_acr_values			  | default acr values																							 |
| require_auth_time				  | require the authentication time																				 |
| policy_uri     				  | URL that the RP provides to the end-user to read about how the profile data will be used				     |
| tos_uri        				  | URL that the RP provides to the end-user to read about the RP's terms of service        				     |
| id_token_signed_response_alg    | The public key for validating the signature is provided by retrieving the JWK Set referenced by the JWKS_URI |
| response_types				  | list of OAUTH2.0 response types																				 |
| grant_types 					  | list of OAUTH2.0 grant types																				 |
| redirect_uris					  | list of redirect_uris																						 |
| json_web_keys					  | list of json web keys																						 |
| contacts						  | list of contacts																							 |
| request_uris					  | list of request uris																						 |
| post_logout_redirect_uris		  | list of post logout redirect uris																			 |
| jwks_uri						  | JSON web key set document																					 |
| jwks							  | JSON web keys 																								 |
| userinfo_signed_response_alg    | user info signed response algorithm																			 |
| userinfo_encrypted_response_alg | user info encrypted response algorithm  																	 |
| userinfo_encrypted_response_enc | user info encrypted response enc																			 |
| request_object_signing_alg	  | request objects signing algorithm																			 |
| request_object_encryption_alg   | request object encryption algorithm																			 |
| request_object_encryption_enc   | request object encryption enc																				 |
| default_max_age				  | default max age																								 |
| initiate_login_uri			  | initial login uri																							 |
| id_token_encrypted_response_alg | REQUIRED for encrypting the ID token issued to this client													 |
| id_token_encrypted_response_enc | REQUIRED for encrypting the ID token issued to this client													 |

## Update client

Update the client information.

### HTTP request

`PUT http://manageidserver.com/clients`

#### Query parameters

| Parameter      				  | Description                   															                     |
| ------------------------------- | ------------------------------------------------------------------------------------------------------------ |
| client_id     				  | client identifier              																			     |
| allowed_scopes				  | allowed scopes																								 |
| client_name    				  | client name                   																			     |
| logo_uri       				  | logo uri                      																			     |
| client_uri     				  | home page                     																			     |
| application_type				  | application type (native / web)																				 |
| subject_type					  | type of subject																								 |
| token_endpoint_auth_method      | client authentication method for the token endpoint 														 |
| sector_identifier_uri			  | sector identifier uri																						 |
| token_endpoint_auth_signing_alg | token endpoint authentication signing algorithm     														 |
| default_acr_values			  | default acr values																							 |
| require_auth_time				  | require the authentication time																				 |
| policy_uri     				  | URL that the RP provides to the end-user to read about how the profile data will be used				     |
| tos_uri        				  | URL that the RP provides to the end-user to read about the RP's terms of service        				     |
| id_token_signed_response_alg    | The public key for validating the signature is provided by retrieving the JWK Set referenced by the JWKS_URI |
| response_types				  | list of OAUTH2.0 response types																				 |
| grant_types 					  | list of OAUTH2.0 grant types																				 |
| redirect_uris					  | list of redirect_uris																						 |
| json_web_keys					  | list of json web keys																						 |
| contacts						  | list of contacts																							 |
| request_uris					  | list of request uris																						 |
| post_logout_redirect_uris		  | list of post logout redirect uris																			 |
| jwks_uri						  | JSON web key set document																					 |
| jwks							  | JSON web keys 																								 |
| userinfo_signed_response_alg    | user info signed response algorithm																			 |
| userinfo_encrypted_response_alg | user info encrypted response algorithm  																	 |
| userinfo_encrypted_response_enc | user info encrypted response enc																			 |
| request_object_signing_alg	  | request objects signing algorithm																			 |
| request_object_encryption_alg   | request object encryption algorithm																			 |
| request_object_encryption_enc   | request object encryption enc																				 |
| default_max_age				  | default max age																								 |
| initiate_login_uri			  | initial login uri																							 |
| id_token_encrypted_response_alg | REQUIRED for encrypting the ID token issued to this client													 |
| id_token_encrypted_response_enc | REQUIRED for encrypting the ID token issued to this client													 |

## Get client

Get an existing client.

### HTTP request

`GET http://manageidserver.com/clients/{id}`

## Search

Search the clients.

### HTTP request

`POST http://manageidserver.com/clients/.search`

#### Query parameters

| Parameter     | Description                           |
| ------------- | ------------------------------------- |
| client_names  | list of client names                  |
| client_ids    | list of client identifiers            |
| client_types  | list of client types                  |
| start_index   | start index                           |
| count         | number of results                     |
| order         | JSON object used to order the results |

The JSON object `order` contains the following properties

| Parameter   | Description                       |
| ----------- | --------------------------------- |
| target      | column to order : update_datetime |
| type        | asc = 0 & desc = 1                |

## Get all the clients

Get all the clients.

### HTTP request

`GET http://manageidserver.com/clients`

## Remove the client

Remove an existing client

### HTTP request

`DELETE http://manageidserver.com/clients/{id}`

# Errors

| Error code | Meaning        |
| ---------- | -------------- |
| 401        | Not authorized |
| 400        | Bad request    |

List of errors returned by the operation

| Error messages                                | Description |
| --------------------------------------------- | ----------- |
| no parameter in body request                  |             |
| the parameter request_uris is missing         |             |
| the parameter client_id is missing            |             |
| the redirect_uri {0} is not well formed       |             |
| the redirect_uri {0} cannot contains fragment |             |
| the parameter {0} is not correct              |             |
| the scopes '{0}' don't exist                  |             |
| the client doesn't exist                      |             |