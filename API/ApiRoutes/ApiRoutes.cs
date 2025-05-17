namespace API.ApiRoutes
{
    public static class AuthRoutes
    {
        public const string Base = "/api/auth";
        public const string Login = $"{Base}/login";
        public const string ValidateSession = $"{Base}/validate-session";
        public const string Logout = $"{Base}/logout";
    }
    public static class UserRoutes
    {
        public const string Base = "/api/user";
        public const string Profile = $"{Base}/{{userId}}";
    }
}
