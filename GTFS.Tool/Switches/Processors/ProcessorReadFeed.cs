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

using GTFS.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace GTFS.Tool.Switches.Processors
{
    /// <summary>
    /// Represents a processor that reads a feed from disk.
    /// </summary>
    public class ProcessorReadFeed : ProcessorFeedSource
    {
        /// <summary>
        /// Holds the path.
        /// </summary>
        private string _path;

        /// <summary>
        /// Creates a new feed source.
        /// </summary>
        /// <param name="path"></param>
        public ProcessorReadFeed(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Collapses the processors if possible.
        /// </summary>
        /// <param name="processors"></param>
        public override void Collapse(List<ProcessorBase> processors)
        {
            processors.Add(this);
        }

        /// <summary>
        /// Returns true if this reader is ready.
        /// </summary>
        public override bool IsReady
        {
            get { return true; }
        }

        /// <summary>
        /// Executes this processor.
        /// </summary>
        public override void Execute()
        {
            throw new InvalidOperationException("Cannot execute processor, check CanExecute.");
        }

        /// <summary>
        /// Returns true if this processor can be executed.
        /// </summary>
        public override bool CanExecute
        {
            get { return false; ; }
        }

        /// <summary>
        /// Returns the feed produced by this reader.
        /// </summary>
        /// <returns></returns>
        public override IGTFSFeed GetFeed()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>(false);

            // execute the reader.
            return reader.Read(new GTFSDirectorySource(new DirectoryInfo(_path)));
        }
    }
}