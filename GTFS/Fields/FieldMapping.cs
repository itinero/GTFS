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
using System.Linq;
using System.Text;

namespace GTFS.Fields
{
    /// <summary>
    /// Represents a field map.
    /// </summary>
    public class FieldMap
    {
        /// <summary>
        /// Holds the expected-to-actual-conversion dictionary.
        /// </summary>
        private Dictionary<string, string> _expectedToActual;

        /// <summary>
        /// Holds the actual-to-expected-conversion dictionary.
        /// </summary>
        private Dictionary<string, string> _actualToExpected;

        /// <summary>
        /// Creates a new field map.
        /// </summary>
        internal FieldMap()
        {
            _expectedToActual = new Dictionary<string, string>();
            _actualToExpected = new Dictionary<string, string>();
        }

        /// <summary>
        /// Clears all mapping.
        /// </summary>
        public void Clear()
        {
            _expectedToActual.Clear();
            _actualToExpected.Clear();
        }

        /// <summary>
        /// Adds a new mapping between an expected field in the GFTS standard and the actual field in the feed.
        /// </summary>
        /// <param name="expected">The expected field in the GTFS standard.</param>
        /// <param name="actual">The actual field in the feed.</param>
        public void Add(string expected, string actual)
        {
            _actualToExpected.Add(actual, expected);
            _expectedToActual.Add(expected, actual);
        }

        /// <summary>
        /// Returns the actual field for the expected field according to this map.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public string GetActual(string expected)
        {
            string actual;
            if(_expectedToActual.TryGetValue(expected, out actual))
            {
                return actual;
            }
            return expected;
        }

        /// <summary>
        /// Returns the expected field for the actual field according to this map.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public string GetExpected(string actual)
        {
            string expected;
            if (_expectedToActual.TryGetValue(actual, out expected))
            {
                return expected;
            }
            return actual;
        }
    }
}
