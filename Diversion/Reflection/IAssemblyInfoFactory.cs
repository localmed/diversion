namespace Diversion.Reflection
{
    public interface IAssemblyInfoFactory
    {
        IAssemblyInfo FromFile(string assemblyPath);
    }
}
