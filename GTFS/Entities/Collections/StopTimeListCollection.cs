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

namespace GTFS.Entities.Collections
{
    /// <summary>
    /// A collection of StopTimes.
    /// </summary>
    public class StopTimeListCollection : IStopTimeCollection
    {
        /// <summary>
        /// Holds the list containing all stops.
        /// </summary>
        private List<StopTime> _entities;

        /// <summary>
        /// Creates a unique entity collection based on a list.
        /// </summary>
        /// <param name="entities"></param>
        public StopTimeListCollection(List<StopTime> entities)
        {
            _entities = entities;
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(StopTime entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Gets all stop times.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> Get()
        {
            return _entities;
        }

        /// <summary>
        /// Gets all stop times for the given trip.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> GetForTrip(string tripId)
        {
            return _entities.Where(e =>
                {
                    return e.TripId == tripId;
                });
        }

        /// <summary>
        /// Removes all stop times for the given trip.
        /// </summary>
        /// <returns></returns>
        public int RemoveForTrip(string tripId)
        {
            return _entities.RemoveAll(x =>
                {
                    return x.TripId == tripId;
                });
        }

        /// <summary>
        /// Gets all stop times for the given stop.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> GetForStop(string stopId)
        {
            return _entities.Where(x =>
                {
                    return x.StopId == stopId;
                });
        }

        /// <summary>
        /// Removes all stop times for the given stop.
        /// </summary>
        /// <returns></returns>
        public int RemoveForStop(string stopId)
        {
            return _entities.RemoveAll(x =>
                {
                    return x.StopId == stopId;
                });
        }

        /// <summary>
        /// Returns an enumerator that iterates through the entities.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<StopTime> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the entities.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        /// <summary>
        /// This doesn't do anything
        /// </summary>
        /// <returns></returns>
        public void AddRange(IEnumerable<StopTime> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This doesn't do anything
        /// </summary>
        /// <returns></returns>
        public void RemoveRange(IEnumerable<StopTime> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This doesn't do anything
        /// </summary>
        /// <returns></returns>
        public void RemoveForTrips(IEnumerable<string> tripIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This doesn't do anything - placeholder
        /// </summary>
        /// <returns></returns>
        public bool Update(string stopId, string tripId, StopTime newEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This doesn't do anything - placeholder
        /// </summary>
        /// <returns></returns>
        public bool Update(string stopId, string tripId, uint stopSequence, StopTime newEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This doesn't do anything - placeholder
        /// </summary>
        /// <returns></returns>
        public void RemoveAll()
        {
            throw new NotImplementedException();
        }
    }
}
