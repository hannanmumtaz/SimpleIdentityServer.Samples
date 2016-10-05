namespace WebApplication
{
    public class Constants
    {
        public const string RootManagerApiUrl = "https://localhost:5444/api";
        public const string UmaConfigurationUrl = "https://localhost:5445/.well-known/uma-configuration";
        public const string BaseOpenIdUrl = "https://localhost:5443";
        public const string OpenIdConfigurationUrl = BaseOpenIdUrl + "/.well-known/openid-configuration";
        public const string ResourcesUrl = "https://localhost:5444/api/vs/resources";
        public const string ClientId = "f06ec666-94cf-4dc4-9d45-b39e2199b4bd";
        public const string ClientSecret = "da97ac7b-88ca-4856-8ce7-d0e0402c2bbd";
        public const string WebApplicationResource = "resources/WebSite";
        public const string CookieWebApplicationName = "CookieWebApplication";
        public const string Callback = "https://localhost:5105/Authenticate/Callback";
    }
}
