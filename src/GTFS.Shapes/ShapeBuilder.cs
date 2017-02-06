// The MIT License (MIT)

// Copyright (c) 2017 Ben Abelshausen

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
using Itinero;
using Itinero.Profiles;
using System.Collections.Generic;

namespace GTFS.Shapes
{
    /// <summary>
    /// Builds shapes for a feed based on a routing profile.
    /// </summary>
    public static class ShapeBuilder
    {
        /// <summary>
        /// Builds shapes along the routes in the given feed using the given router and profile.
        /// </summary>
        public static void BuildShapes(IGTFSFeed feed, Router router, IProfileInstance profile)
        {
            var t = 0;
            foreach(var trip in feed.Trips)
            {
                t++;
                GTFS.Logging.Logger.Log("ShapeBuilder", Logging.TraceEventType.Information,
                    "Building for trip {0}...{1}/{2}", trip.ToInvariantString(), t, feed.Trips.Count);
                var stopTimes = new List<StopTime>(feed.StopTimes.GetForTrip(trip.Id));
                stopTimes.Sort((x, y) => x.StopSequence.CompareTo(y.StopSequence));

                var stop1 = feed.Stops.Get(stopTimes[0].StopId);
                var stop1Resolved = router.TryResolve(profile, (float)stop1.Latitude, (float)stop1.Longitude);
                for (var i = 0; i < stopTimes.Count - 1; i++)
                {
                    var stop2 = feed.Stops.Get(stopTimes[i + 1].StopId);
                    var stop2Resolved = router.TryResolve(profile, (float)stop2.Latitude, (float)stop2.Longitude);
                    if (stop1Resolved.IsError || stop2Resolved.IsError)
                    {
                        GTFS.Logging.Logger.Log("ShapeBuilder", Logging.TraceEventType.Error, "Could not determine shape between stops, one of points could not be resolved: {0}->{1}",
                            stop1.Id, stop2.Id);
                    }
                    else
                    {
                        var route = router.TryCalculate(profile, stop1Resolved.Value, stop2Resolved.Value);
                        if (route.IsError)
                        {
                            GTFS.Logging.Logger.Log("ShapeBuilder", Logging.TraceEventType.Error, "Could not determine shape between stops, route couldn't be calculated: {0}->{1}",
                                stop1.Id, stop2.Id);
                        }
                        else
                        {

                        }
                    }

                    stop1 = stop2;
                    stop1Resolved = stop2Resolved;
                }
            }
        }
    }
}