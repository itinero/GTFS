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

namespace GTFS.Entities.Enumerations
{
    /// <summary>
    /// The location type identifies whether a stop represents a stop or station.
    /// </summary>
    public enum LocationType
    {
        /// <summary>
        /// A location where passengers board or disembark from a transit vehicle.
        /// </summary>
        Stop = 0,
        /// <summary>
        /// A physical structure or area that contains one or more stop.
        /// </summary>
        Station = 1,
        /// <summary>
        /// A location where passengers can enter or exit a station from the street. If an entrance/exit belongs to multiple stations, it can be linked by pathways to both, but the data provider must pick one of them as parent.
        /// </summary>
        EntranceExit = 2,
        /// <summary>
        /// A location within a station, not matching any other location_type, which can be used to link together pathways define in pathways.txt.
        /// </summary>
        GenericNode = 3,
        /// <summary>
        /// A specific location on a platform, where passengers can board and/or alight vehicles.
        /// </summary>
        BoardingArea = 4
    }
}