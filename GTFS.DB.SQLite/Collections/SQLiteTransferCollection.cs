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
using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;

namespace GTFS.DB.SQLite.Collections
{
    /// <summary>
    /// Represents a collection of Transfers using an SQLite database.
    /// </summary>
    public class SQLiteTransferCollection : ITransferCollection
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
        internal SQLiteTransferCollection(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        /// <summary>
        /// Adds a new transfer.
        /// </summary>
        /// <param name="transfer"></param>
        public void Add(Transfer transfer)
        {
            string sql = "INSERT INTO transfer VALUES (:feed_id, :from_stop_id, :to_stop_id, :transfer_type, :min_transfer_time);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"from_stop_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"to_stop_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"transfer_type", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"min_transfer_time", DbType.String));                

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = transfer.FromStopId;
                command.Parameters[2].Value = transfer.ToStopId;
                command.Parameters[3].Value = transfer.TransferType;
                command.Parameters[4].Value = transfer.MinimumTransferTime;             

                command.ExecuteNonQuery();
            }
        }

        public void AddRange(IEnumerable<Transfer> transfers)
        {
            foreach(var transfer in transfers)
            {
                Add(transfer);
            }            
        }

        /// <summary>
        /// Gets all transfers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Transfer> Get()
        {
            string sql = "SELECT feed_id, from_stop_id, to_stop_id, transfer_type, min_transfer_time FROM transfer";
            var parameters = new List<SQLiteParameter>();

            return new SQLiteEnumerable<Transfer>(_connection, sql, new SQLiteParameter[0], (x) =>
            {
                return new Transfer()
                {
                    FromStopId = x.GetString(1),
                    ToStopId = x.GetString(2),
                    TransferType = (TransferType)x.GetInt64(3),
                    MinimumTransferTime = x.GetString(4)
                };
            });
        }

        /// <summary>
        /// Gets all transfers for the given from stop.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Transfer> GetForFromStop(string stopId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all transfers for the given from stop.
        /// </summary>
        /// <returns></returns>
        public int RemoveForFromStop(string stopId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all transfers for the given to stop.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Transfer> GetForToStop(string stopId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all transfers for the given to stop.
        /// </summary>
        /// <returns></returns>
        public int RemoveForToStop(string stopId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Transfer> GetEnumerator()
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