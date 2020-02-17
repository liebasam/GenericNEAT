using LiebasamUtils.Collections;
using System.Collections.Generic;

namespace GenericNEAT.Operators
{
    public sealed class IDFactory
    {
        public uint NextID { get; private set; }
        readonly SortedDictionary<ulong, uint> idCache;

        /// <summary>
        /// Gets the ID of a vertex placed on the given edge.
        /// </summary>
        public uint GetID(uint idFrom, uint idTo)
        {
            ulong id = Edge.ZipIDs(idFrom, idTo);
            if (idCache.ContainsKey(id))
                return idCache[id];
            return NextID++;
        }

        /// <summary>
        /// Clears the memory of the factory for the next
        /// generation of mutations.
        /// </summary>
        public void ClearCacheAndIncrementNextID()
        {
            idCache.Clear();
            NextID++;
        }
    }
}
