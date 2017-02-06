using GTFS.IO.CSV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GTFS.IO
{
    /// <summary>
    /// Represents a GTFS file target.
    /// </summary>
    public class GTFSTargetFileStream : IGTFSTargetFile
    {
        /// <summary>
        /// Holds the name.
        /// </summary>
        private string _name;

        /// <summary>
        /// Holds the CSV stream writer.
        /// </summary>
        private CSVStreamWriter _streamWriter;

        /// <summary>
        /// Creates a new target file stream.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        public GTFSTargetFileStream(Stream stream, string name)
        {
            _name = name;

            _streamWriter = new CSVStreamWriter(stream);
        }

        /// <summary>
        /// Creates a new target file stream.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <param name="seperator"></param>
        public GTFSTargetFileStream(Stream stream, string name, char seperator)
        {
            _name = name;

            _streamWriter = new CSVStreamWriter(stream, seperator);
        }

        /// <summary>
        /// Returns the name of this target.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns true if this file exists.
        /// </summary>
        public bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Clears all content.
        /// </summary>
        public void Clear()
        {

        }

        /// <summary>
        /// Writes another line of data.
        /// </summary>
        /// <param name="data"></param>
        public void Write(string[] data)
        {
            _streamWriter.Write(data);
        }

        /// <summary>
        /// Closes this target.
        /// </summary>
        public void Close()
        {
            _streamWriter.Flush();
            _streamWriter.Dispose();
        }
    }
}
