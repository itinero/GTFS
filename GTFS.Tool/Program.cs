using GTFS.Tool.Switches;
using GTFS.Tool.Switches.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFS.Tool
{
    class Program
    {
        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            // parse switches first.
            var switches = SwitchParser.ParseSwitches(args);

            // convert commands into data processors.
            if (switches == null)
            {
                throw new Exception("Please specifiy a valid data processing command!");
            }

            var collapsedSwitches = new List<ProcessorBase>();
            for (int idx = 0; idx < switches.Length; idx++)
            {
                var processor = switches[idx].CreateProcessor();
                processor.Collapse(collapsedSwitches);
            }

            if (collapsedSwitches.Count > 1)
            { // there is more than one command left.
                throw new Exception("Command list could not be interpreted. Make sure you have the correct source/filter/target combinations.");
            }

            if (collapsedSwitches[0].CanExecute)
            { // execute the last remaining fully collapsed command.
                collapsedSwitches[0].Execute();
            }
        }
    }
}
