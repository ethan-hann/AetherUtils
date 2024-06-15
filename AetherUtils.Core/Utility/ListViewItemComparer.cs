using System.Collections;

namespace AetherUtils.Core.Utility;

/// <summary>
/// Compares items within the same column in a <see cref="ListView"/>.
/// </summary>
public sealed class ListViewItemComparer : IComparer
{
    /// <summary>
    /// The column index that specifies which column in the list view contains the items being compared.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// The order items should be compared in (ascending/descending).
    /// </summary>
    public SortOrder Order { get; }

    /// <summary>
    /// Create a new item comparer for the specified column using the specified sort order.
    /// </summary>
    /// <param name="column">The column index.</param>
    /// <param name="order">The <see cref="SortOrder"/> used for ordering.</param>
    public ListViewItemComparer(int column, SortOrder order)
    {
        Order = order;
        Column = column;
    }
    
    /// <inheritdoc />
    public int Compare(object? x, object? y)
    {
        var returnVal = -1;

        if (x is ListViewItem item1 && y is ListViewItem item2)
        {
            returnVal = string.Compare(item1.SubItems[Column].Text, item2.SubItems[Column].Text,
                StringComparison.OrdinalIgnoreCase);
        }

        //Determine whether the sort order is descending
        if (Order == SortOrder.Descending)
            returnVal *= -1;
        return returnVal;
    }
}