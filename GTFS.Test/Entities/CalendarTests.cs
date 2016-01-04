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
    public class CalendarTests
    {
        /// <summary>
        /// Tests the mask day properties relation.
        /// </summary>
        [Test]
        public void TestMask()
        {
            var calendar = new Calendar();

            calendar.Mask = 0;

            Assert.AreEqual(calendar.Monday, false);
            Assert.AreEqual(calendar.Tuesday, false);
            Assert.AreEqual(calendar.Wednesday, false);
            Assert.AreEqual(calendar.Thursday, false);
            Assert.AreEqual(calendar.Friday, false);
            Assert.AreEqual(calendar.Saturday, false);
            Assert.AreEqual(calendar.Sunday, false);

            calendar.Mask = 1;

            Assert.AreEqual(calendar.Monday, true);
            Assert.AreEqual(calendar.Tuesday, false);
            Assert.AreEqual(calendar.Wednesday, false);
            Assert.AreEqual(calendar.Thursday, false);
            Assert.AreEqual(calendar.Friday, false);
            Assert.AreEqual(calendar.Saturday, false);
            Assert.AreEqual(calendar.Sunday, false);

            calendar.Mask = 64;

            Assert.AreEqual(calendar.Monday, false);
            Assert.AreEqual(calendar.Tuesday, false);
            Assert.AreEqual(calendar.Wednesday, false);
            Assert.AreEqual(calendar.Thursday, false);
            Assert.AreEqual(calendar.Friday, false);
            Assert.AreEqual(calendar.Saturday, false);
            Assert.AreEqual(calendar.Sunday, true);

            calendar.Mask = 1+4+8+16;

            Assert.AreEqual(calendar.Monday, true);
            Assert.AreEqual(calendar.Tuesday, false);
            Assert.AreEqual(calendar.Wednesday, true);
            Assert.AreEqual(calendar.Thursday, true);
            Assert.AreEqual(calendar.Friday, true);
            Assert.AreEqual(calendar.Saturday, false);
            Assert.AreEqual(calendar.Sunday, false);

            calendar.Mask = 0;
            calendar.Monday = true;
            Assert.AreEqual(1, calendar.Mask);

            calendar.Mask = 0;
            calendar.Sunday = true;
            Assert.AreEqual(64, calendar.Mask);

            calendar.Mask = 0;
            calendar.Monday = true;
            calendar.Wednesday = true;
            calendar.Thursday = true;
            calendar.Friday = true;
            Assert.AreEqual(1+4+8+16, calendar.Mask);
        }

        /// <summary>
        /// Tests merging two calendar objects and have them still represent the same information.
        /// </summary>
        [Test]
        public void TestTryMerge()
        {
            // two simple consequitive weeks.
            var calendar1 = new Calendar()
            {
                ServiceId = "0",
                Mask = 64,
                StartDate = new System.DateTime(2015, 11, 23),
                EndDate = new System.DateTime(2015, 11, 29)
            };
            var calendar2 = new Calendar()
            {
                ServiceId = "0",
                Mask = 64,
                StartDate = new System.DateTime(2015, 11, 30),
                EndDate = new System.DateTime(2015, 12, 06)
            };

            Calendar merge;
            Assert.IsTrue(calendar1.TryMerge(calendar2, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(64, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 29), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 06), merge.EndDate);

            Assert.IsTrue(calendar2.TryMerge(calendar1, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(64, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 29), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 06), merge.EndDate);

            // two calendars one completely overlapping the other.
            calendar1 = new Calendar()
            {
                ServiceId = "0",
                Mask = 64,
                StartDate = new System.DateTime(2015, 11, 23),
                EndDate = new System.DateTime(2015, 11, 29)
            };
            calendar2 = new Calendar()
            {
                ServiceId = "0",
                Mask = 64,
                StartDate = new System.DateTime(2015, 11, 23),
                EndDate = new System.DateTime(2015, 12, 06)
            };

            Assert.IsTrue(calendar1.TryMerge(calendar2, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(64, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 29), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 06), merge.EndDate);

            Assert.IsTrue(calendar2.TryMerge(calendar1, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(64, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 29), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 06), merge.EndDate);

            // two calendars one completely overlapping the other.
            calendar1 = new Calendar()
            {
                ServiceId = "0",
                Mask = 64,
                StartDate = new System.DateTime(2015, 11, 23),
                EndDate = new System.DateTime(2015, 11, 29)
            };
            calendar1.TrimDates();
            calendar2 = new Calendar()
            {
                ServiceId = "0",
                Mask = 64,
                StartDate = new System.DateTime(2015, 11, 23),
                EndDate = new System.DateTime(2015, 12, 06)
            };
            calendar2.TrimDates();

            Assert.IsTrue(calendar1.TryMerge(calendar2, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(64, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 29), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 06), merge.EndDate);

            Assert.IsTrue(calendar2.TryMerge(calendar1, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(64, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 29), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 06), merge.EndDate);

            // two calendars one completely overlapping the other.
            calendar1 = new Calendar()
            {
                ServiceId = "0",
                Mask = 64,
                StartDate = new System.DateTime(2015, 11, 23),
                EndDate = new System.DateTime(2015, 11, 29)
            };
            calendar1.TrimDates();
            calendar2 = new Calendar()
            {
                ServiceId = "0",
                Mask = 1+64,
                StartDate = new System.DateTime(2015, 11, 23),
                EndDate = new System.DateTime(2015, 12, 06)
            };
            calendar2.TrimDates();

            Assert.IsTrue(calendar1.TryMerge(calendar2, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(1+64, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 23), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 06), merge.EndDate);

            Assert.IsTrue(calendar2.TryMerge(calendar1, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(1+64, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 23), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 06), merge.EndDate);

            // two calendars with conflicting masks.
            calendar1 = new Calendar()
            {
                ServiceId = "0",
                Mask = 2+64,
                StartDate = new System.DateTime(2015, 11, 23),
                EndDate = new System.DateTime(2015, 11, 29)
            };
            calendar1.TrimDates();
            calendar2 = new Calendar()
            {
                ServiceId = "0",
                Mask = 1+64,
                StartDate = new System.DateTime(2015, 11, 30),
                EndDate = new System.DateTime(2015, 12, 06)
            };
            calendar2.TrimDates();

            Assert.IsFalse(calendar1.TryMerge(calendar2, out merge));
            Assert.IsFalse(calendar2.TryMerge(calendar1, out merge));

            // two calendars both spanning more than one week.
            calendar1 = new Calendar()
            {
                ServiceId = "0",
                Mask = 127,
                StartDate = new System.DateTime(2015, 11, 16),
                EndDate = new System.DateTime(2015, 11, 29)
            };
            calendar1.TrimDates();
            calendar2 = new Calendar()
            {
                ServiceId = "0",
                Mask = 127,
                StartDate = new System.DateTime(2015, 11, 30),
                EndDate = new System.DateTime(2015, 12, 13)
            };
            calendar2.TrimDates();

            Assert.IsTrue(calendar1.TryMerge(calendar2, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(127, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 16), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 13), merge.EndDate);

            Assert.IsTrue(calendar2.TryMerge(calendar1, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(127, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 16), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 13), merge.EndDate);

            // two calendars first one week with a few don't-case other more than a week.
            calendar1 = new Calendar()
            {
                ServiceId = "0",
                Mask = 64 + 32 + 16 + 8,
                StartDate = new System.DateTime(2015, 11, 26),
                EndDate = new System.DateTime(2015, 11, 29)
            };
            calendar1.TrimDates();
            calendar2 = new Calendar()
            {
                ServiceId = "0",
                Mask = 127,
                StartDate = new System.DateTime(2015, 11, 30),
                EndDate = new System.DateTime(2015, 12, 13)
            };
            calendar2.TrimDates();

            Assert.IsTrue(calendar1.TryMerge(calendar2, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(127, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 26), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 13), merge.EndDate);

            Assert.IsTrue(calendar2.TryMerge(calendar1, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(127, merge.Mask);
            Assert.AreEqual(new System.DateTime(2015, 11, 26), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2015, 12, 13), merge.EndDate);

            // two calendars first two weeks with a few don't-cares other the week right after.
            calendar1 = new Calendar()
            {
                ServiceId = "0",
                Mask = 127,
                StartDate = new System.DateTime(2016, 01, 01),
                EndDate = new System.DateTime(2016, 01, 10)
            };
            calendar1.TrimDates();
            calendar2 = new Calendar()
            {
                ServiceId = "0",
                Mask = 127,
                StartDate = new System.DateTime(2016, 01, 11),
                EndDate = new System.DateTime(2016, 01, 17)
            };
            calendar2.TrimDates();

            Assert.IsTrue(calendar1.TryMerge(calendar2, out merge));
            Assert.AreEqual("0", merge.ServiceId);
            Assert.AreEqual(127, merge.Mask);
            Assert.AreEqual(new System.DateTime(2016, 01, 01), merge.StartDate);
            Assert.AreEqual(new System.DateTime(2016, 01, 17), merge.EndDate);
        }
    }
}