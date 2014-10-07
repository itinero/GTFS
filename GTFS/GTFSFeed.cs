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
using System.Collections.Generic;
using System.Linq;

namespace GTFS
{
    /// <summary>
    /// Represents an entire GTFS feed as it exists on disk.
    /// </summary>
    public class GTFSFeed : IGTFSFeed
    {
        /// <summary>
        /// Holds the agencies.
        /// </summary>
        private List<Agency> _agencies;

        /// <summary>
        /// Holds the calendars.
        /// </summary>
        private List<Calendar> _calendars;

        /// <summary>
        /// Holds the calendar dates.
        /// </summary>
        private List<CalendarDate> _calendarDates;

        /// <summary>
        /// Holds the fare attributes.
        /// </summary>
        private List<FareAttribute> _fareAttributes;

        /// <summary>
        /// Holds the fare rules.
        /// </summary>
        private List<FareRule> _fareRules;

        /// <summary>
        /// Holds the feed info.
        /// </summary>
        private FeedInfo _feedInfo;

        /// <summary>
        /// Holds the frequencies.
        /// </summary>
        private List<Frequency> _frequencies;

        /// <summary>
        /// Holds the routes.
        /// </summary>
        private List<Route> _routes;

        /// <summary>
        /// Holds the shapes.
        /// </summary>
        private List<Shape> _shapes;

        /// <summary>
        /// Holds the stops.
        /// </summary>
        private List<Stop> _stops;

        /// <summary>
        /// Holds the stop times.
        /// </summary>
        private List<StopTime> _stopTimes;

        /// <summary>
        /// Holds the transfers.
        /// </summary>
        private List<Transfer> _transfers;

        /// <summary>
        /// Holds the trips.
        /// </summary>
        private List<Trip> _trips;

        /// <summary>
        /// Creates a new feed.
        /// </summary>
        public GTFSFeed()
        {
            _feedInfo = new FeedInfo();
            _agencies = new List<Agency>();
            _calendarDates = new List<CalendarDate>();
            _calendars = new List<Calendar>();
            _fareAttributes = new List<FareAttribute>();
            _fareRules = new List<FareRule>();
            _frequencies = new List<Frequency>();
            _routes = new List<Route>();
            _shapes = new List<Shape>();
            _stops = new List<Stop>();
            _stopTimes = new List<StopTime>();
            _transfers = new List<Transfer>();
            _trips = new List<Trip>();
        }

        /// <summary>
        /// Adds an agency.
        /// </summary>
        /// <param name="agency"></param>
        public void AddAgency(Agency agency)
        {
            _agencies.Add(agency);
        }

        /// <summary>
        /// Gets the agency for the given id.
        /// </summary>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        public Agency GetAgency(string agencyId)
        {
            return this.GetAgencies().FirstOrDefault(x => x.Id == agencyId);
        }

        /// <summary>
        /// Removes the agency with the given id.
        /// </summary>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        public bool RemoveAgency(string agencyId)
        {
            return _agencies.RemoveAll(x => x.Id == agencyId) > 0;
        }

        /// <summary>
        /// Gets all agencies.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Agency> GetAgencies()
        {
            return _agencies;
        }

        /// <summary>
        /// Adds a new calendar.
        /// </summary>
        /// <param name="calendar"></param>
        public void AddCalendar(Calendar calendar)
        {
            _calendars.Add(calendar);
        }

        /// <summary>
        /// Returns all calendars.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Calendar> GetCalendars()
        {
            return _calendars;
        }

        /// <summary>
        /// Returns all calendars for the given service id.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public IEnumerable<Calendar> GetCalendars(string serviceId)
        {
            return _calendars.Where(x => x.ServiceId == serviceId);
        }

        /// <summary>
        /// Removes all calenders for the given service id.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public int RemoveCalendars(string serviceId)
        {
            return _calendars.RemoveAll(x => x.ServiceId == serviceId);
        }

        /// <summary>
        /// Adds a calendar date.
        /// </summary>
        /// <param name="calendar"></param>
        public void AddCalendarDate(CalendarDate calendar)
        {
            _calendarDates.Add(calendar);
        }

        /// <summary>
        /// Returns all calendar dates.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CalendarDate> GetCalendarDates()
        {
            return _calendarDates;
        }

        /// <summary>
        /// Returns all calendar dates for the given service id.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public IEnumerable<CalendarDate> GetCalendarDates(string serviceId)
        {
            return _calendarDates.Where(x => x.ServiceId == serviceId);
        }

