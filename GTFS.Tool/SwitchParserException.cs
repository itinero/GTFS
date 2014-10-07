using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFS.Tool
{
    /// <summary>
    /// A switch parser exception: thrown when switches cannot be parsed properly.
    /// </summary>
    public class SwitchParserException : Exception
    {
        /// <summary>
        /// Holds the switch id.
        /// </summary>
        private readonly string _switchId;

        /// <summary>
        /// Creates a new switch parser exception.
        /// </summary>
        /// <param name="switchId"></param>
        /// <param name="message"></param>
        public SwitchParserException(string switchId, string message)
            : base(message)
        {
            _switchId = switchId;
        }
    }
}
