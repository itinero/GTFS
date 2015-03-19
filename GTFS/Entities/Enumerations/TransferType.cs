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

namespace GTFS.Entities.Enumerations
{
    /// <summary>
    /// The type of connection for the specified (from_stop_id, to_stop_id) pair.
    /// </summary>
    public enum TransferType
    {
        /// <summary>
        /// 0: This is a recommended transfer point between two routes.
        /// </summary>
        Recommended,
        /// <summary>
        /// 1: This is a timed transfer point between two routes. The departing vehicle is expected to wait for the arriving one, with sufficient time for a passenger to transfer between routes.
        /// </summary>
        TimedTransfer,
        /// <summary>
        /// 2: This transfer requires a minimum amount of time between arrival and departure to ensure a connection. The time required to transfer is specified by min_transfer_time.
        /// </summary>
        MinimumTime,
        /// <summary>
        /// 3: Transfers are not possible between routes at this location.
        /// </summary>
        NotPossible
    }
}