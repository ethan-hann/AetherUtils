// DefaultConfigManager.cs : AetherUtils
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

namespace AetherUtils.Core.Configuration;

/// <summary>
/// A default implementation of <see cref="ConfigManager{T}"/> allowing for passing the configuration file path directly in.
/// </summary>
public sealed class DefaultConfigManager : ConfigManager<DefaultConfig>
{
    /// <summary>
    /// Create a manager with the specified config file path.
    /// </summary>
    /// <param name="configFilePath">The full path to a configuration file to save and/or load.</param>
    public DefaultConfigManager(string configFilePath) : base(configFilePath) { }
}