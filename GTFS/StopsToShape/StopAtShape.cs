// The MIT License (MIT)

// Copyright (c) 2015 Ben Abelshausen

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

namespace GTFS.StopsToShape
{
    /// <summary>
    /// Represents the location of a stop in a given trip relative to the route shape.
    /// </summary>
    public class StopAtShape
    {
        /// <summary>
        /// Gets or sets the trip id.
        /// </summary>
        public string TripId { get; set; }

        /// <summary>
        /// Gets or sets the stop id.
        /// </summary>
        public string StopId { get; set; }

        /// <summary>
        /// Gets or sets the shape point sequence.
        /// </summary>
        public uint ShapePointSequence { get; set; }

        /// <summary>
        /// Gets or sets the offset between the shape point at the given sequence and the next in [0-100[%. 
        /// </summary>
        public float StopOffset { get; set; }
    }
}
