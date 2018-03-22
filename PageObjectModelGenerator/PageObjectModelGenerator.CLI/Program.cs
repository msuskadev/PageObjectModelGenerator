using PageObjectModelGenerator.CLI.Utils;
using PageObjectModelGenerator.UIAutomationFramework;
using System;
using System.Collections.Generic;

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
            new Class1().GetAllControls(attributes["-p"]);
            Console.ReadLine();
        }      
    }
}
