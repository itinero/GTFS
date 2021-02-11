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
    /// Represents an attribution.
    /// </summary>
    [FileName("attribution")]
    public class Attribution : GTFSEntity
    {
        /// <summary>
        /// Gets or sets the ID that uniquely identifies an attribution.
        /// </summary>
        [FieldName("attribution_id")]
        public string Id { get; set; }

        /// <summary>
        /// The agency to which the attribution applies. If one agency_id, route_id, or trip_id attribution is defined, the other fields must be empty. If none are specified, the attribution applies to the whole dataset.
        /// </summary>
        [FieldName("agency_id")]
        public string AgencyId { get; set; }

        /// <summary>
        /// This field functions in the same way as agency_id, except the attribution applies to a route. Multiple attributions can apply to the same route.
        /// </summary>
        [FieldName("route_id")]
        public string RouteId { get; set; }

        /// <summary>
        /// This field functions in the same way as agency_id, except the attribution applies to a trip. Multiple attributions can apply to the same trip.
        /// </summary>
        [FieldName("trip_id")]
        public string TripId { get; set; }

        /// <summary>
        /// The name of the organization that the dataset is attributed to.
        /// </summary>
        [Required]
        [FieldName("organization_name")]
        public string OrganizationName { get; set; }

        /// <summary>
        /// The role of the organization is producer. At least one of the fields, either is_producer, is_operator, or is_authority, must be set at 1.
        /// </summary>
        [FieldName("is_producer")]
        public bool? IsProducer { get; set; }

        /// <summary>
        /// The role of the organization is operator. At least one of the fields, either is_producer, is_operator, or is_authority, must be set at 1.
        /// </summary>
        [FieldName("is_operator")]
        public bool? IsOperator { get; set; }

        /// <summary>
        /// The role of the organization is authority. At least one of the fields, either is_producer, is_operator, or is_authority, must be set at 1.
        /// </summary>
        [FieldName("is_authority")]
        public bool? IsAuthority { get; set; }

        /// <summary>
        /// The URL of the organization.
        /// </summary>
        [FieldName("atribution_url")]
        public string URL { get; set; }

        /// <summary>
        /// The email of the organization.
        /// </summary>
        [FieldName("attribution_email")]
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the organization.
        /// </summary>
        [FieldName("attribution_phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Returns a description of this trip.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0}] {1}", this.Id, this.OrganizationName);
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
                hash = hash * 89 + this.AgencyId.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.RouteId.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.TripId.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.OrganizationName.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + (this.IsProducer ?? false).GetHashCode();
                hash = hash * 89 + (this.IsOperator ?? false).GetHashCode();
                hash = hash * 89 + (this.IsAuthority ?? false).GetHashCode();
                hash = hash * 89 + this.URL.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.Email.GetHashCodeEmptyWhenNull();
                hash = hash * 89 + this.Phone.GetHashCodeEmptyWhenNull();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Attribution);
            if (other != null)
            {
                return (this.Id ?? string.Empty) == (other.Id ?? string.Empty) && 
                    (this.AgencyId ?? string.Empty) == (other.AgencyId ?? string.Empty) &&
                    (this.RouteId ?? string.Empty) == (other.RouteId ?? string.Empty) &&
                    (this.TripId ?? string.Empty) == (other.TripId ?? string.Empty) &&
                    (this.OrganizationName ?? string.Empty) == (other.OrganizationName ?? string.Empty) &&
                    (this.IsProducer ?? false) == (other.IsProducer ?? false) &&
                    (this.IsOperator ?? false) == (other.IsOperator ?? false) &&
                    (this.IsAuthority ?? false) == (other.IsAuthority ?? false) &&
                    (this.URL ?? string.Empty) == (other.URL ?? string.Empty) &&
                    (this.Email ?? string.Empty) == (other.Email ?? string.Empty) &&
                    (this.Phone ?? string.Empty) == (other.Phone ?? string.Empty);
            }
            return false;
        }
    }
}