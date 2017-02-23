// The MIT License (MIT)

// Copyright (c) 2017 Ben Abelshausen

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

using GTFS.Entities.Enumerations;
using GTFS.Filters;
using Itinero;
using Itinero.Profiles;
using System;
using System.Collections.Generic;

namespace GTFS.Shapes.Filters
{
    /// <summary>
    /// Represents a GTFS feed filter that adds shapes based on an Itinero RouterDb.
    /// </summary>
    public class GTFSAddShapesFilter : GTFSFeedFilter
    {
        private readonly RouterDb _routerDb;
        private readonly Dictionary<RouteTypeExtended, IProfileInstance> _profilesPerRouteType;

        /// <summary>
        /// Creates a new filter.
        /// </summary>
        public GTFSAddShapesFilter(RouterDb routerDb, Dictionary<RouteTypeExtended, IProfileInstance> profilesPerRouteType = null)
        {
            _routerDb = new RouterDb();

            if (profilesPerRouteType == null)
            { // by default use profiles named, tram, train and transit-bus.
                profilesPerRouteType = new Dictionary<RouteTypeExtended, IProfileInstance>();
                profilesPerRouteType.Add(RouteType.Tram.ToExtended(), routerDb.GetSupportedProfile("tram"));
                profilesPerRouteType.Add(RouteType.Rail.ToExtended(), routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteType.Bus.ToExtended(), routerDb.GetSupportedProfile("transit-bus"));
            }
            _profilesPerRouteType = profilesPerRouteType;
        }

        /// <summary>
        /// Filters the given feed and returns a filtered version.
        /// </summary>
        public override IGTFSFeed Filter(IGTFSFeed feed)
        {
            var shapeBuilder = new ShapeBuilder();
            shapeBuilder.BuildShapes(feed, new Router(_routerDb), (t) =>
            {
                var route = feed.Routes.Get(t.RouteId);
                IProfileInstance profile;
                if (!_profilesPerRouteType.TryGetValue(route.Type, out profile))
                {
                    throw new Exception(string.Format("No profile found for route type: {0}", route.Type));
                }
                return profile;
            });
            return feed;
        }
    }
}