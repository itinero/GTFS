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

namespace GTFS.Entities
{
    /// <summary>
    /// Represents rules for applying fare information for a transit organization's routes.
    /// </summary>
    [FileName("fare_rule")]
    public class FareRule : GTFSEntity
    {
        /// <summary>
        /// Gets or sets a fare.
        /// </summary>
        [Required]
        [FieldName("fare_id")]
        public string FareId { get; set; }

        /// <summary>
        /// Gets or sets a route.
        /// </summary>
        [FieldName("route_id")]
        public string RouteId { get; set; }

        /// <summary>
        /// Gets or sets the fare ID with an origin zone ID. Zone IDs are referenced from the stops.txt file. If you have several origin IDs with the same fare attributes, create a row in fare_rules.txt for each origin ID.
        /// </summary>
        [FieldName("origin_id")]
        public string OriginId { get; set; }

        /// <summary>
        /// Gets or sets the fare ID with a destination zone ID. Zone IDs are referenced from the stops.txt file. If you have several destination IDs with the same fare attributes, create a row in fare_rules.txt for each destination ID.
        /// </summary>
        [FieldName("destination_id")]
        public string DestinationId { get; set; }

        /// <summary>
        /// Gets or sets the fare ID with a zone ID, referenced from the stops.txt file. The fare ID is then associated with itineraries that pass through every contains_id zone.
        /// </summary>
        [FieldName("contains_id")]
        public string ContainsId { get; set; }
        
        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 31;
                hash = hash * 37 + (this.ContainsId ?? string.Empty).GetHashCode();
                hash = hash * 37 + (this.DestinationId ?? string.Empty).GetHashCode();
                hash = hash * 37 + (this.FareId ?? string.Empty).GetHashCode();
                hash = hash * 37 + (this.OriginId ?? string.Empty).GetHashCode();
                hash = hash * 37 + (this.RouteId ?? string.Empty).GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as FareRule);
            if (other != null)
            {
                return (this.ContainsId ?? string.Empty) == (other.ContainsId ?? string.Empty) &&
                    (this.DestinationId ?? string.Empty) == (other.DestinationId ?? string.Empty) &&
                    (this.FareId ?? string.Empty) == (other.ContainsId ?? string.Empty) && 
                    (this.OriginId ?? string.Empty) == (other.OriginId ?? string.Empty) &&
                    (this.RouteId ?? string.Empty) == (other.RouteId ?? string.Empty);
            }
            return false;
        }
    }
}