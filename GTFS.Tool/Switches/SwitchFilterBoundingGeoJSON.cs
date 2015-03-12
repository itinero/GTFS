using GeoAPI.Geometries;
using GTFS.Tool.Switches.Processors;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFS.Tool.Switches
{
    /// <summary>
    /// Represents a switch for filtering data along a geometry specified by a GeoJSON file.
    /// </summary>
    public class SwitchFilterBoundingGeoJSON : Switch
    {   
        /// <summary>
        /// Returns the switches for this command.
        /// </summary>
        /// <returns></returns>
        public override string[] GetSwitch()
        {
            return new string[] { "--bj", "--bounding-geojson" };
        }

        /// <summary>
        /// The bounding geometry GeoJSON file.
        /// </summary>
        public string GeoJSONFile { get; set; }

        /// <summary>
        /// Parse the command arguments for the bounding-box command.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="idx"></param>
        /// <param name="switchOut"></param>
        /// <returns></returns>
        public override int Parse(string[] args, int idx, out Switch switchOut)
        {
            // check next argument.
            if (args.Length < idx)
            {
                throw new SwitchParserException("None", "Invalid path for bounding-geojson command!");
            }

            // everything ok, take the next argument as the path.
            switchOut = new SwitchFilterBoundingGeoJSON()
            {
                GeoJSONFile = args[idx]
            };
            return 1;
        }

        /// <summary>
        /// Creates the processor that belongs to this data.
        /// </summary>
        /// <returns></returns>
        public override ProcessorBase CreateProcessor()
        {
            // parse geojson file.
            using(var geoJSONFile = new FileInfo(this.GeoJSONFile).OpenText())
            {
                string geoJson = geoJSONFile.ReadToEnd();
                var geoJsonReader = new GeoJsonReader();

                var featureCollection = geoJsonReader.Read<FeatureCollection>(geoJson) as FeatureCollection;

                foreach (var feature in featureCollection.Features)
                {
                    return new ProcessorFeedFilter()
                    { // create a bounding box filter.
                        Filter = new GTFS.Filters.GTFSFeedStopsFilter((s) =>
                        {
                            return feature.Geometry.Covers(new Point(new Coordinate(s.Longitude, s.Latitude)));
                        })
                    };
                }
            }
            throw new Exception("No geometries found in GeoJSON");
        }

        /// <summary>
        /// Returns a description of this command.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("--bounding-geojson {0}", this.GeoJSONFile);
        }
    }
}
