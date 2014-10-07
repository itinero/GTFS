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

using GTFS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFS.Tool.Switches.Processors
{
    /// <summary>
    /// Represents a processor that acts as a filter.
    /// </summary>
    public class ProcessorFeedFilter : ProcessorFeedSource
    {     
        /// <summary>
        /// Collapses this processor if possible.
        /// </summary>
        /// <param name="processors"></param>
        public override void Collapse(List<ProcessorBase> processors)
        {
            if (processors == null) { throw new ArgumentNullException("processors"); }
            if (processors.Count == 0) { throw new ArgumentOutOfRangeException("processors", "There has to be at least on processor there to collapse this target."); }
            if (processors[processors.Count - 1] == null) { throw new ArgumentOutOfRangeException("processors", "The last processor in the processors list is null."); }

            // take the last processor and collapse.
            if (processors[processors.Count - 1] is ProcessorFeedSource)
            { // ok, processor is a source.
                var source = processors[processors.Count - 1] as ProcessorFeedSource;
                if (!source.IsReady) { throw new InvalidOperationException("Last processor before filter is a source but it is not ready."); }
                processors.RemoveAt(processors.Count - 1);

                this.Source = source;
                processors.Add(this);
                return;
            }
            throw new InvalidOperationException("Last processor before filter is not a source.");
        }

        /// <summary>
        /// Gets or sets the feed filter.
        /// </summary>
        public GTFSFeedFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets the source for this filter.
        /// </summary>
        public ProcessorFeedSource Source { get; set; }

        /// <summary>
        /// Returns true if this filter is ready.
        /// </summary>
        public override bool IsReady
        {
            get { return this.Source != null &&
                this.Filter != null; }
        }

        /// <summary>
        /// Returns the feed for this source.
        /// </summary>
        /// <returns></returns>
        public override IGTFSFeed GetFeed()
        {
            // read feed.
            var feed = Source.GetFeed();

            // execute filter.
            return this.Filter.Filter(feed);
        }

        /// <summary>
        /// Executes this processor.
        /// </summary>
        public override void Execute()
        {
            throw new InvalidOperationException("Cannot execute processor, check CanExecute.");
        }

        /// <summary>
        /// Returns true if this processor can execute.
        /// </summary>
        public override bool CanExecute
        {
            get { return false; }
        }
    }
}
