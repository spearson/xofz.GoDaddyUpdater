﻿namespace xofz.GoDaddyUpdater.Service.Root
{ 
    using System.Reflection;
    using System.ServiceProcess;

    internal static class EntryPoint
    {
        private static void Main()
        {
            System.AppDomain.CurrentDomain.AssemblyResolve 
                += currentDomain_AssemblyResolve;

            var bootstrapper = new ServiceBootstrapper();
            bootstrapper.Bootstrap();
            var service = bootstrapper.Service;
            ServiceBase[] services;
            if (service != null)
            {
                services = new[]
                {
                    service
                };

                goto run;
            }

            services = System.Array.Empty<ServiceBase>();
            
            run:
            ServiceBase.Run(services);
        }

        private static Assembly currentDomain_AssemblyResolve(
            object sender,
            System.ResolveEventArgs e)
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
