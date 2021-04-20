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

using GTFS.DB;
using GTFS.Entities;
using GTFS.Entities.Enumerations;
using GTFS.Exceptions;
using GTFS.Fields;
using GTFS.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GTFS
{
    /// <summary>
    /// A GTFS reader.
    /// </summary>
    public class GTFSReader<T> where T : IGTFSFeed
    {
        /// <summary>
        /// Flag making this reader very strict about the GTFS-spec.
        /// </summary>
        private bool _strict = true;

        /// <summary>
        /// Creates a new GTFS reader.
        /// </summary>
        public GTFSReader()
            : this(false)
        {

        }

        /// <summary>
        /// Creates a new GTFS reader.
        /// </summary>
        /// <param name="strict">Flag to set strict behaviour.</param>
        public GTFSReader(bool strict)
        {
            _strict = strict;

            this.DateTimeReader = (dateString) =>
                DateTime.ParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            this.DateTimeWriter = (date) => date.ToString("yyyyMMdd");
            this.TimeOfDayReader = (timeOfDayString) =>
            {
                if (string.IsNullOrWhiteSpace(timeOfDayString))
                {
                    return new TimeOfDay()
                    {
                        Hours = 0,
                        Minutes = 0,
                        Seconds = 0
                    };
                }
                else if (!(timeOfDayString.Length == 8 || timeOfDayString.Length == 7))
                {
                    throw new ArgumentException($"Invalid timeOfDayString: {timeOfDayString}");
                }

                var timeOfDay = new TimeOfDay();
                if (timeOfDayString.Length == 8)
                {
                    timeOfDay.Hours = timeOfDayString.FastParse(0, 2);
                    timeOfDay.Minutes = timeOfDayString.FastParse(3, 2);
                    timeOfDay.Seconds = timeOfDayString.FastParse(6, 2);
                    return timeOfDay;
                }

                timeOfDay.Hours = timeOfDayString.FastParse(0, 1);
                timeOfDay.Minutes = timeOfDayString.FastParse(2, 2);
                timeOfDay.Seconds = timeOfDayString.FastParse(5, 2);
                return timeOfDay;
            };
            this.TimeOfDayWriter = (timeOfDay) => { throw new NotImplementedException(); };

            // initialize maps.
            this.AttributionMap = new FieldMap();
            this.AgencyMap = new FieldMap();
            this.CalendarDateMap = new FieldMap();
            this.CalendarMap = new FieldMap();
            this.FareAttributeMap = new FieldMap();
            this.FareRuleMap = new FieldMap();
            this.FeedInfoMap = new FieldMap();
            this.FrequencyMap = new FieldMap();
            this.RouteMap = new FieldMap();
            this.ShapeMap = new FieldMap();
            this.StopMap = new FieldMap();
            this.StopTimeMap = new FieldMap();
            this.TransferMap = new FieldMap();
            this.TripMap = new FieldMap();
            this.LevelMap = new FieldMap();
            this.PathwayMap = new FieldMap();
        }

        /// <summary>
        /// Gets or sets the date time reader.
        /// </summary>
        public Func<string, DateTime> DateTimeReader { get; set; }

        /// <summary>
        /// Gets or sets the line preprocessor.
        /// </summary>
        public Func<string, string> LinePreprocessor { get; set; }

        /// <summary>
        /// Reads a datetime.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private DateTime ReadDateTime(string name, string fieldName, string value)
        {
            try
            {
                return this.DateTimeReader.Invoke(value);
            }
            catch (Exception ex)
            {
                // throw a GFTS parse exception instead.
                throw new GTFSParseException(name, fieldName, value, ex);
            }
        }

        /// <summary>
        /// Gets or sets the date time writer.
        /// </summary>
        public Func<DateTime, string> DateTimeWriter { get; set; }

        /// <summary>
        /// Gets or sets the time of day reader.
        /// </summary>
        public Func<string, TimeOfDay> TimeOfDayReader { get; set; }

        /// <summary>
        /// Reads a timeofday.
        /// </summary>
        /// <returns></returns>
        private TimeOfDay? ReadTimeOfDay(string name, string fieldName, string value)
        {
            try
            {
                return this.TimeOfDayReader.Invoke(value);
            }
            catch (Exception ex)
            {
                // throw a GFTS parse exception instead.
                return new TimeOfDay()
                {
                    Hours = 0,
                    Minutes = 0,
                    Seconds = 0
                };
                throw new GTFSParseException(name, fieldName, value, ex);
            }
        }

        /// <summary>
        /// Gets or sets the time of day writer.
        /// </summary>
        public Func<TimeOfDay, string> TimeOfDayWriter { get; set; }

        /// <summary>
        /// Reads the specified GTFS source into the given GTFS feed object.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        public T Read(T feed, IEnumerable<IGTFSSourceFile> source)
        {
            source = source.ToArray(); // optimization in order not to enumerate multiple times

            // check if all required files are present.
            if (_strict)
            {
                var sourceFileNames = source.Select(x => x.Name).ToArray();

                var missingRequiredFiles = GetRequiredFiles()
                    .Where(x => !sourceFileNames.Contains(x))
                    .ToArray();

                if (missingRequiredFiles.Any())
                {
                    // oeps, file was not found!
                    // TODO: check if we should not return all missing files in the exception?
                    throw new GTFSRequiredFileMissingException(missingRequiredFiles.First());
                }

                var missingRequiredFileSets = GetRequiredFileSets()
                    .Where(requiredFileSet => !requiredFileSet.Any(x => sourceFileNames.Contains(x)))
                    .ToArray();

                if (missingRequiredFileSets.Any())
                {
                    // oeps, no file from file set was found!
                    // TODO: check if we should not return all missing file sets in the exception?
                    throw new GTFSRequiredFileSetMissingException(missingRequiredFileSets.First());
                }
            }

            // read files one-by-one and in the correct order based on the dependency tree.
            var readFiles = this.ReadCustomFilesBefore();
            var dependencyTree = this.GetDependencyTree();
            while (readFiles.Count < source.Count())
            {
                // select a new file based on the dependency tree.
                IGTFSSourceFile selectedFile = null;
                foreach (var file in source)
                {
                    if (!readFiles.Contains(file.Name))
                    {
                        // file has not been read yet!
                        HashSet<string> dependencies = null;
                        if (!dependencyTree.TryGetValue(file.Name, out dependencies))
                        {
                            // there is no entry in the dependency tree, file is independant.
                            selectedFile = file;
                            break;
                        }
                        else
                        {
                            // file depends on other file, check if they have been read already.
                            if (dependencies.All(x => readFiles.Contains(x)))
                            {
                                // all dependencies have been read.
                                selectedFile = file;
                                break;
                            }
                        }
                    }
                }

                // check if there is a next file.
                if (selectedFile == null)
                {
                    throw new Exception(
                        "Could not select a next file based on the current dependency tree and the current file list.");
                }

                // read the file.
                this.Read(selectedFile, feed);
                readFiles.Add(selectedFile.Name);
            }

            return feed;
        }

        /// <summary>
        /// Reads custom files and returns a list of files that have already been read.
        /// </summary>
        /// <returns></returns>
        protected virtual HashSet<string> ReadCustomFilesBefore()
        {
            return new HashSet<string>();
        }

        /// <summary>
        /// Reads one file and it's dependencies from the specified GTFS source into the given GTFS feed object.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="source"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public T Read(T feed, IEnumerable<IGTFSSourceFile> source, IGTFSSourceFile file)
        {
            // build the files-to-read list from the dependencies.
            var dependencyTree = this.GetDependencyTree();
            var filesToRead = new List<string>();
            filesToRead.Add(file.Name);

            // start with a queue of one file and traverse the dependency tree down.
            var queue = new Queue<string>();
            queue.Enqueue(file.Name);
            while (queue.Count > 0)
            {
                // dequeue current file.
                var currentFile = queue.Dequeue();
                filesToRead.Add(currentFile);

                // enqueue dependencies if any.
                HashSet<string> dependencies = null;
                if (dependencyTree.TryGetValue(currentFile, out dependencies))
                {
                    foreach (var dependency in dependencies)
                    {
                        queue.Enqueue(dependency);
                    }
                }
            }

            // loop over the list but starting at the end.
            var readFiles = new HashSet<string>();
            for (int idx = filesToRead.Count - 1; idx >= 0; idx--)
            {
                string fileToRead = filesToRead[idx];
                if (!readFiles.Contains(fileToRead))
                {
                    // files can be in the list more than once.

                    // read the file.
                    this.Read(source.First(x => x.Name.Equals(fileToRead)), feed);
                    readFiles.Add(fileToRead);
                }
            }

            return feed;
        }

        /// <summary>
        /// Returns a collection of all required files.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetRequiredFiles()
        {
            return new[] {"agency", "stops", "routes", "trips", "stop_times"};
        }

        /// <summary>
        /// Returns a collection of required file sets. Each file set contains a 
        /// number of files of which at least one should be in the source files set.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string[]> GetRequiredFileSets()
        {
            return new[]
            {
                new[] {"calendar", "calendar_dates"}
            };
        }

        /// <summary>
        /// Returns the file dependency-tree.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, HashSet<string>> GetDependencyTree()
        {
            var dependencyTree = new Dictionary<string, HashSet<string>>();

            // fare_rules => (routes)
            var dependencies = new HashSet<string>();
            dependencies.Add("routes");
            dependencyTree.Add("fare_rules", dependencies);

            // frequencies => (trips)
            dependencies = new HashSet<string>();
            dependencies.Add("trips");
            dependencyTree.Add("frequencies", dependencies);

            // routes => (agencies)
            dependencies = new HashSet<string>();
            dependencies.Add("agency");
            dependencyTree.Add("routes", dependencies);

            // stop_times => (trips)
            dependencies = new HashSet<string>();
            dependencies.Add("trips");
            dependencyTree.Add("stop_times", dependencies);

            // trips => (routes)
            dependencies = new HashSet<string>();
            dependencies.Add("routes");
            dependencyTree.Add("trips", dependencies);

            // transfers => (stops)
            dependencies = new HashSet<string>();
            dependencies.Add("stops");
            dependencyTree.Add("transfers", dependencies);

            return dependencyTree;
        }

        /// <summary>
        /// A delegate for parsing methods per entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        protected delegate TEntity EntityParseDelegate<TEntity>(T feed, GTFSSourceFileHeader header, string[] data)
            where TEntity : GTFSEntity;

        /// <summary>
        /// A delegate to add entities.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        protected delegate void EntityAddDelegate<TEntity>(TEntity entity);

        /// <summary>
        /// Reads the given file and adds the result to the feed.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="feed"></param>
        protected virtual void Read(IGTFSSourceFile file, T feed)
        {
            switch (file.Name.ToLower())
            {
                case "attributions":
                    this.Read<Attribution>(file, feed, this.ParseAttribution, feed.Attributions.Add);
                    break;
                case "agency":
                    this.Read<Agency>(file, feed, this.ParseAgency, feed.Agencies.Add);
                    break;
                case "calendar":
                    this.Read<Calendar>(file, feed, this.ParseCalender, feed.Calendars.Add);
                    break;
                case "calendar_dates":
                    this.Read<CalendarDate>(file, feed, this.ParseCalendarDate, feed.CalendarDates.Add);
                    break;
                case "fare_attributes":
                    this.Read<FareAttribute>(file, feed, this.ParseFareAttribute, feed.FareAttributes.Add);
                    break;
                case "fare_rules":
                    this.Read<FareRule>(file, feed, this.ParseFareRule, feed.FareRules.Add);
                    break;
                case "feed_info":
                    this.Read<FeedInfo>(file, feed, this.ParseFeedInfo, feed.SetFeedInfo);
                    break;
                case "routes":
                    this.Read<Route>(file, feed, this.ParseRoute, feed.Routes.Add);
                    break;
                case "shapes":
                    this.Read<Shape>(file, feed, this.ParseShape, feed.Shapes.Add);
                    break;
                case "stops":
                    this.Read<Stop>(file, feed, this.ParseStop, feed.Stops.Add);
                    break;
                case "stop_times":
                    this.Read<StopTime>(file, feed, this.ParseStopTime, feed.StopTimes.Add);
                    break;
                case "trips":
                    this.Read<Trip>(file, feed, this.ParseTrip, feed.Trips.Add);
                    break;
                case "transfers":
                    this.Read<Transfer>(file, feed, this.ParseTransfer, feed.Transfers.Add);
                    break;
                case "frequencies":
                    this.Read<Frequency>(file, feed, this.ParseFrequency, feed.Frequencies.Add);
                    break;
            }
        }

        /// <summary>
        /// Gets the pathway fieldmap.
        /// </summary>
        public FieldMap PathwayMap { get; private set; }

        /// <summary>
        /// Parses a pathway row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Pathway ParsePathway(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.

            this.CheckRequiredField(header, header.Name, this.PathwayMap, "pathway_id");
            this.CheckRequiredField(header, header.Name, this.PathwayMap, "from_stop_id");
            this.CheckRequiredField(header, header.Name, this.PathwayMap, "to_stop_id");
            this.CheckRequiredField(header, header.Name, this.PathwayMap, "pathway_mode");
            this.CheckRequiredField(header, header.Name, this.PathwayMap, "is_bidirectional");

            // parse/set all fields.
            Pathway pathway = new Pathway();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParsePathwayField(header, pathway, header.GetColumn(idx), data[idx]);
            }

            return pathway;
        }

        /// <summary>
        /// Parses a pathway field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="pathway"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParsePathwayField(GTFSSourceFileHeader header, Pathway pathway, string fieldName,
            string value)
        {
            switch (fieldName)
            {
                case "pathway_id":
                    pathway.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "from_stop_id":
                    pathway.FromStopId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "to_stop_id":
                    pathway.ToStopId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "pathway_mode":
                    pathway.PathwayMode = (PathwayMode) this.ParseFieldPathwayMode(header.Name, fieldName, value);
                    break;
                case "is_bidirectional":
                    pathway.IsBidirectional =
                        (IsBidirectional) this.ParseFieldIsBidirectional(header.Name, fieldName, value);
                    break;
                case "length":
                    pathway.Length = this.ParseFieldDouble(header.Name, fieldName, value);
                    break;
                case "traversal_time":
                    pathway.TraversalTime = this.ParseFieldInt(header.Name, fieldName, value);
                    break;
                case "stair_count":
                    pathway.StairCount = this.ParseFieldInt(header.Name, fieldName, value);
                    break;
                case "max_slope":
                    pathway.MaxSlope = this.ParseFieldDouble(header.Name, fieldName, value);
                    break;
                case "min_width":
                    pathway.MinWidth = this.ParseFieldDouble(header.Name, fieldName, value);
                    break;
                case "signposted_as":
                    pathway.SignpostedAs = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "reversed_signposted_as":
                    pathway.ReversedSignpostedAs = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the level fieldmap.
        /// </summary>
        public FieldMap LevelMap { get; private set; }

        /// <summary>
        /// Parses a level row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Level ParseLevel(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.

            this.CheckRequiredField(header, header.Name, this.LevelMap, "level_id");
            this.CheckRequiredField(header, header.Name, this.LevelMap, "level_index");

            // parse/set all fields.
            Level level = new Level();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseLevelField(header, level, header.GetColumn(idx), data[idx]);
            }

            return level;
        }

        /// <summary>
        /// Parses a level field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="level"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseLevelField(GTFSSourceFileHeader header, Level level, string fieldName, string value)
        {
            switch (fieldName)
            {
                case "level_id":
                    level.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "level_index":
                    level.Index = (double) this.ParseFieldDouble(header.Name, fieldName, value);
                    break;
                case "level_name":
                    level.Name = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Reads the GTFS file.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="file"></param>
        /// <param name="feed"></param>
        /// <param name="parser"></param>
        /// <param name="addDelegate"></param>
        private void Read<TEntity>(IGTFSSourceFile file, T feed, EntityParseDelegate<TEntity> parser,
            EntityAddDelegate<TEntity> addDelegate)
            where TEntity : GTFSEntity
        {
            // set line preprocessor if any.
            file.LinePreprocessor = this.LinePreprocessor;

            // enumerate all lines.
            var enumerator = file.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                // there is no data, and if there is move to the columns.
                return;
            }

            // read the header.
            var headerColumns = new string[enumerator.Current.Length];
            for (int idx = 0; idx < headerColumns.Length; idx++)
            {
                // 'clean' header columns.
                headerColumns[idx] = this.CleanFieldValue(enumerator.Current[idx]);
            }

            var header = new GTFSSourceFileHeader(file.Name, headerColumns);

            // read fields and keep them sorted.
            if (typeof(IComparable).IsAssignableFrom(typeof(TEntity)))
            {
                var entities = new List<TEntity>();
                while (enumerator.MoveNext())
                {
                    var entity = parser.Invoke(feed, header, enumerator.Current);
                    entities.Add(entity);
                }

                entities.Sort();
                foreach (var entity in entities)
                {
                    addDelegate.Invoke(entity);
                }
            }
            else
            {
                while (enumerator.MoveNext())
                {
                    var entity = parser.Invoke(feed, header, enumerator.Current);
                    addDelegate.Invoke(entity);
                }
            }
        }

        /// <summary>
        /// Gets the attribution fieldmap.
        /// </summary>
        public FieldMap AttributionMap { get; private set; }

        /// <summary>
        /// Parses an attribution row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Attribution ParseAttribution(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.

            this.CheckRequiredField(header, header.Name, this.AttributionMap, "organization_name");

            // if we already have another attribution, then the attribution_id is required
            if (feed.Attributions.Any())
            {
                CheckRequiredField(header, header.Name, AttributionMap, "attribution_id");
            }

            // parse/set all fields.
            var attribution = new Attribution();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseAttributionField(header, attribution, header.GetColumn(idx), data[idx]);
            }

            return attribution;
        }

        /// <summary>
        /// Parses an attribution field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="attribution"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseAttributionField(GTFSSourceFileHeader header, Attribution attribution, string fieldName, string value)
        {
            switch (fieldName)
            {
                case "attribution_id":
                    attribution.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_id":
                    attribution.AgencyId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_id":
                    attribution.RouteId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "trip_id":
                    attribution.TripId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "organization_name":
                    attribution.OrganizationName = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "is_producer":
                    attribution.IsProducer = this.ParseFieldBool(header.Name, fieldName, value);
                    break;
                case "is_operator":
                    attribution.IsOperator = this.ParseFieldBool(header.Name, fieldName, value);
                    break;
                case "is_authority":
                    attribution.IsAuthority = this.ParseFieldBool(header.Name, fieldName, value);
                    break;
                case "attribution_url":
                    attribution.URL = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "attribution_email":
                    attribution.Email = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "attribution_phone":
                    attribution.Phone = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the agency fieldmap.
        /// </summary>
        public FieldMap AgencyMap { get; private set; }

        /// <summary>
        /// Parses an agency row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Agency ParseAgency(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.

            this.CheckRequiredField(header, header.Name, this.AgencyMap, "agency_name");
            this.CheckRequiredField(header, header.Name, this.AgencyMap, "agency_url");
            this.CheckRequiredField(header, header.Name, this.AgencyMap, "agency_timezone");

            // if we already have another agency, then the agency_id is required
            if (feed.Agencies.Any())
            {
                CheckRequiredField(header, header.Name, AgencyMap, "agency_id");
            }

            // parse/set all fields.
            Agency agency = new Agency();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseAgencyField(header, agency, header.GetColumn(idx), data[idx]);
            }

            return agency;
        }

        /// <summary>
        /// Parses an agency field.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="agency"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseAgencyField(GTFSSourceFileHeader header, Agency agency, string fieldName,
            string value)
        {
            switch (fieldName)
            {
                case "agency_id":
                    agency.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_name":
                    agency.Name = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_lang":
                    agency.LanguageCode = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_phone":
                    agency.Phone = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_timezone":
                    agency.Timezone = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_url":
                    agency.URL = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_email":
                    agency.Email = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_fare_url":
                    agency.FareURL = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the calendar fieldmap.
        /// </summary>
        public FieldMap CalendarMap { get; private set; }

        /// <summary>
        /// Parses a calendar row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Calendar ParseCalender(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "service_id");
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "monday");
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "tuesday");
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "wednesday");
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "thursday");
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "friday");
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "saturday");
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "sunday");
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "start_date");
            this.CheckRequiredField(header, header.Name, this.CalendarMap, "end_date");

            // parse/set all fields.
            Calendar calendar = new Calendar();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseCalendarField(feed, header, calendar, header.GetColumn(idx), data[idx]);
            }

            return calendar;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="calendar"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseCalendarField(T feed, GTFSSourceFileHeader header, Calendar calendar,
            string fieldName, string value)
        {
            switch (fieldName)
            {
                case "service_id":
                    calendar.ServiceId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "monday":
                    calendar.Monday = this.ParseFieldBool(header.Name, fieldName, value).Value;
                    break;
                case "tuesday":
                    calendar.Tuesday = this.ParseFieldBool(header.Name, fieldName, value).Value;
                    break;
                case "wednesday":
                    calendar.Wednesday = this.ParseFieldBool(header.Name, fieldName, value).Value;
                    break;
                case "thursday":
                    calendar.Thursday = this.ParseFieldBool(header.Name, fieldName, value).Value;
                    break;
                case "friday":
                    calendar.Friday = this.ParseFieldBool(header.Name, fieldName, value).Value;
                    break;
                case "saturday":
                    calendar.Saturday = this.ParseFieldBool(header.Name, fieldName, value).Value;
                    break;
                case "sunday":
                    calendar.Sunday = this.ParseFieldBool(header.Name, fieldName, value).Value;
                    break;
                case "start_date":
                    calendar.StartDate = this.ReadDateTime(header.Name, fieldName,
                        this.ParseFieldString(header.Name, fieldName, value));
                    break;
                case "end_date":
                    calendar.EndDate = this.ReadDateTime(header.Name, fieldName,
                        this.ParseFieldString(header.Name, fieldName, value));
                    break;
            }
        }

        /// <summary>
        /// Gets the calendar date fieldmap.
        /// </summary>
        public FieldMap CalendarDateMap { get; private set; }

        /// <summary>
        /// Parses a calendar date row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual CalendarDate ParseCalendarDate(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.CalendarDateMap, "service_id");
            this.CheckRequiredField(header, header.Name, this.CalendarDateMap, "date");
            this.CheckRequiredField(header, header.Name, this.CalendarDateMap, "exception_type");

            // parse/set all fields.
            CalendarDate calendarDate = new CalendarDate();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseCalendarDateField(feed, header, calendarDate, header.GetColumn(idx), data[idx]);
            }

            return calendarDate;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="calendarDate"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseCalendarDateField(T feed, GTFSSourceFileHeader header, CalendarDate calendarDate,
            string fieldName, string value)
        {
            switch (fieldName)
            {
                case "service_id":
                    calendarDate.ServiceId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "date":
                    calendarDate.Date = this.ReadDateTime(header.Name, fieldName,
                        this.ParseFieldString(header.Name, fieldName, value));
                    break;
                case "exception_type":
                    calendarDate.ExceptionType = this.ParseFieldExceptionType(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the fare attribute fieldmap.
        /// </summary>
        public FieldMap FareAttributeMap { get; private set; }

        /// <summary>
        /// Parses a fare attribute row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual FareAttribute ParseFareAttribute(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.FareAttributeMap, "fare_id");
            this.CheckRequiredField(header, header.Name, this.FareAttributeMap, "price");
            this.CheckRequiredField(header, header.Name, this.FareAttributeMap, "currency_type");
            this.CheckRequiredField(header, header.Name, this.FareAttributeMap, "payment_method");
            this.CheckRequiredField(header, header.Name, this.FareAttributeMap, "transfers");

            // parse/set all fields.
            FareAttribute fareAttribute = new FareAttribute();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseFareAttributeField(feed, header, fareAttribute, header.GetColumn(idx), data[idx]);
            }

            return fareAttribute;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="fareAttribute"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseFareAttributeField(T feed, GTFSSourceFileHeader header, FareAttribute fareAttribute,
            string fieldName, string value)
        {
            switch (fieldName)
            {
                case "fare_id":
                    fareAttribute.FareId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "price":
                    fareAttribute.Price = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "currency_type":
                    fareAttribute.CurrencyType = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "payment_method":
                    fareAttribute.PaymentMethod = this.ParseFieldPaymentMethodType(header.Name, fieldName, value);
                    break;
                case "transfers":
                    fareAttribute.Transfers = this.ParseFieldUInt(header.Name, fieldName, value);
                    break;
                case "agency_id":
                    fareAttribute.AgencyId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "transfer_duration":
                    fareAttribute.TransferDuration = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the fare rule fieldmap.
        /// </summary>
        public FieldMap FareRuleMap { get; private set; }

        /// <summary>
        /// Parses a fare rule row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual FareRule ParseFareRule(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.FareRuleMap, "fare_id");

            // parse/set all fields.
            FareRule fareRule = new FareRule();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseFareRuleField(feed, header, fareRule, header.GetColumn(idx), data[idx]);
            }

            return fareRule;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="fareRule"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseFareRuleField(T feed, GTFSSourceFileHeader header, FareRule fareRule,
            string fieldName, string value)
        {
            switch (fieldName)
            {
                case "fare_id":
                    fareRule.FareId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_id":
                    fareRule.RouteId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "origin_id":
                    fareRule.OriginId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "destination_id":
                    fareRule.DestinationId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "contains_id":
                    fareRule.ContainsId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the feed info fieldmap.
        /// </summary>
        public FieldMap FeedInfoMap { get; private set; }

        /// <summary>
        /// Parses a feed info row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual FeedInfo ParseFeedInfo(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "feed_publisher_name");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "feed_publisher_url");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "feed_lang");

            // parse/set all fields.
            FeedInfo feedInfo = new FeedInfo();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseFeedInfoField(feed, header, feedInfo, header.GetColumn(idx), data[idx]);
            }

            return feedInfo;
        }

        private void ParseFeedInfoField(T feed, GTFSSourceFileHeader header, FeedInfo feedInfo, string fieldName,
            string value)
        {
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "feed_publisher_name");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "feed_publisher_url");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "feed_lang");
            switch (fieldName)
            {
                case "feed_publisher_name":
                    feedInfo.PublisherName = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "feed_publisher_url":
                    feedInfo.PublisherUrl = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "feed_lang":
                    feedInfo.Lang = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "feed_start_date":
                    feedInfo.StartDate = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "feed_end_date":
                    feedInfo.EndDate = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "feed_version":
                    feedInfo.Version = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the frequence fieldmap.
        /// </summary>
        public FieldMap FrequencyMap { get; private set; }

        /// <summary>
        /// Parses a frequency row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Frequency ParseFrequency(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "trip_id");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "start_time");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "end_time");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "headway_secs");

            // parse/set all fields.
            Frequency frequency = new Frequency();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseFrequencyField(feed, header, frequency, header.GetColumn(idx), data[idx]);
            }

            return frequency;
        }

        /// <summary>
        /// Gets the route fieldmap.
        /// </summary>
        public FieldMap RouteMap { get; private set; }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="frequency"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseFrequencyField(T feed, GTFSSourceFileHeader header, Frequency frequency,
            string fieldName, string value)
        {
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "trip_id");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "start_time");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "end_time");
            this.CheckRequiredField(header, header.Name, this.FrequencyMap, "headway_secs");
            switch (fieldName)
            {
                case "trip_id":
                    frequency.TripId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "start_time":
                    frequency.StartTime = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "end_time":
                    frequency.EndTime = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "headway_secs":
                    frequency.HeadwaySecs = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "exact_times":
                    frequency.ExactTimes = this.ParseFieldBool(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Parses a route row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Route ParseRoute(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.RouteMap, "route_id");

            this.CheckRequiredField(header, header.Name, this.RouteMap, "route_short_name");
            this.CheckRequiredField(header, header.Name, this.RouteMap, "route_long_name");
            this.CheckRequiredField(header, header.Name, this.RouteMap, "route_type");

            // parse/set all fields.
            Route route = new Route();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseRouteField(feed, header, route, header.GetColumn(idx), data[idx]);
            }

            return route;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="route"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseRouteField(T feed, GTFSSourceFileHeader header, Route route, string fieldName,
            string value)
        {
            switch (fieldName)
            {
                case "route_id":
                    route.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "agency_id":
                    route.AgencyId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_short_name":
                    route.ShortName = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_long_name":
                    route.LongName = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_desc":
                    route.Description = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_type":
                    route.Type = this.ParseFieldRouteType(header.Name, fieldName, value);
                    break;
                case "route_url":
                    route.Url = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_color":
                    route.Color = this.ParseFieldColor(header.Name, fieldName, value);
                    break;
                case "route_text_color":
                    route.TextColor = this.ParseFieldColor(header.Name, fieldName, value);
                    break;
                case "continuous_pickup":
                    route.ContinuousPickup = this.ParseFieldContinuousPickup(header.Name, fieldName, value);
                    break;
                case "continuous_drop_off":
                    route.ContinuousDropOff = this.ParseFieldContinuousDropOff(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the shape fieldmap.
        /// </summary>
        public FieldMap ShapeMap { get; private set; }

        /// <summary>
        /// Parses a shape row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Shape ParseShape(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.ShapeMap, "shape_id");
            this.CheckRequiredField(header, header.Name, this.ShapeMap, "shape_pt_lat");
            this.CheckRequiredField(header, header.Name, this.ShapeMap, "shape_pt_lon");
            this.CheckRequiredField(header, header.Name, this.ShapeMap, "shape_pt_sequence");

            // parse/set all fields.
            Shape shape = new Shape();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseShapeField(feed, header, shape, header.GetColumn(idx), data[idx]);
            }

            return shape;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="shape"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseShapeField(T feed, GTFSSourceFileHeader header, Shape shape, string fieldName,
            string value)
        {
            switch (fieldName)
            {
                case "shape_id":
                    shape.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "shape_pt_lat":
                    shape.Latitude = this.ParseFieldDouble(header.Name, fieldName, value).Value;
                    break;
                case "shape_pt_lon":
                    shape.Longitude = this.ParseFieldDouble(header.Name, fieldName, value).Value;
                    break;
                case "shape_pt_sequence":
                    shape.Sequence = this.ParseFieldUInt(header.Name, fieldName, value).Value;
                    break;
                case "shape_dist_traveled":
                    shape.DistanceTravelled = this.ParseFieldDouble(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the stop fieldmap.
        /// </summary>
        public FieldMap StopMap { get; private set; }

        /// <summary>
        /// Parses a stop row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Stop ParseStop(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.StopMap, "stop_id");
            this.CheckRequiredField(header, header.Name, this.StopMap, "stop_name");
            this.CheckRequiredField(header, header.Name, this.StopMap, "stop_lat");
            this.CheckRequiredField(header, header.Name, this.StopMap, "stop_lon");

            // parse/set all fields.
            Stop stop = new Stop();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseStopField(feed, header, stop, header.GetColumn(idx), data[idx]);
            }

            return stop;
        }

        /// <summary>
        /// Parses a stop field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="stop"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseStopField(T feed, GTFSSourceFileHeader header, Stop stop, string fieldName,
            string value)
        {
            switch (fieldName.Trim())
            {
                case "stop_id":
                    stop.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_code":
                    stop.Code = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_name":
                    stop.Name = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_desc":
                    stop.Description = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_lat":
                    var lat = this.ParseFieldDouble(header.Name, fieldName, value);

                    if (this._strict && !lat.HasValue)
                    {
                        throw new GTFSParseException(header.Name, fieldName, value);
                    }

                    stop.Latitude = lat.HasValue ? lat.Value : 0d;
                    break;
                case "stop_lon":
                    var lon = this.ParseFieldDouble(header.Name, fieldName, value);

                    if (this._strict && !lon.HasValue)
                    {
                        throw new GTFSParseException(header.Name, fieldName, value);
                    }

                    stop.Longitude = lon.HasValue ? lon.Value : 0d;
                    break;
                case "zone_id":
                    stop.Zone = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_url":
                    stop.Url = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "location_type":
                    stop.LocationType = this.ParseFieldLocationType(header.Name, fieldName, value);
                    break;
                case "parent_station":
                    stop.ParentStation = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_timezone":
                    stop.Timezone = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "wheelchair_boarding":
                    stop.WheelchairBoarding = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "level_id":
                    stop.LevelId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "platform_code":
                    stop.PlatformCode = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the stop time fieldmap.
        /// </summary>
        public FieldMap StopTimeMap { get; private set; }

        /// <summary>
        /// Parses a stop time row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual StopTime ParseStopTime(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.StopTimeMap, "trip_id");
            this.CheckRequiredField(header, header.Name, this.StopTimeMap, "arrival_time");
            this.CheckRequiredField(header, header.Name, this.StopTimeMap, "departure_time");
            this.CheckRequiredField(header, header.Name, this.StopTimeMap, "stop_id");
            this.CheckRequiredField(header, header.Name, this.StopTimeMap, "stop_sequence");

            // parse/set all fields.
            StopTime stopTime = new StopTime();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseStopTimeField(feed, header, stopTime, header.GetColumn(idx), data[idx]);
            }

            return stopTime;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="stopTime"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseStopTimeField(T feed, GTFSSourceFileHeader header, StopTime stopTime,
            string fieldName, string value)
        {
            switch (fieldName)
            {
                case "trip_id":
                    stopTime.TripId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "arrival_time":
                    stopTime.ArrivalTime = this.ReadTimeOfDay(header.Name, fieldName,
                        this.ParseFieldString(header.Name, fieldName, value));
                    break;
                case "departure_time":
                    stopTime.DepartureTime = this.ReadTimeOfDay(header.Name, fieldName,
                        this.ParseFieldString(header.Name, fieldName, value));
                    break;
                case "stop_id":
                    stopTime.StopId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "stop_sequence":
                    stopTime.StopSequence = this.ParseFieldUInt(header.Name, fieldName, value).Value;
                    break;
                case "stop_headsign":
                    stopTime.StopHeadsign = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "pickup_type":
                    stopTime.PickupType = this.ParseFieldPickupType(header.Name, fieldName, value);
                    break;
                case "drop_off_type":
                    stopTime.DropOffType = this.ParseFieldDropOffType(header.Name, fieldName, value);
                    break;
                case "continuous_pickup":
                    stopTime.ContinuousPickup = this.ParseFieldContinuousPickup(header.Name, fieldName, value);
                    break;
                case "continuous_drop_off":
                    stopTime.ContinuousDropOff = this.ParseFieldContinuousDropOff(header.Name, fieldName, value);
                    break;
                case "shape_dist_traveled":
                    stopTime.ShapeDistTravelled = this.ParseFieldDouble(header.Name, fieldName, value);
                    break;
                case "timepoint":
                    stopTime.TimepointType = this.ParseFieldTimepointType(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the transfer fieldmap.
        /// </summary>
        public FieldMap TransferMap { get; private set; }

        /// <summary>
        /// Parses a transfer row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Transfer ParseTransfer(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.TransferMap, "from_stop_id");
            this.CheckRequiredField(header, header.Name, this.TransferMap, "to_stop_id");
            this.CheckRequiredField(header, header.Name, this.TransferMap, "transfer_type");

            // parse/set all fields.
            var transfer = new Transfer();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseTransferField(feed, header, transfer, header.GetColumn(idx), data[idx]);
            }

            return transfer;
        }

        /// <summary>
        /// Parses a transfer field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="transfer"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseTransferField(T feed, GTFSSourceFileHeader header, Transfer transfer,
            string fieldName, string value)
        {
            switch (fieldName)
            {
                case "from_stop_id":
                    transfer.FromStopId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "to_stop_id":
                    transfer.ToStopId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "transfer_type":
                    transfer.TransferType = this.ParseFieldTransferType(header.Name, fieldName, value);
                    break;
                case "min_transfer_time":
                    transfer.MinimumTransferTime = this.ParseFieldString(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Gets the trip fieldmap.
        /// </summary>
        public FieldMap TripMap { get; private set; }

        /// <summary>
        /// Parses a trip row.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Trip ParseTrip(T feed, GTFSSourceFileHeader header, string[] data)
        {
            // check required fields.
            this.CheckRequiredField(header, header.Name, this.TripMap, "trip_id");
            this.CheckRequiredField(header, header.Name, this.TripMap, "route_id");
            this.CheckRequiredField(header, header.Name, this.TripMap, "service_id");

            // parse/set all fields.
            Trip trip = new Trip();
            for (int idx = 0; idx < data.Length; idx++)
            {
                this.ParseTripField(feed, header, trip, header.GetColumn(idx), data[idx]);
            }

            return trip;
        }

        /// <summary>
        /// Parses a route field.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="header"></param>
        /// <param name="trip"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected virtual void ParseTripField(T feed, GTFSSourceFileHeader header, Trip trip, string fieldName,
            string value)
        {
            switch (fieldName)
            {
                case "trip_id":
                    trip.Id = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "route_id":
                    trip.RouteId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "service_id":
                    trip.ServiceId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "trip_headsign":
                    trip.Headsign = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "trip_short_name":
                    trip.ShortName = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "direction_id":
                    trip.Direction = this.ParseFieldDirectionType(header.Name, fieldName, value);
                    break;
                case "block_id":
                    trip.BlockId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "shape_id":
                    trip.ShapeId = this.ParseFieldString(header.Name, fieldName, value);
                    break;
                case "wheelchair_accessible":
                    trip.AccessibilityType = this.ParseFieldAccessibilityType(header.Name, fieldName, value);
                    break;
            }
        }

        /// <summary>
        /// Checks if a required field is actually in the header.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="fieldMap"></param>
        /// <param name="column"></param>
        protected virtual void CheckRequiredField(GTFSSourceFileHeader header, string name, FieldMap fieldMap,
            string column)
        {
            if (_strict)
            {
                // do not check the requeted fields stuff when not strict.
                string actual = fieldMap.GetActual(column);
                if (!header.HasColumn(actual))
                {
                    throw new GTFSRequiredFieldMissingException(name, actual);
                }
            }
        }

        /// <summary>
        /// Parses a string-field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string ParseFieldString(string name, string fieldName, string value)
        {
            return this.CleanFieldValue(value);
        }

        /// <summary>
        /// Parses a color field into an argb value.
        /// </summary>
        /// <returns></returns>
        protected virtual int? ParseFieldColor(string name, string fieldName, string value)
        {
            // clean first.
            value = this.CleanFieldValue(value);

            try
            {
                return value.ToArgbInt();
            }
            catch (Exception ex)
            {
                // hmm, some unknow exception, field not in correct format, give inner exception as a clue.
                throw new GTFSParseException(name, fieldName, value, ex);
            }
        }

        /// <summary>
        /// Parses a route-type field.
        /// </summary>
        /// <returns></returns>
        protected virtual RouteType ParseFieldRouteType(string name, string fieldName, string value)
        {
            // clean first.
            value = this.CleanFieldValue(value);

            if (!int.TryParse(value, out var routeTypeValue))
            {
                throw new GTFSParseException(name, fieldName, value);
            }

            try
            {
                return (RouteType)routeTypeValue;
            }
            catch
            {
                throw new GTFSParseException(name, fieldName, value);
            }
        }

        /// <summary>
        /// Parses an exception-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual ExceptionType ParseFieldExceptionType(string name, string fieldName, string value)
        {
            // clean first.
            value = this.CleanFieldValue(value);

            //A value of 1 indicates that service has been added for the specified date.
            //A value of 2 indicates that service has been removed for the specified date.

            switch (value)
            {
                case "1":
                    return ExceptionType.Added;
                case "2":
                    return ExceptionType.Removed;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a payment-method type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual PaymentMethodType ParseFieldPaymentMethodType(string name, string fieldName, string value)
        {
            // clean first.
            value = this.CleanFieldValue(value);

            //0 - Fare is paid on board.
            //1 - Fare must be paid before boarding.

            switch (value)
            {
                case "0":
                    return PaymentMethodType.OnBoard;
                case "1":
                    return PaymentMethodType.BeforeBoarding;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a transfer type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual TransferType ParseFieldTransferType(string name, string fieldName, string value)
        {
            // clean first.
            value = this.CleanFieldValue(value);

            // - 0 or (empty) - This is a recommended transfer point between two routes.
            // - 1 - This is a timed transfer point between two routes. The departing vehicle is expected to wait for the arriving one, with sufficient time for a passenger to transfer between routes.
            // - 2 - This transfer requires a minimum amount of time between arrival and departure to ensure a connection. The time required to transfer is specified by min_transfer_time.
            // - 3 - Transfers are not possible between routes at this location.

            switch (value)
            {
                case "0":
                case "":
                    return TransferType.Recommended;
                case "1":
                    return TransferType.TimedTransfer;
                case "2":
                    return TransferType.MinimumTime;
                case "3":
                    return TransferType.NotPossible;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses an accessibility-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private WheelchairAccessibilityType? ParseFieldAccessibilityType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //0 (or empty) - indicates that there is no accessibility information for the trip
            //1 - indicates that the vehicle being used on this particular trip can accommodate at least one rider in a wheelchair
            //2 - indicates that no riders in wheelchairs can be accommodated on this trip

            switch (value)
            {
                case "0":
                    return WheelchairAccessibilityType.NoInformation;
                case "1":
                    return WheelchairAccessibilityType.SomeAccessibility;
                case "2":
                    return WheelchairAccessibilityType.NoAccessibility;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a drop-off-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private DropOffType? ParseFieldDropOffType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //0 - Regularly scheduled drop off
            //1 - No drop off available
            //2 - Must phone agency to arrange drop off
            //3 - Must coordinate with driver to arrange drop off

            switch (value)
            {
                case "0":
                    return DropOffType.Regular;
                case "1":
                    return DropOffType.NoPickup;
                case "2":
                    return DropOffType.PhoneForPickup;
                case "3":
                    return DropOffType.DriverForPickup;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses the timepoing field.
        /// </summary>
        /// <returns></returns>
        private TimePointType ParseFieldTimepointType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return TimePointType.None;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //0 - Times are considered approximate.
            //1 - Times are considered exact.

            switch (value)
            {
                case "0":
                    return TimePointType.Approximate;
                case "1":
                    return TimePointType.Exact;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a pickup-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private PickupType? ParseFieldPickupType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //0 - Regularly scheduled pickup
            //1 - No pickup available
            //2 - Must phone agency to arrange pickup
            //3 - Must coordinate with driver to arrange pickup

            switch (value)
            {
                case "0":
                    return PickupType.Regular;
                case "1":
                    return PickupType.NoPickup;
                case "2":
                    return PickupType.PhoneForPickup;
                case "3":
                    return PickupType.DriverForPickup;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a continuous_pickup field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private ContinuousPickup? ParseFieldContinuousPickup(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //0 - Continuous stopping pickup.
            //1 or empty - No continuous stopping pickup.
            //2 - Must phone an agency to arrange continuous stopping pickup.
            //3 - Must coordinate with a driver to arrange continuous stopping pickup.

            switch (value)
            {
                case "0":
                    return ContinuousPickup.ContinuousStoppingPickup;
                case "1":
                    return ContinuousPickup.None;
                case "2":
                    return ContinuousPickup.PhoneForPickup;
                case "3":
                    return ContinuousPickup.DriverForPickup;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a continuous_drop_off field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private ContinuousDropOff? ParseFieldContinuousDropOff(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //0 - Continuous stopping drop-off.
            //1 or empty - No continuous stopping drop-off.
            //2 - Must phone an agency to arrange continuous stopping drop-off.
            //3 - Must coordinate with a driver to arrange continuous stopping drop-off.

            switch (value)
            {
                case "0":
                    return ContinuousDropOff.ContinuousStoppingDropOff;
                case "1":
                    return ContinuousDropOff.None;
                case "2":
                    return ContinuousDropOff.PhoneForDropOff;
                case "3":
                    return ContinuousDropOff.DriverForDropOff;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a location-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private LocationType? ParseFieldLocationType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //0 or blank - Stop. A location where passengers board or disembark from a transit vehicle.
            //1 - Station. A physical structure or area that contains one or more stop.
            //2 - Entrance/Exit.
            //3 - Generic Node. 
            //4 - Boarding Area.

            switch (value)
            {
                case "0":
                    return LocationType.Stop;
                case "1":
                    return LocationType.Station;
                case "2":
                    return LocationType.EntranceExit;
                case "3":
                    return LocationType.GenericNode;
                case "4":
                    return LocationType.BoardingArea;
            }

            if (_strict)
            {
                // invalid location type.
                throw new GTFSParseException(name, fieldName, value);
            }

            return null;
        }

        /// <summary>
        /// Parses a direction-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private DirectionType? ParseFieldDirectionType(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //0 - travel in one direction (e.g. outbound travel)
            //1 - travel in the opposite direction (e.g. inbound travel)

            switch (value)
            {
                case "0":
                    return DirectionType.OneDirection;
                case "1":
                    return DirectionType.OppositeDirection;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a direction-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private PathwayMode? ParseFieldPathwayMode(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //1 - walkway
            //2 - stairs
            //3 - moving sidewalk/travelator
            //4 - escalator
            //5 - elevator
            //6 - fare gate
            //7 - exit gate

            switch (value)
            {
                case "1":
                    return PathwayMode.Walkway;
                case "2":
                    return PathwayMode.Stairs;
                case "3":
                    return PathwayMode.Travelator;
                case "4":
                    return PathwayMode.Escalator;
                case "5":
                    return PathwayMode.Elevator;
                case "6":
                    return PathwayMode.FareGate;
                case "7":
                    return PathwayMode.ExitGate;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a direction-type field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private IsBidirectional? ParseFieldIsBidirectional(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            //0 - Unidirectional pathway, it can only be used from from_stop_id to to_stop_id.
            //1 - Bidirectional pathway, it can be used in the two directions.

            switch (value)
            {
                case "0":
                    return IsBidirectional.Unidirectional;
                case "1":
                    return IsBidirectional.Bidirectional;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Parses a positive integer field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual uint? ParseFieldUInt(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            uint result;
            if (!uint.TryParse(value, out result))
            {
                // parsing failed!
                throw new GTFSParseException(name, fieldName, value);
            }

            return result;
        }

        /// <summary>
        /// Parses a positive integer field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual int? ParseFieldInt(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            int result;
            if (!int.TryParse(value, out result))
            {
                // parsing failed!
                throw new GTFSParseException(name, fieldName, value);
            }

            return result;
        }

        /// <summary>
        /// Parses a double field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual double? ParseFieldDouble(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            double result;
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!double.TryParse(value, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out result))
            {
                // parsing failed!
                if (_strict)
                {
                    throw new GTFSParseException(name, fieldName, value);
                }

                return null;
            }

            return result;
        }

        /// <summary>
        /// Parses a boolean field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool? ParseFieldBool(string name, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // there is no value.
                return null;
            }

            // clean first.
            value = this.CleanFieldValue(value);

            switch (value)
            {
                case "0":
                    return false;
                case "1":
                    return true;
            }

            throw new GTFSParseException(name, fieldName, value);
        }

        /// <summary>
        /// Cleans a field-value for parsing into a boolean, int, double or date.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string CleanFieldValue(string value)
        {
            if (!_strict)
            {
                // no cleaning when strict!
                value = value.Trim();
                if (value != null && value.Length > 0)
                {
                    // test some stuff.
                    if (value.Length >= 2)
                    {
                        // test for quotes
                        if (value[0] == '"' &&
                            value[value.Length - 1] == '"')
                        {
                            // quotes on both ends.
                            return value.Substring(1, value.Length - 2);
                        }
                    }
                }
            }

            return value;
        }
    }

}
