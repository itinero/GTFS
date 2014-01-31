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

namespace GTFS.Core.IO
{
    /// <summary>
    /// Represents a GTFS-source.
    /// </summary>
    public interface IGTFSSource
    {
        /// <summary>
        /// Returns the agency file.
        /// </summary>
        IGTFSSourceFile AgencyFile { get; }

        /// <summary>
        /// Returns the calendar file.
        /// </summary>
        IGTFSSourceFile CalendarFile { get; }

        /// <summary>
        /// Returns the calendar data file.
        /// </summary>
        IGTFSSourceFile CalendarDateFile { get; }

        /// <summary>
        /// Returns the fare attribute file.
        /// </summary>
        IGTFSSourceFile FareAttributeFile { get; }

        /// <summary>
        /// Returns the fare rule file.
        /// </summary>
        IGTFSSourceFile FareRuleFile { get; }

        /// <summary>
        /// Returns the feed info file.
        /// </summary>
        IGTFSSourceFile FeedInfoFile { get; }

        /// <summary>
        /// Returns the frequency file.
        /// </summary>
        IGTFSSourceFile FrequencyFile { get; }

        /// <summary>
        /// Returns the route file.
        /// </summary>
        IGTFSSourceFile RouteFile { get; }

        /// <summary>
        /// Returns the shape file.
        /// </summary>
        IGTFSSourceFile ShapeFile { get; }

        /// <summary>
        /// Returns the stop file.
        /// </summary>
        IGTFSSourceFile StopFile { get; }

        /// <summary>
        /// Returns the stop time file.
        /// </summary>
        IGTFSSourceFile StopTimeFile { get; }

        /// <summary>
        /// Returns the transfer file.
        /// </summary>
        IGTFSSourceFile TransferFile { get; }

        /// <summary>
        /// Returns the trip file.
        /// </summary>
        IGTFSSourceFile TripFile { get; }
    }
}