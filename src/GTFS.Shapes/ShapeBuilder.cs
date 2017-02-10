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
using Itinero.Algorithms.Collections;
using Itinero.LocalGeo;
using Itinero.Profiles;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GTFS.Shapes
{
    /// <summary>
    /// Builds shapes for a feed based on a routing profile.
    /// </summary>
    public class ShapeBuilder
    {
        private readonly int _maxResolveDistanceInMeter;

        /// <summary>
        /// Creates a new shape builder.
        /// </summary>
        public ShapeBuilder(int maxResolveDistanceInMeter = 200)
        {
            _maxResolveDistanceInMeter = maxResolveDistanceInMeter;
        }
        
        /// <summary>
        /// Called when a stop could not be resolved.
        /// </summary>
        public Action<Stop, Coordinate, Result<RouterPoint>> StopNotResolved;

        /// <summary>
        /// Called when a shape between stops could not be routed.
        /// </summary>
        public Action<Stop, Stop> ShapeNotRoutable;

        /// <summary>
        /// Builds shapes along the routes in the given feed using the given router and profile.
        /// </summary>
        public void BuildShapes(IGTFSFeed feed, Router router, Func<Trip, IProfileInstance> getProfile, bool useStopCache = true)
        {
            // initialize a caching structure of shapes.
            ShapesCache<StopPair> stopShapeCache = null;
            if (useStopCache)
            {
                stopShapeCache = new ShapesCache<StopPair>();
            }
            var shapeCache = new HugeDictionary<TripStops, string>();

            // build a shape per trip.
            var shapeId = 0;
            var stopTimes = new List<int>(feed.StopTimes.Count);
            for (var i = 0; i < feed.StopTimes.Count; i++)
            {
                stopTimes.Add(i);
            }
            stopTimes.Sort((x, y) =>
            {
                var xStopTime = feed.StopTimes[x];
                var yStopTime = feed.StopTimes[y];
                if (xStopTime.TripId == yStopTime.TripId)
                {
                    return xStopTime.StopSequence.CompareTo(yStopTime.StopSequence);
                }
                return xStopTime.TripId.CompareTo(yStopTime.TripId);
            });
            var trips = new List<Trip>(feed.Trips);
            trips.Sort((x, y) => x.Id.CompareTo(y.Id));
            var tripStopTimes = new List<int>();
            var tripStopTimesIndex = 0;
            for (var t = 0; t < trips.Count; t++)
            {
                var trip = trips[t];
                var profile = getProfile(trip);
                GTFS.Logging.Logger.Log("ShapeBuilder", Logging.TraceEventType.Information,
                    "Building for trip {0}...{1}/{2}", trip.ToInvariantString(), t, feed.Trips.Count);

                // extract stop times for current trip.
                tripStopTimes.Clear();
                for (; tripStopTimesIndex < stopTimes.Count; tripStopTimesIndex++)
                {
                    var current = feed.StopTimes[stopTimes[tripStopTimesIndex]];
                    if (current.TripId != trip.Id)
                    {
                        break;
                    }
                    tripStopTimes.Add(stopTimes[tripStopTimesIndex]);
                }

                // check trip cache.
                var tripStops = new TripStops();
                tripStops.Stops = new List<string>();
                for (var i = 0; i < tripStopTimes.Count; i++)
                {
                    var current = feed.StopTimes[tripStopTimes[i]];
                    tripStops.Stops.Add(current.StopId);
                }

                // get trip shape from cache or build it from scratch.
                var shape = new List<Coordinate>();
                var distance = 0f;
                var stopTime1 = feed.StopTimes[tripStopTimes[0]];
                var stop1 = feed.Stops.Get(stopTime1.StopId);
                Coordinate stop1Coordinate;
                var stop1Resolved = this.ResolveStop(router, profile, stop1, out stop1Coordinate);
                if (stop1Resolved.IsError && this.StopNotResolved != null)
                {
                    this.StopNotResolved(stop1, stop1Coordinate, stop1Resolved);
                }
                stopTime1.ShapeDistTravelled = System.Math.Round(distance / 1000, 2);
                feed.StopTimes[tripStopTimes[0]] = stopTime1;
                for (var i = 0; i < tripStopTimes.Count - 1; i++)
                {
                    // calculate shape between two stops.
                    var localShape = new List<Coordinate>();
                    var stopTime2 = feed.StopTimes[tripStopTimes[i + 1]];
                    var stop2 = feed.Stops.Get(stopTime2.StopId);
                    Coordinate stop2Coordinate;
                    var stop2Resolved = this.ResolveStop(router, profile, stop2, out stop2Coordinate);
                    if (stop2Resolved.IsError && this.StopNotResolved != null)
                    {
                        this.StopNotResolved(stop2, stop2Coordinate, stop2Resolved);
                    }

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
                                if (this.ShapeNotRoutable != null)
                                {
                                    this.ShapeNotRoutable(stop1, stop2);
                                }
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
                    distance += localDistance;

                    // update stoptime.
                    stopTime2.ShapeDistTravelled = System.Math.Round(distance / 1000, 2);
                    feed.StopTimes[tripStopTimes[i + 1]] = stopTime2;

                    // move to next pair.
                    stop1 = stop2;
                    stopTime1 = stopTime2;
                    stop1Coordinate = stop2Coordinate;
                    stop1Resolved = stop2Resolved;
                }

                // build shape objects.
                string cachedShapeId;
                if (!shapeCache.TryGetValue(tripStops, out cachedShapeId))
                {
                    shape = shape.ToArray().Simplify(1).ToList();

                    distance = 0f;
                    for (var i = 0; i < shape.Count - 1; i++)
                    {
                        feed.Shapes.Add(new Shape()
                        {
                            Id = shapeId.ToInvariantString(),
                            DistanceTravelled = System.Math.Round(distance / 1000, 2),
                            Sequence = (uint)i,
                            Latitude = System.Math.Round(shape[i].Latitude, 7),
                            Longitude = System.Math.Round(shape[i].Longitude, 7)
                        });

                        distance += Coordinate.DistanceEstimateInMeter(shape[i + 0], shape[i + 1]);
                    }
                    cachedShapeId = shapeId.ToInvariantString();
                    shapeId++;

                    shapeCache[tripStops] = cachedShapeId;
                }
                trip.ShapeId = cachedShapeId;
                feed.Trips.AddOrReplace(trip, (tr) => tr.Id);
            }
        }

        private Dictionary<string, Result<RouterPoint>> _stopsResolvedCache;

        /// <summary>
        /// Resolves the given stop.
        /// </summary>
        private Result<RouterPoint> ResolveStop(Router router, IProfileInstance profile, Stop stop, out Coordinate coordinate)
        {
            if (_stopsResolvedCache == null)
            {
                _stopsResolvedCache = new Dictionary<string, Result<RouterPoint>>();
            }
            coordinate = new Coordinate((float)stop.Latitude, (float)stop.Longitude);
            Result<RouterPoint> result;
            if (_stopsResolvedCache.TryGetValue(stop.Id, out result))
            {
                return result;
            }
            result = router.TryResolve(profile, coordinate); 
            if (result.IsError)
            {
                result = router.TryResolve(profile, coordinate, _maxResolveDistanceInMeter);
            }
            _stopsResolvedCache[stop.Id] = result;
            return result;
        }
    }

    /// <summary>
    /// A pair of stop id's.
    /// </summary>
    public class StopPair
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

    /// <summary>
    /// A sequence of trip stops.
    /// </summary>
    public class TripStops
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