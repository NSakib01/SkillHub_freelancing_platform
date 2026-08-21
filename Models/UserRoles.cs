using System;

namespace SkillHub.Models
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Freelancer = "Freelancer";
        public const string Client = "Client";

        public const string SuperAdminAlias = "SUPER_ADMIN";
        public const string FreelancerAlias = "ADMIN";
        public const string CustomerAlias = "CUSTOMER";

        public static bool IsPublicRegistrationRole(string roleName)
        {
            return string.Equals(roleName, Freelancer, StringComparison.OrdinalIgnoreCase)
                || string.Equals(roleName, Client, StringComparison.OrdinalIgnoreCase);
        }

        public static string ToLegacyUserType(string roleName)
        {
            if (string.Equals(roleName, Admin, StringComparison.OrdinalIgnoreCase))
            {
                return SuperAdminAlias;
            }

            if (string.Equals(roleName, Freelancer, StringComparison.OrdinalIgnoreCase))
            {
                return FreelancerAlias;
            }

            if (string.Equals(roleName, Client, StringComparison.OrdinalIgnoreCase))
            {
                return CustomerAlias;
            }

            throw new ArgumentException("Unknown SkillHub role.", nameof(roleName));
        }
    }

    public static class AccountStatuses
    {
        public const string Active = "Active";
        public const string Suspended = "Suspended";
        public const string Deactivated = "Deactivated";
    }
}
