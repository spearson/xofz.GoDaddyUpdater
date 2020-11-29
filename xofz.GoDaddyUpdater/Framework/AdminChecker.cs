namespace xofz.GoDaddyUpdater.Framework
{
    using System.Security.Principal;
    using xofz.Framework;

    public class AdminChecker
    {
        public AdminChecker()
        {
        }

        public AdminChecker(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual bool CurrentUserIsAdmin()
        {
            var principle = new WindowsPrincipal(
                WindowsIdentity.GetCurrent());
            return principle.IsInRole(
                WindowsBuiltInRole.Administrator);
        }

        protected readonly MethodRunner runner;
    }
}
