using GTFS.Tool.Switches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFS.Tool
{
    /// <summary>
    /// Parsers switchs given to this data processor.
    /// </summary>
    public static class SwitchParser
    {
        /// <summary>
        /// Holds a list of supported switchs.
        /// </summary>
        private static List<Switch> _switches = new List<Switch>(new Switch[] { 
           new SwitchReadFeed(), new SwitchWriteFeed(), new SwitchFilterBoundingBox(), 
           new SwitchFilterRoutes(), new SwitchFilterBoundingGeoJSON(), new SwitchValidateFeed() });

        /// <summary>
        /// Parses the switch line arguments into a sorted list of switchs.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Switch[] ParseSwitches(string[] args)
        {
            // initialize the list of switchs.
            var switchs = new List<Switch>();

            // start parsing arguments one-by-one.
            int idx = 0;
            while (idx < args.Length)
            { // check the next argument for a switch.
                if (!SwitchParser.IsSwitch(args[idx]))
                {
                    throw new SwitchParserException(args[idx], "Invalid switch!");
                }

                // parse the switch.
                Switch switchOut;
                int eatenArguments = SwitchParser.ParseSwitch(args, idx, out switchOut);

                // increase idx.
                idx = idx + eatenArguments;

                // add resulting switch.
                switchs.Add(switchOut);
            }

            return switchs.ToArray();
        }

        /// <summary>
        /// Parses one switchs and returns the number of eaten arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="idx"></param>
        /// <param name="switchOut"></param>
        /// <returns></returns>
        private static int ParseSwitch(string[] args, int idx, out Switch switchOut)
        {
            int eatenArguments = 1; // eat the first argument.
            string switchSwitch = args[idx]; // get the switch switch.

            // make sure no caps mess things up.
            switchSwitch = switchSwitch.ToLower();
            switchSwitch = switchSwitch.Trim();

            // loop over switchs and try to parse.
            foreach (var current in _switches)
            {
                if (current.HasSwitch(switchSwitch))
                { // this switch has the be the one!
                    eatenArguments = eatenArguments +
                                     current.Parse(args, idx + 1, out switchOut);
                    return eatenArguments;
                }
            }
            throw new SwitchParserException(args[idx], "Switch not found!");
        }

        /// <summary>
        /// Remove quotes from strings if they occur at exactly the beginning and end.
        /// </summary>
        /// <param name="stringToParse"></param>
        /// <returns></returns>
        public static string RemoveQuotes(string stringToParse)
        {
            if (string.IsNullOrEmpty(stringToParse))
            {
                return stringToParse;
            }

            if (stringToParse.Length < 2)
            {
                return stringToParse;
            }

            if (stringToParse[0] == '"' && stringToParse[stringToParse.Length - 1] == '"')
            {
                return stringToParse.Substring(1, stringToParse.Length - 2);
            }

            if (stringToParse[0] == '\'' && stringToParse[stringToParse.Length - 1] == '\'')
            {
                return stringToParse.Substring(1, stringToParse.Length - 2);
            }

            return stringToParse;
        }

        /// <summary>
        /// Returns true if the given string can be a switch.
        /// </summary>
        /// <param name="switchSwitch"></param>
        /// <returns></returns>
        public static bool IsSwitch(string switchSwitch)
        {
            // make sure no caps mess things up.
            switchSwitch = switchSwitch.ToLower();
            switchSwitch = switchSwitch.Trim();

            // test all switchs.
            foreach (var current in _switches)
            {
                if (current.HasSwitch(switchSwitch))
                {
                    return true;
                }
            }

            return false; // no switch found!
        }

        /// <summary>
        /// Returns true if the given string contains a key value like 'key=value'.
        /// </summary>
        /// <param name="keyValueString"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool SplitKeyValue(string keyValueString, out string[] keyValue)
        {
            keyValue = null;
            if (keyValueString.Count(x => x == '=') == 1)
            { // there is only one '=' sign here.
                int idx = keyValueString.IndexOf('=');
                if (idx > 0 && idx < keyValueString.Length - 1)
                {
                    keyValue = new string[2];
                    keyValue[0] = keyValueString.Substring(0, idx);
                    keyValue[1] = keyValueString.Substring(idx + 1, keyValueString.Length - (idx + 1));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if the given string contains one or more comma seperated values.
        /// </summary>
        /// <param name="valuesArray"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool SplitValuesArray(string valuesArray, out string[] values)
        {
            values = valuesArray.Split(',');
            return true;
        }
    }
}
