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

using GTFS.Core.Entities;
using GTFS.Core.Entities.Enumerations;
using GTFS.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace GTFS.Core.IO
{
    /// <summary>
    /// A GTFS reader.
    /// </summary>
    public class GTFSReader<T> where T : Feed
    {
        /// <summary>
        /// Reads the specified GTFS source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Feed Read(IGTFSSourceFile[] source)
        {
            // create the feed.
            var feed = new Feed();

            foreach (var file in source)
            { // read each file.
                this.Read(file, feed);
            }

            return feed;            
        }

        /// <summary>
        /// A delegate for parsing methods per entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        private delegate TEntity EntityParseDelegate<TEntity>(Feed feed, GTFSSourceFileHeader header, string[] data)
            where TEntity : GTFSEntity;

        /// <summary>
        /// Reads the given file and adds the result to the feed.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="feed"></param>
        protected virtual void Read(IGTFSSourceFile file, Feed feed)
        {
            switch(file.Name.ToLower())
            {
                case "agency":
                    this.Read<Agency>(file, feed, this.ParseAgency, feed.Agencies);
                    break;
                case "calendar":
                    this.Read<Calendar>(file, feed, this.ParseCalender, feed.Calendars);
                    break;
                case "calendar_date":
                    this.Read<CalendarDate>(file, feed, this.ParseCalendarDate, feed.CalendarDates);
                    break;
                case "fare_attribute":
                    this.Read<FareAttribute>(file, feed, this.ParseFareAttribute, feed.FareAttributes);
                    break;
                case "fare_rule":
                    this.Read<FareRule>(file, feed, this.ParseFareRule, feed.FareRules);
                    break;
                case "feed_info":
                    this.Read<FeedInfo>(file, feed, this.ParseFeedInfo, feed.FeedInfo);
                    break;
                case "routes":
                    this.Read<Route>(file, feed, this.ParseRoute, feed.Routes);
                    break;
                case "shapes":
                    this.Read<Shape>(file, feed, this.ParseShape, feed.Shapes);
                    break;
                case "stops":
                    this.Read<Stop>(file, feed, this.ParseStop, feed.Stops);
                    break;
                case "stop_times":
                    this.Read<StopTime>(file, feed, this.ParseStopTime, feed.StopTimes);
                    break;
                case "trips":
                    this.Read<Trip>(file, feed, this.ParseTrip, feed.Trips);
                    break;
            }
        }

        /// <summary>
        /// Reads the agency file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void Read<TEntity>(IGTFSSourceFile file, Feed feed, EntityParseDelegate<TEntity> parser, List<TEntity> list)
            where TEntity : GTFSEntity
        {
            // enumerate all lines.
            var enumerator = file.GetEnumerator();
            if(!enumerator.MoveNext())
            { // there is no data, and if there is move to the columns.
                return;
            }

            // read the header.
            var header = new GTFSSourceFileHeader(file.Name, enumerator.Current);

            // read fields.
            while (enumerator.MoveNext())
            {
                list.Add(parser.Invoke(feed, header, enumerator.Current));
            }
        }

        /// <summary>
        /// Parses an agency row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Agency ParseAgency(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, "agency_id");
            this.CheckRequiredField(header, header.Name, "agency_name");
            this.CheckRequiredField(header, header.Name, "agency_url");
            this.CheckRequiredField(header, header.Name, "agency_timezone");

            // parse/set all fields.
            Agency agency = new Agency();
            for(int idx = 0; idx < data.Length; idx++)
            {
                this.ParseAgencyField(header, agency, header.GetColumn(idx), data[idx]);
            }
            return agency;
        }

        /// <summary>
        /// Parses an agency field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseAgencyField(GTFSSourceFileHeader header, Agency agency, string fieldName, string value)
        {
            switch (fieldName)
            {
                case "agency_id":
                    agency.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_name":
                    agency.Name = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_lang":
                    agency.LanguageCode = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_phone":
                    agency.Phone = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_timezone":
                    agency.Timezone = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_url":
                    agency.URL = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Parses a calendar row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Calendar ParseCalender(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a calendar date row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual CalendarDate ParseCalendarDate(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a fare attribute row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual FareAttribute ParseFareAttribute(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a fare rule row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual FareRule ParseFareRule(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a feed info row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual FeedInfo ParseFeedInfo(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a frequency row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Frequency ParseFrequency(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a route row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Route ParseRoute(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, "route_id");
            this.CheckRequiredField(header, header.Name, "agency_id");
            this.CheckRequiredField(header, header.Name, "route_short_name");
            this.CheckRequiredField(header, header.Name, "route_long_name");
            this.CheckRequiredField(header, header.Name, "route_desc");
            this.CheckRequiredField(header, header.Name, "route_type");

            // parse/set all fields.
            Route route = new Route();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseRouteField(feed, header, route, header.GetColumn(idx), data[idx]);
            }
            return route;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="route"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseRouteField(Feed feed, GTFSSourceFileHeader header, Route route, string fieldName, string value)
        {
            switch (fieldName)
            {            
                case "route_id":
                    route.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_id":
                    string agencyId = this.ParseFieldString(header.Name, fieldName, value);
                    route.Agency = feed.GetAgency(agencyId);
                    if(route.Agency == null)
                    { // reference agency was not found!
                        throw new GTFSIntegrityException(header.Name, fieldName, value);
                    }
                    break;
                case "route_short_name":
                    route.ShortName = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_long_name":
                    route.LongName= this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_desc":
                    route.Description = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_type":
                    route.Type = this.ParseFieldRouteType(header.Name, fieldName, value);
                    break;
                case "route_url":
                    route.Url = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_color":
                    route.Color = this.ParseFieldColor(header.Name, fieldName, value);
                    break;
                case "route_text_color":
                    route.TextColor = this.ParseFieldColor(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Parses a shapte row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Shape ParseShape(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, "shape_id");
            this.CheckRequiredField(header, header.Name, "shape_pt_lat");
            this.CheckRequiredField(header, header.Name, "shape_pt_lon");
            this.CheckRequiredField(header, header.Name, "shape_pt_sequence");

            // parse/set all fields.
            Shape shape = new Shape();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseShapeField(feed, header, shape, header.GetColumn(idx), data[idx]);
            }
            return shape;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="shape"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseShapeField(Feed feed, GTFSSourceFileHeader header, Shape shape, string fieldName, string value)
        {
            switch (fieldName)
            {
                case "shape_id":
                    shape.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "shape_pt_lat":
                    shape.Latitude = this.ParseFieldDouble(header.Name, fieldName, value).Value;
                    break;
                case "shape_pt_lon":
                    shape.Longitude = this.ParseFieldDouble(header.Name, fieldName, value).Value;
                    break;
                case "shape_pt_sequence":
                    shape.Sequence = this.ParseFieldUInt(header.Name, fieldName, value).Value;
                    break;
                case "shape_dist_traveled":
                    shape.DistanceTravelled = this.ParseFieldDouble(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Parses a stop row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Stop ParseStop(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, "stop_id");
            this.CheckRequiredField(header, header.Name, "stop_name");
            this.CheckRequiredField(header, header.Name, "stop_lat");
            this.CheckRequiredField(header, header.Name, "stop_lon");

            // parse/set all fields.
            Stop stop = new Stop();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseStopField(feed, header, stop, header.GetColumn(idx), data[idx]);
            }
            return stop;
        }

        /// <summary>
        /// Parses a stop field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="stop"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseStopField(Feed feed, GTFSSourceFileHeader header, Stop stop, string fieldName, string value)
        {
            switch (fieldName)
            {
                case "stop_id":
                    stop.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_code":
                    stop.Code = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_name":
                    stop.Name = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_desc":
                    stop.Description = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_lat":
                    stop.Latitude = this.ParseFieldDouble(header.Name, fieldName, value).Value;
                    break;
                case "stop_lon":
                    stop.Longitude = this.ParseFieldDouble(header.Name, fieldName, value).Value;
                    break;
                case "zone_id":
                    stop.Zone = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_url":
                    stop.Url = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "location_type":
                    stop.LocationType = this.ParseFieldLocationType(header.Name, fieldName, value);
                    break;
                case "parent_station":
                    stop.ParentStation = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_timezone":
                    stop.Timezone = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case " wheelchair_boarding ":
                    stop.WheelchairBoarding = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Parses a stop time row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual StopTime ParseStopTime(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, "trip_id");
            this.CheckRequiredField(header, header.Name, "arrival_time");
            this.CheckRequiredField(header, header.Name, "departure_time");
            this.CheckRequiredField(header, header.Name, "stop_id");
            this.CheckRequiredField(header, header.Name, "stop_sequence");
            this.CheckRequiredField(header, header.Name, "stop_id");
            this.CheckRequiredField(header, header.Name, "stop_id");

            // parse/set all fields.
            StopTime stopTime = new StopTime();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseStopTimeField(feed, header, stopTime, header.GetColumn(idx), data[idx]);
            }
            return stopTime;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="trip"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseStopTimeField(Feed feed, GTFSSourceFileHeader header, StopTime stopTime, string fieldName, string value)
        {
            switch (fieldName)
            {
                case "trip_id":
                    string tripId = this.ParseFieldString(header.Name, fieldName, value);
                    stopTime.Trip = feed.GetTrip(tripId);
                    if (stopTime.Trip == null)
                    { // reference agency was not found!
                        throw new GTFSIntegrityException(header.Name, fieldName, value);
                    }
                    break;
                case "arrival_time":
                    stopTime.ArrivalTime = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "departure_time":
                    stopTime.DepartureTime = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_id":
                    string stopId = this.ParseFieldString(header.Name, fieldName, value);
                    stopTime.Stop = feed.GetStop(stopId);
                    if (stopTime.Trip == null)
                    { // reference agency was not found!
                        throw new GTFSIntegrityException(header.Name, fieldName, value);
                    }
                    break;
                case "stop_sequence":
                    stopTime.StopSequence = this.ParseFieldUInt(header.Name, fieldName, value).Value;
                    break;
                case "stop_headsign":
                    stopTime.StopHeadsign = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "pickup_type":
                    stopTime.PickupType = this.ParseFieldPickupType(header.Name, fieldName, value);
                    break;
                case "drop_off_type":
                    stopTime.DropOffType = this.ParseFieldDropOffType(header.Name, fieldName, value);
                    break;
                case "shape_dist_traveled":
                    stopTime.ShapeDistTravelled = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Parses a transfer row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Transfer ParseTransfer(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a trip row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Trip ParseTrip(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, "trip_id");
            this.CheckRequiredField(header, header.Name, "route_id");
            this.CheckRequiredField(header, header.Name, "service_id");
            this.CheckRequiredField(header, header.Name, "shape_pt_sequence");

            // parse/set all fields.
            Trip trip = new Trip();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseTripField(feed, header, trip, header.GetColumn(idx), data[idx]);
            }
            return trip;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="trip"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseTripField(Feed feed, GTFSSourceFileHeader header, Trip trip, string fieldName, string value)
        {
            switch (fieldName)
            {
                case "trip_id":
                    trip.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_id":
                    string routeId = this.ParseFieldString(header.Name, fieldName, value);
                    trip.Route = feed.GetRoute(routeId);
                    if(trip.Route == null)
                    { // reference agency was not found!
                        throw new GTFSIntegrityException(header.Name, fieldName, value);
                    }
                    break;
                case "service_id":
                    trip.ServiceId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "trip_headsign":
                    trip.Headsign = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "trip_short_name":
                    trip.ShortName = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "direction_id":
                    trip.Direction = this.ParseFieldDirectionType(header.Name, fieldName, value);
                    break;
                case "block_id":
                    trip.BlockId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "shape_id":
                    string shapeId = this.ParseFieldString(header.Name, fieldName, value);
                    trip.Shape = feed.GetShapes(shapeId);
                    break;
                case "wheelchair_accessible":
                    trip.AccessibilityType = this.ParseFieldAccessibilityType(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Checks if a required field is actually in the header.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="column"></param>
        protected virtual void CheckRequiredField(GTFSSourceFileHeader header, string name, string column)
        {
            if(!header.HasColumn(column))
            {
                throw new GTFSRequiredFieldMissingException(name, column);
            }
        }

        /// <summary>
        /// Parses a string-field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual string ParseFieldString(string name, string fieldName, string value)
        {
            // throw new GTFSParseException(name, fieldName, value);
            return value;
        }

        /// <summary>
        /// Parses a color field into an argb value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual int? ParseFieldColor(string name, string fieldName, string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            { // detect empty strings.
                return null;
            }

            int red = -1;
            int green = -1;
            int blue = -1;
            int alpha = 255;

            if(value.Length == 7)
            {
                try
                {
                    // a pre-defined RGB value.
                    string rString = value.Substring(1, 2);
                    string gString = value.Substring(3, 2);
                    string bString = value.Substring(5, 2);

                    red = int.Parse(rString);
                    green = int.Parse(gString);
                    blue = int.Parse(bString);
                }
                catch(Exception ex)
                {// hmm, some unknow exception, field not in correct format, give inner exception as a clue.
                    throw new GTFSParseException(name, fieldName, value, ex);
                }
            }
            else
            { // hmm, what kind of string is this going to be? if it is a color, augment the parser.
                throw new GTFSParseException(name, fieldName, value);
            }

            try
            {
                if ((alpha > 255) || (alpha < 0))
                {
                    // alpha out of range!
                    throw new ArgumentOutOfRangeException("alpha", "Value has to be in the range 0-255!");
                }
                if ((red > 255) || (red < 0))
                {
                    // red out of range!
                    throw new ArgumentOutOfRangeException("red", "Value has to be in the range 0-255!");
                }
                if ((green > 255) || (green < 0))
                {
                    // green out of range!
                    throw new ArgumentOutOfRangeException("green", "Value has to be in the range 0-255!");
                }
                if ((blue > 255) || (blue < 0))
                {
                    // red out of range!
                    throw new ArgumentOutOfRangeException("blue", "Value has to be in the range 0-255!");
                }
                return (int)((uint)alpha << 24) + (red << 16) + (green << 8) + blue;
            }
            catch (Exception ex)
            {// hmm, some unknow exception, field not in correct format, give inner exception as a clue.
                throw new GTFSParseException(name, fieldName, value, ex);
            }
        }

        /// <summary>
        /// Parses a route-type field.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual RouteType ParseFieldRouteType(string name, string fieldName, string value)
        {
            //0 - Tram, Streetcar, Light rail. Any light rail or street level system within a metropolitan area.
            //1 - Subway, Metro. Any underground rail system within a metropolitan area.
            //2 - Rail. Used for intercity or long-distance travel.
            //3 - Bus. Used for short- and long-distance bus routes.
            //4 - Ferry. Used for short- and long-distance boat service.
            //5 - Cable car. Used for street-level cable cars where the cable runs beneath the car.
            //6 - Gondola, Suspended cable car. Typically used for aerial cable cars where the car is suspended from the cable.
            //7 - Funicular. Any rail system designed for steep inclines.

            switch(value)
            {
                case "0":
                    return RouteType.Tram;
                case "1":
                    return RouteType.SubwayMetro;
                case "2":
                    return RouteType.Rail;
                case "3":
                    return RouteType.Bus;
                case "4":
                    return RouteType.Ferry;
                case "5":
                    return RouteType.CableCar;
                case "6":
                    return RouteType.Gondola;
                case "7":
                    return RouteType.Funicular;
            }
            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses an accessibility-type field.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private WheelchairAccessibilityType? ParseFieldAccessibilityType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            { // there is no value.
                return null;
            }

            //0 (or empty) - indicates that there is no accessibility information for the trip
            //1 - indicates that the vehicle being used on this particular trip can accommodate at least one rider in a wheelchair
            //2 - indicates that no riders in wheelchairs can be accommodated on this trip

            switch (value)
            {
                case "0":
                    return WheelchairAccessibilityType.NoInformation;
                case "1":
                    return WheelchairAccessibilityType.SomeAccessibility;
                case "2":
                    return WheelchairAccessibilityType.NoAccessibility;
            }
            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a drop-off-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private DropOffType? ParseFieldDropOffType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            { // there is no value.
                return null;
            }

            //0 - Regularly scheduled drop off
            //1 - No drop off available
            //2 - Must phone agency to arrange drop off
            //3 - Must coordinate with driver to arrange drop off

            switch (value)
            {
                case "0":
                    return DropOffType.Regular;
                case "1":
                    return DropOffType.NoPickup;
                case "2":
                    return DropOffType.PhoneForPickup;
                case "3":
                    return DropOffType.DriverForPickup;
            }
            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a pickup-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private PickupType? ParseFieldPickupType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            { // there is no value.
                return null;
            }

            //0 - Regularly scheduled pickup
            //1 - No pickup available
            //2 - Must phone agency to arrange pickup
            //3 - Must coordinate with driver to arrange pickup

            switch (value)
            {
                case "0":
                    return PickupType.Regular;
                case "1":
                    return PickupType.NoPickup;
                case "2":
                    return PickupType.PhoneForPickup;
                case "3":
                    return PickupType.DriverForPickup;
            }
            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a location-type field.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private LocationType? ParseFieldLocationType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            { // there is no value.
                return null;
            }
            
            //0 or blank - Stop. A location where passengers board or disembark from a transit vehicle.
            //1 - Station. A physical structure or area that contains one or more stop.

            switch (value)
            {
                case "0":
                    return LocationType.Stop;
                case "1":
                    return LocationType.Station;
            }
            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a direction-type field.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private DirectionType? ParseFieldDirectionType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            { // there is no value.
                return null;
            }

            //0 - travel in one direction (e.g. outbound travel)
            //1 - travel in the opposite direction (e.g. inbound travel)

            switch (value)
            {
                case "0":
                    return DirectionType.OneDirection;
                case "1":
                    return DirectionType.OppositeDirection;
            }
            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a positive integer field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual uint? ParseFieldUInt(string name, string fieldName, string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            { // there is no value.
                return null;
            }
            uint result;
            if(!uint.TryParse(value, out result))
            { // parsing failed!
                throw new GTFSParseException(name, fieldName, value);
            }
            return result;
        }

        /// <summary>
        /// Parses a double field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual double? ParseFieldDouble(string name, string fieldName, string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            { // there is no value.
                return null;
            }

            double result;
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result))
            { // parsing failed!
                throw new GTFSParseException(name, fieldName, value);
            }
            return result;
        }
    }
}