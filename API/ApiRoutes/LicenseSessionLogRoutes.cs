namespace API.ApiRoutes
{
    public static class LicenseSessionLogRoutes
    {
        public const string Base = "api/licensesession";
        public const string GetByLicenseId = $"{Base}/license/{{licenseId}}";
        public const string GetActiveSessions = $"{Base}/active";
    }
}
