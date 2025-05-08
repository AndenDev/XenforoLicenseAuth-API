namespace Shared.Constants
{
    public static class UserGroupPermissions
    {
        public const int Admin = 3;

        public static string GetRoleName(int userGroupId) => userGroupId switch
        {
            Admin => "admin",
            _ => "user"
        };
    }
}
