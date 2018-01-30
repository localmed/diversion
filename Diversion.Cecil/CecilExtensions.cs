using Mono.Cecil;
using Mono.Cecil.Rocks;
using System.Linq;

namespace Diversion.Cecil
{
    public static class CecilExtensions
    {
        public static TypeDefinition GetBaseDeclaringType(this IMemberDefinition member)
        {
            return member.GetBaseMember().DeclaringType;
        }

        public static IMemberDefinition GetBaseMember(this IMemberDefinition member)
        {
            switch (member)
            {
                case EventDefinition m:
                    {
                        var baseType = GetBaseMember(m.AddMethod ?? m.RemoveMethod).DeclaringType;
                        return Equals(baseType, member.DeclaringType) ? m : baseType.Events.FirstOrDefault(b => b.Name == m.Name);
                    }
                case MethodDefinition m:
                    return m.GetOriginalBaseMethod() ?? m;
                case PropertyDefinition m:
                    {
                        var baseType = GetBaseMember(m.GetMethod ?? m.SetMethod).DeclaringType;
                        return Equals(baseType, member.DeclaringType) ? m : baseType.Properties.FirstOrDefault(b => b.Name == m.Name);
                    }
            }
            return member;
        }
    }
}