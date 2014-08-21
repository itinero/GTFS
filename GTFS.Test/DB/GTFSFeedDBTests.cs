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
using GTFS.IO;
using GTFS.IO.CSV;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace GTFS.Test.DB
{
    /// <summary>
    /// Contains test methods that can be applied to any GTFS feed db.
    /// </summary>
    [TestFixture]
    public abstract class GTFSFeedDBTests
    {
        /// <summary>
        /// Builds the source from embedded streams.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IGTFSSourceFile> BuildSource()
        {
            var source = new List<IGTFSSourceFile>();
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.agency.txt"), "agency"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.calendar.txt"), "calendar"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.calendar_dates.txt"), "calendar_dates"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.fare_attributes.txt"), "fare_attributes"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.fare_rules.txt"), "fare_rules"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.frequencies.txt"), "frequencies"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.routes.txt"), "routes"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.shapes.txt"), "shapes"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.stop_times.txt"), "stop_times"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.stops.txt"), "stops"));
            source.Add(new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.trips.txt"), "trips"));
            return source;
        }

        /// <summary>
        /// Builds a test feed.
        /// </summary>
        /// <returns></returns>
        protected virtual IGTFSFeed BuildTestFeed()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = this.BuildSource();

            // execute the reader.
            return reader.Read(source);
        }

        /// <summary>
        /// Creates a new test db.
        /// </summary>
        /// <returns></returns>
        protected abstract IGTFSFeedDB CreateDB();
        
        /// <summary>
        /// Tests adding a feed.
        /// </summary>
        [Test]
        public void TestAddFeed()
        {
            // get test db.
            var db = this.CreateDB();

            // build test feed.
            var feed = this.BuildTestFeed();

            // add/get to/from db and compare all.
            var feedId = db.AddFeed(feed);
            GTFSAssert.AreEqual(feed, db.GetFeed(feedId));
        }

        /// <summary>
        /// Test removing a feed.
        /// </summary>
        [Test]
        public void TestRemoveFeed()
        {
            // get test db.
            var db = this.CreateDB();

            // build test feed.
            var feed = this.BuildTestFeed();

            // add feed.
            var feedId = db.AddFeed(feed);

            db.RemoveFeed(feedId);

            // get feed.
            feed = db.GetFeed(feedId);
            Assert.IsNull(feed);
        }

        /// <summary>
        /// Test get feeds.
        /// </summary>
        [Test]
        public void TestGetFeeds()
        {
            // get test db.
            var db = this.CreateDB();

            // build test feed.
            var feed = this.BuildTestFeed();

            // add feed.
            var feedId = db.AddFeed(feed);

            db.RemoveFeed(feedId);

            // get feed.
            feed = db.GetFeed(feedId);
            Assert.IsNull(feed);
        }
    }
}
