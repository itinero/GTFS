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

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace GTFS.Entities.Collections
{
    /// <summary>
    /// A collection of GTFS-entities that can be identified by an ID but one may represent multiple objects based on a list.
    /// </summary>
    public class EntityListCollection<T> : IEntityCollection<T>
        where T : GTFSEntity
    {
        /// <summary>
        /// Holds the list containing all stops.
        /// </summary>
        private List<T> _entities;

        /// <summary>
        /// Holds the function to match ID's.
        /// </summary>
        private Func<T, string, bool> _hasId;

        /// <summary>
        /// Creates a unique entity collection based on a list.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="hasId"></param>
        public EntityListCollection(List<T> entities, Func<T, string, bool> hasId)
        {
            _entities = entities;
            _hasId = hasId;
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Adds a range of entities
        /// </summary>
        /// <returns></returns>
        public void AddRange(IEntityCollection<T> entities)
        {
            _entities.AddRange(entities);
        }

        /// <summary>
        /// Returns all entities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Get()
        {
            return _entities;
        }

        /// <summary>
        /// Returns all entities.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public IEnumerable<T> Get(string entityId)
        {
            return _entities.Where((x) =>
            {
                return _hasId(x, entityId);
            });
        }

        /// <summary>
        /// Returns the entities for the given id's.
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public IEnumerable<T> Get(List<string> entityIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all entity ids
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetIds()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the entity with the given id.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public bool Remove(string entityId)
        {
            for (int idx = 0; idx < _entities.Count; idx++)
            {
                var stop = _entities[idx];
                if (_hasId(stop, entityId))
                {
                    _entities.RemoveAt(idx);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes all entities
        /// </summary>
        /// <returns></returns>
        public void RemoveAll()
        {
            _entities = new List<T>();
        }

        /// <summary>
        /// Removes a range of entities by their IDs
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public void RemoveRange(IEnumerable<string> entityIds)
        {
            foreach (var id in entityIds)
            {
                Remove(id);
            };
        }

        /// <summary>
        /// Returns an enumerator that iterates through the entities.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the entities.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }
    }
}