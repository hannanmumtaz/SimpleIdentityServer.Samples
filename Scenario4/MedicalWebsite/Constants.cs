namespace MedicalWebsite
{
    public class Constants
    {
        public const string CookieName = "MedicalWebsite";
        public const string Callback = "https://localhost:5106/Authenticate/Callback";
        public const string ClientId = "MedicalWebsite";
        public const string ClientSecret = "MedicalWebsite";
        public const string ApiUrl = "http://localhost:5107";
        public const string AuthorizationEdp = "https://localhost:5443/authorization";
        public const string RootManagerApiUrl = "https://localhost:5444/api";
        public const string UmaConfigurationUrl = "https://localhost:5445/.well-known/uma-configuration";
        public const string BaseOpenIdUrl = "https://localhost:5443";
        public const string OpenIdConfigurationUrl = BaseOpenIdUrl + "/.well-known/openid-configuration";
        public const string ResourcesUrl = "https://localhost:5444/api/vs/resources";
        public const string ApplicationResourceUrl = "resources/MedicalWebsite/patient/Samples";

        public const string DoctorRole = "doctor";
        public const string PatientRole = "patient";

        public const string ReadScope = "read";
        public const string WriteScope = "write";
    }
}
