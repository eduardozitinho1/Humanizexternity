namespace H13y;

public static partial class Units
{
    /// <summary>Length units. Base: meter.</summary>
    public static class Length
    {
        public static readonly Unit Millimeter = new("mm", 0.001, Dimension.Length);
        public static readonly Unit Centimeter = new("cm", 0.01, Dimension.Length);
        public static readonly Unit Meter = new("m", 1, Dimension.Length);
        public static readonly Unit Kilometer = new("km", 1000, Dimension.Length);

        internal static readonly Unit[] All =
        [
            Millimeter, Centimeter, Meter, Kilometer,
        ];
    }
}
