using Diversion.Reflection;


namespace Diversion.Test.Reflection
{

    public class NvConstructorInfoFacts
    {
        private IReflectionInfoFactory _factory;

        public NvConstructorInfoFacts()
        {
            _factory = new NvReflectionInfoFactory();
        }
    }
}
