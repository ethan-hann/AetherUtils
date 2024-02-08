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
}