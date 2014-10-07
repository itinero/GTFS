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
using System.Linq;

namespace GTFS.Tool.Switches
{
    /// <summary>
    /// Represents a switch that is used to point to a certain functionality.
    /// </summary>
    public abstract class Switch
    {
        /// <summary>
        /// Gets the switch id's.
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetSwitch();

        /// <summary>
        /// Creates a new processor that corresponds with the action in this command.
        /// </summary>
        /// <returns></returns>
        public abstract ProcessorBase CreateProcessor();

        /// <summary>
        /// Returns a description of this command.
        /// </summary>
        /// <remarks>Forces all implementations of Command to implement a description.</remarks>
        /// <returns></returns>
        public abstract override string ToString();

        /// <summary>
        /// Parses string arguments into an actual command object.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="idx"></param>
        /// <param name="switchOut"></param>
        /// <returns></returns>
        public abstract int Parse(string[] args, int idx, out Switch switchOut);

        /// <summary>
        /// Returns true if the given switch points to this command.
        /// </summary>
        /// <param name="switchString"></param>
        /// <returns></returns>
        public virtual bool HasSwitch(string switchString)
        {
            return this.GetSwitch().Contains(switchString);
        }
    }
}
