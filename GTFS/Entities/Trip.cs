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
using System.Collections.Generic;
using System;

namespace GTFS.Entities
{
    /// <summary>
    /// Represents a trip. A trip is a sequence of two or more stops that occurs at specific time.
    /// </summary>
    [FileName("trip")]
    public class Trip : GTFSEntity
    {
        /// <summary>
        /// Gets or sets an ID that identifies a trip.
        /// </summary>
        [Required]
        [FieldName("trip_id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a route.
        /// </summary>
        [Required]
        [FieldName("route_id")]
        public string RouteId { get; set; }

        /// <summary>
        /// Gets or sets an ID that uniquely identifies a set of dates when service is available for one or more routes. This value is referenced from the Calendar or CalendarDates entity.
        /// </summary>
        [Required]
        [FieldName("service_id")]
        public string ServiceId { get; set; }

        /// <summary>
        /// Gets or sets  the text that appears on a sign that identifies the trip's destination to passengers. Use this field to distinguish between different patterns of service in the same route. If the headsign changes during a trip, you can override the trip_headsign by specifying values for the the stop_headsign field in stop_times.txt.
        /// </summary>
        [FieldName("trip_headsign")]
        public string Headsign { get; set; }

        /// <summary>
        /// Gets or sets the text that appears in schedules and sign boards to identify the trip to passengers, for example, to identify train numbers for commuter rail trips. If riders do not commonly rely on trip names, please leave this field blank.
        /// </summary>
        [FieldName("trip_short_name")]
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the direction of travel for a trip. Use this field to distinguish between bi-directional trips with the same route_id. This field is not used in routing; it provides a way to separate trips by direction when publishing time tables. You can specify names for each direction with the trip_headsign field.
        /// </summary>
        [FieldName("direction_id")]
        public DirectionType? Direction { get; set; }

        /// <summary>
        /// Gets or sets the block to which the trip belongs. A block consists of two or more sequential trips made using the same vehicle, where a passenger can transfer from one trip to the next just by staying in the vehicle. The block_id must be referenced by two or more trips in trips.txt.
        /// </summary>
        [FieldName("block_id")]
        public string BlockId { get; set; }

        /// <summary>
        /// Gets or sets a shape for the trip. This value is referenced from the shapes.txt file. The shapes.txt file allows you to define how a line should be drawn on the map to represent a trip.
        /// </summary>
        [FieldName("shape_id")]
        public string ShapeId { get; set; }

        /// <summary>
        /// Gets or sets accessibility information for the trip
        /// </summary>
        [FieldName("wheelchair_accessible")]
        public WheelchairAccessibilityType? AccessibilityType { get; set; }

        /// <summary>
        /// Returns a description of this trip.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.ShortName != null && this.ShortName != "" && this.Headsign != null && this.Headsign != "") return string.Format("{0} (to {1})", this.ShortName, this.Headsign);
            else if (this.ShortName != null && this.ShortName != "") return string.Format("{0}", this.ShortName);
            else if (this.Headsign != null && this.Headsign != "") return string.Format("{0} (to {1})", this.Id, this.Headsign);
            else return this.Id;
        }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 83;
                hash = hash * 89 + this.AccessibilityType.GetHashCode();
                hash = hash * 89 + this.BlockId.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.Direction.GetHashCode();
                hash = hash * 89 + this.Headsign.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.Id.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.RouteId.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.ServiceId.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.ShapeId.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.ShortName.GetHashCodeEmptyWhenNull();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Trip);
            if (other != null)
            {
                return this.AccessibilityType == other.AccessibilityType &&
                    (this.BlockId ?? string.Empty) == (other.BlockId ?? string.Empty) &&
                    this.Direction == other.Direction &&
                    (this.Headsign ?? string.Empty) == (other.Headsign ?? string.Empty) &&
                    (this.Id ?? string.Empty) == (other.Id ?? string.Empty) &&
                    (this.RouteId ?? string.Empty) == (other.RouteId ?? string.Empty) &&
                    (this.ServiceId ?? string.Empty) == (other.ServiceId ?? string.Empty) &&
                    (this.ShapeId ?? string.Empty) == (other.ShapeId ?? string.Empty) &&
                    (this.ShortName ?? string.Empty) == (other.ShortName ?? string.Empty);
            }
            return false;
        }

        /// <summary>
        /// Returns a new trip given another trip object
        /// </summary>
        public static Trip From(Trip trip)
        {
            return new Trip()
            {
                AccessibilityType = trip.AccessibilityType,
                BlockId = trip.BlockId,
                Direction = trip.Direction,
                Headsign = trip.Headsign,
                Id = trip.Id,
                RouteId = trip.RouteId,
                ServiceId = trip.ServiceId,
                ShapeId = trip.ShapeId,
                ShortName = trip.ShortName,
                Tag = trip.Tag
            };
        }
    }
}