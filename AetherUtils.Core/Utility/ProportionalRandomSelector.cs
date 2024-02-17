using System.Security.Cryptography;

namespace AetherUtils.Core.Utility;

/// <summary>
/// Represents a cryptographically strong, random item selector using proportional percentages.
/// </summary>
/// <typeparam name="T">The type of item to select using this selector.</typeparam>
public sealed class ProportionalRandomSelector<T> where T : notnull
{
    //Implemented based on: https://stackoverflow.com/questions/33888612/how-to-make-selection-random-based-on-percentage
    private readonly Dictionary<T, int> _percentages = new();

    /// <summary>
    /// Add an item and its percentage to this selector.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="percentage">The percentage indicating the likelihood of the item being selected.</param>
    public void AddPercentage(T item, int percentage)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        _percentages[item] = percentage;
    }

    /// <summary>
    /// Get a random item from the selector using the percentages defined within.
    /// </summary>
    /// <returns>A random item or <c>null</c> if an item could not be selected.</returns>
    public T? SelectItem()
    {
        //Calculate the sum of all portions.
        var poolSize = _percentages.Values.Sum();

        //Get a random integer between 1 to pool size.
        var randomNumber = RandomNumberGenerator.GetInt32(1, poolSize);
        
        //Get the item which corresponds to the current random number
        var accumulatedProbability = 0;
        foreach (var pair in _percentages)
        {
            accumulatedProbability += pair.Value;
            if (randomNumber <= accumulatedProbability)
                return pair.Key;
        }

        return default;
    }
}