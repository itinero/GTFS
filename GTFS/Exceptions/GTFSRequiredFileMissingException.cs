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

namespace GTFS.Exceptions
{
    /// <summary>
    /// Exception thrown when a required file was not found.
    /// </summary>
    public class GTFSRequiredFileMissingException : GTFSExceptionBase
    {
        /// <summary>
        /// Message format used for formatting the exception.
        /// </summary>
        public static readonly string MessageFormat = "Could not find required file {0}.";

        /// <summary>
        /// Creates a missing file exception.
        /// </summary>
        /// <param name="name"></param>
        public GTFSRequiredFileMissingException(string name)
            : base(string.Format(MessageFormat, name))
        {
            this.Name = name;
        }

        /// <summary>
        /// Returns the name of the file.
        /// </summary>
        public string Name { get; private set; }
    }
}
