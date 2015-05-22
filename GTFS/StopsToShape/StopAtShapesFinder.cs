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

using System;
using System.Collections.Generic;
using System.Linq;

namespace GTFS.StopsToShape
{
    /// <summary>
    /// A stops a shapes finder. Search a route shape and finds the stops along the way.
    /// </summary>
    public class StopAtShapesFinder
    {
        /// <summary>
        /// Finds all stops-at-shapes for the given trip with tolerance [0-20[m.
        /// </summary>
        /// <param name="feed">The feed to search.</param>
        /// <param name="tripId">The trip to search.</param>
        /// <returns></returns>
        public List<StopAtShape> Find(IGTFSFeed feed, string tripId)
        {
            return this.Find(feed, tripId, 20);
        }

        /// <summary>
        /// Finds all stops-at-shapes for the given trip.
        /// </summary>
        /// <param name="feed">The feed to search.</param>
        /// <param name="tripId">The trip to search.</param>
        /// <param name="maxTolerance">The maximum distance.</param>
        /// <returns></returns>
        public List<StopAtShape> Find(IGTFSFeed feed, string tripId, double maxTolerance)
        {
            var minTolerance = 1;
            if (string.IsNullOrWhiteSpace(tripId)) { throw new ArgumentNullException("tripId"); }

            var trip = feed.Trips.Get(tripId);
            if (trip == null) { throw new Exception(string.Format("Trip with id {0} not found.", tripId)); }

            var stopsAtShape = new List<StopAtShape>();
            var shapeId = trip.ShapeId;
            if(!string.IsNullOrWhiteSpace(shapeId))
            { // there is an id.
                var shapes = feed.Shapes.Get(shapeId).ToList();
                shapes.Sort((x, y) =>
                {
                    return x.Sequence.CompareTo(y.Sequence);
                });

                var stops = feed.StopTimes.GetForTrip(tripId).ToList();
                stops.Sort((x, y) =>
                {
                    return x.StopSequence.CompareTo(y.StopSequence);
                });

                int stopIdx = 0;
                int lastFoundShape = -1;
                while (stopIdx < stops.Count)
                { // find a shape entry for the current stop.
                    var stop = feed.Stops.Get(stops[stopIdx].StopId);
                    var shapeFoundIdx = -1;
                    var shapeDistanceTolerance = minTolerance; // when shape point < minTolerance;
                    while (shapeDistanceTolerance < maxTolerance)
                    { // keep searching until tolerance reaches 
                        var shapeDistance = double.MaxValue;
                        // only search from the last found shape on.
                        for (int shapeIdx = lastFoundShape + 1; shapeIdx < shapes.Count; shapeIdx++)
                        {
                            var localDistance = Extensions.DistanceInMeter(stop.Latitude, stop.Longitude,
                                shapes[shapeIdx].Latitude, shapes[shapeIdx].Longitude);
                            if (localDistance < shapeDistanceTolerance)
                            { // is within tolerance.
                                if(shapeDistance != double.MaxValue &&
                                    localDistance > shapeDistance)
                                { // another within tolerance, but higher, stop search.
                                    break;
                                }
                                if(localDistance < shapeDistance)
                                { // ok, distance is better!
                                    shapeDistance = localDistance;
                                    shapeFoundIdx = shapeIdx;
                                }
                            }
                            else
                            { // distance is bigger.
                                if(shapeDistance != double.MaxValue)
                                { // a point was already found.
                                    break;
                                }
                            }
                        }
                        if(shapeDistance != double.MaxValue)
                        { // a point was found.
                            break;
                        }
                        shapeDistanceTolerance = shapeDistanceTolerance * 2;
                    }

                    if(shapeFoundIdx == -1)
                    { // no shape-candidate was found.
                        throw new Exception(string.Format("No shape was found for stop {0} with tolerlance of [{1}-{2}[m.",
                            stop.ToString(), 0, maxTolerance));
                    }

                    // add to result.
                    lastFoundShape = shapeFoundIdx;
                    stopsAtShape.Add(new StopAtShape()
                        {
                            ShapePointSequence = shapes[shapeFoundIdx].Sequence,
                            StopId = stop.Id,
                            StopOffset = 0,
                            TripId = tripId
                        });
                    stopIdx++;
                }
            }

            return stopsAtShape;
        }
    }
}