using AetherUtils.Core.Extensions;
using System.Collections;

namespace AetherUtils.Core.Security
{
    /// <summary>
    /// Implements the <see cref="IEnumerable"/> interface using a <see cref="List{SecretQA}"/> of <see cref="SecretQA"/> objects.
    /// <inheritdoc cref="IEnumerator{SecretQA}"/>
    /// </summary>
    public class SecretQAList : IEnumerable<SecretQA>, IEnumerator<SecretQA>
    {
        private readonly List<SecretQA> _list;
        private readonly IEnumerator<SecretQA> _enumerator;
        private bool disposedValue;

        public int Count => _list.Count;

        SecretQA IEnumerator<SecretQA>.Current => _enumerator.Current;

        public object Current => _enumerator.Current;

        public SecretQAList()
        {
            _list = [];
            _enumerator = _list.GetEnumerator();
        }

        /// <summary>
        /// Get the <see cref="SecretQA"/> at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SecretQA this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        /// <summary>
        /// Add a new <see cref="SecretQA"/> to this list.
        /// </summary>
        /// <param name="secretQA"></param>
        public void Add(SecretQA secretQA) => _list.Add(secretQA);

        /// <summary>
        /// Add a new item with the specified <paramref name="question"/> and <paramref name="answer"/> to the list.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        public void Add(string question, string answer) => _list.Add(new SecretQA(question, answer.ToSecureString()));

        /// <summary>
        /// Clear this list.
        /// </summary>
        public void Clear() => _list.Clear();

        /// <summary>
        /// Remove the first occurance of the <paramref name="secretQA"/> from the list.
        /// </summary>
        /// <param name="secretQA"></param>
        public void Remove(SecretQA secretQA) => _list.Remove(secretQA);

        /// <summary>
        /// Remove the first occurance of the <paramref name="question"/> from the list.
        /// </summary>
        /// <param name="question">The question to remove.</param>
        public void Remove(string question) => _list.RemoveAt(_list.FindIndex(qa => qa.Question.Equals(question)));

        /// <summary>
        /// Remove all occurances of the <paramref name="question"/> from the list.
        /// </summary>
        /// <param name="question"></param>
        public void RemoveAll(string question) => _list.RemoveAll(c => c.Question.Equals(question));

        public IEnumerator GetEnumerator() => this;

        IEnumerator<SecretQA> IEnumerable<SecretQA>.GetEnumerator() => this;

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _list.Clear();
                    _enumerator.Reset();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
