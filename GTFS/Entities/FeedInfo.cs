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
    /// Represents additional information about the GTFS feed itself, including publisher, version, and expiration information.
    /// </summary>
    [FileName("feed_info")]
    public class FeedInfo : GTFSEntity
    {
        /// <summary>
        /// Gets or sets the full name of the organization that publishes the feed. (This may be the same as one of the agency_name values in agency.txt.) GTFS-consuming applications can display this name when giving attribution for a particular feed's data.
        /// </summary>
        [Required]
        [FieldName("feed_publisher_name")]
        public string PublisherName { get; set; }

        /// <summary>
        /// Gets or sets the URL of the feed publishing organization's website. (This may be the same as one of the agency_url values in agency.txt.) The value must be a fully qualified URL that includes http:// or https://, and any special characters in the URL must be correctly escaped. See http://www.w3.org/Addressing/URL/4_URI_Recommentations.html for a description of how to create fully qualified URL values.
        /// </summary>
        [Required]
        [FieldName("feed_publisher_url")]
        public string PublisherUrl { get; set; }

        /// <summary>
        /// Gets or sets a IETF BCP 47 language code specifying the default language used for the text in this feed. This setting helps GTFS consumers choose capitalization rules and other language-specific settings for the feed. For an introduction to IETF BCP 47, please refer to http://www.rfc-editor.org/rfc/bcp/bcp47.txt and http://www.w3.org/International/articles/language-tags/.
        /// </summary>
        [Required]
        [FieldName("feed_lang")]
        public string Lang { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        [FieldName("feed_start_date")]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        [FieldName("feed_end_date")]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets a string here that indicates the current version of their GTFS feed. GTFS-consuming applications can display this value to help feed publishers determine whether the latest version of their feed has been incorporated.
        /// </summary>
        [FieldName("feed_version")]
        public string Version { get; set; }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 37;
                hash = hash * 41 + (this.EndDate ?? string.Empty).GetHashCode();
                hash = hash * 41 + (this.Lang ?? string.Empty).GetHashCode();
                hash = hash * 41 + (this.PublisherName ?? string.Empty).GetHashCode();
                hash = hash * 41 + (this.PublisherUrl ?? string.Empty).GetHashCode();
                hash = hash * 41 + (this.StartDate ?? string.Empty).GetHashCode();
                hash = hash * 41 + (this.Version ?? string.Empty).GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as FeedInfo);
            if (other != null)
            {
                return (this.EndDate ?? string.Empty) == (other.EndDate ?? string.Empty) &&
                    (this.Lang ?? string.Empty) == (other.Lang ?? string.Empty) &&
                    (this.PublisherName ?? string.Empty) == (other.PublisherName ?? string.Empty) &&
                    (this.PublisherUrl ?? string.Empty) == (other.PublisherUrl ?? string.Empty) &&
                    (this.StartDate ?? string.Empty) == (other.StartDate ?? string.Empty) &&
                    (this.Version ?? string.Empty) == (other.Version ?? string.Empty);
            }
            return false;
        }
    }
}