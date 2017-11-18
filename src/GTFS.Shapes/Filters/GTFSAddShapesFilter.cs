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
            _routerDb = routerDb;

            if (profilesPerRouteType == null)
            { // by default use profiles named, tram, train and transit-bus.
                profilesPerRouteType = new Dictionary<RouteTypeExtended, IProfileInstance>();
                
                // add all train types.
                profilesPerRouteType.Add(RouteTypeExtended.RailwayService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.HighSpeedRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.LongDistanceTrains, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.InterRegionalRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.CarTransportRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.SleeperRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.RegionalRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.TouristRailwayService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.RailShuttleWithinComplex, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.SuburbanRailway, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.ReplacementRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.SpecialRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.LorryTransportRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.AllRailServices, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.CrossCountryRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.VehicleTransportRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.RackandPinionRailway, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.AdditionalRailService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.SuburbanRailwayService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.UrbanRailwayService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.MetroCoachService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.UndergroundService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.UrbanRailwayServiceDefault, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.AllUrbanRailwayServices, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.Monorail, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.MetroService, routerDb.GetSupportedProfile("train"));
                profilesPerRouteType.Add(RouteTypeExtended.UndergroundMetroService, routerDb.GetSupportedProfile("train"));

                // add all transit-bus types.
                profilesPerRouteType.Add(RouteTypeExtended.CoachService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.InternationalCoachService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.NationalCoachService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.ShuttleCoachService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.RegionalCoachService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.SpecialCoachService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.SightseeingCoachService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.TouristCoachService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.CommuterCoachService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.AllCoachServices, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.BusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.RegionalBusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.ExpressBusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.StoppingBusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.LocalBusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.NightBusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.PostBusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.SpecialNeedsBus, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.MobilityBusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.MobilityBusforRegisteredDisabled, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.SightseeingBus, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.ShuttleBus, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.SchoolBus, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.SchoolandPublicServiceBus, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.RailReplacementBusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.DemandandResponseBusService, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.AllBusServices, routerDb.GetSupportedProfile("transit-bus"));
                profilesPerRouteType.Add(RouteTypeExtended.TrolleybusService, routerDb.GetSupportedProfile("transit-bus"));

                // add bus types.
                profilesPerRouteType.Add(RouteTypeExtended.TramService, routerDb.GetSupportedProfile("tram"));
                profilesPerRouteType.Add(RouteTypeExtended.CityTramService, routerDb.GetSupportedProfile("tram"));
                profilesPerRouteType.Add(RouteTypeExtended.LocalTramService, routerDb.GetSupportedProfile("tram"));
                profilesPerRouteType.Add(RouteTypeExtended.RegionalTramService, routerDb.GetSupportedProfile("tram"));
                profilesPerRouteType.Add(RouteTypeExtended.SightseeingTramService, routerDb.GetSupportedProfile("tram"));
                profilesPerRouteType.Add(RouteTypeExtended.ShuttleTramService, routerDb.GetSupportedProfile("tram"));
                profilesPerRouteType.Add(RouteTypeExtended.AllTramServices, routerDb.GetSupportedProfile("tram"));
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