using System;
using System.Diagnostics;

namespace Muscurdi.Libs;
public static class SingleAppInstance
{
    public static void Check(string appName)
    {
        string procName = Process.GetCurrentProcess().ProcessName;
        Process[] processes = Process.GetProcessesByName(procName);
        if (processes.Length > 1)
        {
            Console.WriteLine($"An Instance of {appName} is already running.");
            Environment.Exit(0);
        }
    }
}
