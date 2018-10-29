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
    /// Represents a transit agency.
    /// </summary>
    [FileName("agency")]
    public class Agency : GTFSEntity
    {
        /// <summary>
        /// Gets or sets the ID that uniquely identifies a transit agency.
        /// </summary>
        [FieldName("agency_id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the agency name, the full name of the transit agency.
        /// </summary>
        [Required]
        [FieldName("agency_name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL of the transit agency. The value must be a fully qualified URL that includes http:// or https://, and any special characters in the URL must be correctly escaped.
        /// </summary>
        [Required]
        [FieldName("agency_url")]
        public string URL { get; set; }

        /// <summary>
        /// Gets or sets the timezone.
        /// </summary>
        [Required]
        [FieldName("agency_timezone")]
        public string Timezone { get; set; }

        /// <summary>
        /// Gets or set the two-letter ISO 639-1 code for the primary language used by this transit agency.
        /// </summary>
        [FieldName("agency_lang")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the voice telephone number for the specified agency
        /// </summary>
        [FieldName("agency_phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the URL of a web page that allows a rider to purchase tickets or other fare instruments for that agency online. The value must be a fully qualified URL that includes http:// or https://, and any special characters in the URL must be correctly escaped.
        /// </summary>
        [FieldName("agency_fare_url")]
        public string FareURL { get; set; }

        /// <summary>
        /// Gets or sets an email address that is monitored by the agency's customer service department
        /// </summary>
        [FieldName("agency_email")]
        public string Email { get; set; }

        /// <summary>
        /// Returns a description of this trip.
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
                hash = hash * 89 + this.Email.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.FareURL.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.Id.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.LanguageCode.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.Name.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.Phone.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.Timezone.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.URL.GetHashCodeEmptyWhenNull();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Agency);
            if (other != null)
            {
                return (this.Email ?? string.Empty) == (other.Email ?? string.Empty) &&
                    (this.FareURL ?? string.Empty) == (other.FareURL ?? string.Empty) &&
                    (this.Id ?? string.Empty) == (other.Id ?? string.Empty) &&
                    (this.LanguageCode ?? string.Empty) == (other.LanguageCode ?? string.Empty) &&
                    (this.Name ?? string.Empty) == (other.Name ?? string.Empty) &&
                    (this.Phone ?? string.Empty) == (other.Phone ?? string.Empty) &&
                    (this.Timezone ?? string.Empty) == (other.Timezone ?? string.Empty) &&
                    (this.URL ?? string.Empty) == (other.URL ?? string.Empty);
            }
            return false;
        }
    }
}