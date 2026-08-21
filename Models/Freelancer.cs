namespace SkillHub.Models
{
    public sealed class Freelancer : User
    {
        public override string RoleName
        {
            get { return UserRoles.Freelancer; }
        }

        public override string DashboardTitle
        {
            get { return "Freelancer Services Dashboard"; }
        }
    }
}
