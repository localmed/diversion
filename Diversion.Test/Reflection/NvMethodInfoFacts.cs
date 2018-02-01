using Diversion.Reflection;


namespace Diversion.Test.Reflection
{

    public class NvMethodInfoFacts
    {
        private IReflectionInfoFactory _factory;

        public NvMethodInfoFacts()
        {
            _factory = new NvReflectionInfoFactory();
        }
    }
}
