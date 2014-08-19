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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFS.DB.SQLite
{
    /// <summary>
    /// Represents a GFTS feed using an SQLite database.
    /// </summary>
    internal class SQLiteGTFSFeed : IGTFSFeed
    {

        public void AddAgency(Entities.Agency agency)
        {
            throw new NotImplementedException();
        }

        public Entities.Agency GetAgency(string agencyId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAgency(string agencyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Agency> GetAgencies()
        {
            throw new NotImplementedException();
        }

        public void AddCalendar(Entities.Calendar calendar)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Calendar> GetCalendars()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Calendar> GetCalendars(string serviceId)
        {
            throw new NotImplementedException();
        }

        public int RemoveCalendars(string serviceId)
        {
            throw new NotImplementedException();
        }

        public void AddCalendarDate(Entities.CalendarDate calendar)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.CalendarDate> GetCalendarDates()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.CalendarDate> GetCalendarDates(string serviceId)
        {
            throw new NotImplementedException();
        }

        public int RemoveCalendarDates(string serviceId)
        {
            throw new NotImplementedException();
        }

        public void AddFareAttribute(Entities.FareAttribute fareAttribute)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.FareAttribute> GetFareAttributes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.FareAttribute> GetFareAttributes(string fareId)
        {
            throw new NotImplementedException();
        }

        public int RemoveFareAttributes(string fareId)
        {
            throw new NotImplementedException();
        }

        public void AddFareRule(Entities.FareRule fareRule)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.FareRule> GetFareRules()
        {
            throw new NotImplementedException();
        }

        public Entities.FareRule GetFareRule(string fareId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFareRule(string fareId)
        {
            throw new NotImplementedException();
        }

        public void SetFeedInfo(Entities.FeedInfo feedInfo)
        {
            throw new NotImplementedException();
        }

        public Entities.FeedInfo GetFeedInfo()
        {
            throw new NotImplementedException();
        }

        public void AddFrequency(Entities.Frequency frequency)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Frequency> GetFrequencies()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Frequency> GetFrequencies(string tripId)
        {
            throw new NotImplementedException();
        }

        public int RemoveFrequencies(string tripId)
        {
            throw new NotImplementedException();
        }

        public void AddRoute(Entities.Route route)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Route> GetRoutes()
        {
            throw new NotImplementedException();
        }

        public Entities.Route GetRoute(string routeId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRoute(string routeId)
        {
            throw new NotImplementedException();
        }

        public void AddShape(Entities.Shape shape)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Shape> GetShapes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Shape> GetShapes(string tripId)
        {
            throw new NotImplementedException();
        }

        public int RemoveShapes(string tripId)
        {
            throw new NotImplementedException();
        }

        public void AddStop(Entities.Stop stop)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Stop> GetStops()
        {
            throw new NotImplementedException();
        }

        public Entities.Stop GetStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public void AddStopTime(Entities.StopTime stopTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.StopTime> GetStopTimes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.StopTime> GetStopTimesForTrip(string tripId)
        {
            throw new NotImplementedException();
        }

        public int RemoveStopTimesForTrip(string tripId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.StopTime> GetStopTimesForStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public int RemoveStopTimesForStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public void AddTransfer(Entities.Transfer transfer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Transfer> GetTransfers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Transfer> GetTransfersForFromStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public int RemoveTransfersForFromStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Transfer> GetTransfersForToStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public int RemoveTransfersForToStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public void AddTrip(Entities.Trip trip)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.Trip> GetTrips()
        {
            throw new NotImplementedException();
        }

        public Entities.Trip GetTrip(string tripId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveTrip(string tripId)
        {
            throw new NotImplementedException();
        }
    }
}