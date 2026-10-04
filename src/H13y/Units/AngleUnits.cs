namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Angle units. Base: radian (rad).
    /// </summary>
    /// <remarks>
    /// Auto unit selection only considers degrees, so angles always render the way
    /// people read them. Radians, gradians, and turns remain available through the
    /// parser and <see cref="H.Convert"/>.
    /// </remarks>
    public static class Angle
    {
        /// <summary>Radian — the base unit of angle.</summary>
        public static readonly Unit Radian = new("rad", 1, Dimension.Angle);

        /// <summary>Degree — π/180 radians.</summary>
        public static readonly Unit Degree = new("deg", Math.PI / 180, Dimension.Angle);

        /// <summary>Gradian — π/200 radians (400 grads per full circle).</summary>
        public static readonly Unit Gradian = new("grad", Math.PI / 200, Dimension.Angle);

        /// <summary>Turn — 2π radians (one full rotation).</summary>
        public static readonly Unit Turn = new("turn", 2 * Math.PI, Dimension.Angle);

        internal static readonly Unit[] All = [ Degree ];
    }
}
