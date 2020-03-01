using LiebasamUtils.Collections;
using System.Collections.Generic;

namespace GenericNEAT.Operators
{
    public sealed class IDFactory
    {
        public uint NextID { get; private set; }
        readonly SortedDictionary<ulong, uint> edgeIDCache = new SortedDictionary<ulong, uint>();
        readonly SortedDictionary<uint, uint> vertexIDCache = new SortedDictionary<uint, uint>();

        public IDFactory() { }
        public IDFactory(uint nextID) { NextID = nextID; }

        #region Methods
        /// <summary>
        /// Gets the ID of a vertex placed on the given edge.
        /// </summary>
        public uint GetID(uint idFrom, uint idTo)
        {
            ulong id = Edge.ZipIDs(idFrom, idTo);
            lock (edgeIDCache)
            {
                if (edgeIDCache.ContainsKey(id))
                    return edgeIDCache[id];
                else
                    edgeIDCache.Add(id, NextID);
                return NextID++;
            }
        }

        /// <summary>
        /// Gets the ID of a vertex sprouted from the given vertex ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public uint GetID(uint id)
        {
            lock (vertexIDCache)
            {
                if (vertexIDCache.ContainsKey(id))
                    return vertexIDCache[id];
                else
                    vertexIDCache.Add(id, NextID);
                return NextID++;
            }
        }

        /// <summary>
        /// Clears the memory of the factory for the next
        /// generation of mutations.
        /// </summary>
        public void ClearCacheAndIncrementNextID()
        {
            lock (edgeIDCache) { edgeIDCache.Clear(); }
            lock (vertexIDCache) { vertexIDCache.Clear(); }
        }
        #endregion
    }
}
