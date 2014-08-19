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
using System.Data.SQLite;

namespace GTFS.DB.SQLite
{
    /// <summary>
    /// Represents a GFTS feed using an SQLite database.
    /// </summary>
    internal class SQLiteGTFSFeed : IGTFSFeed
    {
        /// <summary>
        /// Holds the connection.
        /// </summary>
        private SQLiteConnection _connection;

        /// <summary>
        /// Holds the id.
        /// </summary>
        private int _id;

        /// <summary>
        /// Creates a new SQLite GTFS feed.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id"></param>
        internal SQLiteGTFSFeed(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        public void AddAgency(Agency agency)
        {
            throw new NotImplementedException();
        }

        public Agency GetAgency(string agencyId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAgency(string agencyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agency> GetAgencies()
        {
            throw new NotImplementedException();
        }

        public void AddCalendar(Calendar calendar)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Calendar> GetCalendars()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Calendar> GetCalendars(string serviceId)
        {
            throw new NotImplementedException();
        }

        public int RemoveCalendars(string serviceId)
        {
            throw new NotImplementedException();
        }

        public void AddCalendarDate(CalendarDate calendar)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CalendarDate> GetCalendarDates()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CalendarDate> GetCalendarDates(string serviceId)
        {
            throw new NotImplementedException();
        }

        public int RemoveCalendarDates(string serviceId)
        {
            throw new NotImplementedException();
        }

        public void AddFareAttribute(FareAttribute fareAttribute)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FareAttribute> GetFareAttributes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FareAttribute> GetFareAttributes(string fareId)
        {
            throw new NotImplementedException();
        }

        public int RemoveFareAttributes(string fareId)
        {
            throw new NotImplementedException();
        }

        public void AddFareRule(FareRule fareRule)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FareRule> GetFareRules()
        {
            throw new NotImplementedException();
        }

        public FareRule GetFareRule(string fareId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFareRule(string fareId)
        {
            throw new NotImplementedException();
        }

        public void SetFeedInfo(FeedInfo feedInfo)
        {
            throw new NotImplementedException();
        }

        public FeedInfo GetFeedInfo()
        {
            throw new NotImplementedException();
        }

        public void AddFrequency(Frequency frequency)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Frequency> GetFrequencies()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Frequency> GetFrequencies(string tripId)
        {
            throw new NotImplementedException();
        }

        public int RemoveFrequencies(string tripId)
        {
            throw new NotImplementedException();
        }

        public void AddRoute(Route route)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Route> GetRoutes()
        {
            throw new NotImplementedException();
        }

        public Route GetRoute(string routeId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRoute(string routeId)
        {
            throw new NotImplementedException();
        }

        public void AddShape(Shape shape)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Shape> GetShapes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Shape> GetShapes(string tripId)
        {
            throw new NotImplementedException();
        }

        public int RemoveShapes(string tripId)
        {
            throw new NotImplementedException();
        }

        public void AddStop(Stop stop)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Stop> GetStops()
        {
            throw new NotImplementedException();
        }

        public Stop GetStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public void AddStopTime(StopTime stopTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StopTime> GetStopTimes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StopTime> GetStopTimesForTrip(string tripId)
        {
            throw new NotImplementedException();
        }

        public int RemoveStopTimesForTrip(string tripId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<StopTime> GetStopTimesForStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public int RemoveStopTimesForStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public void AddTransfer(Transfer transfer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transfer> GetTransfers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transfer> GetTransfersForFromStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public int RemoveTransfersForFromStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transfer> GetTransfersForToStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public int RemoveTransfersForToStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public void AddTrip(Trip trip)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Trip> GetTrips()
        {
            throw new NotImplementedException();
        }

        public Trip GetTrip(string tripId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveTrip(string tripId)
        {
            throw new NotImplementedException();
        }
    }
}