using System;

namespace GTFS.Exceptions
{
    /// <summary>
    /// Abstract base class for all GTFS exceptions.
    /// </summary>
    public abstract class GTFSExceptionBase : Exception
    {
        protected GTFSExceptionBase(string message)
            : base(message)
        {
        }

        protected GTFSExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}