using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents a generic key-value pair that is read-only; Once the pair is instantiated, the values cannot be changed.
    /// </summary>
    /// <typeparam name="K">The <see cref="Type"/> for the key.</typeparam>
    /// <typeparam name="V">The <see cref="Type"/> for the value.</typeparam>
    public readonly struct ReadOnlyPair<K, V>(K? key, V? value)
    {
        public readonly K? Key = key;
        public readonly V? Value = value;
    }
}
