using PageObjectModelGenerator.CLI.Utils;
using PageObjectModelGenerator.Engine;
using System;
using System.Collections.Generic;
using System.IO;

namespace PageObjectModelGenerator.CLI
{
    class Program
    {
        private static Dictionary<string, string> attributes;
        /*
         * 1. to do - get window title from process
         * 2. get all descendants recirsively
         */
        static void Main(string[] args)
        {
            attributes = new ConsoleAttributeParser().ParseArgs(args);
            var pomGenerator = new PomGenerator();
            pomGenerator.GetAllControls(attributes["-p"]);
            
            File.AppendAllText(@"c:\Temp\pom.cs", pomGenerator.GeneratePom("SomeTestPOM"));
            Console.ReadLine();
        }      
    }
}
