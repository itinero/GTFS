using GTFS.IO.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTFS.IO
{
    /// <summary>
    /// Represents a GTFS source file wrapping an enumerable of lines.
    /// </summary>
    public class GTFSSourceFileLines : IGTFSSourceFile
    {
        /// <summary>
        /// Holds the lines.
        /// </summary>
        private IEnumerable<string> _lines;

        /// <summary>
        /// Holds a custom seperator.
        /// </summary>
        private char? _customSeperator;

        /// <summary>
        /// Creates a new GTFS file stream.
        /// </summary>
        /// <param name="lines">The lines to read from.</param>
        /// <param name="name">The name associated with this file stream.</param>
        public GTFSSourceFileLines(IEnumerable<string> lines, string name)
        {
            _lines = lines;
            this.Name = name;
            _customSeperator = null;
        }

        /// <summary>
        /// Creates a new GTFS file stream.
        /// </summary>
        /// <param name="lines">The lines to read from.</param>
        /// <param name="name">The name associated with this file stream.</param>
        /// <param name="seperator">A custom seperator.</param>
        public GTFSSourceFileLines(IEnumerable<string> lines, string name, char seperator)
        {
            _lines = lines;
            this.Name = name;
            _customSeperator = seperator;
        }

        /// <summary>
        /// Gets or sets the line preprocessor.
        /// </summary>
        public Func<string, string> LinePreprocessor { get; set; }

        /// <summary>
        /// Gets the name of this file.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Holds the current reader.
        /// </summary>
        private CSVLineEnumerableReader _reader;

        /// <summary>
        /// Requests a new enumerator.
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerator<string[]> GetEnumerator()
        {
            if(_reader != null)
            {
                throw new InvalidOperationException("A CSVStreamReader can only spawn one enumerator.");
            }
            if (_customSeperator.HasValue)
            { // create reader with custom seperator.
                _reader = new CSVLineEnumerableReader(_lines, _customSeperator.Value);
            }
            else
            { // no seperator here!
                _reader = new CSVLineEnumerableReader(_lines);
            }

            _reader.LinePreprocessor = this.LinePreprocessor;
            return _reader;
        }

        /// <summary>
        /// Requests a new enumerator.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Disposes of all native resources associated with this source file.
        /// </summary>
        public void Dispose()
        {
            if(_reader != null)
            {
                _reader.Dispose();
            }
        }
    }
}
