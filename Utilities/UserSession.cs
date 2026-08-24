using System;
using SkillHub.Models;

namespace SkillHub.Utilities
{
    /// <summary>
    /// Shared signed-in identity. Teammates must read UserId/RoleName instead
    /// of hardcoding account IDs or passing unsafely trusted form values.
    /// </summary>
    public static class UserSession
    {
        private static User _currentUser;

        public static bool IsAuthenticated
        {
            get { return _currentUser != null; }
        }

        public static int UserId
        {
            get { return RequireCurrentUser().UserId; }
        }

        public static int LoginUserId
        {
            get { return UserId; }
        }

        public static string FullName
        {
            get { return RequireCurrentUser().FullName; }
        }

        public static string Email
        {
            get { return RequireCurrentUser().Email; }
        }

        public static string RoleName
        {
            get { return RequireCurrentUser().RoleName; }
        }

        public static string UserType
        {
            get { return UserRoles.ToLegacyUserType(RoleName); }
        }

        public static User CurrentUser
        {
            get { return RequireCurrentUser(); }
        }

        public static void Start(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.UserId <= 0)
            {
                throw new ArgumentException("A session needs a valid database user ID.", nameof(user));
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException("Only active accounts can start a session.");
            }

            _currentUser = user;
        }

        public static void Refresh(User user)
        {
            if (!IsAuthenticated || user == null || user.UserId != UserId)
            {
                throw new InvalidOperationException("Only the current account can refresh its session.");
            }

            Start(user);
        }

        public static void Clear()
        {
            _currentUser = null;
        }

        private static User RequireCurrentUser()
        {
            if (_currentUser == null)
            {
                throw new InvalidOperationException("No SkillHub user is currently signed in.");
            }

            return _currentUser;
        }
    }
}