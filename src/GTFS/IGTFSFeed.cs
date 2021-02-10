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
using GTFS.Entities.Collections;
using System.Collections.Generic;

namespace GTFS
{
    /// <summary>
    /// Abstract representation of a GTFSFeed.
    /// </summary>
    /// <remarks>To be used as a proxy to load data into memory/a database/...</remarks>
    public interface IGTFSFeed
    {
        /// <summary>
        /// Adds new feed info.
        /// </summary>
        /// <param name="feedInfo"></param>
        void SetFeedInfo(FeedInfo feedInfo);

        /// <summary>
        /// Gets the feed info.
        /// </summary>
        /// <returns></returns>
        FeedInfo GetFeedInfo();

        /// <summary>
        /// Gets the collection of agencies.
        /// </summary>
        IUniqueEntityCollection<Attribution> Attributions
        {
            get;
        }

        /// <summary>
        /// Gets the collection of agencies.
        /// </summary>
        IUniqueEntityCollection<Agency> Agencies
        {
            get;
        }

        /// <summary>
        /// Gets the collection of calendars.
        /// </summary>
        IEntityCollection<Calendar> Calendars
        {
            get;
        }

        /// <summary>
        /// Gets the collection of calendar dates.
        /// </summary>
        IEntityCollection<CalendarDate> CalendarDates
        {
            get;
        }

        /// <summary>
        /// Gets the collection of fare attributes.
        /// </summary>
        IEntityCollection<FareAttribute> FareAttributes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of fare rules.
        /// </summary>
        IUniqueEntityCollection<FareRule> FareRules
        {
            get;
        }

        /// <summary>
        /// Gets the collection of frequencies.
        /// </summary>
        IEntityCollection<Frequency> Frequencies
        {
            get;
        }

        /// <summary>
        /// Gets the collection of the routes.
        /// </summary>
        IUniqueEntityCollection<Route> Routes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of shapes.
        /// </summary>
        IEntityCollection<Shape> Shapes
        {
            get;
        }
        
        /// <summary>
        /// Gets the collection of stops.
        /// </summary>
        IUniqueEntityCollection<Stop> Stops
        {
            get;
        }

        /// <summary>
        /// Gets the collection of stop times.
        /// </summary>
        IStopTimeCollection StopTimes
        {
            get;
        }

        /// <summary>
        /// Gets the collection of transfers.
        /// </summary>
        ITransferCollection Transfers
        {
            get;
        }

        /// <summary>
        /// Gets the collection of trips.
        /// </summary>
        IUniqueEntityCollection<Trip> Trips
        {
            get;
        }

        /// <summary>
        /// Gets the collection of levels.
        /// </summary>
        IUniqueEntityCollection<Level> Levels
        {
            get;
        }

        /// <summary>
        /// Gets the collection of pathways.
        /// </summary>
        IUniqueEntityCollection<Pathway> Pathways
        {
            get;
        }
    }
}