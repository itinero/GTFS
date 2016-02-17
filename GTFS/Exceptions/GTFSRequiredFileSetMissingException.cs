using System.Collections.Generic;

namespace GTFS.Exceptions
{
    /// <summary>
    /// Exception thrown when none of the files in a required file set were found.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class GTFSRequiredFileSetMissingException : GTFSExceptionBase
    {
        /// <summary>
        /// Message format used for formatting the exception.
        /// </summary>
        public static readonly string MessageFormat = "Could not find any of the following required files: {0}.";

        /// <summary>
        /// Creates a missing file set exception.
        /// </summary>
        /// <param name="names">The file names from the required file set</param>
        public GTFSRequiredFileSetMissingException(string[] names)
            : base(string.Format(MessageFormat, string.Join(",", names)))
        {
            Names = names;
        }

        /// <summary>
        /// Returns the names of the files.
        /// </summary>
        public IEnumerable<string> Names { get; private set; }
    }
}