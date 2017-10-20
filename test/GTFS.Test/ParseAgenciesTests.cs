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
using GTFS.IO.CSV;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GTFS.Exceptions;

namespace GTFS.Test
{
    /// <summary>
    /// Contains additional parsing tests for agencies.
    /// </summary>
    [TestFixture]
    public class ParseAgenciesTests
    {
        private static Assembly _executingAssembly;

        /// <summary>
        /// Setup the test environment.
        /// </summary>
        [OneTimeSetUp]
        public static void Setup()
        {
            _executingAssembly = Assembly.GetExecutingAssembly();
        }

        /// <summary>
        /// Tests parsing single agency without agency_id.
        /// </summary>
        [Test]
        public void ParseSingleAgencyWithoutAgencyId()
        {
            var reader = new GTFSReader<GTFSFeed>();
            var source = new List<IGTFSSourceFile>
            {
                new GTFSSourceFileStream(_executingAssembly.GetManifestResourceStream("GTFS.Test.other_feed.agency_no_id.txt"),"agency")
            };


            var feed = reader.Read(source, source.First(x => x.Name.Equals("agency")));


            var agencies = feed.Agencies;
            Assert.IsNotNull(agencies);

            var agency = agencies.SingleOrDefault();
            Assert.IsNotNull(agency);
            Assert.IsNull(agency.Id);
        }

        /// <summary>
        /// Tests parsing single agency with agency_id.
        /// </summary>
        [Test]
        public void ParseSingleAgencyWithAgencyId()
        {
            var reader = new GTFSReader<GTFSFeed>();
            var source = new List<IGTFSSourceFile>
            {
                new GTFSSourceFileStream(_executingAssembly.GetManifestResourceStream("GTFS.Test.other_feed.agency_with_id.txt"),"agency")
            };


            var feed = reader.Read(source, source.First(x => x.Name.Equals("agency")));


            var agencies = feed.Agencies;
            Assert.IsNotNull(agencies);

            var agency = agencies.SingleOrDefault();
            Assert.IsNotNull(agency);
            Assert.IsNotNull(agency.Id);
        }

        /// <summary>
        /// Tests parsing multiple agencies with agency_id.
        /// </summary>
        [Test]
        public void ParseAgenciesWithAgencyId()
        {
            var reader = new GTFSReader<GTFSFeed>();
            var source = new List<IGTFSSourceFile>
            {
                new GTFSSourceFileStream(_executingAssembly.GetManifestResourceStream("GTFS.Test.other_feed.agencies_with_id.txt"),"agency")
            };


            var feed = reader.Read(source, source.First(x => x.Name.Equals("agency")));


            var agencies = feed.Agencies;
            Assert.IsNotNull(agencies);
            Assert.IsNotNull(agencies.SingleOrDefault(x => x.Id == "DTA"));
            Assert.IsNotNull(agencies.SingleOrDefault(x => x.Id == "OTA"));
        }

        /// <summary>
        /// Tests parsing multiple agencies with agency_id.
        /// </summary>
        [Test]
        public void ParseAgenciesWithoutAgencyId()
        {
            const string Agency = "agency";
            var reader = new GTFSReader<GTFSFeed>(true);
            var source = new List<IGTFSSourceFile>
            {
                new GTFSSourceFileStream(_executingAssembly.GetManifestResourceStream("GTFS.Test.other_feed.agencies_no_id.txt"),Agency)
            };
            
            Assert.Throws<GTFSRequiredFieldMissingException>(() =>
            {
                reader.Read(source, source.First(x => x.Name.Equals(Agency)));
            },
            GTFSRequiredFieldMissingException.MessageFormat, "agency_id", Agency);
        }

        /// <summary>
        /// Tests parsing single agency with an email address
        /// </summary>
        [Test]
        public void ParseSingleAgencyWithEmail()
        {
            var reader = new GTFSReader<GTFSFeed>();
            var source = new List<IGTFSSourceFile>
            {
                new GTFSSourceFileStream(_executingAssembly.GetManifestResourceStream("GTFS.Test.other_feed.agency_with_email.txt"),"agency")
            };


            var feed = reader.Read(source, source.First(x => x.Name.Equals("agency")));


            var agencies = feed.Agencies;
            Assert.IsNotNull(agencies);

            var agency = agencies.SingleOrDefault();
            Assert.IsNotNull(agency);
            Assert.AreEqual("support@demotransit.com", agency.Email);
        }
    }
}