namespace H13y;

public static partial class Units
{
    /// <summary>Mass units. Base: gram.</summary>
    public static class Mass
    {
        public static readonly Unit Milligram = new("mg", 0.001, Dimension.Mass);
        public static readonly Unit Gram = new("g", 1, Dimension.Mass);
        public static readonly Unit Kilogram = new("kg", 1000, Dimension.Mass);
        public static readonly Unit Tonne = new("t", 1_000_000, Dimension.Mass);

        internal static readonly Unit[] All =
        [
            Milligram, Gram, Kilogram, Tonne,
        ];
    }
}
