namespace xofz.GoDaddyUpdater.Framework
{
    using System.Security.Principal;

    public class AdminChecker
    {
        public virtual bool CurrentUserIsAdmin()
        {
            var principle = new WindowsPrincipal(
                WindowsIdentity.GetCurrent());
            return principle.IsInRole(
                WindowsBuiltInRole.Administrator);
        }
    }
}
