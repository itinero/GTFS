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
using System.Data.Common;
using System.Linq;
using System.Text;

namespace GTFS.DB
{
    /// <summary>
    /// An abstract representation of a GTFS feed db.
    /// </summary>
    public interface IGTFSFeedDB
    {
        /// <summary>
        /// Adds a new feed to this db.
        /// </summary>
        /// <returns></returns>
        int AddFeed();

        /// <summary>
        /// Adds an existing GTFS feed to this db.
        /// </summary>
        /// <param name="feed">The feed to add.</param>
        /// <returns>The id of the new feed.</returns>
        int AddFeed(IGTFSFeed feed);

        /// <summary>
        /// Removes the feed with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveFeed(int id);

        /// <summary>
        /// Returns all feeds.
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> GetFeeds();

        /// <summary>
        /// Returns the feed with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IGTFSFeed GetFeed(int id);

        /// <summary>
        /// Checks if a table with the given name exists in this database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool TableExists(string tableName);

        /// <summary>
        /// Checks if the given table contains a column with the given name.
        /// </summary>
        /// <param name="tableName">The table in this database to check.</param>
        /// <param name="columnName">The column in the given table to look for.</param>
        /// <returns>True if the given table contains a column with the given name.</returns>
        bool ColumnExists(string tableName, string columnName);

        /// <summary>
        /// Returns the data source of the DB
        /// </summary>
        string GetFullDataSource();

        /// <summary>
        /// Returns the connection string
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// The Tag for this DB
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Sort all tables in the DB
        /// </summary>
        void SortAllTables();

        /// <summary>
        /// Deletes and recreates the routes table in a sorted order - may take time
        /// </summary>
        void SortRoutes();

        /// <summary>
        /// Deletes and recreates the trips table in a sorted order - may take time
        /// </summary>
        void SortTrips();

        /// <summary>
        /// Deletes and recreates the stops table in a sorted order - may take time
        /// </summary>
        void SortStops();

        /// <summary>
        /// Deletes and recreates the stop_times table in a sorted order - may take time
        /// </summary>
        void SortStopTimes();

        /// <summary>
        /// Deletes and recreates the frequencies table in a sorted order - may take time
        /// </summary>
        void SortFrequencies();

        /// <summary>
        /// Deletes and recreates the calendars table in a sorted order (first by date then by exception_type) - may take time
        /// </summary>
        void SortCalendars();

        /// <summary>
        /// Deletes and recreates the calendar_dates table in a sorted order (first by date then by exception_type) - may take time
        /// </summary>
        void SortCalendarDates();

        /// <summary>
        /// Deletes and recreates the shapes table in a sorted order (first by id then by sequence) - will take time
        /// </summary>
        void SortShapes();
    }
}