namespace ApiProtection.MedicalPrescriptionsApi
{
    internal static class Constants
    {
        public const string ClientId = "MedicalPrescriptionApi";
        public const string ClientSecret = "qYZfCXJ39fnfByms";
        public const string WellKnownConfiguration = "http://localhost:60004/.well-known/uma2-configuration";
        public const string OpenidWellKnownConfiguration = "http://localhost:60000/.well-known/openid-configuration";

        public static class RouteNames
        {
            public const string PrescriptionsController = "prescriptions";
        }
    }
}
