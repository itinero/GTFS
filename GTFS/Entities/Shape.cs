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
    /// Represents rules for drawing lines on a map to represent a transit organization's routes.
    /// </summary>
    [FileName("shapes")]
    public class Shape : GTFSEntity
    {
        /// <summary>
        /// Gets or sets an ID that uniquely identifies a shape.
        /// </summary>
        [Required]
        [FieldName("shape_id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a shape point's latitude with a shape ID. The field value must be a valid WGS 84 latitude. Each row in shapes.txt represents a shape point in your shape definition.
        /// </summary>
        [Required]
        [FieldName("shape_pt_lat")]
        public double Latitude  { get; set; }

        /// <summary>
        /// Gets or sets a shape point's longitude with a shape ID. The field value must be a valid WGS 84 longitude value from -180 to 180. Each row in shapes.txt represents a shape point in your shape definition.
        /// </summary>
        [Required]
        [FieldName("shape_pt_lon")]
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the sequence order along the shape. The values for shape_pt_sequence must be non-negative integers, and they must increase along the trip. 
        /// </summary>
        [Required]
        [FieldName("shape_pt_sequence")]
        public uint Sequence { get; set; }

        /// <summary>
        /// Gets or sets the distance traveled along a shape from the first shape point.
        /// </summary>
        [FieldName("shape_dist_traveled")]
        public double? DistanceTravelled { get; set; }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 43;
                hash = hash * 47 + this.DistanceTravelled.GetHashCode();
                hash = hash * 47 + (this.Id ?? string.Empty).GetHashCode();
                hash = hash * 47 + this.Latitude.GetHashCode();
                hash = hash * 47 + this.Longitude.GetHashCode();
                hash = hash * 47 + this.Sequence.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Shape);
            if (other != null)
            {
                return this.DistanceTravelled == other.DistanceTravelled && 
                    (this.Id ?? string.Empty) == (other.Id ?? string.Empty) &&
                    this.Latitude == other.Latitude &&
                    this.Longitude == other.Longitude &&
                    this.Sequence == other.Sequence;
            }
            return false;
        }
    }
}