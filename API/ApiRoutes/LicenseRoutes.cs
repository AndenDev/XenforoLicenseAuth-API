namespace API.ApiRoutes
{
    public static class LicenseRoutes
    {
        public static class Admin
        {
            public const string Base = "api/admin/license";
            public const string GetAll = $"{Base}/all";
            public const string GetById = $"{Base}/{{id}}";
            public const string GetByKey = $"{Base}/key/{{key}}";
            public const string GetByUserId = $"{Base}/user/{{userId}}";
            public const string Add = $"{Base}";
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class Client
        {
            public const string Base = "api/client/license";
            public const string Activate = $"{Base}/activate";
            public const string Heartbeat = $"{Base}/heartbeat";
            public const string Validate = $"{Base}/validate";
            public const string Status = $"{Base}/status";
            // add more client-specific routes here
        }
    }
}
