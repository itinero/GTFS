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
using GTFS.Entities.Enumerations;
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
    /// Contains tests for the directory source.
    /// </summary>
    [TestFixture]
    public class DirectorySourceTest
    {
        /// <summary>
        /// Builds the source from embedded streams.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IGTFSSourceFile> BuildSource()
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
        /// Tests parsing agencies.
        /// </summary>
        [Test]
        public void ParseAgencies()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("agency")));

            // test result.
            Assert.IsNotNull(feed.Agencies);
            var agencies = new List<Agency>(feed.Agencies);
            Assert.AreEqual(1, agencies.Count);
            Assert.AreEqual(null, agencies[0].FareURL);
            Assert.AreEqual("DTA", agencies[0].Id);
            Assert.AreEqual(null, agencies[0].LanguageCode);
            Assert.AreEqual("Demo Transit Authority", agencies[0].Name);
            Assert.AreEqual(null, agencies[0].Phone);
            Assert.AreEqual("America/Los_Angeles", agencies[0].Timezone);
            Assert.AreEqual("http://google.com", agencies[0].URL);
        }

        /// <summary>
        /// Tests parsing routes.
        /// </summary>
        [Test]
        public void ParseRoutes()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("routes")));

            // test result.
            Assert.IsNotNull(feed.Routes);
            var routes = feed.Routes.ToList();
            Assert.AreEqual(5, routes.Count);

            //route_id,agency_id,route_short_name,route_long_name,route_desc,route_type,route_url,route_color,route_text_color

            //AB,DTA,10,Airport - Bullfrog,,3,,,
            int idx = 0;
            Assert.AreEqual("AB", routes[idx].Id);
            Assert.AreEqual("DTA", routes[idx].AgencyId);
            Assert.AreEqual("10", routes[idx].ShortName);
            Assert.AreEqual("Airport - Bullfrog", routes[idx].LongName);
            Assert.AreEqual(string.Empty, routes[idx].Description);
            Assert.AreEqual(RouteType.BusService, routes[idx].Type);
            Assert.AreEqual(-3932017, routes[idx].Color);
            Assert.AreEqual(null, routes[idx].TextColor);

            //BFC,DTA,20,Bullfrog - Furnace Creek Resort,,3,,,
            idx = 1;
            Assert.AreEqual("BFC", routes[idx].Id);
            Assert.AreEqual("DTA", routes[idx].AgencyId);
            Assert.AreEqual("20", routes[idx].ShortName);
            Assert.AreEqual("Bullfrog - Furnace Creek Resort", routes[idx].LongName);
            Assert.AreEqual(string.Empty, routes[idx].Description);
            Assert.AreEqual(RouteType.BusService, routes[idx].Type);
            Assert.AreEqual(-1, routes[idx].Color);
            Assert.AreEqual(null, routes[idx].TextColor);

            //STBA,DTA,30,Stagecoach - Airport Shuttle,,3,,,
            idx = 2;
            Assert.AreEqual("STBA", routes[idx].Id);
            Assert.AreEqual("DTA", routes[idx].AgencyId);
            Assert.AreEqual("30", routes[idx].ShortName);
            Assert.AreEqual("Stagecoach - Airport Shuttle", routes[idx].LongName);
            Assert.AreEqual(string.Empty, routes[idx].Description);
            Assert.AreEqual(RouteType.BusService, routes[idx].Type);
            Assert.AreEqual(null, routes[idx].Color);
            Assert.AreEqual(null, routes[idx].TextColor);

            //CITY,DTA,40,City,,3,,,
            idx = 3;
            Assert.AreEqual("CITY", routes[idx].Id);
            Assert.AreEqual("DTA", routes[idx].AgencyId);
            Assert.AreEqual("40", routes[idx].ShortName);
            Assert.AreEqual("City", routes[idx].LongName);
            Assert.AreEqual(string.Empty, routes[idx].Description);
            Assert.AreEqual(RouteType.BusService, routes[idx].Type);
            Assert.AreEqual(null, routes[idx].Color);
            Assert.AreEqual(null, routes[idx].TextColor);

            //AAMV,DTA,50,Airport - Amargosa Valley,,3,,,
            idx = 4;
            Assert.AreEqual("AAMV", routes[idx].Id);
            Assert.AreEqual("DTA", routes[idx].AgencyId);
            Assert.AreEqual("50", routes[idx].ShortName);
            Assert.AreEqual("Airport - Amargosa Valley", routes[idx].LongName);
            Assert.AreEqual(string.Empty, routes[idx].Description);
            Assert.AreEqual(RouteType.BusService, routes[idx].Type);
            Assert.AreEqual(null, routes[idx].Color);
            Assert.AreEqual(null, routes[idx].TextColor);
        }

        /// <summary>
        /// Tests parsing shapes.
        /// </summary>
        [Test]
        public void ParseShapes()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("shapes")));

            // test result.
            Assert.IsNotNull(feed.Shapes);
            var shapes = feed.Shapes.ToList();
            Assert.AreEqual(44, shapes.Count);

            // @ 1: shape_id,shape_pt_lat,shape_pt_lon,shape_pt_sequence,shape_dist_traveled
            // @ 2: shape_1,37.754211,-122.197868,1,
            int idx = 0;
            Assert.AreEqual("shape_1", shapes[idx].Id);
            Assert.AreEqual(37.754211, shapes[idx].Latitude);
            Assert.AreEqual(-122.197868, shapes[idx].Longitude);
            Assert.AreEqual(1, shapes[idx].Sequence);
            Assert.AreEqual(null, shapes[idx].DistanceTravelled);

            // @ 10: shape_3,37.73645,-122.19706,1,
            idx = 8;
            Assert.AreEqual("shape_3", shapes[idx].Id);
            Assert.AreEqual(37.73645, shapes[idx].Latitude);
            Assert.AreEqual(-122.19706, shapes[idx].Longitude);
            Assert.AreEqual(1, shapes[idx].Sequence);
            Assert.AreEqual(null, shapes[idx].DistanceTravelled);
        }

        /// <summary>
        /// Tests parsing trips.
        /// </summary>
        [Test]
        public void ParseTrips()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>(false);

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("trips")));

            // test result.
            Assert.IsNotNull(feed.Trips);
            var trips = feed.Trips.ToList();
            Assert.AreEqual(11, trips.Count);

            // @ 1: route_id,service_id,trip_id,trip_headsign,direction_id,block_id,shape_id
            // @ 2: AB,FULLW,AB1,to Bullfrog,0,1,shape_1
            int idx = 0;
            Assert.AreEqual("AB", trips[idx].RouteId);
            Assert.AreEqual("FULLW", trips[idx].ServiceId);
            Assert.AreEqual("AB1", trips[idx].Id);
            Assert.AreEqual("to Bullfrog", trips[idx].Headsign);
            Assert.AreEqual(DirectionType.OneDirection, trips[idx].Direction);
            Assert.AreEqual("1", trips[idx].BlockId);
            Assert.AreEqual("shape_1", trips[idx].ShapeId);

            // @ 10: BFC,FULLW,BFC1,to Furnace Creek Resort,0,1,shape_6
            idx = 5;
            Assert.AreEqual("BFC", trips[idx].RouteId);
            Assert.AreEqual("FULLW", trips[idx].ServiceId);
            Assert.AreEqual("BFC1", trips[idx].Id);
            Assert.AreEqual("to Furnace Creek Resort", trips[idx].Headsign);
            Assert.AreEqual(DirectionType.OneDirection, trips[idx].Direction);
            Assert.AreEqual("1", trips[idx].BlockId);
            Assert.AreEqual("shape_6", trips[idx].ShapeId);
        }

        /// <summary>
        /// Tests parsing stops.
        /// </summary>
        [Test]
        public void ParseStops()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("stops")));

            // test result.
            Assert.IsNotNull(feed.Stops);
            var stops = feed.Stops.ToList();
            Assert.AreEqual(9, stops.Count);

            // @ 1: stop_id,stop_name,stop_desc,stop_lat,stop_lon,zone_id,stop_url
            // @ 2: FUR_CREEK_RES,Furnace Creek Resort (Demo),,36.425288,-117.133162,,
            int idx = 0;
            Assert.AreEqual("FUR_CREEK_RES", stops[idx].Id);
            Assert.AreEqual("Furnace Creek Resort (Demo)", stops[idx].Name);
            Assert.AreEqual(string.Empty, stops[idx].Description);
            Assert.AreEqual(36.425288, stops[idx].Latitude);
            Assert.AreEqual(-117.133162, stops[idx].Longitude);
            Assert.AreEqual(string.Empty, stops[idx].Url);

            // @ 10: AMV,Amargosa Valley (Demo),,36.641496,-116.40094,,
            idx = 8;
            Assert.AreEqual("AMV", stops[idx].Id);
            Assert.AreEqual("Amargosa Valley (Demo)", stops[idx].Name);
            Assert.AreEqual(string.Empty, stops[idx].Description);
            Assert.AreEqual(36.641496, stops[idx].Latitude);
            Assert.AreEqual(-116.40094, stops[idx].Longitude);
            Assert.AreEqual(string.Empty, stops[idx].Url);
        }

        /// <summary>
        /// Tests parsing stops.
        /// </summary>
        [Test]
        public void ParseStopTimes()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>(false);

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("stop_times")));

            // test result.
            Assert.IsNotNull(feed.StopTimes);
            var stopTimes = feed.StopTimes.ToList();
            Assert.AreEqual(28, stopTimes.Count);

            // @ 1: trip_id,arrival_time,departure_time,stop_id,stop_sequence,stop_headsign,pickup_type,drop_off_time,shape_dist_traveled
            // @ SORTED: AAMV1,8:00:00,8:00:00,BEATTY_AIRPORT,1
            int idx = 0;
            Assert.AreEqual("AAMV1", stopTimes[idx].TripId);
            Assert.AreEqual(new TimeOfDay() { Hours = 8 }, stopTimes[idx].ArrivalTime);
            Assert.AreEqual(new TimeOfDay() { Hours = 8 }, stopTimes[idx].DepartureTime);
            Assert.AreEqual("BEATTY_AIRPORT", stopTimes[idx].StopId);
            Assert.AreEqual(1, stopTimes[idx].StopSequence);
            Assert.IsTrue(string.IsNullOrWhiteSpace(stopTimes[idx].StopHeadsign));
            Assert.AreEqual(null, stopTimes[idx].PickupType);
            Assert.AreEqual(null, stopTimes[idx].DropOffType);
            Assert.AreEqual(null, stopTimes[idx].ShapeDistTravelled);
            Assert.AreEqual(TimePointType.None, stopTimes[idx].TimepointType);

            // @ SORTED: CITY1,6:00:00,6:00:00,STAGECOACH,1,,,,,1
            idx = 16;
            Assert.AreEqual("CITY1", stopTimes[idx].TripId);
            Assert.AreEqual(new TimeOfDay() { Hours = 6, Minutes = 00 }, stopTimes[idx].ArrivalTime);
            Assert.AreEqual(new TimeOfDay() { Hours = 6, Minutes = 00 }, stopTimes[idx].DepartureTime);
            Assert.AreEqual("STAGECOACH", stopTimes[idx].StopId);
            Assert.AreEqual(1, stopTimes[idx].StopSequence);
            Assert.AreEqual(string.Empty, stopTimes[idx].StopHeadsign);
            Assert.AreEqual(null, stopTimes[idx].PickupType);
            Assert.AreEqual(null, stopTimes[idx].DropOffType);
            Assert.AreEqual(null, stopTimes[idx].ShapeDistTravelled);
            Assert.AreEqual(TimePointType.Exact, stopTimes[idx].TimepointType);

            // @ SORTED: CITY1,,,NANAA,2,,,,,0
            idx = 17;
            Assert.AreEqual("CITY1", stopTimes[idx].TripId);
            Assert.AreEqual(new TimeOfDay() { Hours = 0, Minutes = 0, Seconds = 0 }, stopTimes[idx].ArrivalTime);
            Assert.AreEqual(new TimeOfDay() { Hours = 0, Minutes = 0, Seconds = 0 }, stopTimes[idx].DepartureTime);
            Assert.AreEqual("NANAA", stopTimes[idx].StopId);
            Assert.AreEqual(2, stopTimes[idx].StopSequence);
            Assert.AreEqual(string.Empty, stopTimes[idx].StopHeadsign);
            Assert.AreEqual(null, stopTimes[idx].PickupType);
            Assert.AreEqual(null, stopTimes[idx].DropOffType);
            Assert.AreEqual(null, stopTimes[idx].ShapeDistTravelled);
            Assert.AreEqual(TimePointType.Approximate, stopTimes[idx].TimepointType);

            // @ SORTED: STBA,6:20:00,6:20:00,BEATTY_AIRPORT,2,,,,
            idx = 27;
            Assert.AreEqual("STBA", stopTimes[idx].TripId);
            Assert.AreEqual(new TimeOfDay() { Hours = 6, Minutes = 20 }, stopTimes[idx].ArrivalTime);
            Assert.AreEqual(new TimeOfDay() { Hours = 6, Minutes = 20 }, stopTimes[idx].DepartureTime);
            Assert.AreEqual("BEATTY_AIRPORT", stopTimes[idx].StopId);
            Assert.AreEqual(2, stopTimes[idx].StopSequence);
            Assert.AreEqual(string.Empty, stopTimes[idx].StopHeadsign);
            Assert.AreEqual(null, stopTimes[idx].PickupType);
            Assert.AreEqual(null, stopTimes[idx].DropOffType);
            Assert.AreEqual(null, stopTimes[idx].ShapeDistTravelled);
            Assert.AreEqual(TimePointType.None, stopTimes[idx].TimepointType);
        }

        /// <summary>
        /// Tests parsing frequencies.
        /// </summary>
        [Test]
        public void ParseFrequencies()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>(false);

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("frequencies")));

            // test result.
            Assert.IsNotNull(feed.Frequencies);
            var frequencies = feed.Frequencies.ToList();
            Assert.AreEqual(11, frequencies.Count);

            // @ 1: trip_id,start_time,end_time,headway_secs
            // @ 2: STBA,6:00:00,22:00:00,1800

            // @ 1: route_id,service_id,trip_id,trip_headsign,direction_id,block_id,shape_id
            // @ 2: AB,FULLW,AB1,to Bullfrog,0,1,shape_1
            int idx = 0;
            Assert.AreEqual("STBA", frequencies[idx].TripId);
            Assert.AreEqual("6:00:00", frequencies[idx].StartTime);
            Assert.AreEqual("22:00:00", frequencies[idx].EndTime);
            Assert.AreEqual("1800", frequencies[idx].HeadwaySecs);
            Assert.AreEqual(null, frequencies[idx].ExactTimes);

            // @ 10: CITY2,16:00:00,18:59:59,600
            idx = 8;
            Assert.AreEqual("CITY2", frequencies[idx].TripId);
            Assert.AreEqual("16:00:00", frequencies[idx].StartTime);
            Assert.AreEqual("18:59:59", frequencies[idx].EndTime);
            Assert.AreEqual("600", frequencies[idx].HeadwaySecs);
            Assert.AreEqual(null, frequencies[idx].ExactTimes);
        }

        /// <summary>
        /// Tests parsing calendars.
        /// </summary>
        [Test]
        public void ParseCalendars()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("calendar")));

            // test result.
            Assert.IsNotNull(feed.Calendars);
            var calendars = feed.Calendars.ToList();
            Assert.AreEqual(2, calendars.Count);

            // @ 1: service_id,monday,tuesday,wednesday,thursday,friday,saturday,sunday,start_date,end_date
            // @ 2: FULLW,1,1,1,1,1,1,1,20070101,20101231
            int idx = 0;
            Assert.AreEqual("FULLW", calendars[idx].ServiceId);
            Assert.AreEqual(true, calendars[idx].Monday);
            Assert.AreEqual(true, calendars[idx].Tuesday);
            Assert.AreEqual(true, calendars[idx].Wednesday);
            Assert.AreEqual(true, calendars[idx].Thursday);
            Assert.AreEqual(true, calendars[idx].Friday);
            Assert.AreEqual(true, calendars[idx].Saturday);
            Assert.AreEqual(true, calendars[idx].Sunday);
            Assert.AreEqual(new DateTime(2007, 01, 01), calendars[idx].StartDate);
            Assert.AreEqual(new DateTime(2010, 12, 31), calendars[idx].EndDate);

            // @3: WE,0,0,0,0,0,1,1,20070101,20101231
            idx = 1;
            Assert.AreEqual("WE", calendars[idx].ServiceId);
            Assert.AreEqual(false, calendars[idx].Monday);
            Assert.AreEqual(false, calendars[idx].Tuesday);
            Assert.AreEqual(false, calendars[idx].Wednesday);
            Assert.AreEqual(false, calendars[idx].Thursday);
            Assert.AreEqual(false, calendars[idx].Friday);
            Assert.AreEqual(true, calendars[idx].Saturday);
            Assert.AreEqual(true, calendars[idx].Sunday);
            Assert.AreEqual(new DateTime(2007, 01, 01), calendars[idx].StartDate);
            Assert.AreEqual(new DateTime(2010, 12, 31), calendars[idx].EndDate);
        }

        /// <summary>
        /// Tests parsing calendar dates.
        /// </summary>
        [Test]
        public void ParseCalendarDates()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("calendar_dates")));

            // test result.
            Assert.IsNotNull(feed.CalendarDates);
            var calendarDates = new List<CalendarDate>(feed.CalendarDates);
            Assert.AreEqual(1, calendarDates.Count);

            // @ 1: service_id,date,exception_type
            // @ 2: FULLW,20070604,2
            int idx = 0;
            Assert.AreEqual("FULLW", calendarDates[idx].ServiceId);
            Assert.AreEqual(new System.DateTime(2007, 06, 04), calendarDates[idx].Date);
            Assert.AreEqual(ExceptionType.Removed, calendarDates[idx].ExceptionType);
        }

        /// <summary>
        /// Tests parsing routes.
        /// </summary>
        [Test]
        public void ParseFareRules()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("fare_rules")));

            // test result.
            Assert.IsNotNull(feed.FareRules);
            var fareRules = feed.FareRules.ToList();
            Assert.AreEqual(4, fareRules.Count);

            // fare_id,route_id,origin_id,destination_id,contains_id

            //p,AB,,,
            int idx = 0;
            Assert.AreEqual("AB", fareRules[idx].RouteId);
            Assert.AreEqual("p", fareRules[idx].FareId);
            Assert.AreEqual(string.Empty, fareRules[idx].OriginId);
            Assert.AreEqual(string.Empty, fareRules[idx].DestinationId);
            Assert.AreEqual(string.Empty, fareRules[idx].ContainsId);

            //p,STBA,,,
            idx = 1;
            Assert.AreEqual("STBA", fareRules[idx].RouteId);
            Assert.AreEqual("p", fareRules[idx].FareId);
            Assert.AreEqual(string.Empty, fareRules[idx].OriginId);
            Assert.AreEqual(string.Empty, fareRules[idx].DestinationId);
            Assert.AreEqual(string.Empty, fareRules[idx].ContainsId);

            //p,BFC,,,
            idx = 2;
            Assert.AreEqual("BFC", fareRules[idx].RouteId);
            Assert.AreEqual("p", fareRules[idx].FareId);
            Assert.AreEqual(string.Empty, fareRules[idx].OriginId);
            Assert.AreEqual(string.Empty, fareRules[idx].DestinationId);
            Assert.AreEqual(string.Empty, fareRules[idx].ContainsId);

            //a,AAMV,,,
            idx = 3;
            Assert.AreEqual("AAMV", fareRules[idx].RouteId);
            Assert.AreEqual("a", fareRules[idx].FareId);
            Assert.AreEqual(string.Empty, fareRules[idx].OriginId);
            Assert.AreEqual(string.Empty, fareRules[idx].DestinationId);
            Assert.AreEqual(string.Empty, fareRules[idx].ContainsId);
        }

        /// <summary>
        /// Tests parsing routes.
        /// </summary>
        [Test]
        public void ParseFareAttributes()
        {
            // create the reader.
            GTFSReader<GTFSFeed> reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("fare_attributes")));

            // test result.
            Assert.IsNotNull(feed.FareAttributes);
            var fareAttributes = feed.FareAttributes.ToList();
            Assert.AreEqual(2, fareAttributes.Count);

            //fare_id,price,currency_type,payment_method,transfers,transfer_duration

            //p,1.25,USD,0,0,
            int idx = 0;
            Assert.AreEqual("p", fareAttributes[idx].FareId);
            Assert.AreEqual("1.25", fareAttributes[idx].Price);
            Assert.AreEqual("USD", fareAttributes[idx].CurrencyType);
            Assert.AreEqual(PaymentMethodType.OnBoard, fareAttributes[idx].PaymentMethod);
            Assert.AreEqual(0, fareAttributes[idx].Transfers);
            Assert.AreEqual(string.Empty, fareAttributes[idx].TransferDuration);

            //a,5.25,USD,0,0,
            idx = 1;
            Assert.AreEqual("a", fareAttributes[idx].FareId);
            Assert.AreEqual("5.25", fareAttributes[idx].Price);
            Assert.AreEqual("USD", fareAttributes[idx].CurrencyType);
            Assert.AreEqual(PaymentMethodType.OnBoard, fareAttributes[idx].PaymentMethod);
            Assert.AreEqual(0, fareAttributes[idx].Transfers);
            Assert.AreEqual(string.Empty, fareAttributes[idx].TransferDuration);
        }
    }
}