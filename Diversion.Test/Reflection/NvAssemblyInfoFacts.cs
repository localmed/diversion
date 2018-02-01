using Diversion.Reflection;


namespace Diversion.Test.Reflection
{
    public class NvAssemblyInfoFacts
    {
        private IReflectionInfoFactory _factory;

        public NvAssemblyInfoFacts()
        {
            _factory = new NvReflectionInfoFactory();
        }
    }
}
