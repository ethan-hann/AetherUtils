// ListViewItemComparer.cs : AetherUtils
// Copyright (C) 2025  Ethan Hann
// 
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections;
using JetBrains.Annotations;

namespace AetherUtils.Core.Utility;

/// <summary>
///     Compares items within the same column in a <see cref="ListView" />.
/// </summary>
[UsedImplicitly]
public sealed class ListViewItemComparer : IComparer
{
    /// <summary>
    ///     Create a new item comparer for the specified column using the specified sort order.
    /// </summary>
    /// <param name="column">The column index.</param>
    /// <param name="order">The <see cref="SortOrder" /> used for ordering.</param>
    public ListViewItemComparer(int column, SortOrder order)
    {
        Order = order;
        Column = column;
    }

    /// <summary>
    ///     The column index that specifies which column in the list view contains the items being compared.
    /// </summary>
    [UsedImplicitly]
    public int Column { get; }

    /// <summary>
    ///     The order items should be compared in (ascending/descending).
    /// </summary>
    [UsedImplicitly]
    public SortOrder Order { get; }

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