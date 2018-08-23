using Diversion.Cecil;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NuGet.Frameworks;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;

namespace Diversion.Analyze
{
    class Program
    {
        static void Main(string[] args)
        {
            var diversion = new AssemblyDiversionDiviner(new AssemblyInfoFactory(), new DiversionDiviner()).Divine(args[0], args[1]);
            if (args.Length == 3)
            {
                File.WriteAllText(args[2], JsonConvert.SerializeObject(
                    diversion, Formatting.Indented, new JsonSerializerSettings
                    { ContractResolver = new CustomContractResolver() })
                );
            }
            Console.Out.Write(JsonConvert.SerializeObject(
                new NextVersion().Analyze(diversion),
                new NuGetVersionConverter(),
                new NuGetFrameworkConverter()));
#if DEBUG
            Console.Read();
#endif
        }
    }

    class CustomContractResolver : DefaultContractResolver
    {
        protected override List<System.Reflection.MemberInfo> GetSerializableMembers(Type objectType)
        {
            var members = base.GetSerializableMembers(objectType);
            return typeof(IAssemblyDiversion).IsAssignableFrom(objectType) ? members.FindAll(m => !m.Name.Equals("Old") && !m.Name.Equals("New")) : members;
        }
    }

    class NuGetVersionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(NuGetVersion).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new NuGetVersion((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((NuGetVersion)value).ToFullString());
        }
    }


    class NuGetFrameworkConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(NuGetFramework).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var frameworkName = new FrameworkName((string)reader.Value);
            return new NuGetFramework(frameworkName.Identifier, frameworkName.Version, frameworkName.Profile);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((NuGetFramework)value).DotNetFrameworkName);
        }
    }
}
