namespace GenericNEAT
{
    /// <summary>
    /// Delegate function for determining the 'distance' between
    /// two items. A value of 0 denotes the items are identical.
    /// </summary>
    /// <returns>Number greater than or equal to 0.</returns>
    public delegate double DistanceFunction<T>(T a, T b);
}
