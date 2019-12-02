using System;

namespace GTFS.Exceptions
{
    /// <summary>
    /// Abstract base class for all GTFS exceptions.
    /// </summary>
    public abstract class GTFSExceptionBase : Exception
    {
        /// <summary>
        /// Creates a new GTFS exception.
        /// </summary>
        /// <param name="message">The message.</param>
        protected GTFSExceptionBase(string message)
            : base(message)
        {
        }
        
        /// <summary>
        /// Creates a new GTFS exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        protected GTFSExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}