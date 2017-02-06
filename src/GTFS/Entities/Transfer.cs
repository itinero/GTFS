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

using GTFS.Attributes;
using GTFS.Entities.Enumerations;

namespace GTFS.Entities
{
    /// <summary>
    /// Represents a rules for making connections at transfer points between routes.
    /// </summary>
    [FileName("transfer")]
    public class Transfer : GTFSEntity
    {
        /// <summary>
        /// Gets or sets a stop or station where a connection between routes begins.
        /// </summary>
        [Required]
        [FieldName("from_stop_id")]
        public string FromStopId { get; set; }

        /// <summary>
        /// Gets or sets a stop or station where a connection between routes ends.
        /// </summary>
        [Required]
        [FieldName("to_stop_id")]
        public string ToStopId { get; set; }

        /// <summary>
        /// Gets or sets the type of connection for the specified (from_stop_id, to_stop_id) pair.
        /// </summary>
        [Required]
        [FieldName("transfer_type")]
        public TransferType TransferType { get; set; }

        /// <summary>
        /// Gets or sets the amount of time that must be available in an itinerary to permit a transfer between routes at these stops.
        /// </summary>
        [FieldName("min_transfer_time")]
        public string MinimumTransferTime { get; set; }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 71;
                hash = hash * 73 + this.FromStopId.GetHashCodeEmptyWhenNull();
                hash = hash * 73 + this.MinimumTransferTime.GetHashCodeEmptyWhenNull();
                hash = hash * 73 + this.ToStopId.GetHashCodeEmptyWhenNull();
                hash = hash * 73 + this.TransferType.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Transfer);
            if (other != null)
            {
                return (this.FromStopId ?? string.Empty) == (other.FromStopId ?? string.Empty) &&
                    (this.ToStopId ?? string.Empty) == (other.ToStopId ?? string.Empty) &&
                    this.TransferType == other.TransferType &&
                    (this.MinimumTransferTime ?? string.Empty) == (other.MinimumTransferTime ?? string.Empty);
            }
            return false;
        }
    }
}