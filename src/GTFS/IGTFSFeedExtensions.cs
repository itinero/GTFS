// The MIT License (MIT)

// Copyright (c) 2016 Ben Abelshausen

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

using GTFS.Entities;
using System;
using System.Collections.Generic;

namespace GTFS
{
    /// <summary>
    /// Contains extension methods for IGTFSFeed
    /// </summary>
    public static class IGTFSFeedExtensions
    {
        /// <summary>
        /// Returns all stops that statisfy the given filter.
        /// </summary>
        /// <returns></returns>
        public static HashSet<string> GetStopsFor(this IGTFSFeed feed, Func<Stop, bool> stopFilter)
        {
            // collect stopids.
            var stopIds = new HashSet<string>();
            foreach (var stop in feed.Stops)
            {
                if (stopFilter.Invoke(stop))
                { // stop has to be included.
                    stopIds.Add(stop.Id);
                }
            }
            return stopIds;
        }

        /// <summary>
        /// Returns all trips along the stops that statisfy the given filter.
        /// </summary>
        /// <returns></returns>
        public static HashSet<string> GetTripsFor(this IGTFSFeed feed, Func<Stop, bool> stopFilter)
        {
            // collect stopids.
            var stopIds = feed.GetStopsFor(stopFilter);

            // collect tripid's.
            var tripIds = new HashSet<string>();
            foreach (var stopTime in feed.StopTimes)
            {
                if (stopIds.Contains(stopTime.StopId))
                { // save the trip id's to keep.
                    tripIds.Add(stopTime.TripId);
                }
            }
            return tripIds;
        }

        /// <summary>
        /// Returns all routes along the stops that statisfy the given filter.
        /// </summary>
        /// <returns></returns>
        public static HashSet<string> GetRoutesFor(this IGTFSFeed feed, Func<Stop, bool> stopFilter)
        {
            // collect tripid's.
            var tripIds = feed.GetTripsFor(stopFilter);

            // collect routeid's.
            var routeIds = new HashSet<string>();
            foreach (var trip in feed.Trips)
            {
                if (tripIds.Contains(trip.Id))
                {
                    routeIds.Add(trip.RouteId);
                }
            }
            return routeIds;
        }
    }
}