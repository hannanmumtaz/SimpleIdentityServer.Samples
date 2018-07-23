# Manager Resource Owners API

This endpoint is used to manage the resource owners.
An access token valids for the scope `manager` or with an `administrator role` must be passed to the request.

## Search resource owners

### HTTP request

`POST http://manageidserver.com/resource_owners/.search`

#### Query parameters

| Parameter   | Description                                      |
| ----------- | ------------------------------------------------ |
| subjects	  | List of subjects                                 |
| start_index | Start index			                             |
| count		  | Number of records			                     |
| order       | Ascending / Descending order					 |

The `order` parameter contains the following properties

| Property    | Description                                      |
| ----------- | ------------------------------------------------ |
| target	  | 				                                 |
| order		  | 					                             |
