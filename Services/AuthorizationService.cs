using System;
using SkillHub.Models;
using SkillHub.Utilities;

namespace SkillHub.Services
{
    public static class AuthorizationService
    {
        public static void DemandAuthenticated()
        {
            if (!UserSession.IsAuthenticated)
            {
                throw new UnauthorizedAccessException(
                    "Please sign in before opening this screen.");
            }
        }

        public static void DemandRole(string expectedRole)
        {
            DemandAuthenticated();

            if (!string.Equals(
                UserSession.RoleName,
                expectedRole,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException(
                    "Your account does not have permission to open this screen.");
            }
        }

        public static void DemandAdmin()
        {
            DemandRole(UserRoles.Admin);
        }

        public static void DemandCurrentUser(int expectedUserId)
        {
            DemandAuthenticated();

            if (UserSession.UserId != expectedUserId)
            {
                throw new UnauthorizedAccessException(
                    "You can access only your own account records.");
            }
        }
    }
}
