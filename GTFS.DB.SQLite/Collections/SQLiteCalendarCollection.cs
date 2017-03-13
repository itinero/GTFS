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
    /// Represents a collection of Calendars using an SQLite database.
    /// </summary>
    public class SQLiteCalendarCollection : IEntityCollection<Calendar>
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
        internal SQLiteCalendarCollection(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Calendar entity)
        {
            string sql = "INSERT INTO calendar VALUES (:feed_id, :service_id, :monday, :tuesday, :wednesday, :thursday, :friday, :saturday, :sunday, :start_date, :end_date);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"monday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"tuesday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"wednesday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"thursday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"friday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"saturday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"sunday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"start_date", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"end_date", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entity.ServiceId;
                command.Parameters[2].Value = entity.Monday ? 1 : 0;
                command.Parameters[3].Value = entity.Tuesday ? 1 : 0;
                command.Parameters[4].Value = entity.Wednesday ? 1 : 0;
                command.Parameters[5].Value = entity.Thursday ? 1 : 0;
                command.Parameters[6].Value = entity.Friday ? 1 : 0;
                command.Parameters[7].Value = entity.Saturday ? 1 : 0;
                command.Parameters[8].Value = entity.Sunday ? 1 : 0;
                command.Parameters[9].Value = entity.StartDate.ToUnixTime();
                command.Parameters[10].Value = entity.EndDate.ToUnixTime();

                command.ExecuteNonQuery();
            }
        }

        public void AddRange(IEntityCollection<Calendar> entities)
        {
            using (var command = _connection.CreateCommand())
            {
                using (var transaction = _connection.BeginTransaction())
                {
                    foreach (var entity in entities)
                    {
                        string sql = "INSERT INTO calendar VALUES (:feed_id, :service_id, :monday, :tuesday, :wednesday, :thursday, :friday, :saturday, :sunday, :start_date, :end_date);";
                        command.CommandText = sql;
                        command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"monday", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"tuesday", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"wednesday", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"thursday", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"friday", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"saturday", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"sunday", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"start_date", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"end_date", DbType.Int64));

                        command.Parameters[0].Value = _id;
                        command.Parameters[1].Value = entity.ServiceId;
                        command.Parameters[2].Value = entity.Monday ? 1 : 0;
                        command.Parameters[3].Value = entity.Tuesday ? 1 : 0;
                        command.Parameters[4].Value = entity.Wednesday ? 1 : 0;
                        command.Parameters[5].Value = entity.Thursday ? 1 : 0;
                        command.Parameters[6].Value = entity.Friday ? 1 : 0;
                        command.Parameters[7].Value = entity.Saturday ? 1 : 0;
                        command.Parameters[8].Value = entity.Sunday ? 1 : 0;
                        command.Parameters[9].Value = entity.StartDate.ToUnixTime();
                        command.Parameters[10].Value = entity.EndDate.ToUnixTime();

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
        public IEnumerable<Calendar> Get()
        {
            string sql = "SELECT service_id, monday, tuesday, wednesday, thursday, friday, saturday, sunday, start_date, end_date FROM calendar WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Calendar>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Calendar()
                {
                    ServiceId = x.GetString(0),
                    Monday = x.IsDBNull(1) ? false : x.GetInt64(1) == 1,
                    Tuesday = x.IsDBNull(2) ? false : x.GetInt64(2) == 1,
                    Wednesday = x.IsDBNull(3) ? false : x.GetInt64(3) == 1,
                    Thursday = x.IsDBNull(4) ? false : x.GetInt64(4) == 1,
                    Friday = x.IsDBNull(5) ? false : x.GetInt64(5) == 1,
                    Saturday = x.IsDBNull(6) ? false : x.GetInt64(6) == 1,
                    Sunday = x.IsDBNull(7) ? false : x.GetInt64(7) == 1,
                    StartDate = x.GetInt64(8).FromUnixTime(),
                    EndDate = x.GetInt64(9).FromUnixTime()
                };
            });
        }

        /// <summary>
        /// Returns all entities for the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public IEnumerable<Calendar> Get(string entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all entities identified by the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public bool Remove(string entityId)
        {
            string sql = "DELETE FROM calendar WHERE FEED_ID = :feed_id AND service_id = :service_id;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entityId;

                return command.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Calendar> GetEnumerator()
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