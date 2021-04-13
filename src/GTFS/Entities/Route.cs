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
    /// Represents a transit route. A route is a group of trips that are displayed to riders as a single service.
    /// </summary>
    [FileName("route")]
    public class Route : GTFSEntity
    {
        /// <summary>
        /// Gets or sets an ID that uniquely identifies a route. The route_id is dataset unique.
        /// </summary>
        [Required]
        [FieldName("route_id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets an agency for the specified route. Use this field when you are providing data for routes from more than one agency.
        /// </summary>
        [Required]
        [FieldName("agency_id")]
        public string AgencyId { get; set; }

        /// <summary>
        /// Gets or sets the short name of a route. This will often be a short, abstract identifier like "32", "100X", or "Green" that riders use to identify a route, but which doesn't give any indication of what places the route serves. At least one of route_short_name or route_long_name must be specified, or potentially both if appropriate. If the route does not have a short name, please specify a route_long_name and use an empty string as the value for this field.
        /// </summary>
        [Required]
        [FieldName("route_short_name")]
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the full name of a route. This name is generally more descriptive than the route_short_name and will often include the route's destination or stop. At least one of route_short_name or route_long_name must be specified, or potentially both if appropriate. If the route does not have a long name, please specify a route_short_name and use an empty string as the value for this field.
        /// </summary>
        [Required]
        [FieldName("route_long_name")]
        public string LongName { get; set; }

        /// <summary>
        /// Gets or sets the description of a route. Please provide useful, quality information. Do not simply duplicate the name of the route. For example, "A trains operate between Inwood-207 St, Manhattan and Far Rockaway-Mott Avenue, Queens at all times. Also from about 6AM until about midnight, additional A trains operate between Inwood-207 St and Lefferts Boulevard (trains typically alternate between Lefferts Blvd and Far Rockaway)."
        /// </summary>
        [Required]
        [FieldName("route_desc")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of transportation used on this route.
        /// </summary>
        [Required]
        [FieldName("route_type")]
        public RouteType Type { get; set; }

        /// <summary>
        /// Gets or sets the URL of a web page about that particular route. This should be different from the agency_url. The value must be a fully qualified URL that includes http:// or https://, and any special characters in the URL must be correctly escaped.
        /// </summary>
        [FieldName("route_url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a color that corresponds to a route. The color must be provided as a six-character hexadecimal number, for example, 00FFFF. If no color is specified, the default route color is white (FFFFFF).
        /// </summary>
        [FieldName("route_color")]
        public int? Color { get; set; }

        /// <summary>
        /// Gets or sets a legible color to use for text drawn against a background of route_color. The color must be provided as a six-character hexadecimal number, for example, FFD700. If no color is specified, the default text color is black (000000).
        /// </summary>
        [FieldName("route_text_color")]
        public int? TextColor { get; set; }

        /// <summary>
        /// Indicates whether a rider can board the transit vehicle anywhere along the vehicle’s travel path. The path is described by shapes.txt on every trip of the route.
        /// 
        /// The default continuous pickup behavior defined in routes.txt can be overridden in stop_times.txt.
        /// </summary>
        [FieldName("continuous_pickup")]
        public ContinuousPickup? ContinuousPickup { get; set; }

        /// <summary>
        /// Indicates whether a rider can alight the transit vehicle anywhere along the vehicle’s travel path. The path is described by shapes.txt on every trip of the route.
        /// 
        /// The default continuous drop-off behavior defined in routes.txt can be overridden in stop_times.txt.
        /// </summary>
        [FieldName("continuous_drop_off")]
        public ContinuousDropOff? ContinuousDropOff { get; set; }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 41;
                hash = hash * 43 + (this.AgencyId ?? string.Empty).GetHashCode();
                hash = hash * 43 + this.Color.GetHashCode();
                hash = hash * 43 + (this.Description ?? string.Empty).GetHashCode();
                hash = hash * 43 + (this.Id ?? string.Empty).GetHashCode();
                hash = hash * 43 + (this.LongName ?? string.Empty).GetHashCode();
                hash = hash * 43 + (this.ShortName ?? string.Empty).GetHashCode();
                hash = hash * 43 + (this.TextColor ?? -1).GetHashCode();
                hash = hash * 43 + this.Type.GetHashCode();
                hash = hash * 43 + (this.Url ?? string.Empty).GetHashCode();
                hash = hash * 43 + this.ContinuousPickup.GetHashCode();
                hash = hash * 43 + this.ContinuousDropOff.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Route);
            if (other != null)
            {
                return (this.AgencyId ?? string.Empty) == (other.AgencyId ?? string.Empty) &&
                    this.Color == other.Color &&
                    (this.Description ?? string.Empty) == (other.Description ?? string.Empty) &&
                    (this.Id ?? string.Empty) == (other.Id ?? string.Empty) &&
                    (this.LongName ?? string.Empty) == (other.LongName ?? string.Empty) &&
                    (this.ShortName ?? string.Empty) == (other.ShortName ?? string.Empty) &&
                    (this.TextColor ?? -1) == (other.TextColor ?? -1) &&
                    this.Type == other.Type &&
                    (this.Url ?? string.Empty) == (other.Url ?? string.Empty) &&
                    this.ContinuousPickup == other.ContinuousPickup &&
                    this.ContinuousDropOff == other.ContinuousDropOff;
            }
            return false;
        }

        /// <summary>
        /// Returns a description of this route.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(this.LongName))
            {
                return this.LongName;
            }
            else if (!string.IsNullOrWhiteSpace(this.ShortName))
            {
                return this.ShortName;
            }
            else
            {
                return this.Id;
            }
        }

        /// <summary>
        /// Returns a new route given another route object
        /// </summary>
        public static Route From(Route route)
        {
            return new Route()
            {
                AgencyId = route.AgencyId,
                Color = route.Color,
                Description = route.Description,
                Id = route.Id,
                LongName = route.LongName,
                ShortName = route.ShortName,
                Tag = route.Tag,
                TextColor = route.TextColor,
                Type = route.Type,
                Url = route.Url,
                ContinuousPickup = route.ContinuousPickup,
                ContinuousDropOff = route.ContinuousDropOff
            };
        }
    }
}