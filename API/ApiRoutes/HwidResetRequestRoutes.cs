namespace API.ApiRoutes
{
    public static class HwidResetRequestRoutes
    {
        public const string Base = "api/hwidreset";
        public const string GetPending = $"{Base}/pending";
        public const string Add = $"{Base}";
        public const string Approve = $"{Base}/approve/{{id}}";
        public const string Deny = $"{Base}/deny/{{id}}";
    }
}
