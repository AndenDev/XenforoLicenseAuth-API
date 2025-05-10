namespace API.ApiRoutes
{
    public static class AuthRoutes
    {
        public const string Base = "/api/auth";
        public const string Login = $"{Base}/login";
        public const string ValidateSession = $"{Base}/validate-session";
        public const string Logout = $"{Base}/logout";
    }
    public static class HomeRoutes
    {
        public const string Base = "/api/home";
        public const string Summary = $"{Base}/summary";
    }
}
