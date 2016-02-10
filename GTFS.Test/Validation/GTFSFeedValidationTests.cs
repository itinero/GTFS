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
using GTFS.Validation;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GTFS.Test.Validation
{
    /// <summary>
    /// Contains tests for the validation code.
    /// </summary>
    [TestFixture]
    public class GTFSFeedValidationTests
    {
        /// <summary>
        /// Tests a simple valid feed.
        /// </summary>
        [Test]
        public void TestValidFeed()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // validate.
            Assert.IsTrue(GTFSFeedValidation.Validate(feed));
        }

        /// <summary>
        /// Tests validation when agency is missing.
        /// </summary>
        [Test]
        public void TestMissingAgency()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // remove.
            feed.Agencies.Remove("DTA");

            // validate.
            Assert.IsFalse(GTFSFeedValidation.Validate(feed));
        }

        /// <summary>
        /// Tests validation when agency is unknown.
        /// </summary>
        [Test]
        public void TestUnknownAgency()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // change to an unknown agency.
            const string UnknownAgency = "unknown agency";
            Assert.IsTrue(feed.Agencies.All(x => x.Id != UnknownAgency));
            Assert.IsTrue(feed.Routes.Any());
            feed.Routes.First().AgencyId = UnknownAgency;
            
            // validate.
            Assert.IsFalse(GTFSFeedValidation.Validate(feed));
        }

        /// <summary>
        /// Tests validation when agency is null.
        /// </summary>
        [Test]
        public void TestNullAgency()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // remove agency link.
            Assert.IsTrue(feed.Routes.Any());
            feed.Routes.First().AgencyId = null;

            // validate.
            Assert.IsTrue(GTFSFeedValidation.Validate(feed));
        }

        /// <summary>
        /// Tests validation when a stop is missing.
        /// </summary>
        [Test]
        public void TestMissingStop()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // remove.
            feed.Stops.Remove("BULLFROG");

            // validate.
            Assert.IsFalse(GTFSFeedValidation.Validate(feed));
        }

        /// <summary>
        /// Tests validation when a route is missing.
        /// </summary>
        [Test]
        public void TestMissingRoute()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // remove.
            feed.Routes.Remove("AB");

            // validate.
            Assert.IsFalse(GTFSFeedValidation.Validate(feed));
        }

        /// <summary>
        /// Tests validation when a trip is missing.
        /// </summary>
        [Test]
        public void TestMissingTrip()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // remove.
            feed.Trips.Remove("AB1");

            // validate.
            Assert.IsFalse(GTFSFeedValidation.Validate(feed));
        }

        /// <summary>
        /// Tests validation when a stop time is missing.
        /// </summary>
        [Test]
        public void TestMissingStopTime()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // remove.
            (feed.StopTimes.Get() as List<StopTime>)[2].StopSequence = 1024;

            // validate.
            Assert.IsFalse(GTFSFeedValidation.Validate(feed));
        }
    }
}