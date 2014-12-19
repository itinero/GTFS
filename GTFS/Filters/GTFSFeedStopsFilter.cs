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

namespace GTFS.Filters
{
    /// <summary>
    /// Represents a GTFS feed filter that leaves only date related to the given stops.
    /// </summary>
    public class GTFSFeedStopsFilter : GTFSFeedFilter
    {
        /// <summary>
        /// Holds the stop filter function.
        /// </summary>
        private Func<Stop, bool> _stopFilter;

        /// <summary>
        /// Creates a new stops filter.
        /// </summary>
        /// <param name="stopFilter"></param>
        public GTFSFeedStopsFilter(Func<Stop, bool> stopFilter)
        {
            _stopFilter = stopFilter;
        }

        /// <summary>
        /// Creates a new stops filter.
        /// </summary>
        /// <param name="stopIds"></param>
        public GTFSFeedStopsFilter(HashSet<string> stopIds)
        {
            _stopFilter = (s) => stopIds.Contains(s.Id);
        }

        /// <summary>
        /// Filters the given feed and returns a filtered version.
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public override IGTFSFeed Filter(IGTFSFeed feed)
        {
            var filteredFeed = new GTFSFeed();

            // collect stopids and tripids.
            var stopIds = new HashSet<string>();
            var tripIds = new HashSet<string>();

            foreach (var stop in feed.GetStops())
            {
                if (_stopFilter.Invoke(stop))
                { // stop has to be included.
                    stopIds.Add(stop.Id);
                }
            }
            foreach (var stopTime in feed.GetStopTimes())
            {
                if (stopIds.Contains(stopTime.StopId))
                { // save the trip id's to keep.
                    tripIds.Add(stopTime.TripId);
                }
            }
            foreach (var stopTime in feed.GetStopTimes())
            {
                if (tripIds.Contains(stopTime.TripId))
                { // stop is included, keep this stopTime.
                    stopIds.Add(stopTime.StopId);
                }
            }

            // filter stop-times.
            foreach (var stopTime in feed.GetStopTimes())
            {
                if (stopIds.Contains(stopTime.StopId) &&
                    tripIds.Contains(stopTime.TripId))
                { // save the trip id's to keep.
                    filteredFeed.AddStopTime(stopTime);
                }
            }

            // filter stops.
            foreach (var stop in feed.GetStops())
            {
                if (stopIds.Contains(stop.Id))
                { // stop has to be included.
                    filteredFeed.AddStop(stop);
                }
            }

            // filter trips.
            var routeIds = new HashSet<string>();
            var serviceIds = new HashSet<string>();
            var shapeIds = new HashSet<string>();
            foreach (var trip in feed.GetTrips())
            {
                if (tripIds.Contains(trip.Id))
                { // keep this trip, it is related to at least one stop-time.
                    filteredFeed.AddTrip(trip);

                    // keep serviceId, routeId and shapeId.
                    routeIds.Add(trip.RouteId);
                    serviceIds.Add(trip.ServiceId);
                    if (!string.IsNullOrWhiteSpace(trip.ShapeId))
                    {
                        shapeIds.Add(trip.ShapeId);
                    }
                }
            }

            // filter routes.
            var agencyIds = new HashSet<string>();
            foreach (var route in feed.GetRoutes())
            {
                if (routeIds.Contains(route.Id))
                { // keep this route.
                    filteredFeed.AddRoute(route);

                    // keep agency ids.
                    agencyIds.Add(route.AgencyId);
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