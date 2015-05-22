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
using GTFS.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace GTFS.DB.SQLite.Collections
{
    /// <summary>
    /// Represents a collection of StopTimes using an SQLite database.
    /// </summary>
    public class SQLiteStopTimeCollection : IStopTimeCollection
    {
        /// <summary>
        /// Holds the connection.
        /// </summary>
        private SQLiteConnection _connection;

        /// <summary>
        /// Holds the id.
        /// </summary>
        private int _id;

        /// <summary>
        /// Creates a new SQLite GTFS feed.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id"></param>
        internal SQLiteStopTimeCollection(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="stopTime"></param>
        public void Add(StopTime stopTime)
        {
            string sql = "INSERT INTO stop_time VALUES (:feed_id, :trip_id, :arrival_time, :departure_time, :stop_id, :stop_sequence, :stop_headsign, :pickup_type, :drop_off_type, :shape_dist_traveled);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"trip_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"arrival_time", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"departure_time", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"stop_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"stop_sequence", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"stop_headsign", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"pickup_type", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"drop_off_type", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"shape_dist_traveled", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = stopTime.TripId;
                command.Parameters[2].Value = stopTime.ArrivalTime.TotalSeconds;
                command.Parameters[3].Value = stopTime.DepartureTime.TotalSeconds;
                command.Parameters[4].Value = stopTime.StopId;
                command.Parameters[5].Value = stopTime.StopSequence;
                command.Parameters[6].Value = stopTime.StopHeadsign;
                command.Parameters[7].Value = stopTime.PickupType.HasValue ? (int?)stopTime.PickupType.Value : null;
                command.Parameters[8].Value = stopTime.DropOffType.HasValue ? (int?)stopTime.DropOffType.Value : null;
                command.Parameters[9].Value = stopTime.ShapeDistTravelled;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all stop times.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> Get()
        {
            string sql = "SELECT trip_id, arrival_time, departure_time, stop_id, stop_sequence, stop_headsign, pickup_type, drop_off_type, shape_dist_traveled FROM stop_time WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<StopTime>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new StopTime()
                {
                    TripId = x.GetString(0),
                    ArrivalTime = TimeOfDay.FromTotalSeconds(x.GetInt32(1)),
                    DepartureTime = TimeOfDay.FromTotalSeconds(x.GetInt32(2)),
                    StopId = x.GetString(3),
                    StopSequence = (uint)x.GetInt32(4),
                    StopHeadsign = x.IsDBNull(5) ? null : x.GetString(5),
                    PickupType = x.IsDBNull(6) ? null : (PickupType?)x.GetInt64(6),
                    DropOffType = x.IsDBNull(7) ? null : (DropOffType?)x.GetInt64(7),
                    ShapeDistTravelled = x.IsDBNull(8) ? null : x.GetString(8)
                };
            });
        }

        /// <summary>
        /// Gets all stop times for the given trip.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> GetForTrip(string tripId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all stop times for the given trip.
        /// </summary>
        /// <returns></returns>
        public int RemoveForTrip(string tripId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all stop times for the given stop.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> GetForStop(string stopId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all stop times for the given stop.
        /// </summary>
        /// <returns></returns>
        public int RemoveForStop(string stopId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<StopTime> GetEnumerator()
        {
            return this.Get().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Get().GetEnumerator();
        }
    }
}