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

using GTFS.Filters;
using GTFS.Validation;
using NUnit.Framework;

namespace GTFS.Test.Filters
{
    /// <summary>
    /// Contains tests for the stops filters.
    /// </summary>
    [TestFixture]
    public class GTFSFeedStopsFilterTests
    {
        /// <summary>
        /// Tests filtering no stops.
        /// </summary>
        [Test]
        public void TestFilterNoStops()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // create the filter.
            var filter = new GTFSFeedStopsFilter((x) =>
            {
                return true;
            });

            // execute filter.
            var filtered = filter.Filter(feed);
            Assert.IsTrue(GTFSFeedValidation.Validate(filtered));
        }

        /// <summary>
        /// Tests filtering one stop.
        /// </summary>
        [Test]
        public void TestFilterOneStop()
        {
            // create the reader.
            var reader = new GTFSReader<GTFSFeed>();
            var source = GTFSAssert.BuildSource();

            // execute the reader.
            var feed = reader.Read(source);

            // create the filter.
            var filter = new GTFSFeedStopsFilter((x) =>
            {
                return x.Id == "BULLFROG";
            });

            // execute filter.
            var filtered = filter.Filter(feed);
            Assert.IsTrue(GTFSFeedValidation.Validate(filtered));
        }
    }
}