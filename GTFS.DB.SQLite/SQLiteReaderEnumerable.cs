// The MIT License (MIT)

// Copyright (c) 2014 Ben Abelshausen

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

using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace GTFS.DB.SQLite
{
    /// <summary>
    /// Represents an enumerable based on an SQLite data reader.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class SQLiteEnumerable<T> : IEnumerable<T>
        where T : Entities.GTFSEntity
    {
        /// <summary>
        /// Holds the connection.
        /// </summary>
        private SQLiteConnection _connection { get; set; }

        /// <summary>
        /// Holds the sql.
        /// </summary>
        private string _sql;

        /// <summary>
        /// Holds the parameters list.
        /// </summary>
        private SQLiteParameter[] _parameters;

        /// <summary>
        /// Holds function to build entities from reader.
        /// </summary>
        private Func<SQLiteDataReader, T> _entityBuilder { get; set; }

        /// <summary>
        /// Creates a new enumerable based on the given command.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="entityBuilder"></param>
        public SQLiteEnumerable(SQLiteConnection connection, string sql, SQLiteParameter[] parameters, Func<SQLiteDataReader, T> entityBuilder)
        {
            _connection = connection;
            _parameters = parameters;
            _sql = sql;
            _entityBuilder = entityBuilder;
        }

        /// <summary>
        /// Returns the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            var command = _connection.CreateCommand();
            command.CommandText = _sql;
            command.Parameters.AddRange(_parameters);
            return new SQLiteEnumerator<T>(command, _entityBuilder);
        }

        /// <summary>
        /// Returns the enumerator.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Represents an enumerator based on an SQLite datareader.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class SQLiteEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// Holds the data reader.
        /// </summary>
        private SQLiteDataReader _reader { get; set; }

        /// <summary>
        /// Holds the command.
        /// </summary>
        private SQLiteCommand _command { get; set; }

        /// <summary>
        /// Function to build the entity.
        /// </summary>
        private Func<SQLiteDataReader, T> _entityBuilder { get; set; }

        /// <summary>
        /// Creates a new enumerator.
        /// </summary>
        /// <param name="sQLiteDataReader"></param>
        /// <param name="_entityBuilder"></param>
        public SQLiteEnumerator(SQLiteCommand command, Func<SQLiteDataReader, T> _entityBuilder)
        {
            this._command = command;
            this._reader = null;
            this._entityBuilder = _entityBuilder;
        }

        /// <summary>
        /// Returns the current entity.
        /// </summary>
        public T Current
        {
            get
            {
                return _entityBuilder(_reader);
            }
        }

        /// <summary>
        /// Disposes of all resource associated with this enumerator.
        /// </summary>
        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
            if(_command != null)
            {
                _command.Dispose();
                _command = null;
            }
        }

        /// <summary>
        /// Returns the current.
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        /// <summary>
        /// Move to next.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (_reader == null)
            {
                _reader = _command.ExecuteReader();
            }
            return _reader.Read();
        }

        /// <summary>
        /// Resets this enumerator.
        /// </summary>
        public void Reset()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }

            _reader = _command.ExecuteReader();
        }
    }
}
