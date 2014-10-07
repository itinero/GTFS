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

using System.Collections.Generic;

namespace GTFS.Tool.Switches.Processors
{
    /// <summary>
    /// Base class for processor created by a switch.
    /// </summary>
    public abstract class ProcessorBase
    {
        /// <summary>
        /// Collapses the given list of processors by adding this one to it.
        /// </summary>
        /// <param name="processors"></param>
        /// <remarks>This means for example taking a source and filter and converting them to a new source.</remarks>
        public abstract void Collapse(List<ProcessorBase> processors);

        /// <summary>
        /// Returns true if this processor is ready.
        /// </summary>
        /// <remarks>This means this is either a native source, a filter that has been collapsed and has a source, or... in short this processor is ready for business.</remarks>
        public abstract bool IsReady
        {
            get;
        }

        /// <summary>
        /// Executes the tasks in this processor.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Returns true if this processor can be executed.
        /// </summary>
        /// <remarks>Different from IsReady. A source for example can be ready but cannot be executed by itself.</remarks>
        public abstract bool CanExecute
        {
            get;
        }
    }
}
