namespace WebApplication
{
    public class Constants
    {
        public const string RootManagerApiUrl = "https://localhost:5444/api";
        public const string UmaConfigurationUrl = "https://localhost:5445/.well-known/uma-configuration";
        public const string BaseOpenIdUrl = "https://localhost:5443";
        public const string OpenIdConfigurationUrl = BaseOpenIdUrl + "/.well-known/openid-configuration";
        public const string ResourcesUrl = "https://localhost:5444/api/vs/resources";
        public const string ClientId = "3a768a19-504d-4928-a840-a3626b466e33";
        public const string ClientSecret = "fe5ab40c-87da-45ef-9317-e6c7c31197f0";
        public const string WebApplicationResource = "resources/Website";
        public const string CookieWebApplicationName = "CookieWebApplication";
        public const string Callback = "https://localhost:5105/Authenticate/Callback";
    }
}
