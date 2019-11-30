using GTFS.IO;
using System;
using System.IO;

namespace GTFS.Test.Functional
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            // enable logging.
            GTFS.Logging.Logger.LogAction = (o, level, message, parameter) =>
            {
                Console.WriteLine($"[{o}] {level} - {message}");
            };
            
            var reader = new GTFSReader<GTFSFeed>();
            using (var sources = new GTFSDirectorySource(new DirectoryInfo("/data/work/data/GTFS/de_lijn-gtfs")))
            {
                var feed = reader.Read(sources);
                
                var targets = new GTFSDirectoryTarget(new DirectoryInfo(@"de-lijn-copy"));
                var writer = new GTFSWriter<GTFSFeed>();
                writer.Write(feed, targets);
            }
        }
    }
}
