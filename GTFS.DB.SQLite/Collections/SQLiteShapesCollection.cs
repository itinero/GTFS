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
    /// Represents a collection of Shapes using an SQLite database.
    /// </summary>
    public class SQLiteShapeCollection : IEntityCollection<Shape>
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
        internal SQLiteShapeCollection(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Shape entity)
        {
            string sql = "INSERT INTO shape VALUES (:feed_id, :id, :shape_pt_lat, :shape_pt_lon, :shape_pt_sequence, :shape_dist_traveled);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"shape_pt_lat", DbType.Double));
                command.Parameters.Add(new SQLiteParameter(@"shape_pt_lon", DbType.Double));
                command.Parameters.Add(new SQLiteParameter(@"shape_pt_sequence", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"shape_dist_traveled", DbType.Double));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entity.Id;
                command.Parameters[2].Value = entity.Latitude;
                command.Parameters[3].Value = entity.Longitude;
                command.Parameters[4].Value = entity.Sequence;
                command.Parameters[5].Value = entity.DistanceTravelled;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Adds range of entities
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEntityCollection<Shape> entities)
        {
            using (var command = _connection.CreateCommand())
            {
                using (var transaction = _connection.BeginTransaction())
                {
                    foreach(var entity in entities)
                    {
                        string sql = "INSERT INTO shape VALUES (:feed_id, :id, :shape_pt_lat, :shape_pt_lon, :shape_pt_sequence, :shape_dist_traveled);";
                        command.CommandText = sql;
                        command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                        command.Parameters.Add(new SQLiteParameter(@"shape_pt_lat", DbType.Double));
                        command.Parameters.Add(new SQLiteParameter(@"shape_pt_lon", DbType.Double));
                        command.Parameters.Add(new SQLiteParameter(@"shape_pt_sequence", DbType.Int64));
                        command.Parameters.Add(new SQLiteParameter(@"shape_dist_traveled", DbType.Double));

                        command.Parameters[0].Value = _id;
                        command.Parameters[1].Value = entity.Id;
                        command.Parameters[2].Value = entity.Latitude;
                        command.Parameters[3].Value = entity.Longitude;
                        command.Parameters[4].Value = entity.Sequence;
                        command.Parameters[5].Value = entity.DistanceTravelled;

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
        public IEnumerable<Shape> Get()
        {
            string sql = "SELECT id, shape_pt_lat, shape_pt_lon, shape_pt_sequence, shape_dist_traveled FROM shape WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Shape>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Shape()
                {
                    Id = x.GetString(0),
                    Latitude = x.GetDouble(1),
                    Longitude = x.GetDouble(2),
                    Sequence = (uint)x.GetInt64(3),
                    DistanceTravelled = x.IsDBNull(4) ? null : (double?)x.GetDouble(4)
                };
            });
        }

        /// <summary>
        /// Returns all entities for the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public IEnumerable<Shape> Get(string entityId)
        {
            string sql = "SELECT id, shape_pt_lat, shape_pt_lon, shape_pt_sequence, shape_dist_traveled FROM shape WHERE FEED_ID = :id AND id = :shapeId";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;
            parameters.Add(new SQLiteParameter(@"shapeId", DbType.String));
            parameters[1].Value = entityId;

            return new SQLiteEnumerable<Shape>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Shape()
                {
                    Id = x.GetString(0),
                    Latitude = x.GetDouble(1),
                    Longitude = x.GetDouble(2),
                    Sequence = (uint)x.GetInt64(3),
                    DistanceTravelled = x.IsDBNull(4) ? null : (double?)x.GetDouble(4)
                };
            });
        }

        /// <summary>
        /// Removes all entities identified by the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public bool Remove(string entityId)
        {
            string sql = "DELETE FROM shape WHERE FEED_ID = :feed_id AND id = :shape_id;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"shape_id", DbType.String));


                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = entityId;

                return command.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Removes all entities
        /// </summary>
        /// <returns></returns>
        public void RemoveAll()
        {
            string sql = "DELETE from shape WHERE FEED_ID = :feed_id;";
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
        public IEnumerator<Shape> GetEnumerator()
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