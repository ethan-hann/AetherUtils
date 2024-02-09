namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating various time related objects.
/// </summary>
public static class TimeExtensions
{
    /// <summary>
    /// Get the starting <see cref="DateTime"/> of the week based on the supplied <see cref="DateTime"/>.
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> to get the start of the week of.</param>
    /// <returns>A <see cref="DateTime"/> representing the start of the week.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="date"/> was <c>null</c>.</exception>
    public static DateTime StartOfWeek(this DateTime date)
    {
        ArgumentNullException.ThrowIfNull(date, nameof(date));
        
        var dt = date;
        while (dt.DayOfWeek != Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
            dt = dt.AddDays(-1);
        return dt;
    }

    /// <summary>
    /// Get the starting <see cref="DateTime"/> of the month based on the supplied <see cref="DateTime"/>.
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> to get the start of the month of.</param>
    /// <returns>A <see cref="DateTime"/> representing the start of the month.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="date"/> was <c>null</c>.</exception>
    public static DateTime StartOfMonth(this DateTime date)
    {
        ArgumentNullException.ThrowIfNull(date, nameof(date));
        return date.AddDays(1 - date.Day);
    }

    /// <summary>
    /// Get the ending <see cref="DateTime"/> of the month based on the supplied <see cref="DateTime"/>.
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> representing the current date for the month.</param>
    /// <returns>A <see cref="DateTime"/> representing the end of the month.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="date"/> was <c>null</c>.</exception>
    public static DateTime EndOfMonth(this DateTime date)
    {
        ArgumentNullException.ThrowIfNull(date, nameof(date));
        return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }
    
    /// <summary>
    /// Get a <see cref="string"/> representing the <see cref="TimeSpan"/> in the form of:<br/>
    /// <para><c>w day(s), x hour(s), y minute(s), z second(s)</c></para>
    /// </summary>
    /// <param name="timeSpan">The <see cref="TimeSpan"/> to convert.</param>
    /// <returns>A <see cref="string"/> representing the <see cref="TimeSpan"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="timeSpan"/> was <c>null</c>.</exception>
    public static string TimeSpanToString(this TimeSpan timeSpan)
    {
        ArgumentNullException.ThrowIfNull(timeSpan, nameof(timeSpan));
        
        var components = new List<Tuple<int, string>>
        {
            Tuple.Create((int)timeSpan.TotalDays, "day"),
            Tuple.Create(timeSpan.Hours, "hour"),
            Tuple.Create(timeSpan.Minutes, "minute"),
            Tuple.Create(timeSpan.Seconds, "second")
        };

        while (components.Count != 0 && components[0].Item1 == 0)
            components.RemoveAt(0);

        return string.Join(", ",
            components.Select(t => $"{t.Item1} {t.Item2} {(t.Item1 != 1 ? "s" : string.Empty)}"));
    }
}