        /// <summary>
        /// Removes all calendar dates for the given service id.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public int RemoveCalendarDates(string serviceId)
        {
            return _calendarDates.RemoveAll(x => x.ServiceId == serviceId);
        }

        /// <summary>
        /// Adds a fare attribute.
        /// </summary>
        /// <param name="fareAttribute"></param>
        public void AddFareAttribute(FareAttribute fareAttribute)
        {
            _fareAttributes.Add(fareAttribute);
        }

        /// <summary>
        /// Returns all fare attributes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FareAttribute> GetFareAttributes()
        {
            return _fareAttributes;
        }

        /// <summary>
        /// Returns all fare attributes for the given fare id.
        /// </summary>
        /// <param name="fareId"></param>
        /// <returns></returns>
        public IEnumerable<FareAttribute> GetFareAttributes(string fareId)
        {
            return _fareAttributes.Where(x => x.FareId == fareId);
        }

        /// <summary>
        /// Removes all fare attributes for the given fare id.
        /// </summary>
        /// <param name="fareId"></param>
        /// <returns></returns>
        public int RemoveFareAttributes(string fareId)
        {
            return _fareAttributes.RemoveAll(x => x.FareId == fareId);
        }

        /// <summary>
        /// Adds a fare rule.
        /// </summary>
        /// <param name="fareRule"></param>
        public void AddFareRule(FareRule fareRule)
        {
            _fareRules.Add(fareRule);
        }

        /// <summary>
        /// Gets all fare rules.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FareRule> GetFareRules()
        {
            return _fareRules;
        }

        /// <summary>
        /// Gets the fare rules for the given fare id.
        /// </summary>
        /// <param name="fareId"></param>
        /// <returns></returns>
        public FareRule GetFareRule(string fareId)
        {
            return _fareRules.FirstOrDefault(x => x.FareId == fareId);
        }

        /// <summary>
        /// Removes all fare rules for the given fare id.
        /// </summary>
        /// <param name="fareId"></param>
        /// <returns></returns>
        public bool RemoveFareRule(string fareId)
        {
            return _fareRules.RemoveAll(x => x.FareId == fareId) > 0;
        }

        /// <summary>
        /// Sets the feed info.
        /// </summary>
        /// <param name="feedInfo"></param>
        public void SetFeedInfo(FeedInfo feedInfo)
        {
            if (_feedInfo != null)
            {
                _feedInfo.EndDate = feedInfo.EndDate;
                _feedInfo.Lang = feedInfo.Lang;
                _feedInfo.PublisherName = feedInfo.PublisherName;
                _feedInfo.PublisherUrl = feedInfo.PublisherUrl;
                _feedInfo.StartDate = feedInfo.StartDate;
                _feedInfo.Version = _feedInfo.Version;
            }
        }

        /// <summary>
        /// Gets the feed info.
        /// </summary>
        /// <returns></returns>
        public FeedInfo GetFeedInfo()
        {
            return _feedInfo;
        }

        /// <summary>
        /// Adds a frequency.
        /// </summary>
        /// <param name="frequency"></param>
        public void AddFrequency(Frequency frequency)
        {
            _frequencies.Add(frequency);
        }

        /// <summary>
        /// Returns all frequencies.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Frequency> GetFrequencies()
        {
            return _frequencies;
        }

        /// <summary>
        /// Returns the frequencies for the given trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public IEnumerable<Frequency> GetFrequencies(string tripId)
        {
            return _frequencies.Where(x => x.TripId == tripId);
        }

        /// <summary>
        /// Removes all frequencies for the given trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public int RemoveFrequencies(string tripId)
        {
            return _frequencies.RemoveAll(x => x.TripId == tripId);
        }

        /// <summary>
        /// Adds a route.
        /// </summary>
        /// <param name="route"></param>
        public void AddRoute(Route route)
        {
            _routes.Add(route);
        }

        /// <summary>
        /// Returns all routes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Route> GetRoutes()
        {
            return _routes;
        }

        /// <summary>
        /// Returns the route.
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public Route GetRoute(string routeId)
        {
            return _routes.FirstOrDefault(x => x.Id == routeId);
        }

        /// <summary>
        /// Removes the route.
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public bool RemoveRoute(string routeId)
        {
            return _routes.RemoveAll(x => x.Id == routeId) > 0;
        }

