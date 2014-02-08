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
                case "shape":
                    this.Read<Shape>(file, feed, this.ParseShape, feed.Shapes);
                    break;
                case "stop":
                    this.Read<Stop>(file, feed, this.ParseStop, feed.Stops);
                    break;
                case "transfer":
                    this.Read<StopTime>(file, feed, this.ParseStopTime, feed.StopTimes);
                    break;
                case "trip":
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a stop row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Stop ParseStop(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a stop time row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual StopTime ParseStopTime(Feed feed, GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
    }
}