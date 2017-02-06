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

using System.Collections.Generic;

namespace GTFS.Entities.Collections
{
    /// <summary>
    /// Abstract representation of a collection of StopTimes.
    /// </summary>
    public interface IStopTimeCollection : IEnumerable<StopTime>
    {
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity"></param>
        void Add(StopTime entity);

        /// <summary>
        /// Gets all stop times.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StopTime> Get();

        /// <summary>
        /// Gets all stop times for the given trip.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StopTime> GetForTrip(string tripId);

        /// <summary>
        /// Removes all stop times for the given trip.
        /// </summary>
        /// <returns></returns>
        int RemoveForTrip(string tripId);

        /// <summary>
        /// Gets all stop times for the given stop.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StopTime> GetForStop(string stopId);

        /// <summary>
        /// Removes all stop times for the given stop.
        /// </summary>
        /// <returns></returns>
        int RemoveForStop(string stopId);
    }
}