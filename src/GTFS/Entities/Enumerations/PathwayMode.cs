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
    /// Type of pathway between the specified (from_stop_id, to_stop_id) pair
    /// </summary>
    public enum PathwayMode
    {
        /// <summary>
        /// Walkway
        /// </summary>
        Walkway = 1,
        /// <summary>
        /// Stairs
        /// </summary>
        Stairs = 2,
        /// <summary>
        /// Moving Sidewal, Travelator
        /// </summary>
        Travelator = 3,
        /// <summary>
        /// Escalator
        /// </summary>
        Escalator = 4,
        /// <summary>
        /// Elevator
        /// </summary>
        Elevator = 5,
        /// <summary>
        /// fare gate (or payment gate): A pathway that crosses into an area of the station where a proof of payment is required (usually via a physical payment gate).
        /// </summary>
        FareGate = 6,
        /// <summary>
        /// Indicates a pathway exiting an area where proof-of-payment is required into an area where proof-of-payment is no longer required.
        /// </summary>
        ExitGate = 7,

    }
}