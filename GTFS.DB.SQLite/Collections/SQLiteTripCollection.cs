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
using System.Linq;

namespace GTFS.DB.SQLite.Collections
{
    /// <summary>
    /// Represents a collection of Trips using an SQLite database.
    /// </summary>
    public class SQLiteTripCollection : IUniqueEntityCollection<Trip>
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
        internal SQLiteTripCollection(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        public void Add(Trip trip)
        {
            string sql = "INSERT INTO trip VALUES (:feed_id, :id, :route_id, :service_id, :trip_headsign, :trip_short_name, :direction_id, :block_id, :shape_id, :wheelchair_accessible);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"trip_headsign", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"trip_short_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"direction_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"block_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"shape_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"wheelchair_accessible", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = trip.Id;
                command.Parameters[2].Value = trip.RouteId;
                command.Parameters[3].Value = trip.ServiceId;
                command.Parameters[4].Value = trip.Headsign;
                command.Parameters[5].Value = trip.ShortName;
                command.Parameters[6].Value = trip.Direction.HasValue ? (int?)trip.Direction.Value : null;
                command.Parameters[7].Value = trip.BlockId;
                command.Parameters[8].Value = trip.ShapeId;
                command.Parameters[9].Value = trip.AccessibilityType.HasValue ? (int?)trip.AccessibilityType.Value : null;

                command.ExecuteNonQuery();
            }
        }

        public void AddRange(IUniqueEntityCollection<Trip> entities)
        {
            using (var command = _connection.CreateCommand())
            {
                using (var transaction = _connection.BeginTransaction())
                {
                    foreach (var trip in entities)
                    {
                        string sql = "INSERT INTO trip VALUES (:feed_id, :id, :route_id, :service_id, :trip_headsign, :trip_short_name, :direction_id, :block_id, :shape_id, :wheelchair_accessible);";
                        command.CommandText = sql;
                        command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"route_id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"trip_headsign", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"trip_short_name", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"direction_id", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"block_id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"shape_id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"wheelchair_accessible", DbType.Int64));

                        command.Parameters[0].Value = _id;
                        command.Parameters[1].Value = trip.Id;
                        command.Parameters[2].Value = trip.RouteId;
                        command.Parameters[3].Value = trip.ServiceId;
                        command.Parameters[4].Value = trip.Headsign;
                        command.Parameters[5].Value = trip.ShortName;
                        command.Parameters[6].Value = trip.Direction.HasValue ? (int?)trip.Direction.Value : null;
                        command.Parameters[7].Value = trip.BlockId;
                        command.Parameters[8].Value = trip.ShapeId;
                        command.Parameters[9].Value = trip.AccessibilityType.HasValue ? (int?)trip.AccessibilityType.Value : null;

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        public Trip Get(string entityId)
        {
            string sql = "SELECT id, route_id, service_id, trip_headsign, trip_short_name, direction_id, block_id, shape_id, wheelchair_accessible FROM trip WHERE FEED_ID = :id AND ID = :trip_id;";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters.Add(new SQLiteParameter(@"trip_id", DbType.Int64));
            parameters[0].Value = _id;
            parameters[1].Value = entityId;

            return new SQLiteEnumerable<Trip>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Trip()
                {
                    Id = x.GetString(0),
                    ServiceId = x.IsDBNull(1) ? null : x.GetString(1),
                    Headsign = x.IsDBNull(2) ? null : x.GetString(2),
                    ShortName = x.IsDBNull(3) ? null : x.GetString(3),
                    Direction = x.IsDBNull(4) ? null : (DirectionType?)x.GetInt64(4),
                    BlockId = x.IsDBNull(5) ? null : x.GetString(5),
                    ShapeId = x.IsDBNull(6) ? null : x.GetString(6),
                    AccessibilityType = x.IsDBNull(7) ? null : (WheelchairAccessibilityType?)x.GetInt64(7)
                };
            }).FirstOrDefault();
        }



        public Trip Get(int idx)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string entityId)
        {
            string sql = "DELETE FROM trip WHERE FEED_ID = :feed_id AND id = :trip_id;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"trip_id", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entityId;

                return command.ExecuteNonQuery() > 0;
            }
        }

        public void RemoveRange(IEnumerable<string> entityIds)
        {
            using (var command = _connection.CreateCommand())
            {
                using (var transaction = _connection.BeginTransaction())
                {
                    foreach (var tripId in entityIds)
                    {
                        string sql = "DELETE FROM trip WHERE FEED_ID = :feed_id AND id = :trip_id;";
                        command.CommandText = sql;
                        command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"trip_id", DbType.String));

                        command.Parameters[0].Value = _id;
                        command.Parameters[1].Value = tripId;
                        
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        public IEnumerable<Trip> Get()
        {
            string sql = "SELECT id, route_id, service_id, trip_headsign, trip_short_name, direction_id, block_id, shape_id, wheelchair_accessible FROM trip WHERE FEED_ID = :id;";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Trip>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Trip()
                {
                    Id = x.GetString(0),
                    RouteId = x.IsDBNull(1) ? null : x.GetString(1),
                    ServiceId = x.IsDBNull(2) ? null : x.GetString(2),
                    Headsign = x.IsDBNull(3) ? null : x.GetString(3),
                    ShortName = x.IsDBNull(4) ? null : x.GetString(4),
                    Direction = x.IsDBNull(5) ? null : (DirectionType?)x.GetInt64(5),
                    BlockId = x.IsDBNull(6) ? null : x.GetString(6),
                    ShapeId = x.IsDBNull(7) ? null : x.GetString(7),
                    AccessibilityType = x.IsDBNull(8) ? null : (WheelchairAccessibilityType?)x.GetInt64(8)
                };
            });
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Trip> GetEnumerator()
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

        public bool Update(string entityId, Trip entity)
        {
            string sql = "UPDATE trip SET FEED_ID=:feed_id, id=:id, route_id=:route_id, service_id=:service_id, trip_headsign=:trip_headsign, trip_short_name=:trip_short_name, direction_id=:direction_id, block_id=:block_id, shape_id=:shape_id, wheelchair_accessible=:wheelchair_accessible WHERE id=:entityId;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"trip_headsign", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"trip_short_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"direction_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"block_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"shape_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"wheelchair_accessible", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"entityId", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entity.Id;
                command.Parameters[2].Value = entity.RouteId;
                command.Parameters[3].Value = entity.ServiceId;
                command.Parameters[4].Value = entity.Headsign;
                command.Parameters[5].Value = entity.ShortName;
                command.Parameters[6].Value = entity.Direction.HasValue ? (int?)entity.Direction.Value : null;
                command.Parameters[7].Value = entity.BlockId;
                command.Parameters[8].Value = entity.ShapeId;
                command.Parameters[9].Value = entity.AccessibilityType.HasValue ? (int?)entity.AccessibilityType.Value : null;
                command.Parameters[10].Value = entityId;

                return command.ExecuteNonQuery() > 0;
            }
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }
    }
}