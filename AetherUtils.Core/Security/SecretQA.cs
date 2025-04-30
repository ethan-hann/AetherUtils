// SecretQA.cs : AetherUtils
// Copyright (C) 2025  Ethan Hann
// 
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using JetBrains.Annotations;

namespace AetherUtils.Core.Security;

/// <summary>
///     Represents a secret question and answer pair.
/// </summary>
/// <remarks>
///     Once instantiated, the question cannot be changed. However,
///     the answer can be changed using <see cref="ChangeAnswer" />.
/// </remarks>
public sealed class SecretQa
{
    [UsedImplicitly]
    private SecretQa() { }

    /// <summary>
    ///     Create a new <see cref="SecretQa" /> with the specified <paramref name="question" /> and <paramref name="answer" />
    ///     .
    /// </summary>
    /// <param name="question">The question.</param>
    /// <param name="answer">The answer.</param>
    public SecretQa(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }

    /// <summary>
    ///     The question component.
    /// </summary>
    public string Question { get; private set; } = string.Empty;

    /// <summary>
    ///     The answer component.
    /// </summary>
    public string Answer { get; private set; } = string.Empty;

    /// <summary>
    ///     Set a new answer for the secret question.
    /// </summary>
    /// <param name="newAnswer">The new answer to set for the <see cref="Question" />.</param>
    [UsedImplicitly]
    public void ChangeAnswer(string newAnswer) => Answer = newAnswer;
}