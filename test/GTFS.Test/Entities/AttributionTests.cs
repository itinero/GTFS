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
using NUnit.Framework;

namespace GTFS.Test.Entities
{
    /// <summary>
    /// Contains tests for the calendar entity and related functionality.
    /// </summary>
    [TestFixture]
    public class AttributionTests
    {
        [Test]
        public void TestDefault()
        {
            var attribution = new Attribution();

            Assert.AreEqual(attribution.Id, null);
            Assert.AreEqual(attribution.AgencyId, null);
            Assert.AreEqual(attribution.RouteId, null);
            Assert.AreEqual(attribution.TripId, null);
            Assert.AreEqual(attribution.OrganisationName, null);
            Assert.AreEqual(attribution.IsProducer, null);
            Assert.AreEqual(attribution.IsOperator, null);
            Assert.AreEqual(attribution.IsAuthority, null);
            Assert.AreEqual(attribution.URL, null);
            Assert.AreEqual(attribution.Email, null);
            Assert.AreEqual(attribution.Phone, null);
        }

        [Test]
        public void TestExpected()
        {
            var attribution = new Attribution() 
            { 
                OrganisationName = "org",
                IsProducer = true,
                IsOperator = false,
                Email = "hi@hi.com"
            };

            Assert.AreEqual(attribution.Id, null);
            Assert.AreEqual(attribution.AgencyId, null);
            Assert.AreEqual(attribution.RouteId, null);
            Assert.AreEqual(attribution.TripId, null);
            Assert.AreEqual(attribution.OrganisationName, "org");
            Assert.AreEqual(attribution.IsProducer, true);
            Assert.AreEqual(attribution.IsOperator, false);
            Assert.AreEqual(attribution.IsAuthority, null);
            Assert.AreEqual(attribution.URL, null);
            Assert.AreEqual(attribution.Email, "hi@hi.com");
            Assert.AreEqual(attribution.Phone, null);
        }
    }
}