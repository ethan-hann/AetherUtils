using System.Security;

namespace AetherUtils.Core.Security
{
    /// <summary>
    /// Represents a secret question and answer pair. Once instantiated, neither the question nor the answer can be changed.
    /// <inheritdoc cref="ICloneable"/>
    /// </summary>
    public sealed class SecretQa : ICloneable
    {
        /// <summary>
        /// The question component.
        /// </summary>
        public string Question { get; private set; } = string.Empty;

        /// <summary>
        /// The answer component, as a <see cref="SecureString"/>.
        /// </summary>
        public SecureString Answer { get; private set; } = new SecureString();

        private SecretQa() { }

        /// <summary>
        /// Create a new <see cref="SecretQa"/> with the specified <paramref name="question"/> and <paramref name="answer"/>.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        public SecretQa(string question, SecureString answer)
        {
            Question = question;
            Answer = answer;
        }

        public object Clone()
        {
            return new SecretQa(Question, Answer);
        }
    }
}
