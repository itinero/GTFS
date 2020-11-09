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

using GTFS.IO;
using System.Linq;
using System.Collections.Generic;
using GTFS.Entities;
using GTFS.Entities.Enumerations;
using System;
using System.Text;

namespace GTFS
{
    /// <summary>
    /// A GTFS writer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GTFSWriter<T> where T : IGTFSFeed
    {
        /// <summary>
        /// Writes the given feed to the given target files.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="target"></param>
        public void Write(T feed, IEnumerable<IGTFSTargetFile> target)
        {
            // order files by id
            var agenciesToWrite = feed.Agencies.OrderBy(x => x.Id).ToList();
            var calendarDatesToWrite = feed.CalendarDates.OrderBy(x => x.ServiceId).OrderBy(y => y.ExceptionType).OrderBy(z => z.Date).ToList();
            var calendarsToWrite = feed.Calendars.OrderBy(x => x.ServiceId).ToList();
            var fareAttributesToWrite = feed.FareAttributes.OrderBy(x => x.FareId).ToList();
            var fareRulesToWrite = feed.FareRules.OrderBy(x => x.RouteId).ToList();
            var frequenciesToWrite = feed.Frequencies.OrderBy(x => x.TripId).ToList();
            var routesToWrite = feed.Routes.OrderBy(x => x.Id).ToList();
            var stopsToWrite = feed.Stops.OrderBy(x => x.Id).ToList();
            var stopTimesToWrite = feed.StopTimes.OrderBy(x => x.TripId).ToList();
            var tripsToWrite = feed.Trips.OrderBy(x => x.Id).ToList();
            var levelsToWrite = feed.Levels.OrderBy(x => x.Id).ToList();
            var pathwaysToWrite = feed.Pathways.OrderBy(x => x.Id).ToList();

            // write files on-by-one.
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "agency"), agenciesToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "calendar_dates"), calendarDatesToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "calendar"), calendarsToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "fare_attributes"), fareAttributesToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "fare_rules"), fareRulesToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "feed_info"), feed.GetFeedInfo());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "frequencies"), frequenciesToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "routes"), routesToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "shapes"), feed.Shapes);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "stops"), stopsToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "stop_times"), stopTimesToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "transfers"), feed.Transfers);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "trips"), tripsToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "levels"), levelsToWrite);
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "pathways"), pathwaysToWrite);
        }

        /// <summary>
        /// Writes all levels to the given levels file.
        /// </summary>
        /// <param name="levelsFile"></param>
        /// <param name="levels"></param>
        protected virtual void Write(IGTFSTargetFile levelsFile, IEnumerable<Level> levels)
        {
            if (levelsFile == null) return;
            
            bool initialized = false;
            var data = new string[3];
            foreach (var level in levels)
            {
                if (!initialized)
                {
                    if (levelsFile.Exists)
                    {
                        levelsFile.Clear();
                    }

                    // write headers.
                    data[0] = "level_id";
                    data[1] = "level_index";
                    data[2] = "level_name";
                    levelsFile.Write(data);
                    initialized = true;
                }

                // write level details.
                data[0] = this.WriteFieldString("level", "level_id", level.Id);
                data[1] = this.WriteFieldDouble("level", "level_index", level.Index);
                data[2] = this.WriteFieldString("level", "level_name", level.Name, true);
                levelsFile.Write(data);
            }
            levelsFile.Close();
        }

        /// <summary>
        /// Writes all pathways to the given pathways file.
        /// </summary>
        /// <param name="pathwaysFile"></param>
        /// <param name="pathways"></param>
        protected virtual void Write(IGTFSTargetFile pathwaysFile, IEnumerable<Pathway> pathways)
        {
            if (pathwaysFile == null) return;
            
            bool initialized = false;
            var data = new string[12];
            foreach (var pathway in pathways)
            {
                if (!initialized)
                {
                    if (pathwaysFile.Exists)
                    {
                        pathwaysFile.Clear();
                    }

                    // write headers.
                    data[0] = "pathway_id";
                    data[1] = "from_stop_id";
                    data[2] = "to_stop_id";
                    data[3] = "pathway_mode";
                    data[4] = "is_bidirectional";
                    data[5] = "length";
                    data[6] = "traversal_time";
                    data[7] = "stair_count";
                    data[8] = "max_slope";
                    data[9] = "min_width";
                    data[10] = "signposted_as";
                    data[11] = "reversed_signposted_as";
                    pathwaysFile.Write(data);
                    initialized = true;
                }

                // write pathway details.
                data[0] = this.WriteFieldString("pathway", "pathway_id", pathway.Id);
                data[1] = this.WriteFieldString("pathway", "from_stop_id", pathway.FromStopId);
                data[2] = this.WriteFieldString("pathway", "to_stop_id", pathway.ToStopId);
                data[3] = this.WriteFieldPathwayMode("pathway", "pathway_mode", pathway.PathwayMode);
                data[4] = this.WriteFieldIsBidirectional("pathway", "is_bidirectional", pathway.IsBidirectional);
                data[5] = this.WriteFieldDouble("pathway", "length", pathway.Length);
                data[6] = this.WriteFieldInt("pathway", "traversal_time", pathway.TraversalTime);
                data[7] = this.WriteFieldInt("pathway", "stair_count", pathway.StairCount);
                data[8] = this.WriteFieldDouble("pathway", "max_slope", pathway.MaxSlope);
                data[9] = this.WriteFieldDouble("pathway", "min_width", pathway.MinWidth);
                data[10] = this.WriteFieldString("pathway", "signposted_as", pathway.SignpostedAs);
                data[11] = this.WriteFieldString("pathway", "reversed_signposted_as", pathway.ReversedSignpostedAs);
                pathwaysFile.Write(data);
            }
            pathwaysFile.Close();
        }

        /// <summary>
        /// Writes all agencies to the given agencies file.
        /// </summary>
        /// <param name="agenciesFile"></param>
        /// <param name="agencies"></param>
        protected virtual void Write(IGTFSTargetFile agenciesFile, IEnumerable<Agency> agencies)
        {
            if (agenciesFile != null)
            {
                bool initialized = false;
                var data = new string[8];
                foreach (var agency in agencies)
                {
                    if (!initialized)
                    {
                        if (agenciesFile.Exists)
                        {
                            agenciesFile.Clear();
                        }

                        // write headers.
                        data[0] = "agency_id";
                        data[1] = "agency_name";
                        data[2] = "agency_url";
                        data[3] = "agency_timezone";
                        data[4] = "agency_lang";
                        data[5] = "agency_phone";
                        data[6] = "agency_fare_url";
                        data[7] = "agency_email";
                        agenciesFile.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("agency", "agency_id", agency.Id);
                    data[1] = this.WriteFieldString("agency", "agency_name", agency.Name, true);
                    data[2] = this.WriteFieldString("agency", "agency_url", agency.URL);
                    data[3] = this.WriteFieldString("agency", "agency_timezone", agency.Timezone);
                    data[4] = this.WriteFieldString("agency", "agency_lang", agency.LanguageCode);
                    data[5] = this.WriteFieldString("agency", "agency_phone", agency.Phone);
                    data[6] = this.WriteFieldString("agency", "agency_fare_url", agency.FareURL);
                    data[7] = this.WriteFieldString("agency", "agency_email", agency.Email);
                    agenciesFile.Write(data);
                }
                agenciesFile.Close();
            }
        }

        /// <summary>
        /// Writes all calendar dates.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<CalendarDate> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[3];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "service_id";
                        data[1] = "date";
                        data[2] = "exception_type";
                        file.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("calendar_dates", "service_id", entity.ServiceId);
                    data[1] = this.WriteFieldDate("calendar_dates", "date", entity.Date);
                    data[2] = this.WriteFieldExceptionType("calendar_dates", "exception_type", entity.ExceptionType);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the calenders to the calenders file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Calendar> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[10];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "service_id";
                        data[1] = "monday";
                        data[2] = "tuesday";
                        data[3] = "wednesday";
                        data[4] = "thursday";
                        data[5] = "friday";
                        data[6] = "saturday";
                        data[7] = "sunday";
                        data[8] = "start_date";
                        data[9] = "end_date";
                        file.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("calendar", "service_id", entity.ServiceId);
                    data[1] = this.WriteFieldBool("calendar", "monday", entity.Monday);
                    data[2] = this.WriteFieldBool("calendar", "tuesday", entity.Tuesday);
                    data[3] = this.WriteFieldBool("calendar", "wednesday", entity.Wednesday);
                    data[4] = this.WriteFieldBool("calendar", "thursday", entity.Thursday);
                    data[5] = this.WriteFieldBool("calendar", "friday", entity.Friday);
                    data[6] = this.WriteFieldBool("calendar", "saturday", entity.Saturday);
                    data[7] = this.WriteFieldBool("calendar", "sunday", entity.Sunday);
                    data[8] = this.WriteFieldDate("calendar", "start_date", entity.StartDate);
                    data[9] = this.WriteFieldDate("calendar", "end_date", entity.EndDate);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the fare attributes.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<FareAttribute> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[7];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "fare_id";
                        data[1] = "price";
                        data[2] = "currency_type";
                        data[3] = "payment_method";
                        data[4] = "transfers";
                        data[5] = "agency_id";
                        data[6] = "transfer_duration";
                        file.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("fare_attributes", "fare_id", entity.FareId);
                    data[1] = this.WriteFieldString("fare_attributes", "price", entity.Price);
                    data[2] = this.WriteFieldString("fare_attributes", "currency_type", entity.CurrencyType);
                    data[3] = this.WriteFieldPaymentMethod("fare_attributes", "payment_method", entity.PaymentMethod);
                    data[4] = this.WriteFieldUint("fare_attributes", "transfers", entity.Transfers);
                    data[5] = this.WriteFieldString("fare_attributes", "agency_id", entity.AgencyId);
                    data[6] = this.WriteFieldString("fare_attributes", "transfer_duration", entity.TransferDuration);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the fare rules.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<FareRule> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[5];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "fare_id";
                        data[1] = "route_id";
                        data[2] = "origin_id";
                        data[3] = "destination_id";
                        data[4] = "contains_id";
                        file.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("fare_rules", "fare_id", entity.FareId);
                    data[1] = this.WriteFieldString("fare_rules", "route_id", entity.RouteId);
                    data[2] = this.WriteFieldString("fare_rules", "origin_id", entity.OriginId);
                    data[3] = this.WriteFieldString("fare_rules", "destination_id", entity.DestinationId);
                    data[4] = this.WriteFieldString("fare_rules", "contains_id", entity.ContainsId);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the feed info.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entity"></param>
        protected virtual void Write(IGTFSTargetFile file, FeedInfo entity)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[6];
                if (!initialized)
                {
                    if (file.Exists)
                    {
                        file.Clear();
                    }

                    // write headers.
                    data[0] = "feed_publisher_name";
                    data[1] = "feed_publisher_url";
                    data[2] = "feed_lang";
                    data[3] = "feed_start_date";
                    data[4] = "feed_end_date";
                    data[5] = "feed_version";
                    file.Write(data);
                    initialized = true;
                }

                // write details.
                data[0] = this.WriteFieldString("feed_info", "feed_publisher_name", entity.PublisherName, true);
                data[1] = this.WriteFieldString("feed_info", "feed_publisher_url", entity.PublisherUrl, true);
                data[2] = this.WriteFieldString("feed_info", "feed_lang", entity.Lang);
                data[3] = this.WriteFieldString("feed_info", "feed_start_date", entity.StartDate);
                data[4] = this.WriteFieldString("feed_info", "feed_end_date", entity.EndDate);
                data[5] = this.WriteFieldString("feed_info", "feed_version", entity.Version);
                file.Write(data);
                file.Close();
            }
        }

        /// <summary>
        /// Writes the frequencies.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Frequency> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[5];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "trip_id";
                        data[1] = "start_time";
                        data[2] = "end_time";
                        data[3] = "headway_secs";
                        data[4] = "exact_times";
                        file.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("frequency", "trip_id", entity.TripId);
                    data[1] = this.WriteFieldString("frequency", "start_time", entity.StartTime);
                    data[2] = this.WriteFieldString("frequency", "end_time", entity.EndTime);
                    data[3] = this.WriteFieldString("frequency", "headway_secs", entity.HeadwaySecs);
                    data[4] = this.WriteFieldBool("frequency", "exact_times", entity.ExactTimes);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the routes.
        /// </summary>
        /// <param name="routesFile"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile routesFile, IEnumerable<Route> entities)
        {
            if (routesFile != null)
            {
                bool initialized = false;
                var data = new string[11];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (routesFile.Exists)
                        {
                            routesFile.Clear();
                        }

                        // write headers.
                        data[0] = "route_id";
                        data[1] = "agency_id";
                        data[2] = "route_short_name";
                        data[3] = "route_long_name";
                        data[4] = "route_desc";
                        data[5] = "route_type";
                        data[6] = "route_url";
                        data[7] = "route_color";
                        data[8] = "route_text_color";
                        data[9] = "continuous_pickup";
                        data[10] = "continuous_drop_off";
                        routesFile.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("routes", "route_id", entity.Id);
                    data[1] = this.WriteFieldString("routes", "agency_id", entity.AgencyId);
                    data[2] = this.WriteFieldString("routes", "route_short_name", entity.ShortName, true);
                    data[3] = this.WriteFieldString("routes", "route_long_name", entity.LongName, true);
                    data[4] = this.WriteFieldString("routes", "route_desc", entity.Description);
                    data[5] = this.WriteFieldRouteType("routes", "route_type", entity.Type);
                    data[6] = this.WriteFieldString("routes", "route_url", entity.Url);
                    data[7] = this.WriteFieldColor("routes", "route_color", entity.Color);
                    data[8] = this.WriteFieldColor("routes", "route_text_color", entity.TextColor);
                    data[9] = this.WriteFieldContinuousPickup("routes", "continuous_pickup", entity.ContinuousPickup);
                    data[10] = this.WriteFieldContinuousDropOff("routes", "continuous_drop_off", entity.ContinuousDropOff);
                    routesFile.Write(data);
                }
                routesFile.Close();
            }
        }

        /// <summary>
        /// Writes the shapes.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Shape> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[5];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "shape_id";
                        data[1] = "shape_pt_lat";
                        data[2] = "shape_pt_lon";
                        data[3] = "shape_pt_sequence";
                        data[4] = "shape_dist_traveled";
                        file.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("shapes", "shape_id", entity.Id);
                    data[1] = this.WriteFieldDouble("shapes", "shape_pt_lat", entity.Latitude);
                    data[2] = this.WriteFieldDouble("shapes", "shape_pt_lon", entity.Longitude);
                    data[3] = this.WriteFieldUint("shapes", "shape_pt_sequence", entity.Sequence);
                    data[4] = this.WriteFieldDouble("shapes", "shape_dist_traveled", entity.DistanceTravelled);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the stops.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Stop> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[14];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "stop_id";
                        data[1] = "stop_code";
                        data[2] = "stop_name";
                        data[3] = "stop_desc";
                        data[4] = "stop_lat";
                        data[5] = "stop_lon";
                        data[6] = "zone_id";
                        data[7] = "stop_url";
                        data[8] = "location_type";
                        data[9] = "parent_station";
                        data[10] = "stop_timezone";
                        data[11] = "wheelchair_boarding";
                        data[12] = "level_id";
                        data[13] = "platform_code";
                        file.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("stops", "stop_id", entity.Id);
                    data[1] = this.WriteFieldString("stops", "stop_code", entity.Code);
                    data[2] = this.WriteFieldString("stops", "stop_name", entity.Name, true);
                    data[3] = this.WriteFieldString("stops", "stop_desc", entity.Description, true);
                    data[4] = this.WriteFieldDouble("stops", "stop_lat", entity.Latitude);
                    data[5] = this.WriteFieldDouble("stops", "stop_lon", entity.Longitude);
                    data[6] = this.WriteFieldString("stops", "zone_id", entity.Zone);
                    data[7] = this.WriteFieldString("stops", "stop_url", entity.Url);
                    data[8] = this.WriteFieldLocationType("stops", "location_type", entity.LocationType);
                    data[9] = this.WriteFieldString("stops", "parent_station", entity.ParentStation);
                    data[10] = this.WriteFieldString("stops", "stop_timezone", entity.Timezone);
                    data[11] = this.WriteFieldString("stops", "wheelchair_boarding", entity.WheelchairBoarding);
                    data[12] = this.WriteFieldString("stops", "level_id", entity.LevelId);
                    data[13] = this.WriteFieldString("stops", "platform_code", entity.PlatformCode);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the stop times.
        /// </summary>
        /// <param name="stopTimesFile"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile stopTimesFile, IEnumerable<StopTime> entities)
        {
            if (stopTimesFile != null)
            {
                bool initialized = false;
                var data = new string[12];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (stopTimesFile.Exists)
                        {
                            stopTimesFile.Clear();
                        }

                        // write headers.
                        data[0] = "trip_id";
                        data[1] = "arrival_time";
                        data[2] = "departure_time";
                        data[3] = "stop_id";
                        data[4] = "stop_sequence";
                        data[5] = "stop_headsign";
                        data[6] = "pickup_type";
                        data[7] = "drop_off_type";
                        data[8] = "shape_dist_traveled";
                        data[9] = "timepoint";
                        data[10] = "continuous_pickup";
                        data[11] = "continuous_drop_off";
                        stopTimesFile.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("stop_times", "trip_id", entity.TripId);
                    data[1] = this.WriteFieldTimeOfDay("stop_times", "arrival_time", entity.ArrivalTime);
                    data[2] = this.WriteFieldTimeOfDay("stop_times", "departure_time", entity.DepartureTime);
                    data[3] = this.WriteFieldString("stop_times", "stop_id", entity.StopId);
                    data[4] = this.WriteFieldUint("stop_times", "stop_sequence", entity.StopSequence);
                    data[5] = this.WriteFieldString("stop_times", "stop_headsign", entity.StopHeadsign, true);
                    data[6] = this.WriteFieldPickupType("stop_times", "pickup_type", entity.PickupType);
                    data[7] = this.WriteFieldDropOffType("stop_times", "drop_off_type", entity.DropOffType);
                    data[8] = this.WriteFieldDouble("stop_times", "shape_dist_traveled", entity.ShapeDistTravelled);
                    data[9] = this.WriteFieldTimepointType("stop_times", "timepoint", entity.TimepointType);
                    data[10] = this.WriteFieldContinuousPickup("stop_times", "continuous_pickup", entity.ContinuousPickup);
                    data[11] = this.WriteFieldContinuousDropOff("stop_times", "continuous_drop_off", entity.ContinuousDropOff);
                    stopTimesFile.Write(data);
                }
                stopTimesFile.Close();
            }
        }

        /// <summary>
        /// Writes the transfers.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Transfer> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[4];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "from_stop_id";
                        data[1] = "to_stop_id";
                        data[2] = "transfer_type";
                        data[3] = "min_transfer_time";
                        file.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("transfers", "from_stop_id", entity.FromStopId);
                    data[1] = this.WriteFieldString("transfers", "to_stop_id", entity.ToStopId);
                    data[2] = this.WriteFieldTransferType("transfers", "transfer_type", entity.TransferType);
                    data[3] = this.WriteFieldString("transfers", "min_transfer_time", entity.MinimumTransferTime);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the trips.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Trip> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[9];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "trip_id";
                        data[1] = "route_id";
                        data[2] = "service_id";
                        data[3] = "trip_headsign";
                        data[4] = "trip_short_name";
                        data[5] = "direction_id";
                        data[6] = "block_id";
                        data[7] = "shape_id";
                        data[8] = "wheelchair_accessible";
                        file.Write(data);
                        initialized = true;
                    }

                    // write details.
                    data[0] = this.WriteFieldString("trips", "trip_id", entity.Id);
                    data[1] = this.WriteFieldString("trips", "route_id", entity.RouteId);
                    data[2] = this.WriteFieldString("trips", "service_id", entity.ServiceId);
                    data[3] = this.WriteFieldString("trips", "trip_headsign", entity.Headsign);
                    data[4] = this.WriteFieldString("trips", "trip_short_name", entity.ShortName, true);
                    data[5] = this.WriteFieldDirectionType("trips", "direction_id", entity.Direction);
                    data[6] = this.WriteFieldString("trips", "block_id", entity.BlockId);
                    data[7] = this.WriteFieldString("trips", "shape_id", entity.ShapeId);
                    data[8] = this.WriteFieldAccessibilityType("trips", "wheelchair_accessible", entity.AccessibilityType);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes a string-field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string WriteFieldString(string name, string fieldName, string value)
        {
            var quote = !string.IsNullOrWhiteSpace(value) && value.Contains(",");
            return this.WriteFieldString(name, fieldName, value, quote);
        }

        /// <summary>
        /// Writes a string-field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        protected virtual string WriteFieldString(string name, string fieldName, string value, bool quote)
        {
            if (quote)
            { // quotes.
                var valueBuilder = new StringBuilder();
                valueBuilder.Append('"');
                valueBuilder.Append(value);
                valueBuilder.Append('"');
                return valueBuilder.ToString();
            }
            return value;
        }

        /// <summary>
        /// Writes the exception type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldExceptionType(string name, string fieldName, ExceptionType value)
        {
            //A value of 1 indicates that service has been added for the specified date.
            //A value of 2 indicates that service has been removed for the specified date.

            switch (value)
            {
                case ExceptionType.Added:
                    return "1";
                case ExceptionType.Removed:
                    return "2";
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the date.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        protected string WriteFieldDate(string name, string fieldName, DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Writes the bool.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldBool(string name, string fieldName, bool? value)
        {
            if (value.HasValue)
            {
                if (value.Value)
                {
                    return "1";
                }
                return "0";
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the uint.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldUint(string name, string fieldName, uint? value)
        {
            if (value.HasValue)
            {
                return value.Value.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the payment method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldPaymentMethod(string name, string fieldName, PaymentMethodType value)
        {
            //0 - Fare is paid on board.
            //1 - Fare must be paid before boarding.

            switch (value)
            {
                case PaymentMethodType.OnBoard:
                    return "0";
                case PaymentMethodType.BeforeBoarding:
                    return "1";
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes a color.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldColor(string name, string fieldName, int? value)
        {
            return value.ToHexColorString();
        }

        /// <summary>
        /// Writes the route type.
        /// </summary>
        /// <returns></returns>
        private string WriteFieldRouteType(string name, string fieldName, RouteTypeExtended value)
        {
            return ((int)value).ToString();
        }

        /// <summary>
        /// Writes a double.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldDouble(string name, string fieldName, double? value)
        {
            if (value.HasValue)
            {
                return value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes an int.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldInt(string name, string fieldName, int? value)
        {
            if (value.HasValue)
            {
                return value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the location type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldLocationType(string name, string fieldName, LocationType? value)
        {
            if (value.HasValue)
            {
                switch (value.Value)
                {
                    case LocationType.Stop:
                        return "0";
                    case LocationType.Station:
                        return "1";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the drop off type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldDropOffType(string name, string fieldName, DropOffType? value)
        {
            if (value.HasValue)
            {
                switch (value.Value)
                {
                    case DropOffType.Regular:
                        return "0";
                    case DropOffType.NoPickup:
                        return "1";
                    case DropOffType.PhoneForPickup:
                        return "2";
                    case DropOffType.DriverForPickup:
                        return "3";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the timepoint value.
        /// </summary>
        /// <returns></returns>
        protected string WriteFieldTimepointType(string name, string fieldName, TimePointType value)
        {
            switch (value)
            {
                case TimePointType.Approximate:
                    return "0";
                case TimePointType.Exact:
                    return "1";
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the pickup type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldPickupType(string name, string fieldName, PickupType? value)
        {
            if (value.HasValue)
            {
                switch (value.Value)
                {
                    case PickupType.Regular:
                        return "0";
                    case PickupType.NoPickup:
                        return "1";
                    case PickupType.PhoneForPickup:
                        return "2";
                    case PickupType.DriverForPickup:
                        return "3";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the continuous_pickup type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldContinuousPickup(string name, string fieldName, ContinuousPickup? value)
        {
            if (value.HasValue)
            {
                switch (value.Value)
                {
                    case ContinuousPickup.ContinuousStoppingPickup:
                        return "0";
                    case ContinuousPickup.None:
                        return "1";
                    case ContinuousPickup.PhoneForPickup:
                        return "2";
                    case ContinuousPickup.DriverForPickup:
                        return "3";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the continuous_drop_off type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldContinuousDropOff(string name, string fieldName, ContinuousDropOff? value)
        {
            if (value.HasValue)
            {
                switch (value.Value)
                {
                    case ContinuousDropOff.ContinuousStoppingDropOff:
                        return "0";
                    case ContinuousDropOff.None:
                        return "1";
                    case ContinuousDropOff.PhoneForDropOff:
                        return "2";
                    case ContinuousDropOff.DriverForDropOff:
                        return "3";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes a timeofday.
        /// </summary>
        /// <returns></returns>
        protected string WriteFieldTimeOfDay(string name, string fieldName, TimeOfDay? value)
        {
            if (!value.HasValue)
            {
                return string.Empty;
            }
            return string.Format("{0}:{1}:{2}",
                value.Value.Hours.ToString("00"),
                value.Value.Minutes.ToString("00"),
                value.Value.Seconds.ToString("00"));
        }

        /// <summary>
        /// Writes a transfertime.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldTransferType(string name, string fieldName, TransferType value)
        {
            switch (value)
            {
                case TransferType.Recommended:
                    return "0";
                case TransferType.TimedTransfer:
                    return "1";
                case TransferType.MinimumTime:
                    return "2";
                case TransferType.NotPossible:
                    return "3";
            }

            return string.Empty;
        }

        /// <summary>
        /// Writes an accessibility type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldAccessibilityType(string name, string fieldName, WheelchairAccessibilityType? value)
        {
            if (value.HasValue)
            {
                //0 (or empty) - indicates that there is no accessibility information for the trip
                //1 - indicates that the vehicle being used on this particular trip can accommodate at least one rider in a wheelchair
                //2 - indicates that no riders in wheelchairs can be accommodated on this trip

                switch (value.Value)
                {
                    case WheelchairAccessibilityType.NoInformation:
                        return "0";
                    case WheelchairAccessibilityType.SomeAccessibility:
                        return "1";
                    case WheelchairAccessibilityType.NoAccessibility:
                        return "2";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes is bidirectional.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldIsBidirectional(string name, string fieldName, IsBidirectional? value)
        {
            if (value.HasValue)
            {
                //0: Unidirectional pathway, it can only be used from from_stop_id to to_stop_id.
                //1: Bidirectional pathway, it can be used in the two directions.

                switch (value.Value)
                {
                    case IsBidirectional.Unidirectional:
                        return "0";
                    case IsBidirectional.Bidirectional:
                        return "1";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes a pathway mode.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldPathwayMode(string name, string fieldName, PathwayMode? value)
        {
            if (value.HasValue)
            {
                //1 - walkway
                //2 - stairs
                //3 - moving sidewalk/travelator
                //4 - escalator
                //5 - elevator
                //6 - fare gate (or payment gate): A pathway that crosses into an area of the station where a proof of payment is required (usually via a physical payment gate).
                //7 - exit gate: Indicates a pathway exiting an area where proof-of-payment is required into an area where proof-of-payment is no longer required.

                switch (value.Value)
                {
                    case PathwayMode.Walkway:
                        return "1";
                    case PathwayMode.Stairs:
                        return "2";
                    case PathwayMode.Travelator:
                        return "3";
                    case PathwayMode.Escalator:
                        return "4";
                    case PathwayMode.Elevator:
                        return "5";
                    case PathwayMode.FareGate:
                        return "6";
                    case PathwayMode.ExitGate:
                        return "7";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes a direction type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string WriteFieldDirectionType(string name, string fieldName, DirectionType? value)
        {
            if (value.HasValue)
            {

                //0 - travel in one direction (e.g. outbound travel)
                //1 - travel in the opposite direction (e.g. inbound travel)

                switch (value.Value)
                {
                    case DirectionType.OneDirection:
                        return "0";
                    case DirectionType.OppositeDirection:
                        return "1";
                }
            }
            return string.Empty;
        }
    }
}
