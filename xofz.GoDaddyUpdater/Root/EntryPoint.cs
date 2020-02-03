namespace xofz.GoDaddyUpdater.Root
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    internal static class EntryPoint
    {
        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve +=
                currentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var bootstrapper = new FormsBootstrapper();
            bootstrapper.Bootstrap();

            Application.Run(bootstrapper.Shell);
        }

        private static Assembly currentDomain_AssemblyResolve(
            object sender,
            ResolveEventArgs e)
        {
            var resourceName = e.Name;
            var assemblyName = new AssemblyName(resourceName);
            if (resourceName.EndsWith(
                @"Retargetable=Yes"))
            {
                return Assembly.Load(assemblyName);
            }

            var container = Assembly.GetExecutingAssembly();
            var path = assemblyName.Name + @".dll";

            using (var s = container.GetManifestResourceStream(path))
            {
                if (s == null)
                {
                    return null;
                }

                var bytes = new byte[s.Length];
                s.Read(bytes, 0, bytes.Length);
                return Assembly.Load(bytes);
            }
        }
    }
}
