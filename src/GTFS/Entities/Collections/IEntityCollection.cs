// The MIT License (MIT)

// Copyright (c) 2015 Ben Abelshausen

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

using System.Collections.Generic;

namespace GTFS.Entities.Collections
{
    /// <summary>
    /// Abstract representation of a collection of GTFS-entities that can be uniquely identified by an ID and of a single type.
    /// </summary>
    public interface IEntityCollection<T> : IEnumerable<T>
        where T : GTFSEntity
    {
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Adds range of entities
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEntityCollection<T> entities);

        /// <summary>
        /// Returns all entities.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Get();

        /// <summary>
        /// Returns all entities for the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        IEnumerable<T> Get(string entityId);

        /// <summary>
        /// Returns the entities for the given id's.
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        IEnumerable<T> Get(List<string> entityIds);

        /// <summary>
        /// Returns all entity ids.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetIds();

        /// <summary>
        /// Removes all entities identified by the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        bool Remove(string entityId);

        /// <summary>
        /// Removes a range of entities by their IDs
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        void RemoveRange(IEnumerable<string> entityIds);

        /// <summary>
        /// Removes all entities
        /// </summary>
        /// <returns></returns>
        void RemoveAll();
    }
}