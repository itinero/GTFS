using GTFS.Tool.Switches.Processors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFS.Tool.Switches
{
    /// <summary>
    /// Represents a switch for filtering data and just leaving one route.
    /// </summary>
    public class SwitchFilterRoutes : Switch
    {
        /// <summary>
        /// Returns the switches for this command.
        /// </summary>
        /// <returns></returns>
        public override string[] GetSwitch()
        {
            return new string[] { "--fr", "--filter-routes" };
        }

        /// <summary>
        /// The route id's to include/exclude.
        /// </summary>
        public string[] RouteIds { get; set; }

        /// <summary>
        /// The exclude/include flag.
        /// </summary>
        public bool Exclude { get; set; }

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
            if (args.Length < idx + 1)
            {
                throw new SwitchParserException("None", "Invalid --filter-routes command!");
            }

            int startIdx = idx;
            while (idx < args.Length &&
                !SwitchParser.IsSwitch(args[idx]))
            { // parse arguments.
                var argSplit = args[idx].Split('=');

                if (argSplit.Length != 2 ||
                    argSplit[0] == null ||
                    argSplit[1] == null)
                {
                    throw new SwitchParserException(args[idx],
                                                         "Invalid argument for --filter-routes.");
                }

                switch(argSplit[0])
                {
                    case "ids":
                        this.RouteIds = argSplit[1].Split(',');
                        break;
                    case "exclude":
                        this.Exclude = argSplit[1].ToLowerInvariant() == "yes";
                        break;
                    default:
                        throw new SwitchParserException(args[idx],
                                                             "Unknown argument for --filter-routes.");
                }

                idx++;
            }

            // everything ok, take the next argument as the filename.
            switchOut = new SwitchFilterRoutes()
            {
                RouteIds = this.RouteIds,
                Exclude = this.Exclude
            };
            return idx - startIdx;
        }

        /// <summary>
        /// Creates the processor that belongs to this data.
        /// </summary>
        /// <returns></returns>
        public override ProcessorBase CreateProcessor()
        {
            var routeIdSet = new HashSet<string>();
            if (this.RouteIds != null)
            {
                foreach (string routeId in this.RouteIds)
                {
                    routeIdSet.Add(routeId);
                }
            }
            if(this.Exclude)
            { // build a filter that excludes all given ids.
                return new ProcessorFeedFilter()
                { // create a bounding box filter.
                    Filter = new GTFS.Filters.GTFSFeedRoutesFilter(x => !routeIdSet.Contains(x.Id))
                };
            }
            return new ProcessorFeedFilter()
            { // build a filter that includes all given ids.
                Filter = new GTFS.Filters.GTFSFeedRoutesFilter(routeIdSet)
            };
        }

        /// <summary>
        /// Returns a description of this command.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string routeIdString = string.Empty;
            if (this.RouteIds != null && this.RouteIds.Length > 0)
            {
                routeIdString = this.RouteIds[0];
                for(int idx = 1; idx < this.RouteIds.Length; idx++)
                {
                    routeIdString = routeIdString + "," + this.RouteIds[idx];
                }
            }
            if(this.Exclude)
            {
                return string.Format("--filter-routes ids={0} exclude=yes", routeIdString);
            }
            return string.Format("--filter-routes ids={0}", routeIdString);
        }
    }
}