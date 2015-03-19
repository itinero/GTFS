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

using GTFS.Entities;
using GTFS.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace GTFS.DB.SQLite
{
    /// <summary>
    /// Represents a GFTS feed using an SQLite database.
    /// </summary>
    internal class SQLiteGTFSFeed : IGTFSFeed
    {
        /// <summary>
        /// Holds the connection.
        /// </summary>
        private SQLiteConnection _connection;

        /// <summary>
        /// Holds the id.
        /// </summary>
        private int _id;

        /// <summary>
        /// Creates a new SQLite GTFS feed.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id"></param>
        internal SQLiteGTFSFeed(SQLiteConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        /// <summary>
        /// Adds an agency to this feed.
        /// </summary>
        /// <param name="agency"></param>
        public void AddAgency(Agency agency)
        {
            string sql = "INSERT INTO agency VALUES (:feed_id, :id, :agency_name, :agency_url, :agency_timezone, :agency_lang, :agency_phone, :agency_fare_url);";
            using(var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"agency_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"agency_url", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"agency_timezone", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"agency_lang", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"agency_phone", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"agency_fare_url", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = agency.Id;
                command.Parameters[2].Value = agency.Name;
                command.Parameters[3].Value = agency.URL;
                command.Parameters[4].Value = agency.Timezone;
                command.Parameters[5].Value = agency.LanguageCode;
                command.Parameters[6].Value = agency.Phone;
                command.Parameters[7].Value = agency.FareURL;

                command.ExecuteNonQuery();
            }
        }

        public Agency GetAgency(string agencyId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAgency(string agencyId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all agencies.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Agency> GetAgencies()
        {
            string sql = "SELECT id, agency_name, agency_url, agency_timezone, agency_lang, agency_phone, agency_fare_url FROM agency WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Agency>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Agency()
                {
                    Id = x.GetString(0),
                    Name = x.IsDBNull(1) ? null : x.GetString(1),
                    URL = x.IsDBNull(2) ? null : x.GetString(2),
                    Timezone = x.IsDBNull(3) ? null : x.GetString(3),
                    LanguageCode = x.IsDBNull(4) ? null : x.GetString(4),
                    Phone = x.IsDBNull(5) ? null : x.GetString(5),
                    FareURL = x.IsDBNull(6) ? null : x.GetString(6)
                };
            });
        }

        /// <summary>
        /// Adds a calendar to this feed.
        /// </summary>
        /// <param name="calendar"></param>
        public void AddCalendar(Calendar calendar)
        {
            string sql = "INSERT INTO calendar VALUES (:feed_id, :service_id, :monday, :tuesday, :wednesday, :thursday, :friday, :saturday, :sunday, :start_date, :end_date);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"monday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"tuesday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"wednesday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"thursday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"friday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"saturday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"sunday", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"start_date", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"end_date", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = calendar.ServiceId;
                command.Parameters[2].Value = calendar.Monday ? 1 : 0;
                command.Parameters[3].Value = calendar.Tuesday ? 1 : 0;
                command.Parameters[4].Value = calendar.Wednesday ? 1 : 0;
                command.Parameters[5].Value = calendar.Thursday ? 1 : 0;
                command.Parameters[6].Value = calendar.Friday ? 1 : 0;
                command.Parameters[7].Value = calendar.Saturday ? 1 : 0;
                command.Parameters[8].Value = calendar.Sunday ? 1 : 0;
                command.Parameters[9].Value = calendar.StartDate.ToUnixTime();
                command.Parameters[10].Value = calendar.EndDate.ToUnixTime();

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all calendars.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Calendar> GetCalendars()
        {
            string sql = "SELECT service_id, monday, tuesday, wednesday, thursday, friday, saturday, sunday, start_date, end_date FROM calendar WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Calendar>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Calendar()
                {
                    ServiceId = x.GetString(0),
                    Monday = x.IsDBNull(1) ? false : x.GetInt64(1) == 1,
                    Tuesday = x.IsDBNull(2) ? false : x.GetInt64(2) == 1,
                    Wednesday = x.IsDBNull(3) ? false : x.GetInt64(3) == 1,
                    Thursday = x.IsDBNull(4) ? false : x.GetInt64(4) == 1,
                    Friday = x.IsDBNull(5) ? false : x.GetInt64(5) == 1,
                    Saturday = x.IsDBNull(6) ? false : x.GetInt64(6) == 1,
                    Sunday = x.IsDBNull(7) ? false : x.GetInt64(7) == 1,
                    StartDate = x.GetInt64(8).FromUnixTime(),
                    EndDate = x.GetInt64(9).FromUnixTime()
                };
            });
        }

        public IEnumerable<Calendar> GetCalendars(string serviceId)
        {
            throw new NotImplementedException();
        }

        public int RemoveCalendars(string serviceId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a calendar date to this feed.
        /// </summary>
        /// <param name="calendar"></param>
        public void AddCalendarDate(CalendarDate calendar)
        {
            string sql = "INSERT INTO calendar_date VALUES (:feed_id, :service_id, :date, :exception_type);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"date", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"exception_type", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = calendar.ServiceId;
                command.Parameters[2].Value = calendar.Date.ToUnixTime();
                command.Parameters[3].Value = (int)calendar.ExceptionType;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all calendar dates.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CalendarDate> GetCalendarDates()
        {
            string sql = "SELECT service_id, date, exception_type FROM calendar_date WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<CalendarDate>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new CalendarDate()
                {
                    ServiceId = x.GetString(0),
                    Date = x.GetInt64(1).FromUnixTime(),
                    ExceptionType = (ExceptionType)x.GetInt64(2)
                };
            });
        }

        public IEnumerable<CalendarDate> GetCalendarDates(string serviceId)
        {
            throw new NotImplementedException();
        }

        public int RemoveCalendarDates(string serviceId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a fare attribute to this feed.
        /// </summary>
        /// <param name="fareAttribute"></param>
        public void AddFareAttribute(FareAttribute fareAttribute)
        {
            string sql = "INSERT INTO fare_attribute VALUES (:feed_id, :fare_id, :price, :currency_type, :payment_method, :transfers, :transfer_duration);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"fare_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"price", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"currency_type", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"payment_method", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"transfers", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"transfer_duration", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = fareAttribute.FareId;
                command.Parameters[2].Value = fareAttribute.Price;
                command.Parameters[3].Value = fareAttribute.CurrencyType;
                command.Parameters[4].Value = (int)fareAttribute.PaymentMethod ;
                command.Parameters[5].Value = (int)fareAttribute.Transfers;
                command.Parameters[6].Value = fareAttribute.TransferDuration;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all fare attributes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FareAttribute> GetFareAttributes()
        {
            string sql = "SELECT fare_id, price, currency_type, payment_method, transfers, transfer_duration FROM fare_attribute WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<FareAttribute>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new FareAttribute()
                {
                    FareId = x.GetString(0),
                    Price = x.GetString(1),
                    CurrencyType = x.IsDBNull(1) ? null : x.GetString(2),
                    PaymentMethod = (PaymentMethodType)x.GetInt64(3),
                    Transfers = x.IsDBNull(4) ? null : (uint?)x.GetInt64(4),
                    TransferDuration = x.IsDBNull(5) ? null : x.GetString(5)
                };
            });
        }

        public IEnumerable<FareAttribute> GetFareAttributes(string fareId)
        {
            throw new NotImplementedException();
        }

        public int RemoveFareAttributes(string fareId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a fare rule to this feed.
        /// </summary>
        /// <param name="fareRule"></param>
        public void AddFareRule(FareRule fareRule)
        {
            string sql = "INSERT INTO fare_rule VALUES (:feed_id, :fare_id, :route_id, :origin_id, :destination_id, :contains_id);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"fare_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"origin_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"destination_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"contains_id", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = fareRule.FareId;
                command.Parameters[2].Value = fareRule.RouteId;
                command.Parameters[3].Value = fareRule.OriginId;
                command.Parameters[4].Value = fareRule.DestinationId;
                command.Parameters[5].Value = fareRule.ContainsId;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all the fare rules.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FareRule> GetFareRules()
        {
            string sql = "SELECT fare_id, route_id, origin_id, destination_id, contains_id FROM fare_rule WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<FareRule>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new FareRule()
                {
                    FareId = x.GetString(0),
                    RouteId = x.IsDBNull(1) ? null : x.GetString(1),
                    OriginId = x.IsDBNull(2) ? null : x.GetString(2),
                    DestinationId = x.IsDBNull(3) ? null : x.GetString(3),
                    ContainsId = x.IsDBNull(4) ? null : x.GetString(4)
                };
            });
        }

        public FareRule GetFareRule(string fareId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFareRule(string fareId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the feed info.
        /// </summary>
        /// <param name="feedInfo"></param>
        public void SetFeedInfo(FeedInfo feedInfo)
        {
            string sql = "UPDATE feed SET feed_publisher_name = :feed_publisher_name, feed_publisher_url = :feed_publisher_url, feed_lang = :feed_lang, feed_start_date = :feed_start_date, feed_end_date = :feed_end_date, feed_version = feed_version WHERE ID = :id;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_publisher_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"feed_publisher_url", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"feed_lang", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"feed_start_date", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"feed_end_date", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"feed_version", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.Int64));

                command.Parameters[0].Value = feedInfo.PublisherName;
                command.Parameters[1].Value = feedInfo.PublisherUrl;
                command.Parameters[2].Value = feedInfo.Lang;
                command.Parameters[3].Value = feedInfo.StartDate;
                command.Parameters[4].Value = feedInfo.EndDate;
                command.Parameters[5].Value = feedInfo.Version;
                command.Parameters[6].Value = _id;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns the feed info.
        /// </summary>
        /// <returns></returns>
        public FeedInfo GetFeedInfo()
        {
            FeedInfo feedInfo = null;
            string sql = "SELECT ID, feed_publisher_name, feed_publisher_url, feed_lang, feed_start_date, feed_end_date, feed_version FROM feed WHERE ID = :id;";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
                command.Parameters[0].Value = _id;

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        feedInfo = new FeedInfo();
                        feedInfo.PublisherName = reader.IsDBNull(1) ? null : reader.GetString(1);
                        feedInfo.PublisherUrl = reader.IsDBNull(2) ? null : reader.GetString(2);
                        feedInfo.Lang = reader.IsDBNull(3) ? null : reader.GetString(3);
                        feedInfo.StartDate = reader.IsDBNull(4) ? null : reader.GetString(4);
                        feedInfo.EndDate = reader.IsDBNull(5) ? null : reader.GetString(5);
                        feedInfo.Version = reader.IsDBNull(6) ? null : reader.GetString(6);
                        break;
                    }
                }
            }
            return feedInfo;
        }

        /// <summary>
        /// Adds a frequency to this feed.
        /// </summary>
        /// <param name="frequency"></param>
        public void AddFrequency(Frequency frequency)
        {
            string sql = "INSERT INTO frequency VALUES (:feed_id, :trip_id, :start_time, :end_time, :headway_secs, :exact_times);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"trip_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"start_time", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"end_time", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"headway_secs", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"exact_times", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = frequency.TripId;
                command.Parameters[2].Value = frequency.StartTime;
                command.Parameters[3].Value = frequency.EndTime;
                command.Parameters[4].Value = frequency.HeadwaySecs;
                command.Parameters[5].Value = frequency.ExactTimes;

                command.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Gets all frequencies.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Frequency> GetFrequencies()
        {
            string sql = "SELECT trip_id, start_time, end_time, headway_secs, exact_times FROM frequency WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Frequency>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Frequency()
                {
                    TripId = x.GetString(0),
                    StartTime = x.IsDBNull(1) ? null : x.GetString(1),
                    EndTime = x.IsDBNull(2) ? null : x.GetString(2),
                    HeadwaySecs = x.IsDBNull(3) ? null : x.GetString(3),
                    ExactTimes = x.IsDBNull(4) ? null : (bool?)(x.GetInt64(4) == 1)
                };
            });
        }

        public IEnumerable<Frequency> GetFrequencies(string tripId)
        {
            throw new NotImplementedException();
        }

        public int RemoveFrequencies(string tripId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a route to this feed.
        /// </summary>
        /// <param name="route"></param>
        public void AddRoute(Route route)
        {
            string sql = "INSERT INTO route VALUES (:feed_id, :id, :agency_id, :route_short_name, :route_long_name, :route_desc, :route_type, :route_url, :route_color, :route_text_color);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"agency_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_short_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_long_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_desc", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_type", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"route_url", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_color", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"route_text_color", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = route.Id;
                command.Parameters[2].Value = route.AgencyId;
                command.Parameters[3].Value = route.ShortName;
                command.Parameters[4].Value = route.LongName;
                command.Parameters[5].Value = route.Description;
                command.Parameters[6].Value = (int)route.Type;
                command.Parameters[7].Value = route.Url;
                command.Parameters[8].Value = route.Color;
                command.Parameters[9].Value = route.TextColor;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all routes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Route> GetRoutes()
        {
            string sql = "SELECT id, agency_id, route_short_name, route_long_name, route_desc, route_type, route_url, route_color, route_text_color FROM route WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Route>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Route()
                {
                    Id = x.GetString(0),
                    AgencyId = x.IsDBNull(1) ? null : x.GetString(1),
                    ShortName = x.IsDBNull(2) ? null : x.GetString(2),
                    LongName = x.IsDBNull(3) ? null : x.GetString(3),
                    Description = x.IsDBNull(4) ? null : x.GetString(4),
                    Type = (RouteType)x.GetInt64(5),
                    Url = x.IsDBNull(6) ? null : x.GetString(6),
                    Color = x.IsDBNull(7) ? null : (int?)x.GetInt64(7),
                    TextColor = x.IsDBNull(8) ? null : (int?)x.GetInt64(8)
                };
            });
        }

        public Route GetRoute(string routeId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRoute(string routeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new shape to this feed.
        /// </summary>
        /// <param name="shape"></param>
        public void AddShape(Shape shape)
        {
            string sql = "INSERT INTO shape VALUES (:feed_id, :id, :shape_pt_lat, :shape_pt_lon, :shape_pt_sequence, :shape_dist_traveled);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"shape_pt_lat", DbType.Double));
                command.Parameters.Add(new SQLiteParameter(@"shape_pt_lon", DbType.Double));
                command.Parameters.Add(new SQLiteParameter(@"shape_pt_sequence", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"shape_dist_traveled", DbType.Double));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = shape.Id;
                command.Parameters[2].Value = shape.Latitude;
                command.Parameters[3].Value = shape.Longitude ;
                command.Parameters[4].Value = shape.Sequence;
                command.Parameters[5].Value = shape.DistanceTravelled;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all shapes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Shape> GetShapes()
        {
            string sql = "SELECT id, shape_pt_lat, shape_pt_lon, shape_pt_sequence, shape_dist_traveled FROM shape WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Shape>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Shape()
                {
                    Id = x.GetString(0),
                    Latitude = x.GetDouble(1),
                    Longitude = x.GetDouble(2),
                    Sequence = (uint)x.GetInt64(3),
                    DistanceTravelled = x.IsDBNull(4) ? null : (double?)x.GetDouble(4)
                };
            });
        }

        public IEnumerable<Shape> GetShapes(string tripId)
        {
            throw new NotImplementedException();
        }

        public int RemoveShapes(string tripId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a stop to this feed.
        /// </summary>
        /// <param name="stop"></param>
        public void AddStop(Stop stop)
        {
            string sql = "INSERT INTO stop VALUES (:feed_id, :id, :stop_code, :stop_name, :stop_desc, :stop_lat, :stop_lon, :zone_id, :stop_url, :location_type, :parent_station, :stop_timezone, :wheelchair_boarding);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"stop_code", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"stop_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"stop_desc", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"stop_lat", DbType.Double));
                command.Parameters.Add(new SQLiteParameter(@"stop_lon", DbType.Double));
                command.Parameters.Add(new SQLiteParameter(@"zone_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"stop_url", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"location_type", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"parent_station", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"stop_timezone", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"wheelchair_boarding", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = stop.Id;
                command.Parameters[2].Value = stop.Code;
                command.Parameters[3].Value = stop.Name;
                command.Parameters[4].Value = stop.Description;
                command.Parameters[5].Value = stop.Latitude;
                command.Parameters[6].Value = stop.Longitude;
                command.Parameters[7].Value = stop.Zone;
                command.Parameters[8].Value = stop.Url;
                command.Parameters[9].Value = stop.LocationType.HasValue ? (int?)stop.LocationType.Value : null;
                command.Parameters[10].Value = stop.ParentStation;
                command.Parameters[11].Value = stop.Timezone;
                command.Parameters[12].Value = stop.WheelchairBoarding;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all the stops.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stop> GetStops()
        {
            string sql = "SELECT id, stop_code, stop_name, stop_desc, stop_lat, stop_lon, zone_id, stop_url, location_type, parent_station, stop_timezone, wheelchair_boarding FROM stop WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Stop>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Stop()
                {
                    Id = x.GetString(0),
                    Code = x.IsDBNull(1) ? null : x.GetString(1),
                    Name = x.IsDBNull(2) ? null : x.GetString(2),
                    Description = x.IsDBNull(3) ? null : x.GetString(3),
                    Latitude = x.GetDouble(4),
                    Longitude = x.GetDouble(5),
                    Zone = x.IsDBNull(6) ? null : x.GetString(6),
                    Url = x.IsDBNull(7) ? null : x.GetString(7),
                    LocationType = x.IsDBNull(8) ? null : (LocationType?)x.GetInt64(8),
                    ParentStation = x.IsDBNull(9) ? null : x.GetString(9),
                    Timezone = x.IsDBNull(10) ? null : x.GetString(10),
                    WheelchairBoarding = x.IsDBNull(11) ? null : x.GetString(11)
                };
            });
        }

        public Stop GetStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveStop(string stopId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a stop time to this feed.
        /// </summary>
        /// <param name="stopTime"></param>
        public void AddStopTime(StopTime stopTime)
        {
            string sql = "INSERT INTO stop_time VALUES (:feed_id, :trip_id, :arrival_time, :departure_time, :stop_id, :stop_sequence, :stop_headsign, :pickup_type, :drop_off_type, :shape_dist_traveled);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"trip_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"arrival_time", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"departure_time", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"stop_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"stop_sequence", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"stop_headsign", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"pickup_type", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"drop_off_type", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"shape_dist_traveled", DbType.String));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = stopTime.TripId;
                command.Parameters[2].Value = stopTime.ArrivalTime.TotalSeconds;
                command.Parameters[3].Value = stopTime.DepartureTime.TotalSeconds;
                command.Parameters[4].Value = stopTime.StopId;
                command.Parameters[5].Value = stopTime.StopSequence;
                command.Parameters[6].Value = stopTime.StopHeadsign;
                command.Parameters[7].Value = stopTime.PickupType.HasValue ? (int?)stopTime.PickupType.Value : null;
                command.Parameters[8].Value = stopTime.DropOffType.HasValue ? (int?)stopTime.DropOffType.Value : null;
                command.Parameters[9].Value = stopTime.ShapeDistTravelled;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns all stop times.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StopTime> GetStopTimes()
        {
            string sql = "SELECT trip_id, arrival_time, departure_time, stop_id, stop_sequence, stop_headsign, pickup_type, drop_off_type, shape_dist_traveled FROM stop_time WHERE FEED_ID = :id";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<StopTime>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new StopTime()
                {
                    TripId = x.GetString(0),
                    ArrivalTime = TimeOfDay.FromTotalSeconds(x.GetInt32(1)),
                    DepartureTime = TimeOfDay.FromTotalSeconds(x.GetInt32(2)),
                    StopId = x.GetString(3),
                    StopSequence = (uint)x.GetInt32(4),
                    StopHeadsign = x.IsDBNull(5) ? null : x.GetString(5),
                    PickupType = x.IsDBNull(6) ? null : (PickupType?)x.GetInt64(6),
                    DropOffType = x.IsDBNull(7) ? null : (DropOffType?)x.GetInt64(7),
                    ShapeDistTravelled = x.IsDBNull(8) ? null : x.GetString(8)
                };
            });
        }

        public IEnumerable<StopTime> GetStopTimesForTrip(string tripId)
        {
            throw new NotImplementedException();
        }

        public int RemoveStopTimesForTrip(string tripId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<StopTime> GetStopTimesForStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public int RemoveStopTimesForStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public void AddTransfer(Transfer transfer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transfer> GetTransfers()
        {
            string sql = "SELECT feed_id, from_stop_id, to_stop_id, transfer_type, min_transfer_time FROM transfer";
            var parameters = new List<SQLiteParameter>();

            return new SQLiteEnumerable<Transfer>(_connection, sql, new SQLiteParameter[0], (x) =>
            {
                return new Transfer()
                {
                    FromStopId = x.GetString(1),
                    ToStopId = x.GetString(2),
                    TransferType = (TransferType)x.GetInt64(3),
                    MinimumTransferTime = x.GetString(4)
                };
            });
        }

        public IEnumerable<Transfer> GetTransfersForFromStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public int RemoveTransfersForFromStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transfer> GetTransfersForToStop(string stopId)
        {
            throw new NotImplementedException();
        }

        public int RemoveTransfersForToStop(string stopId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new trip to this feed.
        /// </summary>
        /// <param name="trip"></param>
        public void AddTrip(Trip trip)
        {
            string sql = "INSERT INTO trip VALUES (:feed_id, :id, :route_id, :service_id, :trip_headsign, :trip_short_name, :direction_id, :block_id, :shape_id, :wheelchair_accessible);";
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter(@"feed_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"route_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"service_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"trip_headsign", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"trip_short_name", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"direction_id", DbType.Int64));
                command.Parameters.Add(new SQLiteParameter(@"block_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"shape_id", DbType.String));
                command.Parameters.Add(new SQLiteParameter(@"wheelchair_accessible", DbType.Int64));

                command.Parameters[0].Value = _id;
                command.Parameters[1].Value = trip.Id;
                command.Parameters[2].Value = trip.RouteId;
                command.Parameters[3].Value = trip.ServiceId;
                command.Parameters[4].Value = trip.Headsign;
                command.Parameters[5].Value = trip.ShortName;
                command.Parameters[6].Value = trip.Direction.HasValue ? (int?)trip.Direction.Value : null;
                command.Parameters[7].Value = trip.BlockId;
                command.Parameters[8].Value = trip.ShapeId;
                command.Parameters[9].Value = trip.AccessibilityType.HasValue ? (int?)trip.AccessibilityType.Value : null;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets all trips for this feed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Trip> GetTrips()
        {
            string sql = "SELECT id, route_id, service_id, trip_headsign, trip_short_name, direction_id, block_id, shape_id, wheelchair_accessible FROM trip WHERE FEED_ID = :id;";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters[0].Value = _id;

            return new SQLiteEnumerable<Trip>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Trip()
                {
                    Id = x.GetString(0),
                    RouteId = x.IsDBNull(1) ? null : x.GetString(1),
                    ServiceId = x.IsDBNull(2) ? null : x.GetString(2),
                    Headsign = x.IsDBNull(3) ? null : x.GetString(3),
                    ShortName = x.IsDBNull(4) ? null : x.GetString(4),
                    Direction = x.IsDBNull(5) ? null : (DirectionType?)x.GetInt64(5),
                    BlockId = x.IsDBNull(6) ? null : x.GetString(6),
                    ShapeId = x.IsDBNull(7) ? null : x.GetString(7),
                    AccessibilityType = x.IsDBNull(8) ? null : (WheelchairAccessibilityType?)x.GetInt64(8)
                };
            });
        }

        /// <summary>
        /// Returns all the trips for this feed.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public Trip GetTrip(string tripId)
        {
            string sql = "SELECT id, route_id, service_id, trip_headsign, trip_short_name, direction_id, block_id, shape_id, wheelchair_accessible FROM trip WHERE FEED_ID = :id AND ID = :trip_id;";
            var parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter(@"id", DbType.Int64));
            parameters.Add(new SQLiteParameter(@"trip_id", DbType.Int64));
            parameters[0].Value = _id;
            parameters[1].Value = tripId;

            return new SQLiteEnumerable<Trip>(_connection, sql, parameters.ToArray(), (x) =>
            {
                return new Trip()
                {
                    Id = x.GetString(0),
                    ServiceId = x.IsDBNull(1) ? null : x.GetString(1),
                    Headsign = x.IsDBNull(2) ? null : x.GetString(2),
                    ShortName = x.IsDBNull(3) ? null : x.GetString(3),
                    Direction = x.IsDBNull(4) ? null : (DirectionType?)x.GetInt64(4),
                    BlockId = x.IsDBNull(5) ? null : x.GetString(5),
                    ShapeId = x.IsDBNull(6) ? null : x.GetString(6),
                    AccessibilityType = x.IsDBNull(7) ? null : (WheelchairAccessibilityType?)x.GetInt64(7)
                };
            }).FirstOrDefault();
        }

        public bool RemoveTrip(string tripId)
        {
            throw new NotImplementedException();
        }
    }
}