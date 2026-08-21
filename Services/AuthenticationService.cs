using System;
using System.Data;
using System.Data.SqlClient;
using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.Utilities;

namespace SkillHub.Services
{
    /// <summary>
    /// Shared account, registration, authentication and soft-delete workflow.
    /// </summary>
    public sealed class AuthenticationService
    {
        private readonly UserRepository _users;

        public AuthenticationService()
            : this(new UserRepository())
        {
        }

        public AuthenticationService(UserRepository users)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            _users = users;
        }

        public User Login(string email, string password)
        {
            string normalizedEmail = InputValidator.RequireEmail(email);

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is required.");
            }

            User user = _users.GetByEmail(normalizedEmail);

            if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("The email or password is incorrect.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "This account is not active. Contact the platform administrator.");
            }

            _users.RecordSuccessfulLogin(user.UserId);
            UserSession.Start(user);

            return user;
        }

        public int Register(
            string roleName,
            string fullName,
            string email,
            string phone,
            string address,
            string password,
            string passwordConfirmation)
        {
            if (!UserRoles.IsPublicRegistrationRole(roleName))
            {
                throw new ArgumentException(
                    "Public registration is available only for Client or Freelancer accounts.");
            }

            string normalizedName = InputValidator.RequireName(fullName);
            string normalizedEmail = InputValidator.RequireEmail(email);
            string normalizedPhone = InputValidator.NormalizeOptionalPhone(phone);
            string normalizedAddress = InputValidator.NormalizeOptionalAddress(address);

            InputValidator.ValidateNewPassword(password, passwordConfirmation);

            User account = string.Equals(roleName, UserRoles.Client, StringComparison.OrdinalIgnoreCase)
                ? (User)new Client()
                : new Freelancer();

            account.FullName = normalizedName;
            account.Email = normalizedEmail;
            account.Phone = normalizedPhone;
            account.Address = normalizedAddress;
            account.PasswordHash = PasswordHasher.HashPassword(password);
            account.Status = AccountStatuses.Active;

            try
            {
                return _users.Create(account);
            }
            catch (SqlException exception)
            {
                if (exception.Number == 2601 || exception.Number == 2627)
                {
                    throw new ArgumentException(
                        "That email address is already registered. Use another email address.",
                        exception);
                }

                throw;
            }
        }

        public DataTable SearchAccounts(string searchTerm)
        {
            AuthorizationService.DemandAdmin();
            return _users.Search(searchTerm);
        }

        public void UpdateMyProfile(
            string fullName,
            string email,
            string phone,
            string address)
        {
            AuthorizationService.DemandAuthenticated();

            UpdateAccountProfile(UserSession.UserId, fullName, email, phone, address);
        }

        public void UpdateAccountProfile(
            int userId,
            string fullName,
            string email,
            string phone,
            string address)
        {
            AuthorizationService.DemandAuthenticated();

            if (UserSession.UserId != userId)
            {
                AuthorizationService.DemandAdmin();
            }

            string normalizedName = InputValidator.RequireName(fullName);
            string normalizedEmail = InputValidator.RequireEmail(email);
            string normalizedPhone = InputValidator.NormalizeOptionalPhone(phone);
            string normalizedAddress = InputValidator.NormalizeOptionalAddress(address);

            try
            {
                _users.UpdateProfile(
                    userId,
                    normalizedName,
                    normalizedEmail,
                    normalizedPhone,
                    normalizedAddress);
            }
            catch (SqlException exception)
            {
                if (exception.Number == 2601 || exception.Number == 2627)
                {
                    throw new ArgumentException(
                        "That email address already belongs to another account.",
                        exception);
                }

                throw;
            }

            if (UserSession.UserId == userId)
            {
                User updatedUser = _users.GetById(userId);
                UserSession.Refresh(updatedUser);
            }
        }

        public void ChangePassword(
            string currentPassword,
            string newPassword,
            string newPasswordConfirmation)
        {
            AuthorizationService.DemandAuthenticated();

            if (string.IsNullOrEmpty(currentPassword))
            {
                throw new ArgumentException("Your current password is required.");
            }

            User currentUser = _users.GetById(UserSession.UserId);

            if (currentUser == null || !PasswordHasher.VerifyPassword(
                currentPassword,
                currentUser.PasswordHash))
            {
                throw new UnauthorizedAccessException("Your current password is incorrect.");
            }

            InputValidator.ValidateNewPassword(newPassword, newPasswordConfirmation);

            if (PasswordHasher.VerifyPassword(newPassword, currentUser.PasswordHash))
            {
                throw new ArgumentException(
                    "Your new password must be different from the current password.");
            }

            string passwordHash = PasswordHasher.HashPassword(newPassword);
            _users.UpdatePasswordHash(currentUser.UserId, passwordHash);

            UserSession.Refresh(_users.GetById(currentUser.UserId));
        }

        public void DeactivateAccount(int userId)
        {
            AuthorizationService.DemandAdmin();

            if (userId == UserSession.UserId)
            {
                throw new InvalidOperationException(
                    "You cannot deactivate the administrator account you are currently using.");
            }

            _users.Deactivate(userId);
        }

        public void Logout()
        {
            UserSession.Clear();
        }
    }
}
