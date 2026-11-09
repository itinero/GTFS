using System.IO;
using System.Linq;
using System.Text;
using GTFS.IO;
using GTFS.IO.CSV;
using NUnit.Framework;

namespace GTFS.Test
{
    [TestFixture]
    public class CSVReaderTests
    {
        [TestCase(false)]
        [TestCase(true)]
        public void ReadsQuotedTripFields(bool fromStream)
        {
            var lines = new[]
            {
                "route_id,service_id,trip_id,trip_headsign,block_id",
                "r,s,first,\"River, East\",\"A\"\"\"\"B\"",
                "r,s,second,West,\"Block, 2\""
            };
            IGTFSSourceFile file = fromStream
                ? (IGTFSSourceFile)new GTFSSourceFileStream(ToStream(lines), "trips")
                : new GTFSSourceFileLines(lines, "trips");
            var source = new IGTFSSourceFile[]
            {
                new GTFSSourceFileLines(new[] { "agency_name,agency_url,agency_timezone", "Transit,https://example.org,UTC" }, "agency"),
                new GTFSSourceFileLines(new[] { "route_id,route_short_name,route_type", "r,R,3" }, "routes"),
                file
            };
            var feed = new GTFSReader<GTFSFeed>().Read(source, file);

            var trips = feed.Trips.OrderBy(t => t.Id).ToArray();
            Assert.AreEqual(2, trips.Length);
            Assert.AreEqual("River, East", trips[0].Headsign);
            Assert.AreEqual("A\"\"B", trips[0].BlockId);
            Assert.AreEqual("Block, 2", trips[1].BlockId);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ReadsEmptyAndQuotedFieldsWithCustomSeparator(bool fromStream)
        {
            var lines = new[] { ";\" left;right \";\"\"", "\"a\"\"b\";;plain;" };
            using (ICSVReader reader = fromStream
                ? (ICSVReader)new CSVStreamReader(ToStream(lines), ';')
                : new CSVLineEnumerableReader(lines, ';'))
            {
                Assert.IsTrue(reader.MoveNext());
                CollectionAssert.AreEqual(new[] { "", " left;right ", "" }, reader.Current);
                Assert.IsTrue(reader.MoveNext());
                CollectionAssert.AreEqual(new[] { "a\"b", "", "plain", "" }, reader.Current);
                Assert.IsFalse(reader.MoveNext());
            }
        }

        private static MemoryStream ToStream(string[] lines)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(string.Join("\n", lines)));
        }
    }
}
