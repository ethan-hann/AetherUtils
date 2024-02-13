using System.Security;
using AetherUtils.Core.Extensions;

namespace AetherUtils.Core.Security
{
    /// <summary>
    /// Represents a secret question and answer pair.
    /// </summary>
    /// <remarks>Once instantiated, the question cannot be changed. However,
    /// the answer can be changed using <see cref="ChangeAnswer"/>.</remarks>
    public sealed class SecretQa
    {
        /// <summary>
        /// The question component.
        /// </summary>
        public string Question { get; private set; } = string.Empty;

        /// <summary>
        /// The answer component.
        /// </summary>
        public string Answer { get; private set; } = string.Empty;

        private SecretQa() { }

        /// <summary>
        /// Create a new <see cref="SecretQa"/> with the specified <paramref name="question"/> and <paramref name="answer"/>.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="answer">The answer.</param>
        public SecretQa(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }

        /// <summary>
        /// Set a new answer for the secret question.
        /// </summary>
        /// <param name="newAnswer">The new answer to set for the <see cref="Question"/>.</param>
        public void ChangeAnswer(string newAnswer) => Answer = newAnswer;
    }
}
