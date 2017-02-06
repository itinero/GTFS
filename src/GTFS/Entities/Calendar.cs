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
using System;

namespace GTFS.Entities
{
    /// <summary>
    /// Represents dates for service IDs using a weekly schedule. Specify when service starts and ends, as well as days of the week where service is available.
    /// </summary>
    [FileName("calendar")]
    public class Calendar : GTFSEntity
    {
        /// <summary>
        /// Gets or sets an ID that uniquely identifies a set of dates when service is available for one or more routes. Each service_id value can appear at most once in a calendar file. This value is dataset unique. It is referenced by the trips.txt file.
        /// </summary>
        [Required]
        [FieldName("service_id")]
        public string ServiceId { get; set; }

        /// <summary>
        /// Contains a byte that represents the week-mask.
        /// </summary>
        public byte Mask { get; set; }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Mondays.
        /// </summary>
        [Required]
        [FieldName("monday")]
        public bool Monday
        {
            get { return this[DayOfWeek.Monday]; }
            set { this[DayOfWeek.Monday] = value; }
        }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Tuesdays.
        /// </summary>
        [Required]
        [FieldName("tuesday")]
        public bool Tuesday
        {
            get { return this[DayOfWeek.Tuesday]; }
            set { this[DayOfWeek.Tuesday] = value; }
        }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Wednesdays.
        /// </summary>
        [Required]
        [FieldName("wednesday")]
        public bool Wednesday
        {
            get { return this[DayOfWeek.Wednesday]; }
            set { this[DayOfWeek.Wednesday] = value; }
        }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Thursdays.
        /// </summary>
        [Required]
        [FieldName("thursday")]
        public bool Thursday
        {
            get { return this[DayOfWeek.Thursday]; }
            set { this[DayOfWeek.Thursday] = value; }
        }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Fridays.
        /// </summary>
        [Required]
        [FieldName("friday")]
        public bool Friday
        {
            get { return this[DayOfWeek.Friday]; }
            set { this[DayOfWeek.Friday] = value; }
        }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Saturdays.
        /// </summary>
        [Required]
        [FieldName("saturday")]
        public bool Saturday
        {
            get { return this[DayOfWeek.Saturday]; }
            set { this[DayOfWeek.Saturday] = value; }
        }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Sundays.
        /// </summary>
        [Required]
        [FieldName("sunday")]
        public bool Sunday
        {
            get { return this[DayOfWeek.Sunday]; }
            set { this[DayOfWeek.Sunday] = value; }
        }

        /// <summary>
        /// Gets or sets the start date for the service.
        /// </summary>
        [Required]
        [FieldName("start_date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date for the service. This date is included in the service interval.
        /// </summary>
        [Required]
        [FieldName("end_date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the day of week.
        /// </summary>
        public bool this[DayOfWeek dayOfWeek]
        {
            get
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return (this.Mask & 1) > 0;
                    case DayOfWeek.Tuesday:
                        return (this.Mask & 2) > 0;
                    case DayOfWeek.Wednesday:
                        return (this.Mask & 4) > 0;
                    case DayOfWeek.Thursday:
                        return (this.Mask & 8) > 0;
                    case DayOfWeek.Friday:
                        return (this.Mask & 16) > 0;
                    case DayOfWeek.Saturday:
                        return (this.Mask & 32) > 0;
                    case DayOfWeek.Sunday:
                        return (this.Mask & 64) > 0;
                }
                throw new ArgumentOutOfRangeException("Not a valid day of the week.");
            }
            set
            {
                if (value)
                {
                    switch (dayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            this.Mask = (byte)(this.Mask | 1);
                            break;
                        case DayOfWeek.Tuesday:
                            this.Mask = (byte)(this.Mask | 2);
                            break;
                        case DayOfWeek.Wednesday:
                            this.Mask = (byte)(this.Mask | 4);
                            break;
                        case DayOfWeek.Thursday:
                            this.Mask = (byte)(this.Mask | 8);
                            break;
                        case DayOfWeek.Friday:
                            this.Mask = (byte)(this.Mask | 16);
                            break;
                        case DayOfWeek.Saturday:
                            this.Mask = (byte)(this.Mask | 32);
                            break;
                        case DayOfWeek.Sunday:
                            this.Mask = (byte)(this.Mask | 64);
                            break;
                    }
                }
                else
                {
                    switch (dayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            this.Mask = (byte)(this.Mask & (127 - 1));
                            break;
                        case DayOfWeek.Tuesday:
                            this.Mask = (byte)(this.Mask & (127 - 2));
                            break;
                        case DayOfWeek.Wednesday:
                            this.Mask = (byte)(this.Mask & (127 - 4));
                            break;
                        case DayOfWeek.Thursday:
                            this.Mask = (byte)(this.Mask & (127 - 8));
                            break;
                        case DayOfWeek.Friday:
                            this.Mask = (byte)(this.Mask & (127 - 16));
                            break;
                        case DayOfWeek.Saturday:
                            this.Mask = (byte)(this.Mask & (127 - 32));
                            break;
                        case DayOfWeek.Sunday:
                            this.Mask = (byte)(this.Mask & (127 - 64));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a description of this trip.
        /// </summary>
        public override string ToString()
        {
            return string.Format("[{0}] mon-sun {1}:{2}:{3}:{4}:{5}:{6}:{7}",
                this.ServiceId,
                this.Monday ? "1" : "0",
                this.Tuesday ? "1" : "0",
                this.Wednesday ? "1" : "0",
                this.Thursday ? "1" : "0",
                this.Friday ? "1" : "0",
                this.Saturday ? "1" : "0",
                this.Sunday ? "1" : "0");
        }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.EndDate.GetHashCode();
                hash = hash * 23 + this.Mask.GetHashCode();
                hash = hash * 23 + (this.ServiceId ?? string.Empty).GetHashCode();
                hash = hash * 23 + this.StartDate.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns true if the given object contains the same data.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = (obj as Calendar);
            if (other != null)
            {
                return this.EndDate == other.EndDate &&
                    this.StartDate == other.StartDate &&
                    this.Mask == other.Mask &&
                    (this.ServiceId ?? string.Empty) ==
                    (other.ServiceId ?? string.Empty);
            }
            return false;
        }
    }
}