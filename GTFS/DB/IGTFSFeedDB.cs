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
using System.Linq;
using System.Text;

namespace GTFS.DB
{
    /// <summary>
    /// An abstract representation of a GTFS feed db.
    /// </summary>
    public interface IGTFSFeedDB
    {
        /// <summary>
        /// Adds a new GTFS feed to this db.
        /// </summary>
        /// <param name="feed">The feed to add.</param>
        /// <returns>The id of the new feed.</returns>
        int AddFeed(GTFSFeed feed);

        /// <summary>
        /// Removes the feed with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveFeed(int id);

        /// <summary>
        /// Returns all the agencies.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Agency> GetAgencies();

        /// <summary>
        /// Returns all calendars.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Calendar> GetCalendars();

        /// <summary>
        /// Returns all calendar dates.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CalendarDate> GetCalendardDates();

        /// <summary>
        /// Returns all fare attributes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FareAttribute> GetFareAttributes();

        /// <summary>
        /// Returns all fare rules.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FareRule> GetFareRules();

        /// <summary>
        /// Returns all feed-infos.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FeedInfo> GetFeedInfo();

        /// <summary>
        /// Returns all frequencies.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Frequency> GetFrequencies();

        /// <summary>
        /// Returns all routes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Route> GetRoutes();

        /// <summary>
        /// Returns all shapes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Shape> GetShapes();

        /// <summary>
        /// Returns all stops.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Stop> GetStops();

        /// <summary>
        /// Returns all stop times.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StopTime> GetStopTimes();

        /// <summary>
        /// Returns all transfers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Transfer> GetTransfers();

        /// <summary>
        /// Returns all trips.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Trip> GetTrips();
    }
}