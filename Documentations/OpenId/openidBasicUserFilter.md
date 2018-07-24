# Basic user filter

When an external authentication (facebook, google etc ...) is enabled on a website, any users who have an external account  can be authenticated. In SimpleIdentityServer the external accounts who cannot have access to the website can be rejected via `Filter`.

The first filter `BasicFilter` rejects all the external accounts who don't satisfy **ALL** the rules. In fact the claims are checked against **ALL** the rules and if no one is satisfied then the external account is rejected.

A rule contains a set of filter. All the filters are evaluated and if at least one of them is not respected then the rule is rejected. There are three type of filters :

* *Equal*: check if a claim is equal to a specific value.

* *NotEqual* : check if a claim is different to a specific value.

* *RegularExpression* : check if a claim matches a regular expression.

## Nuget package

``SimpleIdentityServer.UserFilter.Basic``

## Usage

In the Startup.cs class add the following piece of code :

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

| Parameter  | Description                                  |
| ---------- | -------------------------------------------- |
| ClaimKey   | Claim key                                    |
| ClaimValue | Regular expression or value                  |
| Operation  | *Equal* or *NotEqual* or *RegularExpression* |
