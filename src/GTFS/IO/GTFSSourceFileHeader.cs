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

using System.Collections.Generic;

namespace GTFS.IO
{
    /// <summary>
    /// Defines a GTFS source file header.
    /// </summary>
    public class GTFSSourceFileHeader
    {    
        /// <summary>
        /// Holds the reverse index of the column names.
        /// </summary>
        private Dictionary<string, int> _indexPerColumn;

        /// <summary>
        /// Holds the column names.
        /// </summary>
        private string[] _columns;

        /// <summary>
        /// Creates a new header.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columns"></param>
        public GTFSSourceFileHeader(string name, string[] columns)
        {
            this.Name = name;
            _columns = new string[columns.Length];

            _indexPerColumn = new Dictionary<string, int>(columns.Length);
            for (int idx = 0; idx < _columns.Length; idx++)
            {
                // create a deep copy of the columns array.
                _columns[idx] = columns[idx];
                // reverse-index the columns.
                _indexPerColumn[_columns[idx]] = idx;
            }
        }

        /// <summary>
        /// Gets the name of the file this header represents.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the column index for given column.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public int GetColumnIndex(string column)
        {
            int value = -1;
            if(!_indexPerColumn.TryGetValue(column, out value))
            { // column not there!
                return -1;
            }
            return value;
        }

        /// <summary>
        /// Returns true if the column with the given name is in this header definition.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool HasColumn(string column)
        {
            return this.GetColumnIndex(column) > -1;
        }

        /// <summary>
        /// Returns the column for the given index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string GetColumn(int idx)
        {
            return _columns[idx];
        }
    }
}