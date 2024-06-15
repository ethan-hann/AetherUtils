namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating <see cref="IDictionary{TKey,TValue}"/> and <see cref="ICollection{T}"/>
/// objects.
/// </summary>
public static class CollectionExtensions
{
    //Used for locking threads while manipulating collections.
    private static readonly object SyncRoot = new();
    
    /// <summary>
    /// Rename a key contained in a dictionary. This method is thread-safe.
    /// </summary>
    /// <remarks>If the <paramref name="newKey"/> is the same as the <paramref name="oldKey"/> using the default equality operator,
    /// no operation is performed.</remarks>
    /// <typeparam name="TKey">The <see cref="Type"/> of the key.</typeparam>
    /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
    /// <param name="dictionary">The dictionary to perform the rename on.</param>
    /// <param name="oldKey">The name of the old key.</param>
    /// <param name="newKey">The name to change <paramref name="oldKey"/> to.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="dictionary"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="newKey"/> already exists in <paramref name="dictionary"/>.</exception>
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
    /// Checks if the two lists contain the same items.
    /// </summary>
    /// <typeparam name="T">The type of items in the lists.</typeparam>
    /// <param name="list1">The source list.</param>
    /// <param name="list2">The list to compare list 1 against.</param>
    /// <returns></returns>
    public static bool AreListsEqual<T>(this IList<T> list1, IList<T> list2) where T : notnull
    {
        if (list1.Count != list2.Count) return false;

        var dict1 = list1.GroupBy(x => x).ToDictionary(g => g.Key, g =>g.Count());
        var dict2 = list2.GroupBy(x => x).ToDictionary(g => g.Key, g =>g.Count());

        return dict1.Count == dict2.Count && !dict1.Except(dict2).Any();
    }
}