namespace WebApplication
{
    public class Constants
    {
        public const string RootManagerApiUrl = "https://localhost:5444/api";
        public const string UmaConfigurationUrl = "https://localhost:5445/.well-known/uma-configuration";
        public const string BaseOpenIdUrl = "https://localhost:5443";
        public const string OpenIdConfigurationUrl = BaseOpenIdUrl + "/.well-known/openid-configuration";
        public const string ResourcesUrl = "https://localhost:5444/api/vs/resources";
        public const string ClientId = "598c1f2f-8c92-4f15-977f-ffee8152337f";
        public const string ClientSecret = "f323eef2-e04e-4243-b1c1-39ca88b84511";
        public const string WebApplicationResource = "resources/WebSite";
        public const string CookieWebApplicationName = "CookieWebApplication";
        public const string Callback = "https://localhost:5105/Authenticate/Callback";
    }
}
