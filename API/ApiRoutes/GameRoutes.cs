namespace API.ApiRoutes
{
    public static class GameRoutes
    {
        public static class Admin
        {
            public const string Base = "api/game";
            public const string GetAll = $"{Base}/all";
            public const string GetById = $"{Base}/{{id}}";
            public const string Add = $"{Base}";
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }
    }
}
