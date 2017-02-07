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
using GTFS.Shapes.Caches;
using Itinero;
using Itinero.LocalGeo;
using Itinero.Profiles;
using System.Collections.Generic;
using System.Linq;

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
        public static void BuildShapes(IGTFSFeed feed, Router router, IProfileInstance profile, bool useTripCache = true,
            bool useStopCache = false)
        {
            // initialize a caching structure of shapes.
            ShapesCache<StopPair> stopShapeCache = null;
            if (useStopCache)
            {
                stopShapeCache = new ShapesCache<StopPair>();
            }
            ShapesCache<TripStops> tripShapeCache = null;
            if (useTripCache)
            {
                tripShapeCache = new ShapesCache<TripStops>();
            }

            // build a shape per trip.
            var t = 0;
            var shapeId = 0;
            var tripIds = new List<string>(feed.Trips.Select(x => x.Id));
            foreach(var tripId in tripIds)
            {
                var trip = feed.Trips.Get(tripId);
                t++;
                GTFS.Logging.Logger.Log("ShapeBuilder", Logging.TraceEventType.Information,
                    "Building for trip {0}...{1}/{2}", trip.ToInvariantString(), t, feed.Trips.Count);
                var stopTimes = new List<StopTime>(feed.StopTimes.GetForTrip(trip.Id));
                stopTimes.Sort((x, y) => x.StopSequence.CompareTo(y.StopSequence));

                // check trip cache.
                var tripStops = new TripStops();
                tripStops.Stops = new List<string>();
                for (var i = 0; i < stopTimes.Count; i++)
                {
                    tripStops.Stops.Add(stopTimes[i].StopId);
                }

                // get trip shape from cache or build it from scratch.
                var shape = new List<Coordinate>();
                IEnumerable<Coordinate> cachedTripShape;
                if (tripShapeCache != null && tripShapeCache.TryGet(tripStops, out cachedTripShape))
                {
                    shape.AddRange(cachedTripShape);
                }
                else
                {
                    // build shape.
                    var stop1 = feed.Stops.Get(stopTimes[0].StopId);
                    var stop1Coordinate = new Coordinate((float)stop1.Latitude, (float)stop1.Longitude);
                    var stop1Resolved = router.TryResolve(profile, stop1Coordinate);
                    stopTimes[0].ShapeDistTravelled = 0;
                    feed.StopTimes.AddOrReplace(stopTimes[0]);
                    for (var i = 0; i < stopTimes.Count - 1; i++)
                    {
                        // calculate shape between two stops.
                        var localShape = new List<Coordinate>();
                        var stop2 = feed.Stops.Get(stopTimes[i + 1].StopId);
                        var stop2Coordinate = new Coordinate((float)stop2.Latitude, (float)stop2.Longitude);
                        var stop2Resolved = router.TryResolve(profile, stop2Coordinate);

                        // check cache.
                        var stopPair = new StopPair()
                        {
                            Stop1 = stop1.Id,
                            Stop2 = stop2.Id
                        };
                        IEnumerable<Coordinate> localCachedShape;
                        if (stopShapeCache != null && stopShapeCache.TryGet(stopPair, out localCachedShape))
                        { // shape is in cache.
                            localShape.AddRange(localCachedShape);
                        }
                        else
                        { // shape not in cache.
                            if (stop1Resolved.IsError || stop2Resolved.IsError)
                            {
                                GTFS.Logging.Logger.Log("ShapeBuilder", Logging.TraceEventType.Error, "Could not determine shape between stops, one of points could not be resolved: {0}->{1}",
                                    stop1.Id, stop2.Id);
                                localShape.Add(stop1Coordinate);
                                localShape.Add(stop2Coordinate);
                            }
                            else
                            {
                                var route = router.TryCalculate(profile, stop1Resolved.Value, stop2Resolved.Value);
                                if (route.IsError)
                                {
                                    GTFS.Logging.Logger.Log("ShapeBuilder", Logging.TraceEventType.Error, "Could not determine shape between stops, route couldn't be calculated: {0}->{1}",
                                        stop1.Id, stop2.Id);
                                    localShape.Add(stop1Coordinate);
                                    localShape.Add(stop2Coordinate);
                                }
                                else
                                {
                                    localShape.AddRange(route.Value.Shape);
                                }
                            }

                            // add to cache.
                            if (stopShapeCache != null)
                            {
                                stopShapeCache.Add(stopPair, localShape);
                            }
                        }

                        // calculate distance and add to main shape.
                        var localDistance = Coordinate.DistanceEstimateInMeter(localShape);
                        if (i > 0)
                        {
                            shape.AddRange(localShape.GetRange(1, localShape.Count - 1));
                        }
                        else
                        {
                            shape.AddRange(localShape);
                        }

                        // move to next pair.
                        stop1 = stop2;
                        stop1Coordinate = stop2Coordinate;
                        stop1Resolved = stop2Resolved;
                    }

                    // add to cache.
                    if (tripShapeCache != null)
                    {
                        tripShapeCache.Add(tripStops, shape);
                    }
                }

                // build shape objects.
                var distance = 0f;
                for (var i = 0; i < shape.Count - 1; i++)
                {
                    feed.Shapes.Add(new Shape()
                    {
                        Id = shapeId.ToInvariantString(),
                        DistanceTravelled = distance,
                        Sequence = (uint)i,
                        Latitude = shape[i].Latitude,
                        Longitude = shape[i].Longitude
                    });

                    distance += Coordinate.DistanceEstimateInMeter(shape[i + 0], shape[i + 1]);
                }
                trip.ShapeId = shapeId.ToInvariantString();
                feed.Trips.AddOrReplace(trip, (tr) => tr.Id);
                shapeId++;
            }
        }

        private class StopPair
        {
            /// <summary>
            /// Gets or sets the stop1.
            /// </summary>
            public string Stop1 { get; set; }

            /// <summary>
            /// Gets or sets the stop2.
            /// </summary>
            public string Stop2 { get; set; }

            /// <summary>
            /// Gets the hashcode.
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.Stop1.GetHashCode() ^
                    this.Stop2.GetHashCode();
            }

            /// <summary>
            /// Returns true if the given object has the same id's.
            /// </summary>
            public override bool Equals(object obj)
            {
                var other = (obj as StopPair);
                if (other == null)
                {
                    return false;
                }
                return other.Stop1.Equals(this.Stop1) &&
                    other.Stop2.Equals(this.Stop2);
            }
        }

        private class TripStops
        {
            /// <summary>
            /// Gets or sets the stops sequence.
            /// </summary>
            public List<string> Stops { get; set; }

            /// <summary>
            /// Gets the hashcode.
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                var hash = this.Stops.Count.GetHashCode();
                for (var i = 0; i < this.Stops.Count; i++)
                {
                    hash = hash ^ this.Stops[i].GetHashCode();
                }
                return hash;
            }

            /// <summary>
            /// Returns true if the given object has the same id's.
            /// </summary>
            public override bool Equals(object obj)
            {
                var other = (obj as TripStops);
                if (other == null)
                {
                    return false;
                }
                if (other.Stops.Count != this.Stops.Count)
                {
                    return false;
                }
                for (var i = 0; i < this.Stops.Count; i++)
                {
                    if (!this.Stops[i].Equals(other.Stops[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}