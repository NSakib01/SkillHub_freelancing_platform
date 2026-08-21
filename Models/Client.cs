namespace SkillHub.Models
{
    public sealed class Client : User
    {
        public override string RoleName
        {
            get { return UserRoles.Client; }
        }

        public override string DashboardTitle
        {
            get { return "Client Marketplace Dashboard"; }
        }
    }
}
