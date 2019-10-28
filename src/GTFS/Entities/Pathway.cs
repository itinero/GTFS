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
    /// Represents a transit agency.
    /// </summary>
    [FileName("pathways")]
    public class Pathway : GTFSEntity
    {
        /// <summary>
        /// The pathway_id field contains an ID that uniquely identifies the pathway. The pathway_id is used by systems as an internal identifier of this record (e.g., primary key in database), and therefore the pathway_id must be dataset unique.
        /// </summary>
        [Required]
        [FieldName("pathway_id")]
        public string Id { get; set; }

        /// <summary>        
        /// Location at which the pathway begins. It contains a stop_id that identifies a platform, entrance/exit, generic node or boarding area from the stops.txt file.
        /// </summary>
        [Required]
        [FieldName("from_stop_id")]
        public string FromStopId { get; set; }

        /// <summary>
        /// Location at which the pathway ends. It contains a stop_id that identifies a platform, entrance/exit, generic node or boarding area from the stops.txt file.
        /// </summary>
        [Required]
        [FieldName("to_stop_id")]
        public string ToStopId { get; set; }

        /// <summary>
        /// Type of pathway between the specified (from_stop_id, to_stop_id) pair.
        /// </summary>
        [Required]
        [FieldName("pathway_mode")]
        public PathwayMode PathwayMode { get; set; }

        /// <summary>
        /// Indicates in which direction the pathway can be used:
        /// </summary>
        [Required]
        [FieldName("is_bidirectional")]
        public IsBidirectional IsBidirectional { get; set; }

        /// <summary>
        /// Horizontal length in meters of the pathway from the origin location (defined in from_stop_id) to the destination location (defined in to_stop_id)
        /// </summary>
        [FieldName("length")]
        public double? Length { get; set; }

        /// <summary>
        /// Average time in seconds needed to walk through the pathway from the origin location (defined in from_stop_id) to the destination location (defined in to_stop_id).
        /// </summary>
        [FieldName("traversal_time")]
        public int? TraversalTime { get; set; }

        /// <summary>
        /// Number of stairs of the pathway.
        /// </summary>
        [FieldName("stair_count")]
        public int? StairCount { get; set; }

        /// <summary>
        /// Maximum slope ratio of the pathway.
        /// </summary>
        [FieldName("max_slope")]
        public double? MaxSlope { get; set; }

        /// <summary>
        /// Minimum width of the pathway in meters.
        /// </summary>
        [FieldName("min_width")]
        public double? MinWidth { get; set; }

        /// <summary>
        /// String of text from physical signage visible to transit riders.
        /// </summary>
        [FieldName("signposted_as")]
        public string SignpostedAs { get; set; }

        /// <summary>
        /// Same than the signposted_as field, but when the pathways is used backward
        /// </summary>
        [FieldName("reversed_signposted_as")]
        public string ReversedSignpostedAs { get; set; }

        /// <summary>
        /// Returns a description of this agency.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0}] {1} - {2}", this.Id, this.FromStopId, this.ToStopId);
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
                hash = hash * 89 + this.Id.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.FromStopId.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.ToStopId.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.PathwayMode.GetHashCode();
                hash = hash * 89 + this.IsBidirectional.GetHashCode();
                hash = hash * 89 + this.Length.GetHashCode();
                hash = hash * 89 + this.TraversalTime.GetHashCode();
                hash = hash * 89 + this.StairCount.GetHashCode();
                hash = hash * 89 + this.MaxSlope.GetHashCode();
                hash = hash * 89 + this.MinWidth.GetHashCode();
                hash = hash * 89 + this.SignpostedAs.GetHashCode();
                hash = hash * 89 + this.ReversedSignpostedAs.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Pathway);
            if (other != null)
            {
                return (this.Id ?? string.Empty) == (other.Id ?? string.Empty) &&
                    (this.FromStopId ?? string.Empty) == (other.FromStopId ?? string.Empty) &&
                    (this.ToStopId ?? string.Empty) == (other.ToStopId ?? string.Empty) &&
                    this.PathwayMode == other.PathwayMode &&
                    this.IsBidirectional == other.IsBidirectional &&
                    this.Length == other.Length &&
                    this.TraversalTime == other.TraversalTime &&
                    this.StairCount == other.StairCount &&
                    this.MaxSlope == other.MaxSlope &&
                    this.MinWidth == other.MinWidth &&
                    (this.SignpostedAs ?? string.Empty) == (other.SignpostedAs ?? string.Empty) &&
                    (this.ReversedSignpostedAs ?? string.Empty) == (other.ReversedSignpostedAs ?? string.Empty);
            }
            return false;
        }
    }
}