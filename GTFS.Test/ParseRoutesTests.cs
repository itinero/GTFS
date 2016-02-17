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

namespace GTFS.Test
{
    /// <summary>
    /// Contains additional parsing tests for routes.
    /// </summary>
    [TestFixture]
    public class ParseRoutesTests
    {
        /// <summary>
        /// Tests parsing routes.
        /// </summary>
        [Test]
        public void ParseRoutesWithDescription()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = new List<IGTFSSourceFile>
            {
                new GTFSSourceFileStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.agency.txt"),"agency"),
                new GTFSSourceFileStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.routes.txt"),"routes")
            };


            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("routes")));

            // test result.
            Assert.IsNotNull(feed.Routes);
            var routes = feed.Routes.ToList();
            Assert.IsTrue(routes.All(x => x.Description == string.Empty));
        }
        
        /// <summary>
        /// Tests parsing routes.
        /// </summary>
        [Test]
        public void ParseRoutesWithoutDescription()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();

            // build the source
            var source = new List<IGTFSSourceFile>
            {
                new GTFSSourceFileStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.sample_feed.agency.txt"),"agency"),
                new GTFSSourceFileStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("GTFS.Test.other_feed.routes.no_desc.txt"),"routes")
            };


            // execute the reader.
            var feed = reader.Read(source, source.First(x => x.Name.Equals("routes")));

            // test result.
            Assert.IsNotNull(feed.Routes);
            var routes = feed.Routes.ToList();
            Assert.IsTrue(routes.All(x => x.Description == null));
        }
    }
}