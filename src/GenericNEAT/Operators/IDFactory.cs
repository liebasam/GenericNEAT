using LiebasamUtils.Collections;
using System.Collections.Generic;

namespace GenericNEAT.Operators
{
    public sealed class IDFactory
    {
        public uint NextID { get; private set; }
        readonly SortedDictionary<ulong, uint> idCache = new SortedDictionary<ulong, uint>();

        public IDFactory() { }
        public IDFactory(uint nextID) { NextID = nextID; }

        /// <summary>
        /// Gets the ID of a vertex placed on the given edge.
        /// </summary>
        public uint GetID(uint idFrom, uint idTo)
        {
            ulong id = Edge.ZipIDs(idFrom, idTo);
            lock (idCache)
            {
                if (idCache.ContainsKey(id))
                    return idCache[id];
                else
                    idCache.Add(id, NextID);
                return NextID++;
            }
        }

        /// <summary>
        /// Clears the memory of the factory for the next
        /// generation of mutations.
        /// </summary>
        public void ClearCacheAndIncrementNextID()
        {
            lock (idCache) { idCache.Clear(); }
        }
    }
}
