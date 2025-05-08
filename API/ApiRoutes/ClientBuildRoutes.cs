namespace API.ApiRoutes
{
    public static class ClientBuildRoutes
    {
        public const string Base = "api/clientbuild";
        public const string GetAll = $"{Base}/all";
        public const string GetByAppId = $"{Base}/application/{{appId}}";
        public const string Add = $"{Base}";
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }
}
