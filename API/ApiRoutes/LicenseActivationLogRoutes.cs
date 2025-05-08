namespace API.ApiRoutes
{
    public static class LicenseActivationLogRoutes
    {
        public const string Base = "api/licenseactivation";
        public const string GetByLicenseId = $"{Base}/license/{{licenseId}}";
        public const string GetByUserId = $"{Base}/user/{{userId}}";
    }
}
