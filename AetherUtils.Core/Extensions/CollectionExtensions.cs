// CollectionExtensions.cs : AetherUtils
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

using JetBrains.Annotations;

namespace AetherUtils.Core.Extensions;

/// <summary>
///     Provides extension methods for manipulating <see cref="IDictionary{TKey,TValue}" /> and
///     <see cref="ICollection{T}" />
///     objects.
/// </summary>
public static class CollectionExtensions
{
    //Used for locking threads while manipulating collections.
    private static readonly object SyncRoot = new();

    /// <summary>
    ///     Rename a key contained in a dictionary. This method is thread-safe.
    /// </summary>
    /// <remarks>
    ///     If the <paramref name="newKey" /> is the same as the <paramref name="oldKey" /> using the default equality
    ///     operator,
    ///     no operation is performed.
    /// </remarks>
    /// <typeparam name="TKey">The <see cref="Type" /> of the key.</typeparam>
    /// <typeparam name="TValue">The <see cref="Type" /> of the value.</typeparam>
    /// <param name="dictionary">The dictionary to perform the rename on.</param>
    /// <param name="oldKey">The name of the old key.</param>
    /// <param name="newKey">The name to change <paramref name="oldKey" /> to.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="dictionary" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="newKey" /> already exists in <paramref name="dictionary" />.</exception>
    public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey oldKey, TKey newKey) where TKey : notnull
    {
        //Implemented based on: https://josipmisko.com/posts/c-sharp-rename-dictionary-key
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));

        if (EqualityComparer<TKey>.Default.Equals(oldKey, newKey))
            return;

        lock (SyncRoot)
        {
            if (!dictionary.TryGetValue(oldKey, out var value)) return;
            if (dictionary.ContainsKey(newKey))
                throw new ArgumentException("The new key already exists in the dictionary");

            dictionary.Remove(oldKey);
            dictionary.Add(newKey, value);
        }
    }

    /// <summary>
    ///     Checks if the two lists contain the same items.
    /// </summary>
    /// <typeparam name="T">The type of items in the lists.</typeparam>
    /// <param name="list1">The source list.</param>
    /// <param name="list2">The list to compare list 1 against.</param>
    /// <returns><c>true</c> if the two lists are equal; <c>false</c> otherwise.</returns>
    [UsedImplicitly]
    public static bool AreListsEqual<T>(this IList<T> list1, IList<T> list2) where T : notnull
    {
        if (list1.Count != list2.Count) return false;

        var dict1 = list1.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        var dict2 = list2.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

        return dict1.Count == dict2.Count && !dict1.Except(dict2).Any();
    }
}