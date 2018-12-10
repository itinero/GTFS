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
using System.IO;

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

        public string ConnectionString { get; }

        public object Tag { get; set; }

        /// <summary>
        /// Creates a new db.
        /// </summary>
        public SQLiteGTFSFeedDB()
        {
            ConnectionString = "Data Source=:memory:;Version=3;New=True;";
            _connection = new SQLiteConnection(ConnectionString, true);
            _connection.Open();

            // build database.
            this.RebuildDB();
        }

        /// <summary>
        /// Creates a new db.
        /// </summary>
        public SQLiteGTFSFeedDB(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SQLiteConnection(ConnectionString, true);
            _connection.Open();

            // build database.
            this.RebuildDB();
        }

        /// <summary>
        /// Creates a new db.
        /// </summary>
        public SQLiteGTFSFeedDB(FileInfo dbFile, int defaultTimeout = 10, int version = 3)
        {
            ConnectionString = $"Data Source={dbFile.FullName};DefaultTimeout={defaultTimeout};Version={version};";
            _connection = new SQLiteConnection(ConnectionString);
            _connection.Open();

            // build database.
            this.RebuildDB();
        }

        /// <summary>
        /// Returns the data source (filename of the db)
        /// </summary>
        public string GetDataSource()
        {
            return _connection.DataSource;
        }

        /// <summary>
        /// Returns the data source in full (location of the db)
        /// </summary>
        public string GetFullDataSource()
        {
            string connStr = _connection.ConnectionString;
            return connStr.Substring(connStr.IndexOf("Data Source=") + ("Data Source=").Length, connStr.IndexOf("Data Source=") + connStr.Substring(connStr.IndexOf("Data Source=") + ("Data Source=").Length).IndexOf(";"));
        }

        /// <summary>
        /// Adds a new empty feed.
        /// </summary>
        /// <returns></returns>
        public int AddFeed()
        {
            string sqlInsertNewFeed = "INSERT INTO feed VALUES (1, null, null, null, null, null, null);";
            using (var command = _connection.CreateCommand())
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
            SortAllTables();
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
                    while (reader.Read())
                    {
                        ids.Add((int)reader.GetInt64(0));
                    }                    
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
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [agency] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [agency_name] TEXT, [agency_url] TEXT, [agency_timezone], [agency_lang] TEXT, [agency_phone] TEXT, [agency_fare_url] TEXT, [agency_email] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [calendar] ( [FEED_ID] INTEGER NOT NULL, [service_id] TEXT NOT NULL, [monday] INTEGER, [tuesday] INTEGER, [wednesday] INTEGER, [thursday] INTEGER, [friday] INTEGER, [saturday] INTEGER, [sunday] INTEGER, [start_date] INTEGER, [end_date] INTEGER );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [calendar_date] ( [FEED_ID] INTEGER NOT NULL, [service_id] TEXT NOT NULL, [date] INTEGER, [exception_type] INTEGER );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [fare_attribute] ( [FEED_ID] INTEGER NOT NULL, [fare_id] TEXT NOT NULL, [price] TEXT, [currency_type] TEXT, [payment_method] INTEGER, [transfers] INTEGER, [transfer_duration] TEXT, [agency_id] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [fare_rule] ( [FEED_ID] INTEGER NOT NULL, [fare_id] TEXT NOT NULL, [route_id] TEXT NOT NULL, [origin_id] TEXT, [destination_id] TEXT, [contains_id] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [frequency] ( [FEED_ID] INTEGER NOT NULL, [trip_id] TEXT NOT NULL, [start_time] TEXT, [end_time] TEXT, [headway_secs] TEXT, [exact_times] INTEGER );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [route] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [agency_id] TEXT, [route_short_name] TEXT, [route_long_name] TEXT, [route_desc] TEXT, [route_type] INTEGER NOT NULL, [route_url] TEXT, [route_color] INTEGER, [route_text_color] INTEGER );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [shape] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [shape_pt_lat] REAL, [shape_pt_lon] REAL, [shape_pt_sequence] INTEGER, [shape_dist_traveled] REAL );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [stop] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [stop_code] TEXT, [stop_name] TEXT, [stop_desc] TEXT, [stop_lat] REAL, [stop_lon] REAL, [zone_id] TEXT, [stop_url] TEXT, [location_type] INTEGER, [parent_station] TEXT, [stop_timezone] TEXT, [wheelchair_boarding] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [stop_time] ( [FEED_ID] INTEGER NOT NULL, [trip_id] TEXT NOT NULL, [arrival_time] INTEGER, [departure_time] INTEGER, [stop_id] TEXT, [stop_sequence] INTEGER, [stop_headsign] TEXT, [pickup_type] INTEGER, [drop_off_type] INTEGER, [shape_dist_traveled] REAL, [timepoint] INTEGER );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [transfer] ( [FEED_ID] INTEGER NOT NULL, [from_stop_id] TEXT, [to_stop_id] TEXT, [transfer_type] INTEGER, [min_transfer_time] TEXT );");
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [trip] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [route_id] TEXT, [service_id] TEXT, [trip_headsign] TEXT, [trip_short_name] TEXT, [direction_id] INTEGER, [block_id] TEXT, [shape_id] TEXT, [wheelchair_accessible] INTEGER );");

            // Alter existing tables, if they don't contain the required columns, for backwards compatibility
            //  add agency_email column to agency
            if (!ColumnExists("agency", "agency_email"))
            {
                this.ExecuteNonQuery("ALTER TABLE [agency] ADD [agency_email] TEXT;");
            }
            //  add agency_id column to fare_attribute
            if (!ColumnExists("fare_attribute", "agency_id"))
            {
                this.ExecuteNonQuery("ALTER TABLE [fare_attribute] ADD [agency_id] TEXT;");
            }
        }

        /// <summary>
        /// Checks if a table with the given name exists in this database.
        /// </summary>
        /// <param name="tableName">The table in this database to look for.</param>
        /// <returns>True if th</returns>
        public bool TableExists(string tableName)
        {
            using (var command = new SQLiteCommand($"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';", _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var value = reader.GetValue(0);
                        if (tableName.Equals(value))
                        {
                            reader.Close();
                            return true;
                        }
                    }
                    reader.Close();
                    return false;
                }
            }
        }

        /// <summary>
        /// Checks if the given table contains a column with the given name.
        /// </summary>
        /// <param name="tableName">The table in this database to check.</param>
        /// <param name="columnName">The column in the given table to look for.</param>
        /// <returns>True if the given table contains a column with the given name.</returns>
        public bool ColumnExists(string tableName, string columnName)
        {
            using (var command = new SQLiteCommand("PRAGMA table_info(" + tableName + ")", _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var value = reader.GetValue(1);//column 1 from the result contains the column names
                        if (columnName.Equals(value))
                        {
                            reader.Close();
                            return true;
                        }
                    }

                    reader.Close();
                    return false;
                }
            }
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
        private int ExecuteNonQuery(string sql)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes and recreates the routes, trips, stops, stop_times, frequencies and calendar_dates tables in a sorted order - may take some time
        /// </summary>
        public void SortAllTables()
        {
            SortRoutes();
            SortTrips();
            SortStops();
            SortStopTimes();
            SortFrequencies();
            SortCalendars();
            SortCalendarDates();
            SortShapes();
        }

        /// <summary>
        /// Deletes and recreates the routes table in a sorted order - may take time
        /// </summary>
        public void SortRoutes()
        {
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [route_sorted] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [agency_id] TEXT, [route_short_name] TEXT, [route_long_name] TEXT, [route_desc] TEXT, [route_type] INTEGER NOT NULL, [route_url] TEXT, [route_color] INTEGER, [route_text_color] INTEGER );");
            this.ExecuteNonQuery("INSERT INTO route_sorted (FEED_ID, id, agency_id, route_short_name, route_long_name, route_desc, route_type, route_url, route_color, route_text_color) SELECT FEED_ID, id, agency_id, route_short_name, route_long_name, route_desc, route_type, route_url, route_color, route_text_color FROM route ORDER BY id ASC;");
            this.ExecuteNonQuery("DROP TABLE route");
            this.ExecuteNonQuery("ALTER TABLE route_sorted RENAME TO route");
        }

        /// <summary>
        /// Deletes and recreates the trips table in a sorted order - may take time
        /// </summary>
        public void SortTrips()
        {
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [trip_sorted] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [route_id] TEXT, [service_id] TEXT, [trip_headsign] TEXT, [trip_short_name] TEXT, [direction_id] INTEGER, [block_id] TEXT, [shape_id] TEXT, [wheelchair_accessible] INTEGER );");
            this.ExecuteNonQuery("INSERT INTO trip_sorted (FEED_ID, id, route_id, service_id, trip_headsign, trip_short_name, direction_id, block_id, shape_id, wheelchair_accessible) SELECT FEED_ID, id, route_id, service_id, trip_headsign, trip_short_name, direction_id, block_id, shape_id, wheelchair_accessible FROM trip ORDER BY id ASC;");
            this.ExecuteNonQuery("DROP TABLE trip");
            this.ExecuteNonQuery("ALTER TABLE trip_sorted RENAME TO trip");
        }

        /// <summary>
        /// Deletes and recreates the stops table in a sorted order - may take time
        /// </summary>
        public void SortStops()
        {
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [stop_sorted] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [stop_code] TEXT, [stop_name] TEXT, [stop_desc] TEXT, [stop_lat] REAL, [stop_lon] REAL, [zone_id] TEXT, [stop_url] TEXT, [location_type] INTEGER, [parent_station] TEXT, [stop_timezone] TEXT, [wheelchair_boarding] TEXT );");
            this.ExecuteNonQuery("INSERT INTO stop_sorted (FEED_ID, id, stop_code, stop_name, stop_desc, stop_lat, stop_lon, zone_id, stop_url, location_type, parent_station, stop_timezone, wheelchair_boarding) SELECT FEED_ID, id, stop_code, stop_name, stop_desc, stop_lat, stop_lon, zone_id, stop_url, location_type, parent_station, stop_timezone, wheelchair_boarding FROM stop ORDER BY id ASC;");
            this.ExecuteNonQuery("DROP TABLE stop");
            this.ExecuteNonQuery("ALTER TABLE stop_sorted RENAME TO stop");
        }

        /// <summary>
        /// Deletes and recreates the stop_times table in a sorted order - may take time
        /// </summary>
        public void SortStopTimes()
        {
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [stop_time_sorted] ( [FEED_ID] INTEGER NOT NULL, [trip_id] TEXT NOT NULL, [arrival_time] INTEGER, [departure_time] INTEGER, [stop_id] TEXT, [stop_sequence] INTEGER, [stop_headsign] TEXT, [pickup_type] INTEGER, [drop_off_type] INTEGER, [shape_dist_traveled] REAL, [timepoint] INTEGER );");
            this.ExecuteNonQuery("INSERT INTO stop_time_sorted (FEED_ID, trip_id, arrival_time, departure_time, stop_id, stop_sequence, stop_headsign, pickup_type, drop_off_type, shape_dist_traveled, timepoint) SELECT FEED_ID, trip_id, arrival_time, departure_time, stop_id, stop_sequence, stop_headsign, pickup_type, drop_off_type, shape_dist_traveled, timepoint FROM stop_time ORDER BY trip_id ASC, stop_sequence ASC;");
            this.ExecuteNonQuery("DROP TABLE stop_time");
            this.ExecuteNonQuery("ALTER TABLE stop_time_sorted RENAME TO stop_time");
        }

        /// <summary>
        /// Deletes and recreates the frequencies table in a sorted order - may take time
        /// </summary>
        public void SortFrequencies()
        {
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [frequency_sorted] ( [FEED_ID] INTEGER NOT NULL, [trip_id] TEXT NOT NULL, [start_time] TEXT, [end_time] TEXT, [headway_secs] TEXT, [exact_times] INTEGER );");
            this.ExecuteNonQuery("INSERT INTO frequency_sorted (FEED_ID, trip_id, start_time, end_time, headway_secs, exact_times) SELECT FEED_ID, trip_id, start_time, end_time, headway_secs, exact_times FROM frequency ORDER BY trip_id ASC;");
            this.ExecuteNonQuery("DROP TABLE frequency");
            this.ExecuteNonQuery("ALTER TABLE frequency_sorted RENAME TO frequency");
        }

        public void SortCalendars()
        {
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [calendar_sorted] ( [FEED_ID] INTEGER NOT NULL, [service_id] TEXT NOT NULL, [monday] INTEGER, [tuesday] INTEGER, [wednesday] INTEGER, [thursday] INTEGER, [friday] INTEGER, [saturday] INTEGER, [sunday] INTEGER, [start_date] INTEGER, [end_date] INTEGER );");
            this.ExecuteNonQuery("INSERT INTO calendar_sorted (FEED_ID, service_id, monday, tuesday, wednesday, thursday, friday, saturday, sunday, start_date, end_date) SELECT FEED_ID, service_id, monday, tuesday, wednesday, thursday, friday, saturday, sunday, start_date, end_date FROM calendar ORDER BY service_id ASC;");
            this.ExecuteNonQuery("DROP TABLE calendar");
            this.ExecuteNonQuery("ALTER TABLE calendar_sorted RENAME TO calendar");
        }

        /// <summary>
        /// Deletes and recreates the calendar_dates table in a sorted order (first by date then by exception_type) - may take time
        /// </summary>
        public void SortCalendarDates()
        {
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [calendar_date_sorted] ( [FEED_ID] INTEGER NOT NULL, [service_id] TEXT NOT NULL, [date] INTEGER, [exception_type] INTEGER );");
            this.ExecuteNonQuery("INSERT INTO calendar_date_sorted (FEED_ID, service_id, date, exception_type) SELECT FEED_ID, service_id, date, exception_type FROM calendar_date ORDER BY date, exception_type ASC;");
            this.ExecuteNonQuery("DROP TABLE calendar_date");
            this.ExecuteNonQuery("ALTER TABLE calendar_date_sorted RENAME TO calendar_date");
        }

        /// <summary>
        /// Deletes and recreates the shapes table in a sorted order (first by id then by sequence) - will take time
        /// </summary>
        public void SortShapes()
        {
            this.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [shape_sorted] ( [FEED_ID] INTEGER NOT NULL, [id] TEXT NOT NULL, [shape_pt_lat] REAL, [shape_pt_lon] REAL, [shape_pt_sequence] INTEGER, [shape_dist_traveled] REAL );");
            this.ExecuteNonQuery("INSERT INTO shape_sorted (FEED_ID, id, shape_pt_lat, shape_pt_lon, shape_pt_sequence, shape_dist_traveled) SELECT FEED_ID, id, shape_pt_lat, shape_pt_lon, shape_pt_sequence, shape_dist_traveled FROM shape ORDER BY id, shape_pt_sequence ASC;");
            this.ExecuteNonQuery("DROP TABLE shape");
            this.ExecuteNonQuery("ALTER TABLE shape_sorted RENAME TO shape");
        }
    }
}