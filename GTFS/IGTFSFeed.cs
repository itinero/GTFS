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

namespace GTFS
{
    /// <summary>
    /// Abstract representation of a GTFSFeed.
    /// </summary>
    /// <remarks>To be used as a proxy to load data into memory/a database/...</remarks>
    public interface IGTFSFeed
    {
        /// <summary>
        /// Adds a new agency.
        /// </summary>
        /// <param name="agency"></param>
        void AddAgency(Agency agency);

        /// <summary>
        /// Adds a new calendar.
        /// </summary>
        /// <param name="calendar"></param>
        void AddCalendar(Calendar calendar);

        /// <summary>
        /// Adds a new calendar date.
        /// </summary>
        /// <param name="calendar"></param>
        void AddCalendarDate(CalendarDate calendar);

        /// <summary>
        /// Adds a new fare attribute.
        /// </summary>
        /// <param name="fareAttribute"></param>
        void AddFareAttribute(FareAttribute fareAttribute);

        /// <summary>
        /// Adds a new fare rule.
        /// </summary>
        /// <param name="fareRule"></param>
        void AddFareRule(FareRule fareRule);

        /// <summary>
        /// Adds new feed info.
        /// </summary>
        /// <param name="feedInfo"></param>
        void AddFeedInfo(FeedInfo feedInfo);

        /// <summary>
        /// Adds a new frequency.
        /// </summary>
        /// <param name="frequency"></param>
        void AddFrequency(Frequency frequency);

        /// <summary>
        /// Adds a new route.
        /// </summary>
        /// <param name="route"></param>
        void AddRoute(Route route);

        /// <summary>
        /// Adds a new shape.
        /// </summary>
        /// <param name="shape"></param>
        void AddShape(Shape shape);

        /// <summary>
        /// Adds a new stop.
        /// </summary>
        /// <param name="stop"></param>
        void AddStop(Stop stop);

        /// <summary>
        /// Adds a new stop time.
        /// </summary>
        /// <param name="stopTime"></param>
        void AddStopTime(StopTime stopTime);

        /// <summary>
        /// Adds a new transfer.
        /// </summary>
        /// <param name="transfer"></param>
        void AddTransfer(Transfer transfer);

        /// <summary>
        /// Adds a new trip.
        /// </summary>
        /// <param name="trip"></param>
        void AddTrip(Trip trip);
    }
}