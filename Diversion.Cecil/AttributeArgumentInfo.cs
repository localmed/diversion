using Diversion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diversion.Cecil
{
    public class AttributeArgumentInfo : IAttributeArgumentInfo
    {
        public AttributeArgumentInfo(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public object Value { get; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AttributeArgumentInfo) obj);
        }

        private bool Equals(AttributeArgumentInfo other)
        {
            return string.Equals(Name, other.Name) && ((Value as Array)?.OfType<object>().SequenceEqual(other.Value as IEnumerable<object>) ?? Equals(Value, other.Value));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ (Value != null ? ((Value as Array)?.OfType<object>().Take(16).Aggregate(19, (result, item) => result * 31 + item.GetHashCode()) ?? Value.GetHashCode()) : 0);
            }
        }

        public override string ToString() => $"{Name} = {Value}";
    }
}