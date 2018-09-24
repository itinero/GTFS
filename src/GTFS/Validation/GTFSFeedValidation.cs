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

using GTFS.Entities;
using System;
using System.Collections.Generic;

namespace GTFS.Validation
{
    /// <summary>
    /// Contains code to validate GTFS feed.
    /// </summary>
    public class GTFSFeedValidation
    {
        /// <summary>
        /// Validates a GTFS feed.
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public static bool Validate(IGTFSFeed feed)
        {
            var messages = string.Empty;
            return GTFSFeedValidation.Validate(feed, out messages);
        }

        /// <summary>
        /// Validates a GTFS feed.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static bool Validate(IGTFSFeed feed, out string messages)
        {
            // check agencies.
            var agencyIds = new HashSet<string>();
            foreach (var agency in feed.Agencies)
            {
                if (agencyIds.Contains(agency.Id))
                { // oeps, duplicate id.
                    messages = string.Format("Duplicate agency id found: {0}", agency.Id);
                    return false;
                }
                agencyIds.Add(agency.Id);
            }

            // check stops.
            var stopIds = new HashSet<string>();
            foreach(var stop in feed.Stops)
            {
                if (stopIds.Contains(stop.Id))
                { // oeps, duplicate id.
                    messages = string.Format("Duplicate stop id found: {0}", stop.Id);
                    return false;
                }
                stopIds.Add(stop.Id);
            }

            // check routes.
            var routeIds = new HashSet<string>();
            foreach(var route in feed.Routes)
            {
                if (routeIds.Contains(route.Id))
                { // oeps, duplicate id.
                    messages = string.Format("Duplicate route id found: {0}", route.Id);
                    return false;
                }
                routeIds.Add(route.Id);
                if (route.AgencyId != null && !agencyIds.Contains(route.AgencyId))
                {// oeps, unknown id.
                    messages = string.Format("Unknown agency found in route {0}: {1}", route.Id, route.AgencyId);
                    return false;
                }
            }

            // check trips for routes.
            var tripIds = new HashSet<string>();
            foreach(var trip in feed.Trips)
            {
                if (tripIds.Contains(trip.Id))
                { // oeps, duplicate id.
                    messages = string.Format("Duplicate trip id found: {0}", trip.Id);
                    return false;
                }
                tripIds.Add(trip.Id);
                if (!routeIds.Contains(trip.RouteId))
                { // oeps, unknown id.
                    messages = string.Format("Unknown route found in trip {0}: {1}", trip.Id, trip.RouteId);
                    return false;
                }
            }

            // check stop times.
            var stopTimesIndex = new Dictionary<string, List<StopTime>>();
            var stopTimes = new HashSet<Tuple<string, uint>>();
            foreach(var stopTime in feed.StopTimes)
            {
                var stopTimeId = new Tuple<string, uint>(stopTime.TripId, stopTime.StopSequence);
                if (stopTimes.Contains(stopTimeId))
                { // oeps, duplicate id.
                    messages = string.Format("Duplicate stop_time entry found: {0}", stopTime.TripId, stopTime.StopSequence);
                    return false;
                }
                stopTimes.Add(stopTimeId);

                List<StopTime> stopTimeList;
                if (!stopTimesIndex.TryGetValue(stopTime.TripId, out stopTimeList))
                { 
                    stopTimeList = new List<StopTime>();
                    stopTimesIndex.Add(stopTime.TripId, stopTimeList);
                }
                stopTimeList.Add(stopTime);

                if (!stopIds.Contains(stopTime.StopId))
                { // oeps, unknown id.
                    messages = string.Format("Unknown stop found in stop_time {0}: {1}", stopTime.StopId, stopTime.StopId);
                    return false;
                }
                if (!tripIds.Contains(stopTime.TripId))
                { // oeps, unknown id.
                    messages = string.Format("Unknown trip found in stop_time {0}: {1}", stopTime.StopId, stopTime.TripId);
                    return false;
                }
            }

            // check all sequences.
            foreach (var stopTimesPair in stopTimesIndex)
            {
                uint current = 0, previous;
                foreach (var stopTime in stopTimesPair.Value)
                {
                    previous = current;
                    current = stopTime.StopSequence;
                    if (previous != 0)
                    {
                        if (previous >= current)
                        {
                            messages = string.Format("Stop sequences values shall increase and be unique in stop_times file for trip id {0}.", 
                                stopTimesPair.Key);
                            return false;
                        }
                    }
                }
            }
            messages = string.Empty;
            return true;
        }
    }
}