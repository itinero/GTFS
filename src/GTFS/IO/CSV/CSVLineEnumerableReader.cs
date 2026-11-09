using System;
using System.Collections.Generic;

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
                CSVRow.Parse(line, _seperator, ref _current);
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
