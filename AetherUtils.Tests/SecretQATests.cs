// SecretQATests.cs : AetherUtils
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

using AetherUtils.Core.Security;

namespace AetherUtils.Tests;

public class SecretQATests
{
    [Test]
    public void CreateListTest()
    {
        List<SecretQa> list = [];

        for (var i = 0; i < 10; i++)
            list.Add(new SecretQa($"Question {i}", $"Answer {i}"));

        Assert.That(list, Has.Count.EqualTo(10));
    }

    [Test]
    public void GetItemTest()
    {
        List<SecretQa> list = [];

        for (var i = 0; i < 10; i++)
            list.Add(new SecretQa($"Question {i}", $"Answer {i}"));

        list.RemoveAll(s => s.Answer.Equals("Question 2"));

        Assert.That(list[1].Answer, Is.EqualTo("Answer 1"));
    }
}