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
using System.Text;

namespace GTFS.DB.SQLite.Collections
{
    /// <summary>
    /// Represents a collection of CalendarDates using an SQLite database.
    /// </summary>
    public class SQLiteCalendarDateCollection : IEntityCollection<CalendarDate>
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
        internal SQLiteCalendarDateCollection(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(CalendarDate entity)
        {
            string sql = "INSERT INTO calendar_date VALUES (:feed_id, :service_id, :date, :exception_type);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"date", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"exception_type", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entity.ServiceId;
                command.Parameters[2].Value = entity.Date.ToUnixTime();
                command.Parameters[3].Value = (int)entity.ExceptionType;

                command.ExecuteNonQuery();
            }
        }

        public void AddRange(IEntityCollection<CalendarDate> entities)
        {
            using (var command = _connection.CreateCommand())
            {
                using (var transaction = _connection.BeginTransaction())
                {
                    foreach (var entity in entities)
                    {
                        string sql = "INSERT INTO calendar_date VALUES (:feed_id, :service_id, :date, :exception_type);";
                        command.CommandText = sql;
                        command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"date", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"exception_type", DbType.Int64));

                        command.Parameters[0].Value = _id;
                        command.Parameters[1].Value = entity.ServiceId;
                        command.Parameters[2].Value = entity.Date.ToUnixTime();
                        command.Parameters[3].Value = (int)entity.ExceptionType;

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
        public IEnumerable<CalendarDate> Get()
        {
            string sql = "SELECT service_id, date, exception_type FROM calendar_date WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<CalendarDate>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new CalendarDate()
                {
                    ServiceId = x.GetString(0),
                    Date = x.GetInt64(1).FromUnixTime(),
                    ExceptionType = (ExceptionType)x.GetInt64(2)
                };
            });
        }

        /// <summary>
        /// Returns all entities for the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public IEnumerable<CalendarDate> Get(string entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the entities for the given id's.
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public IEnumerable<CalendarDate> Get(List<string> entityIds)
        {
            if (entityIds.Count == 0)
            {
                return new List<CalendarDate>();
            }
            var sql = new StringBuilder("SELECT service_id, date, exception_type FROM calendar_date WHERE FEED_ID = :feed_id AND service_id = :service_id0");
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("feed_id", DbType.Int64));
            parameters[0].Value = _id;
            int i = 0;
            foreach (var entityId in entityIds)
            {
                if (i > 0) sql.Append($" OR service_id = :service_id{i}");
                parameters.Add(new SQLiteParameter($"service_id{i}", DbType.String));
                parameters[1 + i].Value = entityId;
                i++;
            }

            return new SQLiteEnumerable<CalendarDate>(_connection, sql.ToString(), parameters.ToArray(), (x) =>
            {
                return new CalendarDate()
                {
                    ServiceId = x.GetString(0),
                    Date = x.GetInt64(1).FromUnixTime(),
                    ExceptionType = (ExceptionType)x.GetInt64(2)
                };
            });
        }

        public IEnumerable<string> GetIds()
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
            string sql = "DELETE FROM calendar_date WHERE FEED_ID = :feed_id AND service_id = :service_id;";
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
        /// Removes a range of entities by their IDs
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public void RemoveRange(IEnumerable<string> entityIds)
        {
            using (var command = _connection.CreateCommand())
            {
                using (var transaction = _connection.BeginTransaction())
                {
                    foreach (var entityId in entityIds)
                    {
                        string sql = "DELETE FROM calendar_date WHERE FEED_ID = :feed_id AND service_id = :service_id;";
                        command.CommandText = sql;
                        command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));

                        command.Parameters[0].Value = _id;
                        command.Parameters[1].Value = entityId;

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        public void RemoveAll()
        {
            string sql = "DELETE from calendar_date WHERE FEED_ID = :feed_id;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<CalendarDate> GetEnumerator()
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