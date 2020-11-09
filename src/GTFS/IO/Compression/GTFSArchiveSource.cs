using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using GTFS.IO;
using GTFS.IO.CSV;

namespace GTFS.IO.Compression
{
    /// <summary>
    /// A GTFS archive source.
    /// </summary>
    public class GTFSArchiveSource : IEnumerable<IGTFSSourceFile>, IDisposable
    {
        private readonly System.IO.Compression.ZipArchive _archive;
        
        private char? _customSeparator;
        private List<IGTFSSourceFile> _sourceFiles;

        /// <summary>
        /// Creates a new source.
        /// </summary>
        /// <param name="stream">The stream to the archive.</param>
        /// <param name="separator">A custom separator if any.</param>
        public GTFSArchiveSource(Stream stream, char? separator = null)
        {
            _archive = new ZipArchive(stream);
            _customSeparator = separator;
        }
        
        /// <summary>
        /// Builds a list of source files;
        /// </summary>
        /// <returns></returns>
        private void BuildSource()
        {
            if (_sourceFiles != null)
            {
                foreach (var sourceFile in _sourceFiles)
                {
                    sourceFile.Dispose();
                }
            }
            
            var entries = _archive.Entries;
            _sourceFiles = new List<IGTFSSourceFile>(10);

            foreach (var entry in entries)
            {
                if (!entry.Name.ToLower().EndsWith(".txt")) continue;
                
                var nameWithoutExtension = Path.GetFileNameWithoutExtension(entry.Name);
                
                _sourceFiles.Add(_customSeparator.HasValue
                    ? new GTFSSourceFileStream(entry.Open(), nameWithoutExtension, _customSeparator.Value)
                    : new GTFSSourceFileStream(entry.Open(), nameWithoutExtension));
            }
        }

        /// <summary>
        /// Returns the enumerator for this IEnumerable.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IGTFSSourceFile> GetEnumerator()
        {
            this.BuildSource();
            return _sourceFiles.GetEnumerator();
        }

        /// <summary>
        /// Returns the enumerator for this IEnumerable.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            this.BuildSource();
            return _sourceFiles.GetEnumerator();
        }

        /// <summary>
        /// Disposes of all native resources associated with this source.
        /// </summary>
        public void Dispose()
        {
            if (_sourceFiles == null) return;
            
            foreach (var sourceFile in _sourceFiles)
            {
                sourceFile.Dispose();
            }

            _sourceFiles = null;
            _archive.Dispose();
        }
    }
}