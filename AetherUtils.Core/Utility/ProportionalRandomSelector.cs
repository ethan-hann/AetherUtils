// // ProportionalRandomSelector.cs : AetherUtils
// // Copyright (C) 2025  Ethan Hann
// //
// // MIT License
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// //
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// //
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.

using System.Security.Cryptography;

namespace AetherUtils.Core.Utility;

/// <summary>
///     Represents a cryptographically strong, random item selector using proportional percentages.
/// </summary>
/// <typeparam name="T">The type of item to select using this selector.</typeparam>
public sealed class ProportionalRandomSelector<T> where T : notnull
{
    //Implemented based on: https://stackoverflow.com/questions/33888612/how-to-make-selection-random-based-on-percentage
    private readonly Dictionary<T, int> _percentages = new();

    /// <summary>
    ///     Add an item and its percentage to this selector.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="percentage">The percentage indicating the likelihood of the item being selected.</param>
    public void AddPercentage(T item, int percentage)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        _percentages[item] = percentage;
    }

    /// <summary>
    ///     Get a random item from the selector using the percentages defined within.
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