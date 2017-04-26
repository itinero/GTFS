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
    /// Represents a collection of Routes using an SQLite database.
    /// </summary>
    public class SQLiteRouteCollection : IUniqueEntityCollection<Route>
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
        internal SQLiteRouteCollection(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Route entity)
        {
            string sql = "INSERT INTO route VALUES (:feed_id, :id, :agency_id, :route_short_name, :route_long_name, :route_desc, :route_type, :route_url, :route_color, :route_text_color);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"agency_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_short_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_long_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_desc", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_type", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"route_url", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_color", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"route_text_color", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entity.Id;
                command.Parameters[2].Value = entity.AgencyId;
                command.Parameters[3].Value = entity.ShortName;
                command.Parameters[4].Value = entity.LongName;
                command.Parameters[5].Value = entity.Description;
                command.Parameters[6].Value = (int)entity.Type;
                command.Parameters[7].Value = entity.Url;
                command.Parameters[8].Value = entity.Color;
                command.Parameters[9].Value = entity.TextColor;

                command.ExecuteNonQuery();
            }
        }

        public void AddRange(IUniqueEntityCollection<Route> entities)
        {
            using (var command = _connection.CreateCommand())
            {
                using (var transaction = _connection.BeginTransaction())
                {
                    foreach (var entity in entities)
                    {
                        string sql = "INSERT INTO route VALUES (:feed_id, :id, :agency_id, :route_short_name, :route_long_name, :route_desc, :route_type, :route_url, :route_color, :route_text_color);";
                        command.CommandText = sql;
                        command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"agency_id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"route_short_name", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"route_long_name", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"route_desc", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"route_type", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"route_url", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"route_color", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"route_text_color", DbType.Int64));

                        command.Parameters[0].Value = _id;
                        command.Parameters[1].Value = entity.Id;
                        command.Parameters[2].Value = entity.AgencyId;
                        command.Parameters[3].Value = entity.ShortName;
                        command.Parameters[4].Value = entity.LongName;
                        command.Parameters[5].Value = entity.Description;
                        command.Parameters[6].Value = (int)entity.Type;
                        command.Parameters[7].Value = entity.Url;
                        command.Parameters[8].Value = entity.Color;
                        command.Parameters[9].Value = entity.TextColor;

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Gets the entity with the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Route Get(string entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the entity at the given index.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Route Get(int idx)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the entity with the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public bool Remove(string entityId)
        {
            string sql = "DELETE FROM route WHERE FEED_ID = :feed_id AND id = :route_id;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"route_id", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entityId;

                return command.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Returns all entities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Route> Get()
        {
            string sql = "SELECT id, agency_id, route_short_name, route_long_name, route_desc, route_type, route_url, route_color, route_text_color FROM route WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Route>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Route()
                {
                    Id = x.GetString(0),
                    AgencyId = x.IsDBNull(1) ? null : x.GetString(1),
                    ShortName = x.IsDBNull(2) ? null : x.GetString(2),
                    LongName = x.IsDBNull(3) ? null : x.GetString(3),
                    Description = x.IsDBNull(4) ? null : x.GetString(4),
                    Type = (RouteTypeExtended)x.GetInt64(5),
                    Url = x.IsDBNull(6) ? null : x.GetString(6),
                    Color = x.IsDBNull(7) ? null : (int?)x.GetInt64(7),
                    TextColor = x.IsDBNull(8) ? null : (int?)x.GetInt64(8)
                };
            });
        }

        /// <summary>
        /// Returns the number of entities.
        /// </summary>
        public int Count
        {
            get
            {
                string sql = "SELECT count(id) FROM route;";
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = sql;
                    return int.Parse(command.ExecuteScalar().ToString());
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Route> GetEnumerator()
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

        public bool Update(string entityId, Route entity)
        {
            string sql = "UPDATE route SET FEED_ID=:feed_id, id=:id, agency_id=:agency_id, route_short_name=:route_short_name, route_long_name=:route_long_name, route_desc=:route_desc, route_type=:route_type, route_url=:route_url, route_color=:route_color, route_text_color=:route_text_color WHERE id=:entityId;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"agency_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_short_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_long_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_desc", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_type", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"route_url", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_color", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"route_text_color", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"entityId", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entity.Id;
                command.Parameters[2].Value = entity.AgencyId;
                command.Parameters[3].Value = entity.ShortName;
                command.Parameters[4].Value = entity.LongName;
                command.Parameters[5].Value = entity.Description;
                command.Parameters[6].Value = (int)entity.Type;
                command.Parameters[7].Value = entity.Url;
                command.Parameters[8].Value = entity.Color;
                command.Parameters[9].Value = entity.TextColor;
                command.Parameters[10].Value = entityId;

                return command.ExecuteNonQuery() > 0;
            }
        }

        public void RemoveRange(IEnumerable<string> entityIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }
    }
}