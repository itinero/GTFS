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
    /// Exception thrown when a referred id has not been found.
    /// </summary>
    public class GTFSIntegrityException : GTFSExceptionBase
    {
        /// <summary>
        /// Creates a parsing exception.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public GTFSIntegrityException(string name, string fieldName, string value)
            : base(string.Format("Could not find referenced entity for value {0} in field {1} in file {2}.", value, fieldName, name))
        {
            this.Name = name;
            this.FieldName = fieldName;
            this.Value = value;
        }

        /// <summary>
        /// Returns the name of the file.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the field name of the file.
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Returns the value that could not be parsed.
        /// </summary>
        public string Value { get; private set; }
    }
}