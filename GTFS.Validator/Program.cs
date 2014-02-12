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

using GTFS.IO;
using System;
using System.IO;

namespace GTFS.Validator
{
    /// <summary>
    /// Contains the entry point of the application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(args.Length == 1)
            { // path or argument provided, check if directory exists.
                var directory = new DirectoryInfo(args[0]);
                if(directory.Exists)
                { // ok directory exists, try to parse GTFS.
                    try
                    {
                        var directorySource = new GTFSDirectorySource(directory);

                        // instantiate parser with default feed.
                        var parser = new GTFSReader<GTFSFeed>();
                        var feed = parser.Read(directorySource);

                        // report the result.
                        Console.WriteLine("Parsed {0} Agencies", feed.Agencies.Count);
                        Console.WriteLine("Parsed {0} CalendarDates", feed.CalendarDates.Count);
                        Console.WriteLine("Parsed {0} Calendars", feed.Calendars.Count);
                        Console.WriteLine("Parsed {0} FareAttributes", feed.FareAttributes.Count);
                        Console.WriteLine("Parsed {0} FareRules", feed.FareRules.Count);
                        Console.WriteLine("Parsed {0} FeedInfo", feed.FeedInfo.Count);
                        Console.WriteLine("Parsed {0} Frequencies", feed.Frequencies.Count);
                        Console.WriteLine("Parsed {0} Routes", feed.Routes.Count);
                        Console.WriteLine("Parsed {0} Shapes", feed.Shapes.Count);
                        Console.WriteLine("Parsed {0} Stops", feed.Stops.Count);
                        Console.WriteLine("Parsed {0} StopTimes", feed.StopTimes.Count);
                        Console.WriteLine("Parsed {0} Transfers", feed.Transfers.Count);
                        Console.WriteLine("Parsed {0} Trips", feed.Trips.Count);

                        Console.WriteLine(string.Empty);
                        Console.WriteLine("GTFS Feed in directory {0} was parsed successfully!",
                            directory);
                        Console.ReadLine();
                        return;
                    }
                    catch(Exception ex)
                    { // report on the error that occurred.
                        Console.WriteLine("GTFS Feed in directory {0} could not be parsed: {1}",
                            directory, ex.ToString());
                        Console.ReadLine();
                        return;
                    }
                }
                Console.WriteLine("GTFS feed directory not found!");
                Console.WriteLine("Usage: GTFS.Validator.exe path/to/gtfs");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("GTFS feed directory not provided!");
            Console.WriteLine("Usage: GTFS.Validator.exe path/to/gtfs");
            Console.ReadLine();
        }
    }
}