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

using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace GTFS.DB.SQLite
{
    /// <summary>
    /// Represents a database that contains GTFS feeds.
    /// </summary>
    public class SQLiteGTFSFeedDB : IGTFSFeedDB
    {        
        /// <summary>
        /// Holds a connection.
        /// </summary>
        private SQLiteConnection _connection;

        /// <summary>
        /// Creates a new db.
        /// </summary>
        public SQLiteGTFSFeedDB()
        {
            _connection = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
            _connection.Open();

            // build database.
            this.RebuildDB();
        }

        /// <summary>
        /// Creates a new db.
        /// </summary>
        public SQLiteGTFSFeedDB(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();

            // build database.
            this.RebuildDB();
        }

        /// <summary>
        /// Adds a new empty feed.
        /// </summary>
        /// <returns></returns>
        public int AddFeed()
        {
            string sqlInsertNewFeed = "INSERT INTO feed VALUES (null, null, null, null, null, null, null);";
            using(var command = _connection.CreateCommand())
            {
                command.CommandText = sqlInsertNewFeed;
                command.ExecuteNonQuery();
            }
            return (int)_connection.LastInsertRowId;
        }

        /// <summary>
        /// Adds the given feed.
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public int AddFeed(IGTFSFeed feed)
        {
            int newId = this.AddFeed();
            feed.CopyTo(this.GetFeed(newId));
            return newId;
        }

        /// <summary>
        /// Removes the given feed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveFeed(int id)
        {
            this.RemoveAll("agency", id);
            this.RemoveAll("calendar", id);
            this.RemoveAll("calendar_date", id);
            this.RemoveAll("fare_attribute", id);
            this.RemoveAll("fare_rule", id);
            this.RemoveAll("frequency", id);
            this.RemoveAll("route", id);
            this.RemoveAll("shape", id);
            this.RemoveAll("stop", id);
            this.RemoveAll("stop_time", id);
            this.RemoveAll("transfer", id);
            this.RemoveAll("trip", id);

            string sql = "DELETE FROM feed WHERE ID = :id"; ;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
                command.Parameters[0].Value = id;
                return command.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Returns all feeds.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetFeeds()
        {
            string sql = "SELECT id FROM feed";
            var ids = new List<int>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    ids.Add((int)reader.GetInt64(0));
                }
            }
            return ids;
        }

        /// <summary>
        /// Returns the feed for the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IGTFSFeed GetFeed(int id)
        {
            string sql = "SELECT id FROM feed WHERE ID = :id";
            var ids = new List<int>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter("id", DbType.Int64));
                command.Parameters[0].Value = id;

                using (var reader = command.ExecuteReader())
                { // ok, feed was found!
                    while (reader.Read())
                    {
                        return new SQLiteGTFSFeed(_connection, id);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Rebuilds the database.
        /// </summary>
        private void RebuildDB()
        {
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [feed] ( [ID] INTEGER NOT NULL PRIMARY KEY, [feed_publisher_name] TEXT, [feed_publisher_url] TEXT, [feed_lang] TEXT,  [feed_start_date] TEXT, [feed_end_date] TEXT, [feed_version] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [agency] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [agency_name] TEXT, [agency_url] TEXT, [agency_timezone], [agency_lang] TEXT, [agency_phone] TEXT, [agency_fare_url] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [calendar] ( [FEED_ID] INTEGER NOT NULL, [service_id] TEXT NOT NULL, [monday] INTEGER, [tuesday] INTEGER, [wednesday] INTEGER, [thursday] INTEGER, [friday] INTEGER, [saturday] INTEGER, [sunday] INTEGER, [start_date] INTEGER, [end_date] INTEGER );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [calendar_date] ( [FEED_ID] INTEGER NOT NULL, [service_id] TEXT NOT NULL, [date] INTEGER, [exception_type] INTEGER );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [fare_attribute] ( [FEED_ID] INTEGER NOT NULL, [fare_id] TEXT NOT NULL, [price] TEXT, [currency_type] TEXT, [payment_method] INTEGER, [transfers] INTEGER, [transfer_duration] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [fare_rule] ( [FEED_ID] INTEGER NOT NULL, [fare_id] TEXT NOT NULL, [route_id] TEXT NOT NULL, [origin_id] TEXT, [destination_id] TEXT, [contains_id] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [frequency] ( [FEED_ID] INTEGER NOT NULL, [trip_id] TEXT NOT NULL, [start_time] TEXT, [end_time] TEXT, [headway_secs] TEXT, [exact_times] INTEGER );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [route] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [agency_id] TEXT, [route_short_name] TEXT, [route_long_name] TEXT, [route_desc] TEXT, [route_type] INTEGER NOT NULL, [route_url] TEXT, [route_color] INTEGER, [route_text_color] INTEGER );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [shape] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [shape_pt_lat] REAL, [shape_pt_lon] REAL, [shape_pt_sequence] INTEGER, [shape_dist_traveled] REAL );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [stop] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [stop_code] TEXT, [stop_name] TEXT, [stop_desc] TEXT, [stop_lat] REAL, [stop_lon] REAL, [zone_id] TEXT, [stop_url] TEXT, [location_type] INTEGER, [parent_station] TEXT, [stop_timezone] TEXT, [wheelchair_boarding] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [stop_time] ( [FEED_ID] INTEGER NOT NULL, [trip_id] TEXT NOT NULL, [arrival_time] INTEGER, [departure_time] INTEGER, [stop_id] TEXT, [stop_sequence] INTEGER, [stop_headsign] TEXT, [pickup_type] INTEGER, [drop_off_type] INTEGER, [shape_dist_traveled] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [transfer] ( [FEED_ID] INTEGER NOT NULL, [from_stop_id] TEXT, [to_stop_id] TEXT, [transfer_type] INTEGER, [min_transfer_time] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [trip] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [route_id] TEXT, [service_id] TEXT, [trip_headsign] TEXT, [trip_short_name] TEXT, [direction_id] INTEGER, [block_id] TEXT, [shape_id] TEXT, [wheelchair_accessible] INTEGER );");
        }

        /// <summary>
        /// Deletes all info from one table about one feed.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="id"></param>
        private void RemoveAll(string table, int id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE FEED_ID = :feed_id", table);
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters[0].Value = id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes sql on the db.
        /// </summary>
        /// <param name="sql"></param>
        private void ExecuteNonQuery(string sql)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
    }
}