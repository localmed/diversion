using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Diversion
{
    class AppDomainContext : IDisposable
    {
        private readonly AppDomain _domain;
        private readonly AssemblyLoader _loader;
        private bool _disposed;

        public AppDomainContext(string location)
        {
            _domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, new AppDomainSetup{ ApplicationBase = location });
            _loader = (AssemblyLoader)_domain.CreateInstanceFromAndUnwrap(
                    typeof(AssemblyLoader).Assembly.Location,
                    typeof(AssemblyLoader).FullName);
            _loader.ReflectLoad(Directory.EnumerateFiles(location, "*.dll", SearchOption.AllDirectories).ToArray());
            _loader.Load(GetRequiredAssemblies(GetType().Assembly).ToArray());
        }

        private IEnumerable<string> GetRequiredAssemblies(Assembly assembly)
        {
            yield return assembly.Location;
            foreach (var dependency in assembly.GetReferencedAssemblies())
                yield return AppDomain.CurrentDomain.Load(dependency).Location;
        }

        public AppDomain Domain
        {
            get { return _domain; }
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                try
                {
                    _disposed = true;
                    AppDomain.Unload(_domain);
                }
                catch (Exception)
                {
                    Trace.TraceError("Could not unload app domain.");
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    public class AssemblyLoader : MarshalByRefObject
    {
        public AssemblyLoader()
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (s, a) =>
            {
                try
                {
                    return Assembly.ReflectionOnlyLoad(a.Name);
                }
                catch
                {
                    Trace.TraceWarning($"Could not load {a.Name} into reflection only context.");
                    return null;
                }
            };
        }

        public void Load(string[]assemblies)
        {
            foreach (var assembly in assemblies)
                try
                {
                    Assembly.LoadFrom(assembly);
                }
                catch
                {
                    Trace.TraceWarning($"Could not load {assembly} into execution context.");
                }
        }

        public void ReflectLoad(string[] assemblies)
        {
            foreach (var assembly in assemblies)
                try
                {
                    Assembly.ReflectionOnlyLoadFrom(assembly);
                }
                catch
                {
                    Trace.TraceWarning($"Could not load {assembly} into reflection only context.");
                }
        }
    }
}