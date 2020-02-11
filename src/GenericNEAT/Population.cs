using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericNEAT
{
    public sealed class Population<T>
    {
        readonly List<Specie<T>> _species;

        #region Properties
        /// <summary>
        /// Function used for determining distance between items.
        /// </summary>
        public DistanceFunction<T> DistanceFunc { get; }

        /// <summary>
        /// The maximum distance between a centroid and any member
        /// of its specie.
        /// </summary>
        public double SpecieRadius { get; }

        /// <summary>
        /// The number of unique species in the population.
        /// </summary>
        public int SpecieCount => _species.Count;

        /// <summary>
        /// Total number of items in the population, regardless of specie.
        /// </summary>
        public int ItemCount => _species.Select(s => s.Count).Sum();

        public int Count => throw new NotImplementedException();

        public IEnumerable<T> this[uint key] => throw new NotImplementedException();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an empty population with the specified distance function
        /// and radius.
        /// </summary>
        public Population(DistanceFunction<T> distanceFunc, double specieRadius)
        {
            if (distanceFunc is null)
                throw new ArgumentNullException(nameof(distanceFunc));
            if (specieRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(specieRadius));
            DistanceFunc = distanceFunc;
            _species = new List<Specie<T>>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a new specie to the population. This method returns false
        /// if the specie with the same ID already exists.
        /// </summary>
        public bool AddSpecie(uint id, T centroid)
        {
            if (_species.Any(s => s.ID == id))
                return false;
            _species.Add(new Specie<T>(id, centroid));
            return true;
        }

        /// <summary>
        /// Adds an item to the first specie to which it fits. This
        /// method returns false if the item does not fit any specie.
        /// </summary>
        /// <returns>True if the item was added, false otherwise.</returns>
        public bool AddItem(T item)
        {
            for (int i = 0; i < _species.Count; i++)
                if (DistanceFunc(item, _species[i].Centroid) < SpecieRadius)
                {
                    _species[i].Add(item);
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Tests if the population contains a specie with the given ID.
        /// </summary>
        public bool ContainsSpecie(uint id) => _species.Any(s => s.ID == id);

        /// <summary>
        /// Tests if the population contains a particular item.
        /// </summary>
        public bool ContainsItem(T item) => _species.Any(s => s.Contains(item));
        #endregion
    }
}
