using GTFS.Entities.Enumerations;
using GTFS.IO;
using GTFS.Shapes;
using Itinero;
using Itinero.Geo;
using Itinero.IO.Osm;
using Itinero.Profiles;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
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

            var profilePerRouteType = new Dictionary<RouteTypeExtended, IProfileInstance>();
            profilePerRouteType.Add(RouteType.Tram.ToExtended(), routerDb.GetSupportedProfile("tram"));
            profilePerRouteType.Add(RouteType.Rail.ToExtended(), routerDb.GetSupportedProfile("train"));
            profilePerRouteType.Add(RouteType.Bus.ToExtended(), routerDb.GetSupportedProfile("transit-bus"));
            
            var resolvedErrors = new FeatureCollection();
            var reader = new GTFSReader<GTFSFeed>();
            using (var sources = new GTFSDirectorySource(new DirectoryInfo(@"C:\work\data\gtfs\delijn")))
            {
                var feed = reader.Read(sources);

                var shapeBuilder = new ShapeBuilder();
                var stopsInError = new HashSet<string>();
                shapeBuilder.StopNotResolved = (stop, location, routerpoint) =>
                {
                    if (stopsInError.Contains(stop.Id))
                    {
                        return;
                    }
                    stopsInError.Add(stop.Id);
                    var attributesTable = new AttributesTable();
                    attributesTable.AddAttribute("error", routerpoint.ErrorMessage);
                    attributesTable.AddAttribute("stop_name", stop.Name);
                    attributesTable.AddAttribute("stop_id", stop.Id);
                    resolvedErrors.Add(new Feature(new Point(new GeoAPI.Geometries.Coordinate(location.Longitude, location.Latitude)),
                        attributesTable));
                };
                shapeBuilder.BuildShapes(feed, router, (t) =>
                {
                    var route = feed.Routes.Get(t.RouteId);
                    IProfileInstance profile;
                    if (!profilePerRouteType.TryGetValue(route.Type, out profile))
                    {
                        throw new Exception(string.Format("No profile found for route type: {0}", route.Type));
                    }
                    return profile;
                }, true, true);

                var resolvedErrorsJson = ToJson(resolvedErrors);

                var shapeFeatures = new FeatureCollection();
                for (var i = 0; i < shapeBuilder.TripShapes.ShapeCount; i++)
                {
                    var shape = new List<Itinero.LocalGeo.Coordinate>(shapeBuilder.TripShapes.ShapesArray[i]);
                    var lineString = new LineString(shape.ToCoordinatesArray());
                    var feature = new Feature(lineString, new AttributesTable());
                    shapeFeatures.Add(feature);
                }
                var shapeFeaturesJson = ToJson(shapeFeatures);
                using (var stream = new StreamWriter(File.Open("temp.geojson", FileMode.Create)))
                {
                    stream.Write(shapeFeaturesJson);
                }

                var targets = new GTFSDirectoryTarget(new DirectoryInfo(@"C:\work\data\gtfs\nmbs-shapes"));
                var writer = new GTFSWriter<GTFSFeed>();
                writer.Write(feed, targets);
            }
        }
        
        private static string ToJson(FeatureCollection featureCollection)
        {
            var jsonSerializer = new NetTopologySuite.IO.GeoJsonSerializer();
            var jsonStream = new StringWriter();
            jsonSerializer.Serialize(jsonStream, featureCollection);
            var json = jsonStream.ToInvariantString();
            return json;
        }
    }
}
