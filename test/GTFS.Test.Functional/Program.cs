using GTFS.IO;
using System;
using System.IO;
using GTFS.IO.Compression;

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

            // read from archive.
            var reader = new GTFSReader<GTFSFeed>();
            var feed = reader.Read("gtfs1.zip");

            // write to folder.
            var path = "output";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var writer = new GTFSWriter<GTFSFeed>();
            writer.Write(feed, path);
            
            // read from folder.
            feed = reader.Read(path);
        }
    }
}
