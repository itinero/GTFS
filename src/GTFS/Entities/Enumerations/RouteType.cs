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
        /// Tram, Streetcar, Light rail. Any light rail or street level system within a metropolitan area.
        /// </summary>
        Tram,
        /// <summary>
        /// Subway, Metro. Any underground rail system within a metropolitan area.
        /// </summary>
        SubwayMetro,
        /// <summary>
        /// Rail. Used for intercity or long-distance travel.
        /// </summary>
        Rail,
        /// <summary>
        /// Bus. Used for short- and long-distance bus routes.
        /// </summary>
        Bus,
        /// <summary>
        /// Ferry. Used for short- and long-distance boat service.
        /// </summary>
        Ferry,
        /// <summary>
        /// Cable tram. Used for street-level rail cars where the cable runs beneath the vehicle, e.g., cable car in San Francisco.
        /// </summary>
        CableCar,
        /// <summary>
        /// Aerial lift, suspended cable car (e.g., gondola lift, aerial tramway). Cable transport where cabins, cars, gondolas or open chairs are suspended by means of one or more cables.
        /// </summary>
        Gondola,
        /// <summary>
        /// Funicular. Any rail system designed for steep inclines.
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
        /// <summary>
        /// RailwayService
        /// </summary>
        RailwayService = 100,
        /// <summary>
        /// HighSpeedRailService
        /// </summary>
        HighSpeedRailService = 101,
        /// <summary>
        /// LongDistanceTrains
        /// </summary>
        LongDistanceTrains = 102,
        /// <summary>
        /// InterRegionalRailService
        /// </summary>
        InterRegionalRailService = 103,
        /// <summary>
        /// CarTransportRailService
        /// </summary>
        CarTransportRailService = 104,
        /// <summary>
        /// SleeperRailService
        /// </summary>
        SleeperRailService = 105,
        /// <summary>
        /// RegionalRailService
        /// </summary>
        RegionalRailService = 106,
        /// <summary>
        /// TouristRailwayService
        /// </summary>
        TouristRailwayService = 107,
        /// <summary>
        /// RailShuttleWithinComplex
        /// </summary>
        RailShuttleWithinComplex = 108,
        /// <summary>
        /// SuburbanRailway
        /// </summary>
        SuburbanRailway = 109,
        /// <summary>
        /// ReplacementRailService
        /// </summary>
        ReplacementRailService = 110,
        /// <summary>
        /// SpecialRailService
        /// </summary>
        SpecialRailService = 111,
        /// <summary>
        /// LorryTransportRailService
        /// </summary>
        LorryTransportRailService = 112,
        /// <summary>
        /// AllRailServices
        /// </summary>
        AllRailServices = 113,
        /// <summary>
        /// CrossCountryRailService
        /// </summary>
        CrossCountryRailService = 114,
        /// <summary>
        /// VehicleTransportRailService
        /// </summary>
        VehicleTransportRailService = 115,
        /// <summary>
        /// RackandPinionRailway
        /// </summary>
        RackandPinionRailway = 116,
        /// <summary>
        /// AdditionalRailService
        /// </summary>
        AdditionalRailService = 117,
        /// <summary>
        /// CoachService
        /// </summary>
        CoachService = 200,
        /// <summary>
        /// InternationalCoachService
        /// </summary>
        InternationalCoachService = 201,
        /// <summary>
        /// NationalCoachService
        /// </summary>
        NationalCoachService = 202,
        /// <summary>
        /// ShuttleCoachService
        /// </summary>
        ShuttleCoachService = 203,
        /// <summary>
        /// RegionalCoachService
        /// </summary>
        RegionalCoachService = 204,
        /// <summary>
        /// SpecialCoachService
        /// </summary>
        SpecialCoachService = 205,
        /// <summary>
        /// SightseeingCoachService
        /// </summary>
        SightseeingCoachService = 206,
        /// <summary>
        /// TouristCoachService
        /// </summary>
        TouristCoachService = 207,
        /// <summary>
        /// CommuterCoachService
        /// </summary>
        CommuterCoachService = 208,
        /// <summary>
        /// AllCoachServices
        /// </summary>
        AllCoachServices = 209,
        /// <summary>
        /// SuburbanRailwayService
        /// </summary>
        SuburbanRailwayService = 300,
        /// <summary>
        /// UrbanRailwayService
        /// </summary>
        UrbanRailwayService = 400,
        /// <summary>
        /// MetroCoachService
        /// </summary>
        MetroCoachService = 401,
        /// <summary>
        /// UndergroundService
        /// </summary>
        UndergroundService = 402,
        /// <summary>
        /// UrbanRailwayServiceDefault
        /// </summary>
        UrbanRailwayServiceDefault = 403,
        /// <summary>
        /// AllUrbanRailwayServices
        /// </summary>
        AllUrbanRailwayServices = 404,
        /// <summary>
        /// MonorailExtended
        /// </summary>
        MonorailExtended = 405,
        /// <summary>
        /// MetroService
        /// </summary>
        MetroService = 500,
        /// <summary>
        /// UndergroundMetroService
        /// </summary>
        UndergroundMetroService = 600,
        /// <summary>
        /// BusService
        /// </summary>
        BusService = 700,
        /// <summary>
        /// RegionalBusService
        /// </summary>
        RegionalBusService = 701,
        /// <summary>
        /// ExpressBusService
        /// </summary>
        ExpressBusService = 702,
        /// <summary>
        /// StoppingBusService
        /// </summary>
        StoppingBusService = 703,
        /// <summary>
        /// LocalBusService
        /// </summary>
        LocalBusService = 704,
        /// <summary>
        /// NightBusService
        /// </summary>
        NightBusService = 705,
        /// <summary>
        /// PostBusService
        /// </summary>
        PostBusService = 706,
        /// <summary>
        /// SpecialNeedsBus
        /// </summary>
        SpecialNeedsBus = 707,
        /// <summary>
        /// MobilityBusService
        /// </summary>
        MobilityBusService = 708,
        /// <summary>
        /// MobilityBusforRegisteredDisabled
        /// </summary>
        MobilityBusforRegisteredDisabled = 709,
        /// <summary>
        /// SightseeingBus
        /// </summary>
        SightseeingBus = 710,
        /// <summary>
        /// ShuttleBus
        /// </summary>
        ShuttleBus = 711,
        /// <summary>
        /// SchoolBus
        /// </summary>
        SchoolBus = 712,
        /// <summary>
        /// SchoolandPublicServiceBus
        /// </summary>
        SchoolandPublicServiceBus = 713,
        /// <summary>
        /// RailReplacementBusService
        /// </summary>
        RailReplacementBusService = 714,
        /// <summary>
        /// DemandandResponseBusService
        /// </summary>
        DemandandResponseBusService = 715,
        /// <summary>
        /// AllBusServices
        /// </summary>
        AllBusServices = 716,
        /// <summary>
        /// ShareTaxiService
        /// </summary>
        ShareTaxiService = 717,
        /// <summary>
        /// TrolleybusService
        /// </summary>
        TrolleybusService = 800,
        /// <summary>
        /// TramService
        /// </summary>
        TramService = 900,
        /// <summary>
        /// CityTramService
        /// </summary>
        CityTramService = 901,
        /// <summary>
        /// LocalTramService
        /// </summary>
        LocalTramService = 902,
        /// <summary>
        /// RegionalTramService
        /// </summary>
        RegionalTramService = 903,
        /// <summary>
        /// SightseeingTramService
        /// </summary>
        SightseeingTramService = 904,
        /// <summary>
        /// ShuttleTramService
        /// </summary>
        ShuttleTramService = 905,
        /// <summary>
        /// AllTramServices
        /// </summary>
        AllTramServices = 906,
        /// <summary>
        /// WaterTransportService
        /// </summary>
        WaterTransportService = 1000,
        /// <summary>
        /// InternationalCarFerryService
        /// </summary>
        InternationalCarFerryService = 1001,
        /// <summary>
        /// NationalCarFerryService
        /// </summary>
        NationalCarFerryService = 1002,
        /// <summary>
        /// RegionalCarFerryService
        /// </summary>
        RegionalCarFerryService = 1003,
        /// <summary>
        /// LocalCarFerryService
        /// </summary>
        LocalCarFerryService = 1004,
        /// <summary>
        /// InternationalPassengerFerryService
        /// </summary>
        InternationalPassengerFerryService = 1005,
        /// <summary>
        /// NationalPassengerFerryService
        /// </summary>
        NationalPassengerFerryService = 1006,
        /// <summary>
        /// RegionalPassengerFerryService
        /// </summary>
        RegionalPassengerFerryService = 1007,
        /// <summary>
        /// LocalPassengerFerryService
        /// </summary>
        LocalPassengerFerryService = 1008,
        /// <summary>
        /// PostBoatService
        /// </summary>
        PostBoatService = 1009,
        /// <summary>
        /// TrainFerryService
        /// </summary>
        TrainFerryService = 1010,
        /// <summary>
        /// RoadLinkFerryService
        /// </summary>
        RoadLinkFerryService = 1011,
        /// <summary>
        /// AirportLinkFerryService
        /// </summary>
        AirportLinkFerryService = 1012,
        /// <summary>
        /// CarHighSpeedFerryService
        /// </summary>
        CarHighSpeedFerryService = 1013,
        /// <summary>
        /// PassengerHighSpeedFerryService
        /// </summary>
        PassengerHighSpeedFerryService = 1014,
        /// <summary>
        /// SightseeingBoatService
        /// </summary>
        SightseeingBoatService = 1015,
        /// <summary>
        /// SchoolBoat
        /// </summary>
        SchoolBoat = 1016,
        /// <summary>
        /// CableDrawnBoatService
        /// </summary>
        CableDrawnBoatService = 1017,
        /// <summary>
        /// RiverBusService
        /// </summary>
        RiverBusService = 1018,
        /// <summary>
        /// ScheduledFerryService
        /// </summary>
        ScheduledFerryService = 1019,
        /// <summary>
        /// ShuttleFerryService
        /// </summary>
        ShuttleFerryService = 1020,
        /// <summary>
        /// AllWaterTransportServices
        /// </summary>
        AllWaterTransportServices = 1021,
        /// <summary>
        /// AirService
        /// </summary>
        AirService = 1100,
        /// <summary>
        /// InternationalAirService
        /// </summary>
        InternationalAirService = 1101,
        /// <summary>
        /// DomesticAirService
        /// </summary>
        DomesticAirService = 1102,
        /// <summary>
        /// IntercontinentalAirService
        /// </summary>
        IntercontinentalAirService = 1103,
        /// <summary>
        /// DomesticScheduledAirService
        /// </summary>
        DomesticScheduledAirService = 1104,
        /// <summary>
        /// ShuttleAirService
        /// </summary>
        ShuttleAirService = 1105,
        /// <summary>
        /// IntercontinentalCharterAirService
        /// </summary>
        IntercontinentalCharterAirService = 1106,
        /// <summary>
        /// InternationalCharterAirService
        /// </summary>
        InternationalCharterAirService = 1107,
        /// <summary>
        /// RoundTripCharterAirService
        /// </summary>
        RoundTripCharterAirService = 1108,
        /// <summary>
        /// SightseeingAirService
        /// </summary>
        SightseeingAirService = 1109,
        /// <summary>
        /// HelicopterAirService
        /// </summary>
        HelicopterAirService = 1110,
        /// <summary>
        /// DomesticCharterAirService
        /// </summary>
        DomesticCharterAirService = 1111,
        /// <summary>
        /// SchengenAreaAirService
        /// </summary>
        SchengenAreaAirService = 1112,
        /// <summary>
        /// AirshipService
        /// </summary>
        AirshipService = 1113,
        /// <summary>
        /// AllAirServices
        /// </summary>
        AllAirServices = 1114,
        /// <summary>
        /// FerryService
        /// </summary>
        FerryService = 1200,
        /// <summary>
        /// TelecabinService
        /// </summary>
        TelecabinService = 1300,
        /// <summary>
        /// TelecabinServiceDefault
        /// </summary>
        TelecabinServiceDefault = 1301,
        /// <summary>
        /// TelecabinServiceDefault
        /// </summary>
        CableCarService = 1302,
        /// <summary>
        /// ElevatorService
        /// </summary>
        ElevatorService = 1303,
        /// <summary>
        /// ChairLiftService
        /// </summary>
        ChairLiftService = 1304,
        /// <summary>
        /// DragLiftService
        /// </summary>
        DragLiftService = 1305,
        /// <summary>
        /// SmallTelecabinService
        /// </summary>
        SmallTelecabinService = 1306,
        /// <summary>
        /// AllTelecabinServices
        /// </summary>
        AllTelecabinServices = 1307,
        /// <summary>
        /// FunicularService
        /// </summary>
        FunicularService = 1400,
        /// <summary>
        /// FunicularServiceDefault
        /// </summary>
        FunicularServiceDefault = 1401,
        /// <summary>
        /// AllFunicularService
        /// </summary>
        AllFunicularService = 1402,
        /// <summary>
        /// TaxiService
        /// </summary>
        TaxiService = 1500,
        /// <summary>
        /// CommunalTaxiService. Informal/Share Taxi
        /// </summary>
        CommunalTaxiService = 1501,
        /// <summary>
        /// WaterTaxiService
        /// </summary>
        WaterTaxiService = 1502,
        /// <summary>
        /// RailTaxiService
        /// </summary>
        RailTaxiService = 1503,
        /// <summary>
        /// BikeTaxiService
        /// </summary>
        BikeTaxiService = 1504,
        /// <summary>
        /// LicensedTaxiService
        /// </summary>
        LicensedTaxiService = 1505,
        /// <summary>
        /// PrivateHireServiceVehicle
        /// </summary>
        PrivateHireServiceVehicle = 1506,
        /// <summary>
        /// AllTaxiServices
        /// </summary>
        AllTaxiServices = 1507,
        /// <summary>
        /// SelfDrive
        /// </summary>
        SelfDrive = 1600,
        /// <summary>
        /// HireCar
        /// </summary>
        HireCar = 1601,
        /// <summary>
        /// HireVan
        /// </summary>
        HireVan = 1602,
        /// <summary>
        /// HireMotorbike
        /// </summary>
        HireMotorbike = 1603,
        /// <summary>
        /// HireCycle
        /// </summary>
        HireCycle = 1604,
        /// <summary>
        /// MiscellaneousService
        /// </summary>
        MiscellaneousService = 1700,
        /// <summary>
        /// CableCarExtended
        /// </summary>
        CableCarExtended = 1701,
        /// <summary>
        /// HorsedrawnCarriage
        /// </summary>
        HorsedrawnCarriage = 1702
    }

    /// <summary>
    /// Contains extension methods for the route type.
    /// </summary>
    public static class RouteTypeExtensions
    {
        /// <summary>
        /// Tries to convert the given extended type to a standard route type.
        /// </summary>
        /// <param name="extendedType">The extended route type.</param>
        /// <param name="routeType">The standard route type, if any.</param>
        /// <returns>True if a route type exists for the extended type, false otherwise.</returns>
        public static bool TryToStandard(this RouteType extendedType, out RouteType routeType)
        {
            var et = (int)extendedType;
            if (et >= 100 && et < 200)
            {
                routeType = RouteType.Rail;
                return true;
            }

            if (et >= 200 && et < 300)
            {
                routeType = RouteType.Bus;
                return true;
            }

            if (et >= 300 && et < 400)
            {
                routeType = RouteType.Rail;
                return true;
            }

            if (et >= 400 && et < 700)
            {
                routeType = RouteType.SubwayMetro;
                return true;
            }

            if (et >= 700 && et < 900)
            { // trolley bus we consider to be a bus service.
                routeType = RouteType.Bus;
                return true;
            }

            if (et >= 900 && et < 1000)
            {
                routeType = RouteType.Tram;
                return true;
            }

            if (extendedType == RouteType.FunicularService ||
                extendedType == RouteType.FunicularServiceDefault)
            {
                routeType = RouteType.Funicular;
                return true;
            }

            if (extendedType == RouteType.WaterTransportService ||
                extendedType == RouteType.FerryService)
            {
                routeType = RouteType.Ferry;
                return true;
            }

            routeType = default;
            return false;
        }
    }
}