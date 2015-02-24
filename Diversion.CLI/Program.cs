using System;

namespace Diversion.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new NextVersion().Determine(args[0], args[1]));
        }
    }
}
