using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTFS.IO.CSV
{
    /// <summary>
    /// Wraps an IEnumerable containing line strings are a CSV reader.
    /// </summary>
    public class CSVLineEnumerableReader : ICSVReader
    {
        /// <summary>
        /// Holds the lines.
        /// </summary>
        private readonly IEnumerable<string> _lines;

        /// <summary>
        /// Holds the serperator char.
        /// </summary>
        private char _seperator = ',';

        /// <summary>
        /// Creates a new CSV stream.
        /// </summary>
        /// <param name="lines">The lines to read from.</param>
        public CSVLineEnumerableReader(IEnumerable<string> lines)
        {
            _lines = lines;
        }

        /// <summary>
        /// Creates a new CSV stream.
        /// </summary>
        /// <param name="lines">The lines to read from.</param>
        /// <param name="seperator">A custom seperator.</param>
        public CSVLineEnumerableReader(IEnumerable<string> lines, char seperator)
        {
            _lines = lines;
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
                if(_current == null)
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

        }

        /// <summary>
        /// Returns the current line.
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        /// <summary>
        /// Holds the enumerator.
        /// </summary>
        private IEnumerator<string> _enumerator = null;

        /// <summary>
        /// Move to the next line.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if(_enumerator == null)
            {
                _enumerator = _lines.GetEnumerator();
            }
            if (_enumerator.MoveNext())
            {
                string line = _enumerator.Current;
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
                for (int charIdx = 0; charIdx < line.Length; charIdx++)
                {
                    var curChar = line[charIdx];
                    if (curChar == '"')
                    { // do nothing when in between quotes.
                        between = !between;
                    }
                    else if (!between && curChar == _seperator)
                    { // not between quotes and a seperator means a new column.
                        if (idx >= _current.Length)
                        { // this is extremely ineffecient but should almost never happen except when parsing invalid feeds.
                            Array.Resize(ref _current, _current.Length + 1);
                        }
                        _current[idx] = line.Substring(previousCharIdx, charIdx - previousCharIdx);
                        idx++;
                        previousCharIdx = charIdx + 1;
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
            _enumerator = null;
            _current = null; // reset current data.
        }
    }
}
