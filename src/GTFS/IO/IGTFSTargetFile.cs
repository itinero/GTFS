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

namespace GTFS.IO
{
    /// <summary>
    /// Abstract representation of a writeable GTFS file.
    /// </summary>
    public interface IGTFSTargetFile
    {
        /// <summary>
        /// Returns the name of the file (ex: agency, route, shape, ...).
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns true if the file already exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Clears all data in this file.
        /// </summary>
        void Clear();

        /// <summary>
        /// Writes a line of data to this file.
        /// </summary>
        /// <param name="data"></param>
        void Write(string[] data);

        /// <summary>
        /// Closes this file.
        /// </summary>
        void Close();
    }
}
