// The MIT License (MIT)

// Copyright (c) 2014 Ben Abelshausen

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

using GTFS.Tool.Switches.Processors;

namespace GTFS.Tool.Switches
{
    /// <summary>
    /// Represents a switch that points to the writing a feed functionality.
    /// </summary>
    public class SwitchWriteFeed : Switch
    {
        /// <summary>
        /// Returns the switch id's.
        /// </summary>
        /// <returns></returns>
        public override string[] GetSwitch()
        {
            return new string[] { "--wf", "--write-feed" };
        }

        /// <summary>
        /// The path to write from.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Parse the command arguments for the write-feed command.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="idx"></param>
        /// <param name="switchOut"></param>
        /// <returns></returns>
        public override int Parse(string[] args, int idx, out Switch switchOut)
        {
            // check next argument.
            if (args.Length < idx)
            {
                throw new SwitchParserException("None", "Invalid path name for write-feed command!");
            }

            // everything ok, take the next argument as the path.
            switchOut = new SwitchWriteFeed()
            {
                Path = args[idx]
            };
            return 1;
        }

        /// <summary>
        /// Creates the processor that corresponds to this command.
        /// </summary>
        /// <returns></returns>
        public override ProcessorBase CreateProcessor()
        {
            return new ProcessorWriteFeed(this.Path);
        }

        /// <summary>
        /// Returns a description of this command.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("--write-feed {0}", this.Path);
        }
    }
}
