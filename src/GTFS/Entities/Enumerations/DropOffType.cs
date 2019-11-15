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

namespace GTFS.Entities.Enumerations
{
    /// <summary>
    /// Indicates whether passengers are dropped off at a stop as part of the normal schedule or whether a drop off at the stop is not available. This field also allows the transit agency to indicate that passengers must call the agency or notify the driver to arrange a drop off at a particular stop. Valid values for this field are:
    /// </summary>
    public enum DropOffType
    {
        /// <summary>
        /// Regularly scheduled drop off.
        /// </summary>
        Regular = 0,
        /// <summary>
        /// No drop off available.
        /// </summary>
        NoPickup = 1,
        /// <summary>
        /// Must phone agency to arrange drop off.
        /// </summary>
        PhoneForPickup = 2, // TODO: rename this enum to 'PhoneForDropOff'. it is a breaking change though
        /// <summary>
        /// Must coordinate with driver to arrange drop off.
        /// </summary>
        DriverForPickup = 3 // TODO: rename this enum to 'DriverForDropOff'. it is a breaking change though
    }
}