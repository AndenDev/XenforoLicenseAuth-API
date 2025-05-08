namespace API.ApiRoutes
{
    public static class UserRefreshTokenRoutes
    {
        public const string Base = "api/refresh-token";
        public const string GetByToken = $"{Base}/{{token}}";
        public const string Add = $"{Base}";
        public const string Delete = $"{Base}/{{token}}";
    }
}
