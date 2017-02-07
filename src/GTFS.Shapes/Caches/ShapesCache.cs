// The MIT License (MIT)

// Copyright (c) 2017 Ben Abelshausen

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

using Itinero.Algorithms.Collections;
using Itinero.Graphs.Geometric.Shapes;
using Itinero.LocalGeo;
using System.Collections.Generic;

namespace GTFS.Shapes.Caches
{
    /// <summary>
    /// A cache of shapes index by generic objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ShapesCache<T>
    {
        private readonly HugeDictionary<T, long> _shapeMap;
        private readonly ShapesArray _shapeArray;
        private readonly int ShapeArrayBlock = 1000;

        /// <summary>
        /// Creates a new cache.
        /// </summary>
        public ShapesCache()
        {
            _shapeMap = new HugeDictionary<T, long>();
            _shapeArray = new Itinero.Graphs.Geometric.Shapes.ShapesArray(ShapeArrayBlock);
        }

        private long _nextShapeArrayId = 0;

        /// <summary>
        /// Adds a shape.
        /// </summary>
        public void Add(T key, IEnumerable<Coordinate> shape)
        {
            if (_nextShapeArrayId >= _shapeArray.Length)
            {
                _shapeArray.Resize(_shapeArray.Length + ShapeArrayBlock);
            }
            _shapeArray[_nextShapeArrayId] = new Itinero.Graphs.Geometric.Shapes.ShapeEnumerable(shape);
            _shapeMap[key] = _nextShapeArrayId;
            _nextShapeArrayId++;
        }

        /// <summary>
        /// Tries to get a shape for the given key.
        /// </summary>
        public bool TryGet(T key, out IEnumerable<Coordinate> shape)
        {
            long idx;
            if (_shapeMap.TryGetValue(key, out idx))
            {
                shape = _shapeArray[idx];
                return true;
            }
            shape = null;
            return false;
        }

        /// <summary>
        /// Gets the shapes array.
        /// </summary>
        public ShapesArray ShapesArray
        {
            get
            {
                return _shapeArray;
            }
        }

        /// <summary>
        /// Gets the number of shapes.
        /// </summary>
        public long ShapeCount
        {
            get
            {
                return _nextShapeArrayId;
            }
        }
    }
}