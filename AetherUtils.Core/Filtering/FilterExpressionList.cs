using System.Collections;

namespace AetherUtils.Core.Filtering
{
    /// <summary>
    /// Implements the <see cref="IEnumerable"/> interface using a <see cref="List{T}"/> of <see cref="IFilter"/> objects.
    /// </summary>
    public class FilterExpressionList : IEnumerable, IEnumerator
    {
        readonly List<IFilter> items;
        readonly IEnumerator enumerator;

        public FilterExpressionList()
        {
            items = new List<IFilter>();
            enumerator = items.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public IFilter Current => (IFilter)enumerator.Current;

        object IEnumerator.Current => enumerator.Current;

        public void Reset()
        {
            enumerator.Reset();
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        /// <summary>
        /// Add a new <see cref="IFilter"/> to the list.
        /// </summary>
        /// <param name="filterExpression">The <see cref="IFilter"/> expression.</param>
        public void Add(IFilter filterExpression)
        {
            items.Add(filterExpression);
        }

        /// <summary>
        /// Get the <see cref="IFilter"/> expression at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The <see cref="IFilter"/> expression at the specified index.</returns>
        public IFilter this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        /// <summary>
        /// Get the number of objects in the list.
        /// </summary>
        public int Count => items.Count;
    }
}
