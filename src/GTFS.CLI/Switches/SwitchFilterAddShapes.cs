// The MIT License (MIT)

// Copyright (c) 2017 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using GTFS.CLI.Switches.Processors;
using System.IO;

namespace GTFS.CLI.Switches
{
    /// <summary>
    /// A switch to add shapes.
    /// </summary>
    public class SwitchFilterAddShapes : Switch
    {
        /// <summary>
        /// Returns the switches for this command.
        /// </summary>
        /// <returns></returns>
        public override string[] GetSwitch()
        {
            return new string[] {"--add-shapes" };
        }

        /// <summary>
        /// The routerdb.
        /// </summary>
        public string RouterDb { get; set; }

        /// <summary>
        /// Parse the command arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="idx"></param>
        /// <param name="switchOut"></param>
        /// <returns></returns>
        public override int Parse(string[] args, int idx, out Switch switchOut)
        {
            // check next argument.
            if (args.Length < idx + 1)
            {
                throw new SwitchParserException("None", "Invalid --add-shapes command!");
            }

            int startIdx = idx;
            while (idx < args.Length &&
                !SwitchParser.IsSwitch(args[idx]))
            { // parse arguments.
                var argSplit = args[idx].Split('=');

                if (argSplit.Length != 2 ||
                    argSplit[0] == null ||
                    argSplit[1] == null)
                {
                    throw new SwitchParserException(args[idx],
                        "Invalid argument for --add-shapes.");
                }

                switch (argSplit[0])
                {
                    case "routerdb":
                        this.RouterDb = argSplit[1];
                        break;
                    default:
                        throw new SwitchParserException(args[idx],
                            "Unknown argument for --add-shapes.");
                }

                idx++;
            }

            // everything ok, take the next argument as the filename.
            switchOut = new SwitchFilterAddShapes()
            {
                RouterDb = this.RouterDb,
            };
            return idx - startIdx;
        }

        /// <summary>
        /// Creates the processor that belongs to this data.
        /// </summary>
        /// <returns></returns>
        public override ProcessorBase CreateProcessor()
        {
            Itinero.RouterDb routerDb;
            using (var stream = File.OpenRead(this.RouterDb))
            {
                routerDb = Itinero.RouterDb.Deserialize(stream);
            }
            return new ProcessorFeedFilter()
            {
                Filter = new GTFS.Shapes.Filters.GTFSAddShapesFilter(routerDb)
            };
        }

        /// <summary>
        /// Returns a description of this command.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("--add-shapes routerdb={0}", this.RouterDb);
        }
    }
}