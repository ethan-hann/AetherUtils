namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating <see cref="Size"/> objects.
/// </summary>
public static class SizeExtensions
{
    /// <summary>
    /// Get the center <see cref="Point"/> for the specified <see cref="Size"/>.
    /// </summary>
    /// <param name="size">The <see cref="Size"/> to get the center of.</param>
    /// <returns>A <see cref="Point"/> representing the center point of the <see cref="Size"/>.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="size"/> was <c>null</c>.</exception>
    public static Point GetCenterPoint(this Size size)
    {
        ArgumentNullException.ThrowIfNull(size, nameof(size));
        
        return new Point(size.Width / 2, size.Height / 2);
    }

    /// <summary>
    /// Get the center <see cref="PointF"/> for the specified <see cref="SizeF"/>.
    /// </summary>
    /// <param name="size">The <see cref="SizeF"/> to get the center of.</param>
    /// <returns>A <see cref="PointF"/> representing the center point of the <see cref="SizeF"/>.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="size"/> was <c>null</c>.</exception>
    public static PointF GetCenterPointF(this SizeF size)
    {
        ArgumentNullException.ThrowIfNull(size, nameof(size));
        
        return new PointF(size.Width / 2.0f, size.Height / 2.0f);
    }
}