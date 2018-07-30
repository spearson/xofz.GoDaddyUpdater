namespace xofz.GoDaddyUpdater.Service.Root
{
    using System;
    using System.Reflection;
    using System.ServiceProcess;

    internal static class EntryPoint
    {
        private static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve 
                += (sender, e) => loadEmbeddedAssembly(e.Name);

            var bootstrapper = new ServiceBootstrapper();
            bootstrapper.Bootstrap();
            var services = new ServiceBase[]
            {
                bootstrapper.Service
            };
            ServiceBase.Run(services);
        }

        private static Assembly loadEmbeddedAssembly(string name)
        {
            var assemblyName = new AssemblyName(name);
            if (name.EndsWith("Retargetable=Yes"))
            {
                return Assembly.Load(assemblyName);
            }

            var container = Assembly.GetExecutingAssembly();
            var path = assemblyName.Name + ".dll";

            using (var stream = container.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    return null;
                }

                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return Assembly.Load(bytes);
            }
        }
    }
}
