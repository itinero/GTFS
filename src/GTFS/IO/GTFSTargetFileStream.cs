// The MIT License (MIT)

// Copyright (c) 2016 Ben Abelshausen

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

using GTFS.IO.CSV;
using System.IO;

namespace GTFS.IO
{
    /// <summary>
    /// Represents a GTFS file target.
    /// </summary>
    public class GTFSTargetFileStream : IGTFSTargetFile
    {
        /// <summary>
        /// Holds the name.
        /// </summary>
        private string _name;

        /// <summary>
        /// Holds the CSV stream writer.
        /// </summary>
        private CSVStreamWriter _streamWriter;

        /// <summary>
        /// Creates a new target file stream.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        public GTFSTargetFileStream(Stream stream, string name)
        {
            _name = name;

            _streamWriter = new CSVStreamWriter(stream);
        }

        /// <summary>
        /// Creates a new target file stream.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <param name="seperator"></param>
        public GTFSTargetFileStream(Stream stream, string name, char seperator)
        {
            _name = name;

            _streamWriter = new CSVStreamWriter(stream, seperator);
        }

        /// <summary>
        /// Returns the name of this target.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns true if this file exists.
        /// </summary>
        public bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Clears all content.
        /// </summary>
        public void Clear()
        {

        }

        /// <summary>
        /// Writes another line of data.
        /// </summary>
        /// <param name="data"></param>
        public void Write(string[] data)
        {
            _streamWriter.Write(data);
        }

        /// <summary>
        /// Closes this target.
        /// </summary>
        public void Close()
        {
            _streamWriter.Flush();
            _streamWriter.Dispose();
        }
    }
}