        /// <summary>
        /// Adds a shape.
        /// </summary>
        /// <param name="shape"></param>
        public void AddShape(Shape shape)
        {
            _shapes.Add(shape);
        }

        /// <summary>
        /// Returns all the shapes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Shape> GetShapes()
        {
            return _shapes;
        }

        /// <summary>
        /// Returns all the shapes for the given trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public IEnumerable<Shape> GetShapes(string tripId)
        {
            return _shapes.Where(x => x.Id == tripId);
        }

        /// <summary>
        /// Removes all the shapes for the given trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public int RemoveShapes(string tripId)
        {
            return _shapes.RemoveAll(x => x.Id == tripId);
        }

        /// <summary>
        /// Adds a stop.
        /// </summary>
        /// <param name="stop"></param>
        public void AddStop(Stop stop)
        {
            _stops.Add(stop);
        }

        /// <summary>
        /// Returns all the stops.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stop> GetStops()
        {
            return _stops;
        }

        /// <summary>
        /// Gets the stop for the given id.
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public Stop GetStop(string stopId)
        {
            return _stops.FirstOrDefault(x => x.Id == stopId);
        }

        /// <summary>
        /// Removes the stop with the given id.
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public bool RemoveStop(string stopId)
        {
            return _stops.RemoveAll(x => x.Id == stopId) > 0;
        }

        /// <summary>
        /// Adds a stop time.
        /// </summary>
        /// <param name="stopTime"></param>
        public void AddStopTime(StopTime stopTime)
        {
            _stopTimes.Add(stopTime);
        }

        /// <summary>
        /// Returns all stop times.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> GetStopTimes()
        {
            return _stopTimes;
        }

        /// <summary>
        /// Returns the stop times for the given trip.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public IEnumerable<StopTime> GetStopTimesForTrip(string tripId)
        {
            return _stopTimes.Where(x => x.TripId == tripId);
        }

        /// <summary>
        /// Removes the stop times for the given trip.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public int RemoveStopTimesForTrip(string tripId)
        {
            return _stopTimes.RemoveAll(x => x.TripId == tripId);
        }

        /// <summary>
        /// Returns the stop times for the given stop.
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public IEnumerable<StopTime> GetStopTimesForStop(string stopId)
        {
            return _stopTimes.Where(x => x.StopId == stopId);
        }

        /// <summary>
        /// Removes the stop times for the given stop.
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public int RemoveStopTimesForStop(string stopId)
        {
            return _stopTimes.RemoveAll(x => x.StopId == stopId);
        }

        /// <summary>
        /// Adds a transfer.
        /// </summary>
        /// <param name="transfer"></param>
        public void AddTransfer(Transfer transfer)
        {
            _transfers.Add(transfer);
        }

        /// <summary>
        /// Returns all transfers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Transfer> GetTransfers()
        {
            return _transfers;
        }

        /// <summary>
        /// Returns all transfers with the given stop as from stop.
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public IEnumerable<Transfer> GetTransfersForFromStop(string stopId)
        {
            return _transfers.Where(x => x.FromStop.Id == stopId);
        }

        /// <summary>
        /// Removes all transfers with the given stop as from stop.
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public int RemoveTransfersForFromStop(string stopId)
        {
            return _transfers.RemoveAll(x => x.FromStop.Id == stopId);
        }

        /// <summary>
        /// Returns all transfers with the given stop as to stop.
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public IEnumerable<Transfer> GetTransfersForToStop(string stopId)
        {
            return _transfers.Where(x => x.ToStop.Id == stopId);
        }

        /// <summary>
        /// Removes all transfers with the given stop as to stop.
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public int RemoveTransfersForToStop(string stopId)
        {
            return _transfers.RemoveAll(x => x.ToStop.Id == stopId);
        }

        /// <summary>
        /// Adds a trip.
        /// </summary>
        /// <param name="trip"></param>
        public void AddTrip(Trip trip)
        {
            _trips.Add(trip);
        }

        /// <summary>
        /// Returns all the trips.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Trip> GetTrips()
        {
            return _trips;
        }

        /// <summary>
        /// Returns the trip with the given id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public Trip GetTrip(string tripId)
        {
            return _trips.FirstOrDefault(x => x.Id == tripId);
        }

        /// <summary>
        /// Removes the trip with the given id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public bool RemoveTrip(string tripId)
        {
            return _trips.RemoveAll(x => x.Id == tripId) > 0;
        }
    }
}