namespace API.ApiRoutes
{
    public static class AuthRoutes
    {
        public const string Base = "api/auth";
        public const string Login = $"{Base}/login";
        public const string Refresh = $"{Base}/refresh-token";
        public const string Logout = $"{Base}/logout";
    }
}
