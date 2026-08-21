namespace SkillHub.Models
{
    public sealed class Admin : User
    {
        public override string RoleName
        {
            get { return UserRoles.Admin; }
        }

        public override string DashboardTitle
        {
            get { return "Platform Administration Dashboard"; }
        }
    }
}
