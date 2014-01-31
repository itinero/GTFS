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
using System.Collections.Generic;
namespace GTFS.Core.IO
{
    /// <summary>
    /// A GTFS reader.
    /// </summary>
    public class GTFSReader
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

            // read the files.
            this.ReadAgency(source.AgencyFile, feed.Agencies);
            this.ReadCalendar(source.CalendarFile, feed.Calendars);
            this.ReadCalendarDate(source.CalendarDateFile, feed.CalendarDates);
            this.ReadFareAttribute(source.FareAttributeFile, feed.FareAttributes);
            this.ReadFareRule(source.FareRuleFile, feed.FareRules);
            this.ReadFeedInfo(source.FeedInfoFile, feed.FeedInfo);
            this.ReadFrequency(source.FrequencyFile, feed.Frequencies);
            this.ReadRoute(source.RouteFile, feed.Routes);
            this.ReadShape(source.ShapeFile, feed.Shapes);
            this.ReadStop(source.StopFile, feed.Stops);
            this.ReadStopTime(source.StopTimeFile, feed.StopTimes);
            this.ReadTransfer(source.TransferFile, feed.Transfers);
            this.ReadTrip(source.TripFile, feed.Trips);

            return feed;            
        }

        /// <summary>
        /// Reads the agency file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadAgency(IGTFSSourceFile file, List<Agency> list)
        {

        }

        /// <summary>
        /// Reads the calendar file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadCalendar(IGTFSSourceFile file, List<Calendar> list)
        {

        }

        /// <summary>
        /// Reads the calender date file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadCalendarDate(IGTFSSourceFile file, List<CalendarDate> list)
        {

        }

        /// <summary>
        /// Reads the fare attribute file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadFareAttribute(IGTFSSourceFile file, List<FareAttribute> list)
        {

        }

        /// <summary>
        /// Reads the trip file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadTrip(IGTFSSourceFile file, List<Trip> list)
        {

        }

        /// <summary>
        /// Reads the transfer file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadTransfer(IGTFSSourceFile file, List<Transfer> list)
        {

        }

        /// <summary>
        /// Reads the stop times file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadStopTime(IGTFSSourceFile file, List<StopTime> list)
        {

        }

        /// <summary>
        /// Reads the stops file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadStop(IGTFSSourceFile file, List<Stop> list)
        {

        }

        /// <summary>
        /// Reads the shape file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadShape(IGTFSSourceFile file, List<Shape> list)
        {

        }

        /// <summary>
        /// Reads the route file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadRoute(IGTFSSourceFile file, List<Route> list)
        {

        }

        /// <summary>
        /// Reads the frequency file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadFrequency(IGTFSSourceFile file, List<Frequency> list)
        {

        }

        /// <summary>
        /// Returns the feed info file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="feedInfo"></param>
        private void ReadFeedInfo(IGTFSSourceFile file, FeedInfo feedInfo)
        {

        }

        /// <summary>
        /// Reads the fare rule file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        private void ReadFareRule(IGTFSSourceFile file, List<FareRule> list)
        {

        }
    }
}