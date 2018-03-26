using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PageObjectModelGenerator.CLI.Utils
{
    public class ConsoleAttributeParser
    {
        private const string ParametersRegex = @"-\w{1}";
        public Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            string key, value;

            for (int i = 0; i < args.Length; i++)
            {
                key = value = string.Empty;
                if (args[i].Length == 2 && Regex.IsMatch(args[i], ParametersRegex))
                {
                    key = args[i];
                }

                if (key != string.Empty && i < args.Length - 1 && !Regex.IsMatch(args[i + 1], ParametersRegex))
                {
                    value = args[i + 1];
                    i++;
                }

                if (key != string.Empty)
                {
                    attributes.Add(key, value);
                }
            }

            return attributes;
        }
    }
}
