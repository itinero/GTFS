using System;
using System.IO;
using GTFS.IO;

namespace GTFS
{
    /// <summary>
    /// Contains extension methods for the GTFS writer.
    /// </summary>
    public static class GTFSWriterExtensions
    {
        /// <summary>
        /// Writes a GTFS feed.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="feed">The feed.</param>
        /// <param name="path">The path.</param>
        /// <typeparam name="T">The feed type.</typeparam>
        public static void Write<T>(this GTFSWriter<T> writer, T feed, string path) where T : IGTFSFeed, new()
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (Directory.Exists(path))
            {
                var target = new GTFSDirectoryTarget(new DirectoryInfo(path));
                writer.Write(feed, target);
                return;
            }
            
            throw new ArgumentException("Could not write GTFS feed, directory not found.", nameof(path));
        }
    }
}