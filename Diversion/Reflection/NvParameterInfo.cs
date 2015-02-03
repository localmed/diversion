using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diversion.Reflection
{
    class NvParameterInfo : IParameterInfo
    {
        public IReadOnlyList<IAttributeInfo> Attributes
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeInfo Type
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }
    }

    class NvAttributeInfo : IAttributeInfo
    {
        public ITypeInfo Type
        {
            get { throw new NotImplementedException(); }
        }

        public IReadOnlyList<IAttributeArgumentInfo> Arguments
        {
            get { throw new NotImplementedException(); }
        }
    }
}
