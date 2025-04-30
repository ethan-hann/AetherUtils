// PasswordRuleBuilder.cs : AetherUtils
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

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
///     Internal builder class for a <see cref="PasswordRule" />.
/// </summary>
internal sealed class PasswordRuleBuilder : IPasswordRuleBuilder
{
    private readonly PasswordRule _rule;

    internal PasswordRuleBuilder()
    {
        _rule = new PasswordRule(false);
    }

    public IPasswordRuleBuilder AllowWhitespace()
    {
        _rule.AllowWhiteSpace();
        return this;
    }

    public IPasswordRuleBuilder AllowNumbers()
    {
        _rule.AllowNumbers();
        return this;
    }

    public IPasswordRuleBuilder MinimumLength(int length)
    {
        _rule.MinimumLength(length);
        return this;
    }

    public IPasswordRuleBuilder MinimumNumberCount(int count)
    {
        _rule.MinimumNumberCount(count);
        return this;
    }

    public IPasswordRuleBuilder MinimumSpecialCount(int count)
    {
        _rule.MinimumSpecialCount(count);
        return this;
    }

    public IPasswordRuleBuilder AllowSpecials()
    {
        _rule.AllowSpecials();
        return this;
    }

    public IPasswordRuleBuilder Expires(DateTime expires)
    {
        _rule.Expires(expires);
        return this;
    }

    public PasswordRule Build()
    {
        _rule.FinishedBuilding();
        return _rule;
    }
}