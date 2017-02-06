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
    /// Type of accessibility for wheelchair boardings are possible from the specified stop or station.
    /// </summary>
    public enum WheelchairAccessibilityType
    {
        /// <summary>
        /// Indicates that there is no accessibility information
        /// </summary>
        NoInformation,
        /// <summary>
        /// Indicates that at least some vehicles at this stop can be boarded by a rider in a wheelchair or there exists some accessible path from outside the station to the specific stop/platform.
        /// </summary>
        SomeAccessibility,
        /// <summary>
        /// Wheelchair boarding is not possible at this stop or there exists no accessible path from outside the station to the specific stop/platform.
        /// </summary>
        NoAccessibility
    }
}