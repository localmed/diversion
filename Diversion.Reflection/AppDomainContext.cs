using System;
using System.Diagnostics;

namespace Diversion
{
    class AppDomainContext : IDisposable
    {
        private readonly AppDomain _domain;
        private bool _disposed;

        public AppDomainContext(string location)
        {
            _domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, new AppDomainSetup{ ApplicationBase = location});
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
}