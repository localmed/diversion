using System;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvAttributeArgumentInfo : IAttributeArgumentInfo
    {
        private readonly string _name;
        private readonly object _value;

        public NvAttributeArgumentInfo(string name, object value)
        {
            _name = name;
            _value = value;
        }

        public string Name
        {
            get { return _name; }
        }

        public object Value
        {
            get { return _value; }
        }
    }
}