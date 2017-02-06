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
using GTFS.IO;
using GTFS.IO.CSV;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GTFS.Test
{
    /// <summary>
    /// Contains test extensions/helper methods.
    /// </summary>
    public static class GTFSAssert
    {
        /// <summary>
        /// Builds the source from embedded streams.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IGTFSSourceFile> BuildSource()
        {
            var source = new List<IGTFSSourceFile>();
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.agency.txt"), "agency"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.calendar.txt"), "calendar"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.calendar_dates.txt"), "calendar_dates"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.fare_attributes.txt"), "fare_attributes"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.fare_rules.txt"), "fare_rules"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.frequencies.txt"), "frequencies"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.routes.txt"), "routes"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.shapes.txt"), "shapes"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.stop_times.txt"), "stop_times"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.stops.txt"), "stops"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.trips.txt"), "trips"));
            return source;
        }

        /// <summary>
        /// Compares two feeds.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(IGTFSFeed actual, IGTFSFeed expected)
        {
            // first compare feed info.
            GTFSAssert.AreEqual(actual.GetFeedInfo(), expected.GetFeedInfo());

            // compare agencies.
            GTFSAssert.AreEqual<Agency>(actual.Agencies, expected.Agencies,
                (x, y) => x.Id == y.Id, (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<CalendarDate>(actual.CalendarDates, expected.CalendarDates,
                (x, y) => x.ServiceId == y.ServiceId && x.Date == y.Date && x.ExceptionType == y.ExceptionType, (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<Calendar>(actual.Calendars, expected.Calendars,
                (x, y) => x.ToString() == y.ToString(), (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<FareAttribute>(actual.FareAttributes, expected.FareAttributes,
                (x, y) => x.FareId == y.FareId, (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<FareRule>(actual.FareRules, expected.FareRules,
                (x, y) => x.ContainsId == y.ContainsId && x.DestinationId == y.DestinationId &&
                    x.FareId == y.FareId && x.OriginId == y.OriginId && x.RouteId == y.RouteId, 
                    (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<Frequency>(actual.Frequencies, expected.Frequencies,
                (x, y) => x.TripId == y.TripId && x.StartTime == y.StartTime, (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<Route>(actual.Routes, expected.Routes,
                (x, y) => x.Id == y.Id, (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<Shape>(actual.Shapes, expected.Shapes,
                (x, y) => x.Id == y.Id && x.Sequence == y.Sequence, (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<Stop>(actual.Stops, expected.Stops,
                (x, y) => x.Id == y.Id, (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<StopTime>(actual.StopTimes, expected.StopTimes,
                (x, y) => x.TripId == y.TripId && x.StopId == y.StopId && x.StopSequence == y.StopSequence, (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<Transfer>(actual.Transfers, expected.Transfers,
                (x, y) => x.FromStopId == y.FromStopId && x.ToStopId == y.ToStopId && x.TransferType == y.TransferType, (x, y) => GTFSAssert.AreEqual(x, y));
            GTFSAssert.AreEqual<Trip>(actual.Trips, expected.Trips,
                (x, y) => x.Id == y.Id, (x, y) => GTFSAssert.AreEqual(x, y));
        }

        /// <summary>
        /// Compares the two enumerables.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actuals"></param>
        /// <param name="expectees"></param>
        /// <param name="idEqual"></param>
        /// <param name="areEqualAction"></param>
        public static void AreEqual<T>(IEnumerable<T> actuals, IEnumerable<T> expectees, 
            Func<T, T, bool> idEqual, Action<T, T> areEqualAction)
        {
            Assert.AreEqual(actuals.Count(), expectees.Count());
            foreach(var actual in actuals)
            {
                var expected = expectees.First(x => idEqual(x, actual));
                Assert.IsNotNull(expected);
                areEqualAction(actual, expected);
            }
        }

        /// <summary>
        /// Compares two feed-infos.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(FeedInfo actual, FeedInfo expected)
        {
            if(actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.EndDate, expected.EndDate);
            Assert.AreEqual(actual.Lang, expected.Lang);
            Assert.AreEqual(actual.PublisherName, expected.PublisherName);
            Assert.AreEqual(actual.PublisherUrl, expected.PublisherUrl);
            Assert.AreEqual(actual.StartDate, expected.StartDate);
            Assert.AreEqual(actual.Version, expected.Version);
        }

        /// <summary>
        /// Compares two agencies.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(Agency actual, Agency expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.FareURL, expected.FareURL);
            Assert.AreEqual(actual.Id, expected.Id);
            Assert.AreEqual(actual.LanguageCode, expected.LanguageCode);
            Assert.AreEqual(actual.Name, expected.Name);
            Assert.AreEqual(actual.Phone, expected.Phone);
            Assert.AreEqual(actual.Timezone, expected.Timezone);
            Assert.AreEqual(actual.URL, expected.URL);
        }

        /// <summary>
        /// Compares two calendars.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(Calendar actual, Calendar expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.EndDate, expected.EndDate);
            Assert.AreEqual(actual.Friday, expected.Friday);
            Assert.AreEqual(actual.Monday, expected.Monday);
            Assert.AreEqual(actual.Saturday, expected.Saturday);
            Assert.AreEqual(actual.ServiceId, expected.ServiceId);
            Assert.AreEqual(actual.StartDate, expected.StartDate);
            Assert.AreEqual(actual.Sunday, expected.Sunday);
            Assert.AreEqual(actual.Thursday, expected.Thursday);
            Assert.AreEqual(actual.Tuesday, expected.Tuesday);
            Assert.AreEqual(actual.Wednesday, expected.Wednesday);
        }

        /// <summary>
        /// Compares two calendar dates.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(CalendarDate actual, CalendarDate expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.Date, expected.Date);
            Assert.AreEqual(actual.ExceptionType, expected.ExceptionType);
            Assert.AreEqual(actual.ServiceId, expected.ServiceId);
        }

        /// <summary>
        /// Compares two fare attributes.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>

        public static void AreEqual(FareAttribute actual, FareAttribute expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.CurrencyType, expected.CurrencyType);
            Assert.AreEqual(actual.FareId, expected.FareId);
            Assert.AreEqual(actual.PaymentMethod, expected.PaymentMethod);
            Assert.AreEqual(actual.Price, expected.Price);
            Assert.AreEqual(actual.TransferDuration, expected.TransferDuration);
            Assert.AreEqual(actual.Transfers, expected.Transfers);
        }

        /// <summary>
        /// Compares two fare rules.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(FareRule actual, FareRule expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.ContainsId, expected.ContainsId);
            Assert.AreEqual(actual.DestinationId, expected.DestinationId);
            Assert.AreEqual(actual.FareId, expected.FareId);
            Assert.AreEqual(actual.OriginId, expected.OriginId);
            Assert.AreEqual(actual.RouteId, expected.RouteId);
        }

        /// <summary>
        /// Compares two frequencies
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(Frequency actual, Frequency expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.EndTime, expected.EndTime);
            Assert.AreEqual(actual.ExactTimes, expected.ExactTimes);
            Assert.AreEqual(actual.HeadwaySecs, expected.HeadwaySecs);
            Assert.AreEqual(actual.StartTime, expected.StartTime);
            Assert.AreEqual(actual.TripId, expected.TripId);
        }

        /// <summary>
        /// Compares two routes.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(Route actual, Route expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.AgencyId, expected.AgencyId);
            Assert.AreEqual(actual.Color, expected.Color);
            Assert.AreEqual(actual.Description, expected.Description);
            Assert.AreEqual(actual.Id, expected.Id);
            Assert.AreEqual(actual.LongName, expected.LongName);
            Assert.AreEqual(actual.ShortName, expected.ShortName);
            Assert.AreEqual(actual.TextColor, expected.TextColor);
            Assert.AreEqual(actual.Type, expected.Type);
            Assert.AreEqual(actual.Url, expected.Url);
        }

        /// <summary>
        /// Compares two shapes.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(Shape actual, Shape expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.DistanceTravelled, expected.DistanceTravelled);
            Assert.AreEqual(actual.Id, expected.Id);
            Assert.AreEqual(actual.Latitude, expected.Latitude);
            Assert.AreEqual(actual.Longitude, expected.Longitude);
            Assert.AreEqual(actual.Sequence, expected.Sequence);
        }

        /// <summary>
        /// Compares two stops.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(Stop actual, Stop expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.Code, expected.Code);
            Assert.AreEqual(actual.Description, expected.Description);
            Assert.AreEqual(actual.Id, expected.Id);
            Assert.AreEqual(actual.Latitude, expected.Latitude);
            Assert.AreEqual(actual.LocationType, expected.LocationType);
            Assert.AreEqual(actual.Longitude, expected.Longitude);
            Assert.AreEqual(actual.Name, expected.Name);
            Assert.AreEqual(actual.ParentStation, expected.ParentStation);
            Assert.AreEqual(actual.Timezone, expected.Timezone);
            Assert.AreEqual(actual.Url, expected.Url);
            Assert.AreEqual(actual.WheelchairBoarding, expected.WheelchairBoarding);
            Assert.AreEqual(actual.Zone, expected.Zone);
        }

        /// <summary>
        /// Compares two stoptimes.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(StopTime actual, StopTime expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.ArrivalTime, expected.ArrivalTime);
            Assert.AreEqual(actual.DepartureTime, expected.DepartureTime);
            Assert.AreEqual(actual.DropOffType, expected.DropOffType);
            Assert.AreEqual(actual.PickupType, expected.PickupType);
            Assert.AreEqual(actual.ShapeDistTravelled, expected.ShapeDistTravelled);
            Assert.AreEqual(actual.StopHeadsign, expected.StopHeadsign);
            Assert.AreEqual(actual.StopId, expected.StopId);
            Assert.AreEqual(actual.StopSequence, expected.StopSequence);
            Assert.AreEqual(actual.TripId, expected.TripId);
        }

        /// <summary>
        /// Compares two transfers.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(Transfer actual, Transfer expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.FromStopId, expected.FromStopId);
            Assert.AreEqual(actual.MinimumTransferTime, expected.MinimumTransferTime);
            Assert.AreEqual(actual.ToStopId, expected.ToStopId);
            Assert.AreEqual(actual.TransferType, expected.TransferType);
        }

        /// <summary>
        /// Compares two trips.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void AreEqual(Trip actual, Trip expected)
        {
            if (actual == null)
            {
                Assert.IsNull(expected);
                return;
            }
            Assert.AreEqual(actual.AccessibilityType, expected.AccessibilityType);
            Assert.AreEqual(actual.BlockId, expected.BlockId);
            Assert.AreEqual(actual.Direction, expected.Direction);
            Assert.AreEqual(actual.Headsign, expected.Headsign);
            Assert.AreEqual(actual.Id, expected.Id);
            Assert.AreEqual(actual.RouteId, expected.RouteId);
            Assert.AreEqual(actual.ServiceId, expected.ServiceId);
            Assert.AreEqual(actual.ShapeId, expected.ShapeId);
            Assert.AreEqual(actual.ShortName, expected.ShortName);
        }
    }
}
