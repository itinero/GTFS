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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GTFS.Entities.Collections
{
    /// <summary>
    /// A collection of StopTimes.
    /// </summary>
    public class StopTimeListCollection : IStopTimeCollection
    {
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
        /// This doesn't do anything
        /// </summary>
        /// <returns></returns>
        public void AddRange(IEnumerable<StopTime> entities)
        {
            _entities.AddRange(entities);
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
        /// Gets all stop times for the given trips.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> GetForTrips(IEnumerable<string> tripIds)
        {
            return _entities.Where(e =>
            {
                return tripIds.Contains(e.TripId);
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
        /// This doesn't do anything
        /// </summary>
        /// <returns></returns>
        public void RemoveRange(IEnumerable<StopTime> entities)
        {
            throw new NotImplementedException();
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
        /// This doesn't do anything
        /// </summary>
        /// <returns></returns>
        public void RemoveForTrips(IEnumerable<string> tripIds)
        {
            _entities.RemoveAll(x =>
            {
                return tripIds.Contains(x.TripId);
            });
        }

        /// <summary>
        /// Replaces the internal list of stop_times with a new, empty list.
        /// </summary>
        /// <returns></returns>
        public void RemoveAll()
        {
            _entities = new List<StopTime>();
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
        /// Gets the number of entities.
        /// </summary>
        public int Count
        {
            get
            {
                return _entities.Count;
            }
        }

        /// <summary>
        /// Gets or sets the entity at the given idx.
        /// </summary>
        public StopTime this[int idx]
        {
            get
            {
                return _entities[idx];
            }

            set
            {
                _entities[idx] = value;
            }
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
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }
    }
}
