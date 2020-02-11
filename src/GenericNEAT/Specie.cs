using System;
using System.Collections;
using System.Collections.Generic;

namespace GenericNEAT
{
    /// <summary>
    /// Collection of items with a centroid and unique ID.
    /// </summary>
    public sealed class Specie<T> : ICollection<T>, ICloneable
    {
        readonly List<T> _items;

        /// <summary>
        /// Center of the specie. Is not necessarily a member
        /// of the collection.
        /// </summary>
        public T Centroid { get; }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public uint ID { get; }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        #region Constructors
        /// <summary>
        /// Creates an empty specie.
        /// </summary>
        public Specie(uint id, T centroid)
        {
            ID = id;
            Centroid = centroid;
        }

        /// <summary>
        /// Creates a pre-populated specie.
        /// </summary>
        public Specie(uint id, T centroid, IEnumerable<T> collection)
        {
            ID = id;
            Centroid = centroid;
            _items = new List<T>(collection);
        }
        #endregion

        #region Collection Methods
        public void Add(T item) => _items.Add(item);

        public void Clear() => _items.Clear();

        public bool Contains(T item) => _items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public bool Remove(T item) => _items.Remove(item);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
        #endregion

        #region Methods
        public object Clone() => new Specie<T>(ID, Centroid, _items);
        #endregion
    }
}
