using System;

namespace Diversion.Reflection
{
    [Serializable]
    class NvAttributeArgumentInfo : IAttributeArgumentInfo
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((NvAttributeArgumentInfo) obj);
        }

        private bool Equals(NvAttributeArgumentInfo other)
        {
            return string.Equals(_name, other._name) && Equals(_value, other._value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_name.GetHashCode() * 397) ^ (_value != null ? _value.GetHashCode() : 0);
            }
        }
    }
}