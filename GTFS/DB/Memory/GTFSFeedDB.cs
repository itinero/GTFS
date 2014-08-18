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
using System.Collections.Generic;
using System.Linq;

namespace GTFS.DB.Memory
{
    /// <summary>
    /// A naive in-memory implementation of a GTFS feed db.
    /// </summary>
    public class GTFSFeedDB : IGTFSFeedDB
    {
        /// <summary>
        /// Holds all the feeds that have been added.
        /// </summary>
        private List<GTFSFeed> _feeds = new List<GTFSFeed>();

        /// <summary>
        /// Adds a new feed.
        /// </summary>
        /// <param name="feed">The feed to add.</param>
        /// <returns>The id of the new feed.</returns>
        public int AddFeed(GTFSFeed feed)
        {
            int newId = _feeds.Count;
            _feeds.Add(feed);
            return newId;
        }

        /// <summary>
        /// Removes the feed with the given id.
        /// </summary>
        /// <param name="id">The id of the feed to remove.</param>
        /// <returns></returns>
        public bool RemoveFeed(int id)
        {
            if(id < _feeds.Count && _feeds[id] != null)
            {
                _feeds[id] = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns an enumerable of all non-null feeds.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<GTFSFeed> GetNonNullFeeds()
        {
            return _feeds.Where((x) => { return x != null; });
        }

        /// <summary>
        /// Returns all the agencies.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Agency> GetAgencies()
        {
            IEnumerable<Agency> entities = new List<Agency>();
            foreach(var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.Agencies);
            }
            return entities;
        }

        /// <summary>
        /// Returns all calendars.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Calendar> GetCalendars()
        {
            IEnumerable<Calendar> entities = new List<Calendar>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.Calendars);
            }
            return entities;
        }
        
        /// <summary>
        /// Returns all calendar dates.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CalendarDate> GetCalendardDates()
        {
            IEnumerable<CalendarDate> entities = new List<CalendarDate>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.CalendarDates);
            }
            return entities;
        }

        /// <summary>
        /// Returns all fare attributes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FareAttribute> GetFareAttributes()
        {
            IEnumerable<FareAttribute> entities = new List<FareAttribute>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.FareAttributes);
            }
            return entities;
        }

        /// <summary>
        /// Returns all calendar dates.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FareRule> GetFareRules()
        {
            IEnumerable<FareRule> entities = new List<FareRule>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.FareRules);
            }
            return entities;
        }

        /// <summary>
        /// Returns all feed infos.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FeedInfo> GetFeedInfo()
        {
            IEnumerable<FeedInfo> entities = new List<FeedInfo>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.FeedInfo);
            }
            return entities;
        }

        /// <summary>
        /// Returns all frequencies.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Frequency> GetFrequencies()
        {
            IEnumerable<Frequency> entities = new List<Frequency>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.Frequencies);
            }
            return entities;
        }

        /// <summary>
        /// Returns all routes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Route> GetRoutes()
        {
            IEnumerable<Route> entities = new List<Route>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.Routes);
            }
            return entities;
        }

        /// <summary>
        /// Returns all shapes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Shape> GetShapes()
        {
            IEnumerable<Shape> entities = new List<Shape>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.Shapes);
            }
            return entities;
        }

        /// <summary>
        /// Returns all stops.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stop> GetStops()
        {
            IEnumerable<Stop> entities = new List<Stop>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.Stops);
            }
            return entities;
        }

        /// <summary>
        /// Returns all stop times.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> GetStopTimes()
        {
            IEnumerable<StopTime> entities = new List<StopTime>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.StopTimes);
            }
            return entities;
        }

        /// <summary>
        /// Returns all transfers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Transfer> GetTransfers()
        {
            IEnumerable<Transfer> entities = new List<Transfer>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.Transfers);
            }
            return entities;
        }

        /// <summary>
        /// Returns all trips.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Trip> GetTrips()
        {
            IEnumerable<Trip> entities = new List<Trip>();
            foreach (var feed in this.GetNonNullFeeds())
            {
                entities = entities.Concat(feed.Trips);
            }
            return entities;
        }
    }
}
