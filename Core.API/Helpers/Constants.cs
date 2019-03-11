using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.API.Models;

namespace Core.API.Helpers
{
    public static class Constants
    {
        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string Moderator = "Moderator";
            public const string VIP = "VIP";
            public const string Member = "Member";

            public static List<Role> GetUserRoles()
            {
                var properties = typeof(UserRoles)
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                    .ToList();

                var roles = new List<Role>();
                foreach (var fieldInfo in properties)
                {
                    roles.Add(new Role { Name = fieldInfo.GetRawConstantValue() as string });
                }
                return roles;
            }
        }

        public static class Policy
        {
            public const string RequireAdmin = "RequireAdminPolicy";
            public const string ModeratePhoto = "ModeratePhotoPolicy";
            public const string VipOnly = "VipOnlyPolicy";
        }
    }
}