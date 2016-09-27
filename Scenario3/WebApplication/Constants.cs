namespace WebApplication
{
    public class Constants
    {
        public const string RootManagerApiUrl = "https://localhost:5444/api";
        public const string UmaConfigurationUrl = "https://localhost:5445/.well-known/uma-configuration";
        public const string BaseOpenIdUrl = "https://localhost:5443";
        public const string OpenIdConfigurationUrl = BaseOpenIdUrl + "/.well-known/openid-configuration";
        public const string ResourcesUrl = "https://localhost:5444/api/vs/resources";
        public const string ClientId = "70c665a2-88ed-4dd6-be68-38a7988690fd";
        public const string ClientSecret = "4551ede1-3cf0-4a1f-b943-e2199875c98f";
        public const string WebApplicationResource = "resources/WebSite";
        public const string CookieWebApplicationName = "CookieWebApplication";
        public const string Callback = "https://localhost:5105/Authenticate/Callback";
    }
}
