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

using GTFS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTFS.DB
{
    /// <summary>
    /// An abstract representation of a GTFS feed db.
    /// </summary>
    public interface IGTFSFeedDB
    {
        /// <summary>
        /// Adds a new feed to this db.
        /// </summary>
        /// <returns></returns>
        int AddFeed();

        /// <summary>
        /// Adds an existing GTFS feed to this db.
        /// </summary>
        /// <param name="feed">The feed to add.</param>
        /// <returns>The id of the new feed.</returns>
        int AddFeed(IGTFSFeed feed);

        /// <summary>
        /// Removes the feed with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveFeed(int id);

        /// <summary>
        /// Returns all feeds.
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> GetFeeds();

        /// <summary>
        /// Returns the feed with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IGTFSFeed GetFeed(int id);
    }
}