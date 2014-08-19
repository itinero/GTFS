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

namespace GTFS
{
    /// <summary>
    /// Abstract representation of a GTFSFeed.
    /// </summary>
    /// <remarks>To be used as a proxy to load data into memory/a database/...</remarks>
    public interface IGTFSFeed
    {
        /// <summary>
        /// Adds an agency.
        /// </summary>
        /// <param name="agency"></param>
        void AddAgency(Agency agency);

        /// <summary>
        /// Gets the agency with the given id.
        /// </summary>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        Agency GetAgency(string agencyId);

        /// <summary>
        /// Removes the agency with the given id.
        /// </summary>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        bool RemoveAgency(string agencyId);

        /// <summary>
        /// Returns all agencies.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Agency> GetAgencies();

        /// <summary>
        /// Adds a new calendar.
        /// </summary>
        /// <param name="calendar"></param>
        void AddCalendar(Calendar calendar);

        /// <summary>
        /// Returns all calendars.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Calendar> GetCalendars();

        /// <summary>
        /// Gets the calendars for the given service id.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        IEnumerable<Calendar> GetCalendars(string serviceId);

        /// <summary>
        /// Removes all calendars for the given service id.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns>The number of calendars removed.</returns>
        int RemoveCalendars(string serviceId);

        /// <summary>
        /// Adds a new calendar date.
        /// </summary>
        /// <param name="calendar"></param>
        void AddCalendarDate(CalendarDate calendar);

        /// <summary>
        /// Returns all calendar dates.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CalendarDate> GetCalendarDates();

        /// <summary>
        /// Returns all calendar dates for the given service id.
        /// </summary>
        /// <returns></returns>
        /// <param name="serviceId"></param>
        IEnumerable<CalendarDate> GetCalendarDates(string serviceId);

        /// <summary>
        /// Removes all calendar dates for the given service id.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns>The number of calendar dates removed.</returns>
        int RemoveCalendarDates(string serviceId);

        /// <summary>
        /// Adds a new fare attribute.
        /// </summary>
        /// <param name="fareAttribute"></param>
        void AddFareAttribute(FareAttribute fareAttribute);

        /// <summary>
        /// Gets all fare attributes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FareAttribute> GetFareAttributes();

        /// <summary>
        /// Gets all fare attributes for the given fare id.
        /// </summary>
        /// <param name="fareId"></param>
        /// <returns></returns>
        IEnumerable<FareAttribute> GetFareAttributes(string fareId);

        /// <summary>
        /// Removes all fare attributes for the given fare id.
        /// </summary>
        /// <param name="fareId"></param>
        /// <returns></returns>
        int RemoveFareAttributes(string fareId);

        /// <summary>
        /// Adds a new fare rule.
        /// </summary>
        /// <param name="fareRule"></param>
        void AddFareRule(FareRule fareRule);

        /// <summary>
        /// Gets all fare rules.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FareRule> GetFareRules();

        /// <summary>
        /// Gets the fare rule for the given id.
        /// </summary>
        ///<param name="fareId"></param>
        /// <returns></returns>
        FareRule GetFareRule(string fareId);

        /// <summary>
        /// Removes the fare rule for the given id.
        /// </summary>
        ///<param name="fareId"></param>
        /// <returns>true if removed.</returns>
        bool RemoveFareRule(string fareId);

        /// <summary>
        /// Adds new feed info.
        /// </summary>
        /// <param name="feedInfo"></param>
        void SetFeedInfo(FeedInfo feedInfo);

        /// <summary>
        /// Gets the feed info.
        /// </summary>
        /// <returns></returns>
        FeedInfo GetFeedInfo();

        /// <summary>
        /// Adds a new frequency.
        /// </summary>
        /// <param name="frequency"></param>
        void AddFrequency(Frequency frequency);

        /// <summary>
        /// Returns all frequencies.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Frequency> GetFrequencies();

        /// <summary>
        /// Returns all frequencies for the given trip.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        IEnumerable<Frequency> GetFrequencies(string tripId);

        /// <summary>
        /// Removes all frequencies for the given trip.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        int RemoveFrequencies(string tripId);

        /// <summary>
        /// Adds a new route.
        /// </summary>
        /// <param name="route"></param>
        void AddRoute(Route route);

        /// <summary>
        /// Returns all routes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Route> GetRoutes();

        /// <summary>
        /// Returns the route for the given id.
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        Route GetRoute(string routeId);

        /// <summary>
        /// Removes the route for the given id.
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        bool RemoveRoute(string routeId);

        /// <summary>
        /// Adds a new shape.
        /// </summary>
        /// <param name="shape"></param>
        void AddShape(Shape shape);

        /// <summary>
        /// Gets all shapes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Shape> GetShapes();

        /// <summary>
        /// Gets all shapes for the given trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        IEnumerable<Shape> GetShapes(string tripId);

        /// <summary>
        /// Removes all shapes for the given trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        int RemoveShapes(string tripId);

        /// <summary>
        /// Adds a new stop.
        /// </summary>
        /// <param name="stop"></param>
        void AddStop(Stop stop);

        /// <summary>
        /// Gets all stops.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Stop> GetStops();

        /// <summary>
        /// Gets the stop with the given id.
        /// </summary>
        /// <returns></returns>
        Stop GetStop(string stopId);

        /// <summary>
        /// Removes the stop with the given id.
        /// </summary>
        /// <returns></returns>
        bool RemoveStop(string stopId);

        /// <summary>
        /// Adds a new stop time.
        /// </summary>
        /// <param name="stopTime"></param>
        void AddStopTime(StopTime stopTime);

        /// <summary>
        /// Gets all stop times.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StopTime> GetStopTimes();

        /// <summary>
        /// Gets all stop times for the given trip.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StopTime> GetStopTimesForTrip(string tripId);

        /// <summary>
        /// Removes all stop times for the given trip.
        /// </summary>
        /// <returns></returns>
        int RemoveStopTimesForTrip(string tripId);

        /// <summary>
        /// Gets all stop times for the given stop.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StopTime> GetStopTimesForStop(string stopId);

        /// <summary>
        /// Removes all stop times for the given stop.
        /// </summary>
        /// <returns></returns>
        int RemoveStopTimesForStop(string stopId);

        /// <summary>
        /// Adds a new transfer.
        /// </summary>
        /// <param name="transfer"></param>
        void AddTransfer(Transfer transfer);

        /// <summary>
        /// Gets all transfers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Transfer> GetTransfers();

        /// <summary>
        /// Gets all transfers for the given from stop.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Transfer> GetTransfersForFromStop(string stopId);

        /// <summary>
        /// Removes all transfers for the given from stop.
        /// </summary>
        /// <returns></returns>
        int RemoveTransfersForFromStop(string stopId);

        /// <summary>
        /// Gets all transfers for the given to stop.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Transfer> GetTransfersForToStop(string stopId);

        /// <summary>
        /// Removes all transfers for the given to stop.
        /// </summary>
        /// <returns></returns>
        int RemoveTransfersForToStop(string stopId);

        /// <summary>
        /// Adds a new trip.
        /// </summary>
        /// <param name="trip"></param>
        void AddTrip(Trip trip);

        /// <summary>
        /// Gets all trips.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Trip> GetTrips();

        /// <summary>
        /// Returns the trip with the given id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        Trip GetTrip(string tripId);

        /// <summary>
        /// Removes the given trip.
        /// </summary>
        /// <param name="tripId"></param>
        bool RemoveTrip(string tripId);
    }
}