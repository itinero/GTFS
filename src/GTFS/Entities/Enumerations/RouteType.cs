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

namespace GTFS.Entities.Enumerations
{
    /// <summary>
    /// Describes the type of transportation used on a route.
    /// </summary>
    public enum RouteType
    {
        /// <summary>
        /// Any light rail or street level system within a metropolitan area.
        /// </summary>
        Tram,
        /// <summary>
        /// Any underground rail system within a metropolitan area.
        /// </summary>
        SubwayMetro,
        /// <summary>
        /// Used for intercity or long-distance travel.
        /// </summary>
        Rail,
        /// <summary>
        /// Used for short- and long-distance bus routes.
        /// </summary>
        Bus,
        /// <summary>
        /// Used for short- and long-distance boat service.
        /// </summary>
        Ferry,
        /// <summary>
        /// Used for street-level cable cars where the cable runs beneath the car.
        /// </summary>
        CableCar,
        /// <summary>
        /// Typically used for aerial cable cars where the car is suspended from the cable.
        /// </summary>
        Gondola,
        /// <summary>
        /// Any rail system designed for steep inclines.
        /// </summary>
        Funicular,
        /// <summary>
        /// Trolleybus. Electric buses that draw power from overhead wires using poles. 
        /// </summary>
        Trolleybus = 11,
        /// <summary>
        /// Monorail. Railway in which the track consists of a single rail or a beam.
        /// </summary>
        Monorail = 12,
    }

    /// <summary>
    /// Contains extension methods for the route type.
    /// </summary>
    public static class RouteTypeExtensions
    {
        /// <summary>
        /// Converts a route type to it's extended equivalent.
        /// </summary>
        /// <returns></returns>
        public static RouteTypeExtended ToExtended(this RouteType routeType)
        {
            //0 - Tram, Light Rail, Streetcar - 900
            //1 - Subway, Metro - 400
            //2 - Rail - 100
            //3 - Bus - 700
            //4 - Ferry - 1000
            //5 - Cable Car - ?
            //6 - Gondola, Suspended cable car - 1300
            //7 - Funicular - 1400
            //11 - Trolleybus - 800
            //12 - Monorail - 405

            switch (routeType)
            {
                case RouteType.Bus:
                    return RouteTypeExtended.BusService;
                case RouteType.CableCar:
                    return RouteTypeExtended.CableCarService;
                case RouteType.Rail:
                    return RouteTypeExtended.RailwayService;
                case RouteType.Tram:
                    return RouteTypeExtended.TramService;
                case RouteType.SubwayMetro:
                    return RouteTypeExtended.UrbanRailwayService;
                case RouteType.Ferry:
                    return RouteTypeExtended.WaterTransportService;
                case RouteType.Gondola:
                    return RouteTypeExtended.TelecabinService;
                case RouteType.Funicular:
                    return RouteTypeExtended.FunicularService;
                case RouteType.Trolleybus:
                    return RouteTypeExtended.TrolleybusService;
                case RouteType.Monorail:
                    return RouteTypeExtended.Monorail;
            }
            throw new System.Exception("Cannot convert route type.");
        }
    }
}