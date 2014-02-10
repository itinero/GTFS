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
using System.Collections.Generic;
using System.IO;

namespace GTFS.IO.CSV
{
    /// <summary>
    /// Holds a CSV stream reader.
    /// </summary>
    public class CSVStreamReader : IEnumerator<string[]>
    {
        /// <summary>
        /// Holds the stream reader.
        /// </summary>
        private readonly StreamReader _stream;

        /// <summary>
        /// Holds the serperator char.
        /// </summary>
        private readonly char _seperator = ',';

        /// <summary>
        /// Creates a new CSV stream.
        /// </summary>
        /// <param name="stream"></param>
        public CSVStreamReader(Stream stream)
        {
            _stream = new StreamReader(stream);
        }

        /// <summary>
        /// Holds the current line.
        /// </summary>
        private string[] _current = null;

        /// <summary>
        /// Returns the current line.
        /// </summary>
        public string[] Current
        {
            get
            {
                if(_current == null)
                {
                    throw new InvalidOperationException("No current data available, use MoveNext() to move to the first line or Reset() to reset the reader.");
                }
                return _current;
            }
        }

        /// <summary>
        /// Disposes of all resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            _stream.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Move to the next line.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if(_stream.Peek() > -1)
            {
                string line = _stream.ReadLine();
                _current = line.Split(_seperator);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resets this enumerator.
        /// </summary>
        public void Reset()
        {
            if (!_stream.BaseStream.CanSeek) { throw new NotSupportedException("Resetting a CSVStreamReader encapsulating an unseekable stream is not supported! Make sure the stream is seekable."); }

            // move the base-stream back to the beginning.
            _stream.BaseStream.Seek(0, SeekOrigin.Begin);
            _current = null; // reset current data.
        }
    }
}