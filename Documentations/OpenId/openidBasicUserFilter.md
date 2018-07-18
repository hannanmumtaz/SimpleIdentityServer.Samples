# Basic user filter

## Nuget package

``SimpleIdentityServer.UserFilter.Basic``

## Usage

```csharp
services.AddUserFilter(new UserFilterBasicOptions
{
	Rules = new List<FilterRule>
  {
  	new FilterRule
    {
    	Name = "invalid_rule",
      Comparisons = new List<FilterComparison>
      {
      	new FilterComparison
        {
        	ClaimKey = "key",
          ClaimValue = "key",
          Operation = ComparisonOperations.Equal
        }
      }
    }
  }
});
```

### FilterComparison Parameters

| Parameter  | Description                            |
| ---------- | -------------------------------------- |
| ClaimKey   | Claim key                              |
| ClaimValue | Regular expression or value            |
| Operation  | Equal or NotEqual or RegularExpression |


