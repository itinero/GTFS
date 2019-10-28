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

using GTFS.Entities.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GTFS
{
    /// <summary>
    /// Contains common extensions for this GTFS library.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Parses an integer number from a string at the given index and with the given length.
        /// </summary>
        public static int FastParse(this string value, int idx, int count)
        {
            int result = 0;
            int relIdx = count;
            for (int c = idx; c < idx + count; c++)
            {
                relIdx--;
                if (relIdx == 0)
                {
                    result = result + value[c].FastParse();
                    return result;
                }
                else if (relIdx == 1)
                {
                    result = result +
                        value[c].FastParse() * 10;
                }
                else if (relIdx == 2)
                {
                    result = result +
                        value[c].FastParse() * 100;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("value", "Value string too long, can be max 3 chars long.");
                }
            }
            return result;
        }

        /// <summary>
        /// Parses a number from a char value.
        /// </summary>
        public static int FastParse(this char value)
        {
            switch(value)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
            }
            throw new ArgumentOutOfRangeException("value", value.ToString());
        }

        /// <summary>
        /// Copies this feed to the given feed.
        /// </summary>
        public static void CopyTo(this IGTFSFeed thisFeed, IGTFSFeed feed)
        {
            var feedInfo = thisFeed.GetFeedInfo();
            if (feedInfo != null)
            {
                feed.SetFeedInfo(feedInfo);
            }

            feed.Agencies.AddRange(thisFeed.Agencies);
            feed.CalendarDates.AddRange(thisFeed.CalendarDates);
            feed.Calendars.AddRange(thisFeed.Calendars);
            feed.FareAttributes.AddRange(thisFeed.FareAttributes);
            feed.FareRules.AddRange(thisFeed.FareRules);
            feed.Frequencies.AddRange(thisFeed.Frequencies);
            feed.Routes.AddRange(thisFeed.Routes);
            feed.Shapes.AddRange(thisFeed.Shapes);
            feed.Stops.AddRange(thisFeed.Stops);
            feed.StopTimes.AddRange(thisFeed.StopTimes);
            feed.Transfers.AddRange(thisFeed.Transfers);
            feed.Trips.AddRange(thisFeed.Trips);
            feed.Levels.AddRange(thisFeed.Levels);
            feed.Pathways.AddRange(thisFeed.Pathways);
        }

        /// <summary>
        /// Merges the content of the given feed with this feed by adding or replacing entities.
        /// </summary>
        public static void Merge(this IGTFSFeed thisFeed, IGTFSFeed feed)
        {
            var feedInfo = feed.GetFeedInfo();
            if (feedInfo != null)
            {
                thisFeed.SetFeedInfo(feedInfo);
            }
            foreach (var entity in feed.Agencies)
            {
                thisFeed.Agencies.AddOrReplace(entity, x => x.Id);
            }
            foreach (var entity in feed.CalendarDates)
            {
                thisFeed.CalendarDates.AddOrReplace(entity);
            }
            foreach (var entity in feed.Calendars)
            {
                thisFeed.Calendars.AddOrReplace(entity);
            }
            foreach (var entity in feed.FareAttributes)
            {
                thisFeed.FareAttributes.AddOrReplace(entity);
            }
            foreach (var entity in feed.FareRules)
            {
                thisFeed.FareRules.AddOrReplace(entity, x => x.FareId);
            }
            foreach (var entity in feed.Frequencies)
            {
                thisFeed.Frequencies.AddOrReplace(entity);
            }
            foreach (var entity in feed.Routes)
            {
                thisFeed.Routes.AddOrReplace(entity, x => x.Id);
            }
            foreach (var entity in feed.Shapes)
            {
                thisFeed.Shapes.AddOrReplace(entity);
            }
            foreach (var entity in feed.Stops)
            {
                thisFeed.Stops.AddOrReplace(entity, x => x.Id);
            }
            foreach (var entity in feed.StopTimes)
            {
                thisFeed.StopTimes.AddOrReplace(entity);
            }
            foreach (var entity in feed.Transfers)
            {
                thisFeed.Transfers.AddOrReplace(entity);
            }
            foreach (var entity in feed.Trips)
            {
                thisFeed.Trips.AddOrReplace(entity, x => x.Id);
            }
            foreach (var entity in feed.Levels)
            {
                thisFeed.Levels.AddOrReplace(entity, x => x.Id);
            }
            foreach (var entity in feed.Pathways)
            {
                thisFeed.Pathways.AddOrReplace(entity, x => x.Id);
            }
        }

        /// <summary>
        /// Converts a number of milliseconds from 1/1/1970 into a standard DateTime.
        /// </summary>
        public static DateTime FromUnixTime(this long milliseconds)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// Converts a standard DateTime into the number of milliseconds since 1/1/1970.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }

        /// <summary>
        /// Calculates the distance in meter between the two given coordinates.
        /// </summary>
        public static double DistanceInMeter(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var radius_earth = 6371000;

            var degToRandian = Math.PI / 180;
            var lat1_rad = latitude1 * degToRandian;
            var lon1_rad = longitude1 * degToRandian;
            var lat2_rad = latitude2 * degToRandian;
            var lon2_rad = longitude2 * degToRandian;
            var dLat = (lat2_rad - lat1_rad);
            var dLon = (lon2_rad - lon1_rad);
            var a = Math.Pow(Math.Sin(dLat / 2), 2) +
                Math.Cos(lat1_rad) * Math.Cos(lat2_rad) *
                Math.Pow(Math.Sin(dLon / 2), 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = radius_earth * c;
            return distance;
        }

        /// <summary>
        /// Converts an integer rgb color value to a hex color string.
        /// </summary>
        /// <returns></returns>
        public static string ToHexColorString(this int? value)
        {
            if (value.HasValue)
            {
                var r = (short)(((uint)value.Value >> 16) % 256);
                var g = (short)(((uint)value.Value >> 8) % 256);
                var b = (short)((uint)value.Value % 256);

                return string.Format("{0}{1}{2}",
                                     r.ToString("X2"),
                                     g.ToString("X2"),
                                     b.ToString("X2"));
            }
            return string.Empty;
        }

        /// <summary>
        /// Adds or replaces an entity in the given collection.
        /// </summary>
        public static void AddOrReplace<T>(this IEntityCollection<T> collection, T entity)
            where T : Entities.GTFSEntity
        {
            if (!collection.Contains(entity))
            {
                collection.Add(entity);
            }
        }

        /// <summary>
        /// Adds or replaces an entity in the given collection.
        /// </summary>
        public static void AddOrReplace(this IStopTimeCollection collection, Entities.StopTime entity)
        {
            if (!collection.Contains(entity))
            {
                collection.Add(entity);
            }
        }

        /// <summary>
        /// Adds or replaces an entity in the given collection.
        /// </summary>
        public static void AddOrReplace(this ITransferCollection collection, Entities.Transfer entity)
        {
            if (!collection.Contains(entity))
            {
                collection.Add(entity);
            }
        }

        /// <summary>
        /// Adds or replaces an entity in the given collection.
        /// </summary>
        public static void AddOrReplace<T>(this IUniqueEntityCollection<T> collection, T entity, Func<T, string> getId)
            where T : Entities.GTFSEntity
        {
            var ent = collection.Get(getId(entity));
            if (ent != null)
            {
                collection.Remove(getId(entity));
            }
            collection.Add(entity);
        }

        /// <summary>
        /// Converts a hex color string to an argb value.
        /// </summary>
        /// <returns></returns>
        public static int? ToArgbInt(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            { // detect empty strings.
                return null;
            }

            if (value.Length == 6)
            { // assume # is missing.
                return Int32.Parse("FF" + value, NumberStyles.HexNumber,
                                    CultureInfo.InvariantCulture);
            }
            else if (value.Length == 7)
            { // assume #rrggbb
                return Int32.Parse("FF" + value.Replace("#", ""), NumberStyles.HexNumber,
                                    CultureInfo.InvariantCulture);
            }
            else if (value.Length == 9)
            {
                return Int32.Parse(value.Replace("#", ""), NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture);
            }
            else if (value.Length == 10)
            {
                return Int32.Parse(value.Replace("0x", ""), NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture);
            }
            else
            {
                throw new ArgumentOutOfRangeException("value", string.Format("The given string can is not a hex-color: {0}.", value));
            }
        }

        /// <summary>
        /// Returns a string representing the object in a culture invariant way.
        /// </summary>
        public static string ToInvariantString(this object obj)
        {
            return obj is IConvertible ? ((IConvertible)obj).ToString(CultureInfo.InvariantCulture)
                : obj is IFormattable ? ((IFormattable)obj).ToString(null, CultureInfo.InvariantCulture)
                : obj.ToString();
        }

        /// <summary>
        /// Splits the given list of entities into groups of size maxGroupCount.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_self"></param>
        /// <param name="maxGroupCount"></param>
        /// <returns></returns>
        public static Dictionary<int, List<T>> SplitIntoGroupsByGroupIdx<T>(this IEnumerable<T> _self, int maxGroupCount = 900)
        {
            var dict = new Dictionary<int, List<T>>();
            int groupIdx = 0;
            int numElementsInCurrentGroup = 0;
            foreach (var item in _self)
            {
                if (numElementsInCurrentGroup == maxGroupCount)
                {
                    groupIdx++;
                    numElementsInCurrentGroup = 0;
                }

                if (!dict.ContainsKey(groupIdx))
                {
                    dict.Add(groupIdx, new List<T>());
                }
                dict[groupIdx].Add(item);
                numElementsInCurrentGroup++;
            }

            return dict;
        }
    }
}