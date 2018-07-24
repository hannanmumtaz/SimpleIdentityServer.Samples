# Basic account filter

When an external authentication (facebook, google etc ...) is enabled on a website, any users who have an external account  can be authenticated. In SimpleIdServer the external accounts which should not have access to the website can be rejected with a `Filter`.

The first `BasicFilter` excludes  all external accounts which do not satisfy **ALL** rules. Actually the claims are checked against **ALL** rules and if none is satisfied then the external account is rejected.

A filter contains a set of rules. All rules are evaluated and if at least one of them is not satisfied then the filter fails. There are three type of filters:

* *Equal*: check if a claim is equal to a specific value.

* *NotEqual*: check if a claim is different from a specific value.

* *RegularExpression*: check if a claim matches a regular expression.

## Nuget package

``SimpleIdentityServer.AccountFilter.Basic``

## Usage

In the Startup.cs class add the following piece of code:

```csharp
services.AddAccountFilter(new AccountFilterBasicOptions
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
                    ClaimKey = "organization",
                    ClaimValue = "org",
                    Operation = ComparisonOperations.Equal
                },
                new FilterComparison
                {
                    ClaimKey = "email",
                    ClaimValue = "spammer@spamming.com",
                    Operation = ComparisonOperations.NotEqual
                },
                new FilterComparison
                {
                    ClaimKey = "email",
                    ClaimValue = ".*@org.com$",
                    Operation = ComparisonOperations.NotEqual
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

## Sample project

To run the sample application please follow the steps below :

1. Fetch the [sample projects](https://github.com/thabart/SimpleIdentityServer.Samples.git).

2. Open the folder /SimpleIdentityServer.Samples/AccountFilter and execute the command **launch.cmd**.

In a browser open the url `http://localhost:64950` and try to connect with your facebook account, an error should be displayed because the filter rejects your authentication.
