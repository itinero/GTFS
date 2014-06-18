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
        /// Contains a binary value that indicates whether the service is valid for all Mondays.
        /// </summary>
        [Required]
        [FieldName("monday")]
        public bool Monday { get; set; }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Tuesdays.
        /// </summary>
        [Required]
        [FieldName("tuesday")]
        public bool Tuesday { get; set; }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Wednesdays.
        /// </summary>
        [Required]
        [FieldName("wednesday")]
        public bool Wednesday { get; set; }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Thursdays.
        /// </summary>
        [Required]
        [FieldName("thursday")]
        public bool Thursday { get; set; }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Fridays.
        /// </summary>
        [Required]
        [FieldName("friday")]
        public bool Friday { get; set; }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Saturdays.
        /// </summary>
        [Required]
        [FieldName("saturday")]
        public bool Saturday { get; set; }

        /// <summary>
        /// Contains a binary value that indicates whether the service is valid for all Sundays.
        /// </summary>
        [Required]
        [FieldName("sunday")]
        public bool Sunday { get; set; }

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
        /// Returns true if this calendar covers the given date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool CoversDate(DateTime date)
        {
            date = date.Date;
            if (this.StartDate <= date && this.EndDate >= date)
            { // ok in range.
                switch(date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return this.Monday;
                    case DayOfWeek.Tuesday:
                        return this.Tuesday;
                    case DayOfWeek.Wednesday:
                        return this.Wednesday;
                    case DayOfWeek.Thursday:
                        return this.Thursday;
                    case DayOfWeek.Friday:
                        return this.Friday;
                    case DayOfWeek.Saturday:
                        return this.Saturday;
                    case DayOfWeek.Sunday:
                        return this.Sunday;
                }
            }
            return false;
        }
    }
}