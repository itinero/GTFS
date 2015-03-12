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

using GTFS.Entities;
using System;
using System.Collections.Generic;

namespace GTFS.Filters
{
    /// <summary>
    /// Represents a GTFS feed filter that leaves only data related to the given routes.
    /// </summary>
    public class GTFSFeedRoutesFilter : GTFSFeedFilter
    {
        /// <summary>
        /// Holds the routes filter function.
        /// </summary>
        private Func<Route, bool> _routesFilter;

        /// <summary>
        /// Creates a new routes filter.
        /// </summary>
        /// <param name="routesFilter"></param>
        public GTFSFeedRoutesFilter(Func<Route, bool> routesFilter)
        {
            _routesFilter = routesFilter;
        }

        /// <summary>
        /// Creates a new routes filter.
        /// </summary>
        /// <param name="routeIds"></param>
        public GTFSFeedRoutesFilter(HashSet<string> routeIds)
        {
            _routesFilter = (r) => routeIds.Contains(r.Id);
        }

        /// <summary>
        /// Filters the given feed and returns a filtered version.
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public override IGTFSFeed Filter(IGTFSFeed feed)
        {
            var filteredFeed = new GTFSFeed();

            // filter routes.
            var routeIds = new HashSet<string>();
            var agencyIds = new HashSet<string>();
            foreach (var route in feed.GetRoutes())
            {
                if (_routesFilter.Invoke(route))
                { // keep this route.
                    filteredFeed.AddRoute(route);
                    routeIds.Add(route.Id);

                    // keep agency ids.
                    agencyIds.Add(route.AgencyId);
                }
            }

            // filter trips.
            var serviceIds = new HashSet<string>();
            var tripIds = new HashSet<string>();
            var shapeIds = new HashSet<string>();
            foreach (var trip in feed.GetTrips())
            {
                if (routeIds.Contains(trip.RouteId))
                { // keep this trip, it is related to at least one stop-time.
                    filteredFeed.AddTrip(trip);
                    tripIds.Add(trip.Id);

                    // keep serviceId, routeId and shapeId.
                    serviceIds.Add(trip.ServiceId);
                    if (!string.IsNullOrWhiteSpace(trip.ShapeId))
                    {
                        shapeIds.Add(trip.ShapeId);
                    }
                }
            }

            // filter stop-times.
            var stopIds = new HashSet<string>();
            foreach (var stopTime in feed.GetStopTimes())
            {
                if (tripIds.Contains(stopTime.TripId))
                { // stop is included, keep this stopTime.
                    filteredFeed.AddStopTime(stopTime);

                    // save the trip id's to keep.
                    stopIds.Add(stopTime.StopId);
                }
            }


            // filter stops.
            foreach (var stop in feed.GetStops())
            {
                if (stopIds.Contains(stop.Id))
                { // stop has to be included.
                    stopIds.Add(stop.Id);
                    filteredFeed.AddStop(stop);
                }
            }

            // filter agencies.
            foreach (var agency in feed.GetAgencies())
            {
                if (agencyIds.Contains(agency.Id))
                { // keep this agency.
                    filteredFeed.AddAgency(agency);
                }
            }

            // filter calendars.
            foreach (var calendar in feed.GetCalendars())
            {
                if (serviceIds.Contains(calendar.ServiceId))
                { // keep this calendar.                    
                    filteredFeed.AddCalendar(calendar);
                }
            }

            // filter calendar-dates.
            foreach (var calendarDate in feed.GetCalendarDates())
            {
                if (serviceIds.Contains(calendarDate.ServiceId))
                { // keep this calendarDate.                    
                    filteredFeed.AddCalendarDate(calendarDate);
                }
            }

            // filter fare rules.
            var fareIds = new HashSet<string>();
            foreach (var fareRule in feed.GetFareRules())
            {
                if (routeIds.Contains(fareRule.RouteId))
                { // keep this fare rule.
                    filteredFeed.AddFareRule(fareRule);

                    // keep fare ids.
                    fareIds.Add(fareRule.FareId);
                }
            }

            // filter fare attributes.
            foreach (var fareAttribute in feed.GetFareAttributes())
            {
                if (fareIds.Contains(fareAttribute.FareId))
                { // keep this fare attribute.
                    filteredFeed.AddFareAttribute(fareAttribute);
                }
            }

            // filter frequencies.
            foreach (var frequency in feed.GetFrequencies())
            {
                if (tripIds.Contains(frequency.TripId))
                { // keep this frequency.
                    filteredFeed.AddFrequency(frequency);
                }
            }

            foreach (var transfer in feed.GetTransfers())
            {
                if (stopIds.Contains(transfer.FromStop.Id) &&
                    stopIds.Contains(transfer.ToStop.Id))
                {
                    filteredFeed.AddTransfer(transfer);
                }
            }
            return filteredFeed;
        }
    }
}