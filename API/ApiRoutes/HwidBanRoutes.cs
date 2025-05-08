namespace API.ApiRoutes
{
    public static class HwidBanRoutes
    {
        public const string Base = "api/hwidban";
        public const string GetAll = $"{Base}/all";
        public const string Check = $"{Base}/check/{{hwid}}";
        public const string Add = $"{Base}";
        public const string Remove = $"{Base}/{{hwid}}";
    }
}
