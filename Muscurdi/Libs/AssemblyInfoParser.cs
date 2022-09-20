using System.Reflection;

namespace Muscurdi.Libs;
public static class AssemblyInfoParser
{
    public static string GetCommitHashFromAssembly()
    {
        var version = Assembly.GetExecutingAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "dev";
        int index = -1;
        if ((index = version.IndexOf('+')) < 0) return "dev";
        return version.Substring(version.IndexOf('+') + 1);
    }
}
