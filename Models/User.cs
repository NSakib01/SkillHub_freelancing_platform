using System;

namespace SkillHub.Models
{
    /// <summary>
    /// Common abstract account model: abstraction and inheritance are visible
    /// in every authenticated role without duplicating identity fields.
    /// </summary>
    public abstract class User
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public abstract string RoleName { get; }

        public virtual string DashboardTitle
        {
            get { return RoleName + " Dashboard"; }
        }

        public bool IsActive
        {
            get
            {
                return string.Equals(
                    Status,
                    AccountStatuses.Active,
                    StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
