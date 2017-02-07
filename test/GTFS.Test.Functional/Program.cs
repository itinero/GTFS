using GTFS.IO;
using Itinero;
using Itinero.IO.Osm;
using Itinero.Profiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GTFS.Test.Functional
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            // enable logging.
            OsmSharp.Logging.Logger.LogAction = (o, level, message, parameters) =>
            {
                Console.WriteLine(string.Format("[{0}] {1} - {2}", o, level, message));
            };
            Itinero.Logging.Logger.LogAction = (o, level, message, parameters) =>
            {
                Console.WriteLine(string.Format("[{0}] {1} - {2}", o, level, message));
            };
            GTFS.Logging.Logger.LogAction = (o, level, message, parameter) =>
            {
                Console.WriteLine(string.Format("[{0}] {1} - {2}", o, level, message));
            };

            //var train = DynamicVehicle.LoadFromStream(File.OpenRead(@".\Osm\profiles\train.lua"));
            //var tram = DynamicVehicle.LoadFromStream(File.OpenRead(@".\Osm\profiles\tram.lua"));
            //var transitBus = DynamicVehicle.LoadFromStream(File.OpenRead(@".\Osm\profiles\transit-bus.lua"));
            //var routerDb = new RouterDb();
            //routerDb.LoadOsmData(File.OpenRead(@"C:\work\data\OSM\belgium-latest.osm.pbf"), tram, train, transitBus);
            //using (var stream = File.Open("transit.routerdb", FileMode.Create))
            //{
            //    routerDb.Serialize(stream);
            //}
            var routerDb = RouterDb.Deserialize(File.OpenRead("transit.routerdb"));
            var router = new Router(routerDb);
            var train = routerDb.GetSupportedVehicle("train");

            var reader = new GTFSReader<GTFSFeed>();
            using (var sources = new GTFSDirectorySource(new DirectoryInfo(@"C:\work\data\gtfs\nmbs")))
            {
                var feed = reader.Read(sources);

                GTFS.Shapes.ShapeBuilder.BuildShapes(feed, router, train.Shortest());

                var targets = new GTFSDirectoryTarget(new DirectoryInfo(@"C:\work\data\gtfs\nmbs-shapes"));
                var writer = new GTFSWriter<GTFSFeed>();
                writer.Write(feed, targets);
            }
        }
    }
}
