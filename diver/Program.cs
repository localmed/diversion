using System;
using Microsoft.DotNet.Cli.Utils;

namespace diver
{
    class Program
    {
        public static int Main(string[] args)
        {
            var exitCode = Command
                .CreateDotNet("build", Array.Empty<string>())
                .WorkingDirectory("..\\..\\..")
                .OnErrorLine(line => Console.WriteLine(line))
                .Execute()
                .ExitCode;
            Console.ReadKey();
            return exitCode;
        }
    }
}
