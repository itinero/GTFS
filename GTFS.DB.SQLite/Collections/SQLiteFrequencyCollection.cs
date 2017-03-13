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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace GTFS.DB.SQLite.Collections
{
    /// <summary>
    /// Represents a collection of Frequencies using an SQLite database.
    /// </summary>
    public class SQLiteFrequencyCollection : IEntityCollection<Frequency>
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
        internal SQLiteFrequencyCollection(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Frequency entity)
        {
            string sql = "INSERT INTO frequency VALUES (:feed_id, :trip_id, :start_time, :end_time, :headway_secs, :exact_times);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"trip_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"start_time", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"end_time", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"headway_secs", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"exact_times", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entity.TripId;
                command.Parameters[2].Value = entity.StartTime;
                command.Parameters[3].Value = entity.EndTime;
                command.Parameters[4].Value = entity.HeadwaySecs;
                command.Parameters[5].Value = entity.ExactTimes;

                command.ExecuteNonQuery();
            }
        }

        public void AddRange(IEntityCollection<Frequency> entities)
        {
            using (var command = _connection.CreateCommand())
            {
                using (var transaction = _connection.BeginTransaction())
                {
                    foreach (var entity in entities)
                    {
                        string sql = "INSERT INTO frequency VALUES (:feed_id, :trip_id, :start_time, :end_time, :headway_secs, :exact_times);";
                        command.CommandText = sql;
                        command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"trip_id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"start_time", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"end_time", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"headway_secs", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"exact_times", DbType.Int64));

                        command.Parameters[0].Value = _id;
                        command.Parameters[1].Value = entity.TripId;
                        command.Parameters[2].Value = entity.StartTime;
                        command.Parameters[3].Value = entity.EndTime;
                        command.Parameters[4].Value = entity.HeadwaySecs;
                        command.Parameters[5].Value = entity.ExactTimes;

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Returns all entities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Frequency> Get()
        {
            string sql = "SELECT trip_id, start_time, end_time, headway_secs, exact_times FROM frequency WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Frequency>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Frequency()
                {
                    TripId = x.GetString(0),
                    StartTime = x.IsDBNull(1) ? null : x.GetString(1),
                    EndTime = x.IsDBNull(2) ? null : x.GetString(2),
                    HeadwaySecs = x.IsDBNull(3) ? null : x.GetString(3),
                    ExactTimes = x.IsDBNull(4) ? null : (bool?)(x.GetInt64(4) == 1)
                };
            });
        }

        /// <summary>
        /// Returns all entities for the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public IEnumerable<Frequency> Get(string entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all entities identified by the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public bool Remove(string tripId)
        {
            string sql = "DELETE FROM frequency WHERE FEED_ID = :feed_id AND trip_id = :trip_id;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"trip_id", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = tripId;

                return command.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Frequency> GetEnumerator()
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

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }
    }
}