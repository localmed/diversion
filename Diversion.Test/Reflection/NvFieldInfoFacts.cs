using Diversion.Reflection;


namespace Diversion.Test.Reflection
{

    public class NvFieldInfoFacts
    {
        private IReflectionInfoFactory _factory;

        public NvFieldInfoFacts()
        {
            _factory = new NvReflectionInfoFactory();
        }
    }
}
