using GTFS.DB;
using GTFS.DB.SQLite;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFS.Test.DB.SQLite
{
    [TestFixture]
    public class SQLiteGTFSFeedDBTests
    {
        public IGTFSFeedDB CreateDB()
        {
            return new SQLiteGTFSFeedDB();
        }

        [Test]
        public void DoTest()
        {
            var db = this.CreateDB();
        }
    }
}
