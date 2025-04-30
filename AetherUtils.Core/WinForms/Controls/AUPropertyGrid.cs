// // AUPropertyGrid.cs : AetherUtils
// // Copyright (C) 2025  Ethan Hann
// //
// // MIT License
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// //
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// //
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.

using System.ComponentModel;
using JetBrains.Annotations;

namespace AetherUtils.Core.WinForms.Controls;

/// <summary>
///     Represents a custom <see cref="PropertyGrid" /> control with support for additional buttons on the ToolStrip of
///     the property grid. Each button can be added using the <see cref="AddButton" /> method. When all buttons are added,
///     <see cref="DrawButtons" /> can be called to add them to the control.
/// </summary>
/// <remarks>
///     A button can be retrieved using <see cref="GetButton" />; this allows you to add an event handler to the button.
///     Most
///     commonly, an OnClick event handler should be added to a button.
/// </remarks>
[UsedImplicitly]
public partial class AuPropertyGrid : PropertyGrid
{
    private readonly Dictionary<string, ToolStripButton> _customButtons = [];

    private ToolStrip? _toolStrip;

    /// <summary>
    ///     Create a new property grid, specifying whether to hide the property page button by default.
    /// </summary>
    /// <param name="hidePropertyPageButton">Should the property page button be hidden?</param>
    public AuPropertyGrid(bool hidePropertyPageButton)
    {
        InitializeComponent();

        HidePropertyPageButton = hidePropertyPageButton;
        Setup();
    }

    /// <summary>
    ///     Create a new property grid. The property page button is hidden by default.
    /// </summary>
    /// <param name="container"></param>
    public AuPropertyGrid(IContainer container)
    {
        container.Add(this);
        InitializeComponent();
        Setup();
    }

    /// <summary>
    ///     Create a new property grid, specifying whether to hide the property page button by default.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="hidePropertyPageButton"></param>
    public AuPropertyGrid(IContainer container, bool hidePropertyPageButton)
    {
        container.Add(this);
        InitializeComponent();
        HidePropertyPageButton = hidePropertyPageButton;
        Setup();
    }

    private bool HidePropertyPageButton { get; } = true;

    private void Setup()
    {
        foreach (Control c in Controls)
        {
            if (c is not ToolStrip toolStrip) continue;
            _toolStrip = toolStrip;
            break;
        }

        //This should never happen but if it does, just return before trying to use the tool strip.
        if (_toolStrip == null) return;

        //Hide the property pages button if required.
        _toolStrip.Items[4].Visible = !HidePropertyPageButton;
    }

    /// <summary>
    ///     Draw the custom buttons on the property grid. This method should be called after adding or
    ///     removing buttons from the property grid via <see cref="AddButton" /> or <see cref="RemoveButton" />.
    /// </summary>
    [UsedImplicitly]
    public void DrawButtons()
    {
        if (_toolStrip == null) return;

        //Suspend the layout while we add buttons to the tool strip.
        SuspendLayout();

        //Remove all custom buttons from the property grid first, in case the user added buttons.
        _customButtons.Values.ToList().ForEach(b => _toolStrip.Items.Remove(b));

        //Re-add the new buttons to the property grid.
        foreach (var button in _customButtons)
        {
            button.Value.Tag = button.Key;
            _toolStrip.Items.Add(button.Value);
        }

        //Resume the layout and redraw the control.
        ResumeLayout();
        Invalidate();
    }

    /// <summary>
    ///     Get the button specified by the key.
    /// </summary>
    /// <param name="key">The unique key of the button to get.</param>
    /// <returns></returns>
    [UsedImplicitly]
    public ToolStripButton? GetButton(string key) =>
        _customButtons.FirstOrDefault(b => b.Key.Equals(key)).Value;

    /// <summary>
    ///     Add a new button to the property grid with the specified key.
    /// </summary>
    /// <param name="key">A unique key to identify the button.</param>
    /// <param name="button">The <see cref="ToolStripButton" /> to add.</param>
    /// <returns><c>true</c> if the button was added; <c>false</c> otherwise.</returns>
    [UsedImplicitly]
    public bool AddButton(string key, ToolStripButton button)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        return _customButtons.TryAdd(key, button);
    }

    /// <summary>
    ///     Remove the button specified by <paramref name="key" /> from the property grid.
    /// </summary>
    /// <param name="key">The unique key of the button to remove.</param>
    /// <returns><c>true</c> if the button was removed; <c>false</c> otherwise.</returns>
    [UsedImplicitly]
    public bool RemoveButton(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        return _customButtons.Remove(key);
    }
}