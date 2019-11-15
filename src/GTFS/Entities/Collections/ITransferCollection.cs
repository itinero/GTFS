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

namespace GTFS.Entities.Collections
{
    /// <summary>
    /// Abstract representation of a collection of Transfer.
    /// </summary>
    public interface ITransferCollection : IEnumerable<Transfer>
    {
        /// <summary>
        /// Adds a new transfer.
        /// </summary>
        /// <param name="transfer"></param>
        void Add(Transfer transfer);

        /// <summary>
        /// Adds a new transfer.
        /// </summary>
        /// <param name="transfer"></param>
        void AddRange(IEnumerable<Transfer> transfer);

        /// <summary>
        /// Gets all transfers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Transfer> Get();

        /// <summary>
        /// Gets all transfers for the given from stop.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Transfer> GetForFromStop(string stopId);

        /// <summary>
        /// Removes all transfers for the given from stop.
        /// </summary>
        /// <returns></returns>
        int RemoveForFromStop(string stopId);

        /// <summary>
        /// Gets all transfers for the given to stop.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Transfer> GetForToStop(string stopId);

        /// <summary>
        /// Removes all transfers for the given to stop.
        /// </summary>
        /// <returns></returns>
        int RemoveForToStop(string stopId);
    }
}