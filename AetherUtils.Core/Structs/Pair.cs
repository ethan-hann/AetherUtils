using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents a generic key-value pair.
    /// </summary>
    /// <typeparam name="K">The <see cref="Type"/> for the key.</typeparam>
    /// <typeparam name="V">The <see cref="Type"/> for the value.</typeparam>
    public struct Pair<K, V>(K? key, V? value)
    {
        public K? Key { get; set; } = key;
        public V? Value { get; set; } = value;
    }
}
