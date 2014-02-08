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

using GTFS.Core;
using GTFS.Core.Entities.Enumerations;
using GTFS.Core.IO;
using NUnit.Framework;
using System.Reflection;

namespace GTFS.Test
{
    /// <summary>
    /// Contains basic parsing tests for each entity.
    /// </summary>
    [TestFixture]
    public class ParseEntities
    {
        /// <summary>
        /// Tests parsing agencies.
        /// </summary>
        [Test]
        public void ParseAgencies()
        {
            // create the reader.
            GTFSReader<Feed> reader = new GTFSReader<Feed>();

            // define the agency source file.
            GTFSSourceFileStream sourceFile = new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.agency.txt"), "agency");

            // define the array of source files.
            var source = new IGTFSSourceFile[1];
            source[0] = sourceFile;

            // execute the reader.
            var feed = reader.Read(source);

            // test result.
            Assert.IsNotNull(feed.Agencies);
            Assert.AreEqual(1, feed.Agencies.Count);
            Assert.AreEqual(null, feed.Agencies[0].FareURL);
            Assert.AreEqual("DTA", feed.Agencies[0].Id);
            Assert.AreEqual(null, feed.Agencies[0].LanguageCode);
            Assert.AreEqual("Demo Transit Authority", feed.Agencies[0].Name);
            Assert.AreEqual(null, feed.Agencies[0].Phone);
            Assert.AreEqual("America/Los_Angeles", feed.Agencies[0].Timezone);
            Assert.AreEqual("http://google.com", feed.Agencies[0].URL);
        }

        /// <summary>
        /// Tests parsing routes.
        /// </summary>
        [Test]
        public void ParseRoutes()
        {
            // create the reader.
            GTFSReader<Feed> reader = new GTFSReader<Feed>();

            // define the agency source file.
            GTFSSourceFileStream agencyFile = new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.agency.txt"), "agency");
            GTFSSourceFileStream routeFile = new GTFSSourceFileStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.routes.txt"), "routes");

            // define the array of source files.
            var source = new IGTFSSourceFile[2];
            source[0] = agencyFile;
            source[1] = routeFile;

            // execute the reader.
            var feed = reader.Read(source);

            // test result.
            Assert.IsNotNull(feed.Routes);
            Assert.AreEqual(5, feed.Routes.Count);
            
            //route_id,agency_id,route_short_name,route_long_name,route_desc,route_type,route_url,route_color,route_text_color

            //AB,DTA,10,Airport - Bullfrog,,3,,,
            int idx = 0;
            Assert.AreEqual("AB", feed.Routes[idx].Id);
            Assert.IsNotNull(feed.Routes[idx].Agency);
            Assert.AreEqual("DTA", feed.Routes[idx].Agency.Id);
            Assert.AreEqual("10", feed.Routes[idx].ShortName);
            Assert.AreEqual("Airport - Bullfrog", feed.Routes[idx].LongName);
            Assert.AreEqual(string.Empty, feed.Routes[idx].Description);
            Assert.AreEqual(RouteType.Bus, feed.Routes[idx].Type);
            Assert.AreEqual(null, feed.Routes[idx].Color);
            Assert.AreEqual(null, feed.Routes[idx].TextColor);

            //BFC,DTA,20,Bullfrog - Furnace Creek Resort,,3,,,
            idx = 1;
            Assert.AreEqual("BFC", feed.Routes[idx].Id);
            Assert.IsNotNull(feed.Routes[idx].Agency);
            Assert.AreEqual("DTA", feed.Routes[idx].Agency.Id);
            Assert.AreEqual("20", feed.Routes[idx].ShortName);
            Assert.AreEqual("Bullfrog - Furnace Creek Resort", feed.Routes[idx].LongName);
            Assert.AreEqual(string.Empty, feed.Routes[idx].Description);
            Assert.AreEqual(RouteType.Bus, feed.Routes[idx].Type);
            Assert.AreEqual(null, feed.Routes[idx].Color);
            Assert.AreEqual(null, feed.Routes[idx].TextColor);

            //STBA,DTA,30,Stagecoach - Airport Shuttle,,3,,,
            idx = 2;
            Assert.AreEqual("STBA", feed.Routes[idx].Id);
            Assert.IsNotNull(feed.Routes[idx].Agency);
            Assert.AreEqual("DTA", feed.Routes[idx].Agency.Id);
            Assert.AreEqual("30", feed.Routes[idx].ShortName);
            Assert.AreEqual("Stagecoach - Airport Shuttle", feed.Routes[idx].LongName);
            Assert.AreEqual(string.Empty, feed.Routes[idx].Description);
            Assert.AreEqual(RouteType.Bus, feed.Routes[idx].Type);
            Assert.AreEqual(null, feed.Routes[idx].Color);
            Assert.AreEqual(null, feed.Routes[idx].TextColor);

            //CITY,DTA,40,City,,3,,,
            idx = 3;
            Assert.AreEqual("CITY", feed.Routes[idx].Id);
            Assert.IsNotNull(feed.Routes[idx].Agency);
            Assert.AreEqual("DTA", feed.Routes[idx].Agency.Id);
            Assert.AreEqual("40", feed.Routes[idx].ShortName);
            Assert.AreEqual("City", feed.Routes[idx].LongName);
            Assert.AreEqual(string.Empty, feed.Routes[idx].Description);
            Assert.AreEqual(RouteType.Bus, feed.Routes[idx].Type);
            Assert.AreEqual(null, feed.Routes[idx].Color);
            Assert.AreEqual(null, feed.Routes[idx].TextColor);

            //AAMV,DTA,50,Airport - Amargosa Valley,,3,,,
            idx = 4;
            Assert.AreEqual("AAMV", feed.Routes[idx].Id);
            Assert.IsNotNull(feed.Routes[idx].Agency);
            Assert.AreEqual("DTA", feed.Routes[idx].Agency.Id);
            Assert.AreEqual("50", feed.Routes[idx].ShortName);
            Assert.AreEqual("Airport - Amargosa Valley", feed.Routes[idx].LongName);
            Assert.AreEqual(string.Empty, feed.Routes[idx].Description);
            Assert.AreEqual(RouteType.Bus, feed.Routes[idx].Type);
            Assert.AreEqual(null, feed.Routes[idx].Color);
            Assert.AreEqual(null, feed.Routes[idx].TextColor);
        }
    }
}