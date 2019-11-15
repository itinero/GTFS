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
    /// Represents a transit station level.
    /// </summary>
    [FileName("levels")]
    public class Level : GTFSEntity
    {
        /// <summary>
        /// Id of the level that can be referenced from stops.txt.
        /// </summary>
        [Required]
        [FieldName("level_id")]
        public string Id { get; set; }

        /// <summary>
        /// Numeric index of the level that indicates relative position of this level in relation to other levels (levels with higher indices are assumed to be located above levels with lower indices).
        /// Ground level should have index 0, with levels above ground indicated by positive indices and levels below ground by negative indices.
        /// </summary>
        [Required]
        [FieldName("level_index")]
        public double Index { get; set; }

        /// <summary>
        /// Optional name of the level(that matches level lettering/numbering used inside the building or the station). Is useful for elevator routing(e.g. “take the elevator to level “Mezzanine” or “Platforms” or “-1”).
        /// </summary>
        [FieldName("level_name")]
        public string Name { get; set; }

        /// <summary>
        /// Returns a description of this agency.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0}] {1}", this.Id, this.Name);
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
                hash = hash * 89 + this.Index.GetHashCode();
                hash = hash * 89 + this.Name.GetHashCodeEmptyWhenNull();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Level);
            if (other != null)
            {
                return this.Index == other.Index &&
                    (this.Id ?? string.Empty) == (other.Id ?? string.Empty) &&
                    (this.Name ?? string.Empty) == (other.Name ?? string.Empty);
            }
            return false;
        }
    }
}