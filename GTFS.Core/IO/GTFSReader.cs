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
        public Feed Read(IGTFSSource source)
        {
            // create the feed.
            var feed = new Feed();

            foreach(var file in source.Files)
            { // read each file.
                this.Read(file, feed);
            }

            return feed;            
        }

        /// <summary>
        /// A delegate for parsing methods per entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private delegate T EntityParseDelegate<T>(GTFSSourceFileHeader header, string[] data)
            where T : GTFSEntity;

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
                    this.Read<Agency>(file, this.ParseAgency, feed.Agencies);
                    break;
                case "calendar":
                    this.Read<Calendar>(file, this.ParseCalender, feed.Calendars);
                    break;
                case "calendar_date":
                    this.Read<CalendarDate>(file, this.ParseCalendarDate, feed.CalendarDates);
                    break;
                case "fare_attribute":
                    this.Read<FareAttribute>(file, this.ParseFareAttribute, feed.FareAttributes);
                    break;
                case "fare_rule":
                    this.Read<FareRule>(file, this.ParseFareRule, feed.FareRules);
                    break;
                case "feed_info":
                    this.Read<FeedInfo>(file, this.ParseFeedInfo, feed.FeedInfo);
                    break;
                case "route_file":
                    this.Read<Frequency>(file, this.ParseFrequency, feed.Frequencies);
                    break;
                case "shape":
                    this.Read<Shape>(file, this.ParseShape, feed.Shapes);
                    break;
                case "stop":
                    this.Read<Stop>(file, this.ParseStop, feed.Stops);
                    break;
                case "transfer":
                    this.Read<StopTime>(file, this.ParseStopTime, feed.StopTimes);
                    break;
                case "trip":
                    this.Read<Trip>(file, this.ParseTrip, feed.Trips);
                    break;
            }
        }

        /// <summary>
        /// Reads the agency file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void Read<T>(IGTFSSourceFile file, EntityParseDelegate<T> parser, List<T> list)
            where T : GTFSEntity
        {
            // enumerate all lines.
            var enumerator = file.GetEnumerator();
            if(!enumerator.MoveNext())
            { // there is no data, and if there is move to the columns.
                return;
            }

            // read the header.
            var header = new GTFSSourceFileHeader(enumerator.Current);

            // read fields.
            while (enumerator.MoveNext())
            {
                list.Add(parser.Invoke(header, enumerator.Current));
            }
        }

        /// <summary>
        /// Parses an agency row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Agency ParseAgency(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a calendar row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Calendar ParseCalender(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a calendar date row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual CalendarDate ParseCalendarDate(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a fare attribute row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual FareAttribute ParseFareAttribute(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a fare rule row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual FareRule ParseFareRule(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a feed info row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual FeedInfo ParseFeedInfo(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a frequency row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Frequency ParseFrequency(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a route row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Route ParseRoute(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a shapte row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Shape ParseShape(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a stop row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Stop ParseStop(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a stop time row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual StopTime ParseStopTime(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a transfer row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Transfer ParseTransfer(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a trip row.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Trip ParseTrip(GTFSSourceFileHeader header, string[] data)
        {
            throw new NotImplementedException();
        }
    }
}