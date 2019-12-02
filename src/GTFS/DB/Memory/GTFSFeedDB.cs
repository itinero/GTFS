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
using System;
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
        private List<IGTFSFeed> _feeds = new List<IGTFSFeed>();

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public string ConnectionString => string.Empty;

        /// <summary>
        /// Adds a new empty feed to this db.
        /// </summary>
        /// <returns></returns>
        public int AddFeed()
        {
            return this.AddFeed(new GTFSFeed());
        }

        /// <summary>
        /// Adds a new feed.
        /// </summary>
        /// <param name="feed">The feed to add.</param>
        /// <returns>The id of the new feed.</returns>
        public int AddFeed(IGTFSFeed feed)
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
            if (id < _feeds.Count && _feeds[id] != null)
            {
                _feeds[id] = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns all feeds.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetFeeds()
        {
            return Enumerable.Range(0, _feeds.Count);
        }

        /// <summary>
        /// Returns the feed with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IGTFSFeed GetFeed(int id)
        {
            return _feeds[id];
        }

        /// <summary>
        /// Returns true if the given table exists.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool TableExists(string tableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns true if the given column in the given table exists.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ColumnExists(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the full data source.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public string GetFullDataSource()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sort all tables.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SortAllTables()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts routes.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SortRoutes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sort trips.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SortTrips()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts stops.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SortStops()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts stop times.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SortStopTimes()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Sorts frequencies.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SortFrequencies()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts calendars.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SortCalendars()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts calendar dates.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SortCalendarDates()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sort shapes.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SortShapes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets tag.
        /// </summary>
        public object Tag { get; set; }
    }
}