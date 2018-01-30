using Diversion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diversion.Cecil
{
    [Serializable]
    public class AttributeArgumentInfo : IAttributeArgumentInfo
    {
        private readonly string _name;
        private readonly object _value;

        public AttributeArgumentInfo(string name, object value)
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
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AttributeArgumentInfo) obj);
        }

        private bool Equals(AttributeArgumentInfo other)
        {
            return string.Equals(_name, other._name) && ((_value as Array)?.OfType<object>().SequenceEqual(other._value as IEnumerable<object>) ?? Equals(_value, other._value));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_name.GetHashCode() * 397) ^ (_value != null ? ((_value as Array)?.OfType<object>().Take(16).Aggregate(19, (result, item) => result * 31 + item.GetHashCode()) ?? _value.GetHashCode()) : 0);
            }
        }

        public override string ToString() => $"{Name} = {Value}";
    }
}