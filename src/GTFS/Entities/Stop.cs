// The MIT License (MIT)

// Copyright (c) 2016 Ben Abelshausen

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
    /// Represents an individual location where vehicles pick up or drop off passengers.
    /// </summary>
    [FileName("stops")]
    public class Stop : GTFSEntity
    {
        /// <summary>
        /// Gets or sets an ID that uniquely identifies a stop or station. Multiple routes may use the same stop. The stop_id is dataset unique.
        /// </summary>
        [Required]
        [FieldName("stop_id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets short text or a number that uniquely identifies the stop for passengers. Stop codes are often used in phone-based transit information systems or printed on stop signage to make it easier for riders to get a stop schedule or real-time arrival information for a particular stop.
        /// </summary>
        [FieldName("stop_code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name of a stop or station. Please use a name that people will understand in the local and tourist vernacular.
        /// </summary>
        [FieldName("stop_name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of a stop. Please provide useful, quality information. Do not simply duplicate the name of the stop.
        /// </summary>
        [FieldName("stop_desc")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the latitude of a stop or station. The field value must be a valid WGS 84 latitude.
        /// </summary>
        [Required]
        [FieldName("stop_lat")]
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of a stop or station. The field value must be a valid WGS 84 longitude value from -180 to 180.
        /// </summary>
        [Required]
        [FieldName("stop_lon")]
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the fare zone for a stop. Zone IDs are required if you want to provide fare information using fare rules. If this stop ID represents a station, the zone ID is ignored.
        /// </summary>
        [FieldName("zone_id")]
        public string Zone { get; set; }

        /// <summary>
        /// Gets or set the URL of a web page about a particular stop. This should be different from the agency_url and the route_url fields. The value must be a fully qualified URL that includes http:// or https://, and any special characters in the URL must be correctly escaped.
        /// </summary>
        [FieldName("stop_url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the location field that identifies whether this stop represents a stop or station. If no location type is specified, or the location type is blank, stops are treated as regular stops. Stations may have different properties from stops when they are represented on a map or used in trip planning.
        /// </summary>
        [FieldName("location_type")]
        public LocationType? LocationType { get; set; }

        /// <summary>
        /// Gets or sets the station associated with the stop. To use this field, a stop must also exist where this stop ID is assigned LocationType=Station.
        /// </summary>
        [FieldName("parent_station")]
        public string ParentStation { get; set; }

        /// <summary>
        /// Gets or sets the timezone in which this stop or station is located. Please refer to Wikipedia List of Timezones for a list of valid values. If omitted, the stop should be assumed to be located in the timezone specified by agency_timezone in agency.txt.
        /// </summary>
        [FieldName("stop_timezone")]
        public string Timezone { get; set; }

        /// <summary>
        /// Gets or sets whether wheelchair boardings are possible from the specified stop or station. The field can have the following values:
        /// </summary>
        [FieldName(" wheelchair_boarding ")]
        public string WheelchairBoarding { get; set; }

        /// <summary>
        /// Level of the location. The same level can be used by multiple unlinked stations.
        /// </summary>
        [FieldName("level_id")]
        public string LevelId { get; set; }

        /// <summary>
        /// Gets or sets the platform code. It is optional. Do not include the platform terms (e.g. platform) itself. Instead only 'A' or '1'.
        /// </summary>
        [FieldName("platform_code")]
        public string PlatformCode { get; set; }

        /// <summary>
        /// Returns a description of this stop.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var stationText = this.LocationType == Enumerations.LocationType.Station ? " (station)" : "";
            if (!string.IsNullOrWhiteSpace(this.Name))
            {
                return this.Name + stationText;
            }
            else
            {
                return this.Id + stationText;
            }
        }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 41;
                hash = hash * 43 + this.Code.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.Description.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.Id.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.Latitude.GetHashCode();
                hash = hash * 43 + this.LocationType.GetHashCode();
                hash = hash * 43 + this.Longitude.GetHashCode();
                hash = hash * 43 + this.Name.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.ParentStation.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.Timezone.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.Url.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.WheelchairBoarding.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.Zone.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.LevelId.GetHashCodeEmptyWhenNull();
                hash = hash * 43 + this.PlatformCode.GetHashCodeEmptyWhenNull();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Stop);
            if (other != null)
            {
                return (this.Code ?? string.Empty) == (other.Code ?? string.Empty) &&
                    (this.Description ?? string.Empty) == (other.Description ?? string.Empty) &&
                    (this.Id ?? string.Empty) == (other.Id ?? string.Empty) &&
                    this.Latitude == other.Latitude &&
                    this.LocationType == other.LocationType &&
                    this.Longitude == other.Longitude &&
                    (this.Name ?? string.Empty) == (other.Name ?? string.Empty) &&
                    (this.ParentStation ?? string.Empty) == (other.ParentStation ?? string.Empty) &&
                    (this.Timezone ?? string.Empty) == (other.Timezone ?? string.Empty) &&
                    (this.Url ?? string.Empty) == (other.Url ?? string.Empty) &&
                    (this.WheelchairBoarding ?? string.Empty) == (other.WheelchairBoarding ?? string.Empty) &&
                    (this.Zone ?? string.Empty) == (other.Zone ?? string.Empty) &&
                    (this.LevelId ?? string.Empty) == (other.LevelId ?? string.Empty) &&
                    (this.PlatformCode ?? string.Empty) == (other.PlatformCode ?? string.Empty);
            }
            return false;
        }

        /// <summary>
        /// Returns a new Stop object created from a previous stop object
        /// </summary>
        public static Stop From(Stop other, string newStopId = null)
        {
            return new Stop()
            {
                Id = newStopId ?? other.Id,
                Code = other.Code,
                Name = other.Name,
                Description = other.Description,
                Latitude = other.Latitude,
                Longitude = other.Longitude,
                Zone = other.Zone,
                Url = other.Url,
                LocationType = other.LocationType,
                ParentStation = other.ParentStation,
                Timezone = other.Timezone,
                WheelchairBoarding = other.WheelchairBoarding,
                LevelId = other.LevelId,
                PlatformCode = other.PlatformCode,
                Tag = other.Tag
            };
        }

        /// <summary>
        /// Returns true the location type is stop.
        /// </summary>
        /// <returns></returns>
        public bool IsTypeStop()
        {
            return !this.LocationType.HasValue || this.LocationType.Value == Enumerations.LocationType.Stop;
        }

        /// <summary>
        /// Returns true if the location type is station.
        /// </summary>
        /// <returns></returns>
        public bool IsTypeStation()
        {
            return this.LocationType.HasValue && this.LocationType.Value == Enumerations.LocationType.Station;
        }
    }
}
