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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GTFS.IO.CSV
{
    /// <summary>
    /// Holds a CSV stream reader.
    /// </summary>
    public class CSVStreamReader : ICSVReader
    {
        /// <summary>
        /// Holds the stream reader.
        /// </summary>
        private readonly StreamReader _stream;

        /// <summary>
        /// Holds the serperator char.
        /// </summary>
        private char _seperator = ',';

        /// <summary>
        /// Creates a new CSV stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public CSVStreamReader(Stream stream)
        {
            _stream = new StreamReader(stream, new UTF8Encoding());
        }

        /// <summary>
        /// Creates a new CSV stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="seperator">A custom seperator.</param>
        public CSVStreamReader(Stream stream, char seperator)
        {
            _stream = new StreamReader(stream, new UTF8Encoding());
            _seperator = seperator;
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
                if (_current == null)
                {
                    throw new InvalidOperationException("No current data available, use MoveNext() to move to the first line or Reset() to reset the reader.");
                }
                return _current;
            }
        }

        /// <summary>
        /// Gets or sets the line preprocessor.
        /// </summary>
        public Func<string, string> LinePreprocessor { get; set; }

        /// <summary>
        /// Disposes of all resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            _stream.Dispose();
        }


        /// <summary>
        /// Returns the current line.
        /// </summary>
        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        /// <summary>
        /// Move to the next line.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (_stream.Peek() > -1)
            {
                string line = _stream.ReadLine();
                if (this.LinePreprocessor != null)
                {
                    line = this.LinePreprocessor.Invoke(line);
                }
                if (_current == null)
                { // overestimate column count, one resize per file.
                    _current = new string[20];
                }
                int idx = 0;
                bool between = false;
                int previousCharIdx = 0;
                var chars = new List<char>();
                for (int charIdx = 0; charIdx < line.Length; charIdx++)
                {
                    var curChar = line[charIdx];
                    if (curChar == '"')
                    {
                        var nextCharIdx = charIdx + 1;
                        if (nextCharIdx < line.Length)
                        {
                            var nextChar = line[nextCharIdx];
                            if (nextChar == '"')
                            {
                                chars.Add('"');
                                charIdx = nextCharIdx;
                                continue;
                            }
                        }

                        // do nothing when in between quotes.
                        between = !between;
                    }
                    else if (!between && curChar == _seperator)
                    { // not between quotes and a seperator means a new column.
                        if (idx >= _current.Length)
                        { // this is extremely ineffecient but should almost never happen except when parsing invalid feeds.
                            Array.Resize(ref _current, _current.Length + 1);
                        }
                        _current[idx] = new string(chars.ToArray());
                        // _current[idx] = line.Substring(previousCharIdx, charIdx - previousCharIdx);
                        chars.Clear();
                        idx++;
                        previousCharIdx = charIdx + 1;
                    }
                    else
                    { // keep char list.
                        chars.Add(curChar);
                    }
                }
                if (idx >= _current.Length)
                { // this is extremely ineffecient but should almost never happen except when parsing invalid feeds.
                    Array.Resize(ref _current, _current.Length + 1);
                }
                _current[idx] = line.Substring(previousCharIdx, line.Length - previousCharIdx);
                if (_current.Length > idx + 1)
                { // current array is too long.
                    // this is extremely ineffecient but should almost never happen except when parsing invalid feeds.
                    Array.Resize(ref _current, idx + 1);
                }
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