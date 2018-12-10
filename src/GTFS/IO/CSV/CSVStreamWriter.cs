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

using System;
using System.IO;
using System.Text;

namespace GTFS.IO.CSV
{
    /// <summary>
    /// Holds a CSV stream reader.
    /// </summary>
    public class CSVStreamWriter : ICSVWriter
    {
        /// <summary>
        /// Holds the stream reader.
        /// </summary>
        private readonly StreamWriter _stream;

        /// <summary>
        /// Holds the serperator char.
        /// </summary>
        private readonly char _seperator = ',';

        /// <summary>
        /// Creates a new CSV stream.
        /// </summary>
        /// <param name="stream"></param>
        public CSVStreamWriter(Stream stream)
        {
            _stream = new StreamWriter(stream, new UTF8Encoding());
        }

        /// <summary>
        /// Creates a new CSV stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="seperator"></param>
        public CSVStreamWriter(Stream stream, char seperator)
        {
            _stream = new StreamWriter(stream, new UTF8Encoding());
            _seperator = seperator;
        }

        /// <summary>
        /// Writes one line into the target csv-stream.
        /// </summary>
        /// <param name="line"></param>
        public void Write(string[] line)
        {
            if (line == null) { throw new ArgumentNullException("line"); }

            if (line.Length == 0)
            { // not data, just write an empty line.
                _stream.WriteLine();
                return;
            }

            for (int idx = 0; idx < line.Length - 1; idx++)
            {
                _stream.Write(line[idx]);
                _stream.Write(_seperator);
            }
            _stream.WriteLine(line[line.Length - 1]);
        }

        /// <summary>
        /// Flushes this writer.
        /// </summary>
        public void Flush()
        {
            _stream.Flush();
        }

        /// <summary>
        /// Disposes of all resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}