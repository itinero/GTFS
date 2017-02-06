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
    /// Represents a GTFS feed filter that leaves only data related to the some filtered stops.
    /// 
    /// - Leaves only trips related to the filtered stops.
    /// - Leaves trips intact, will add stops again to keep complete trips.
    /// - Filters out all other data not related to any of the trips or stops.
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

            foreach (var stop in feed.Stops)
            {
                if (_stopFilter.Invoke(stop))
                { // stop has to be included.
                    stopIds.Add(stop.Id);
                }
            }
            foreach (var stopTime in feed.StopTimes)
            {
                if (stopIds.Contains(stopTime.StopId))
                { // save the trip id's to keep.
                    tripIds.Add(stopTime.TripId);
                }
            }
            foreach (var stopTime in feed.StopTimes)
            {
                if (tripIds.Contains(stopTime.TripId))
                { // stop is included, keep this stopTime.
                    stopIds.Add(stopTime.StopId);
                }
            }

            // filter stop-times.
            foreach (var stopTime in feed.StopTimes)
            {
                if (stopIds.Contains(stopTime.StopId) &&
                    tripIds.Contains(stopTime.TripId))
                { // save the trip id's to keep.
                    filteredFeed.StopTimes.Add(stopTime);
                }
            }

            // filter stops.
            foreach (var stop in feed.Stops)
            {
                if (stopIds.Contains(stop.Id))
                { // stop has to be included.
                    filteredFeed.Stops.Add(stop);
                }
            }

            // filter trips.
            var routeIds = new HashSet<string>();
            var serviceIds = new HashSet<string>();
            var shapeIds = new HashSet<string>();
            foreach (var trip in feed.Trips)
            {
                if (tripIds.Contains(trip.Id))
                { // keep this trip, it is related to at least one stop-time.
                    filteredFeed.Trips.Add(trip);

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
            foreach (var route in feed.Routes)
            {
                if (routeIds.Contains(route.Id))
                { // keep this route.
                    filteredFeed.Routes.Add(route);

                    // keep agency ids.
                    agencyIds.Add(route.AgencyId);
                }
            }

            // filter agencies.
            foreach (var agency in feed.Agencies)
            {
                if (agencyIds.Contains(agency.Id))
                { // keep this agency.
                    filteredFeed.Agencies.Add(agency);
                }
            }

            // filter calendars.
            foreach (var calendar in feed.Calendars)
            {
                if (serviceIds.Contains(calendar.ServiceId))
                { // keep this calendar.                    
                    filteredFeed.Calendars.Add(calendar);
                }
            }

            // filter calendar-dates.
            foreach (var calendarDate in feed.CalendarDates)
            {
                if (serviceIds.Contains(calendarDate.ServiceId))
                { // keep this calendarDate.                    
                    filteredFeed.CalendarDates.Add(calendarDate);
                }
            }

            // filter fare rules.
            var fareIds = new HashSet<string>();
            foreach (var fareRule in feed.FareRules)
            {
                if (routeIds.Contains(fareRule.RouteId))
                { // keep this fare rule.
                    filteredFeed.FareRules.Add(fareRule);

                    // keep fare ids.
                    fareIds.Add(fareRule.FareId);
                }
            }

            // filter fare attributes.
            foreach (var fareAttribute in feed.FareAttributes)
            {
                if (fareIds.Contains(fareAttribute.FareId))
                { // keep this fare attribute.
                    filteredFeed.FareAttributes.Add(fareAttribute);
                }
            }

            // filter frequencies.
            foreach (var frequency in feed.Frequencies)
            {
                if (tripIds.Contains(frequency.TripId))
                { // keep this frequency.
                    filteredFeed.Frequencies.Add(frequency);
                }
            }

            foreach (var transfer in feed.Transfers)
            {
                if (stopIds.Contains(transfer.FromStopId) &&
                    stopIds.Contains(transfer.ToStopId))
                {
                    filteredFeed.Transfers.Add(transfer);
                }
            }

            // filter shapes.
            foreach(var shape in feed.Shapes)
            {
                if(shapeIds.Contains(shape.Id))
                {
                    filteredFeed.Shapes.Add(shape);
                }
            }
            return filteredFeed;
        }
    }
}