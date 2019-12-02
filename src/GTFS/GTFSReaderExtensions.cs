using System;
using System.Collections.Generic;
using System.IO;
using GTFS.DB;
using GTFS.IO;
using GTFS.IO.Compression;

namespace GTFS
{
    /// <summary>
    /// Contains extension methods for the GTFS reader.
    /// </summary>
    public static class GTFSReaderExtensions
    {
        /// <summary>
        /// Reads a GTFS feed from the given source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Read<T>(this GTFSReader<T> reader, IEnumerable<IGTFSSourceFile> source)
            where T : IGTFSFeed, new()
        {
            return reader.Read(new T(), source);
        }

        /// <summary>
        /// Reads a GTFS feed from the given source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="source"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static T Read<T>(this GTFSReader<T> reader, IEnumerable<IGTFSSourceFile> source, IGTFSSourceFile file)
            where T : IGTFSFeed, new()
        {
            return reader.Read(new T(), source, file);
        }

        /// <summary>
        /// Reads a GTFS feed directly into a GTFS feed db.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="db"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int Read(this GTFSReader<IGTFSFeed> reader, IGTFSFeedDB db, IEnumerable<IGTFSSourceFile> source)
        {
            int newFeed = db.AddFeed();
            reader.Read(db.GetFeed(newFeed), source);
            return newFeed;
        }

        /// <summary>
        /// Reads a GTFS feed.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="path">The path, this has to be either a folder or a zip archive.</param>
        /// <param name="separator">A custom separator.</param>
        /// <typeparam name="T">The GTFS feed type.</typeparam>
        /// <returns>The GTFS feed.</returns>
        public static T Read<T>(this GTFSReader<T> reader, string path, char? separator = null) where T : IGTFSFeed, new()
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (Directory.Exists(path))
            {
                using (var source = new GTFSDirectorySource(path, separator))
                {
                    return reader.Read<T>(source);
                }
            }
            else if (File.Exists(path) && path.ToLower().EndsWith(".zip"))
            {
                using (var source = new GTFSArchiveSource(File.OpenRead(path), separator))
                {
                    return reader.Read<T>(source);
                }
            }
            
            throw new ArgumentException("Could not open GTFS feed, directory or archive not found.", nameof(path));
        }
    }
}