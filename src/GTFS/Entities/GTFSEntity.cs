// The MIT License (MIT)

// Copyright (c) 2014 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace GTFS.Entities
{
    /// <summary>
    /// Event handler delegate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GTFSEntityChangedEventHandler(object sender, GTFSEntityChangedEventArgs e);

    /// <summary>
    /// Represents a base-class for all GTFS entities.
    /// </summary>
    public abstract class GTFSEntity
    {
        /// <summary>
        /// Gets or sets a tag.
        /// </summary>
        /// <remarks>Can be used to attach extra information.</remarks>
        public object Tag { get; set; }

        /// <summary>
        /// Entity changed event.
        /// </summary>
        public event GTFSEntityChangedEventHandler EntityChanged;

        internal void OnEntityChanged()
        {
            if (EntityChanged != null)
            {
                if (!(this is Calendar) && !(this is CalendarDate))
                {
                    throw new NotImplementedException($"Entity change events are only implemented for Calendar and CalendarDate at this time.");
                }
                EntityChanged(this, GTFSEntityChangedEventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Event arguments.
    /// </summary>
    public class GTFSEntityChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns an empty set of args.
        /// </summary>
        public new static GTFSEntityChangedEventArgs Empty => new GTFSEntityChangedEventArgs();
    }
}