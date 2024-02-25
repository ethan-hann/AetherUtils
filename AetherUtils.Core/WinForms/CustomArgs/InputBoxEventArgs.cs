using AetherUtils.Core.WinForms.Controls;

namespace AetherUtils.Core.WinForms.CustomArgs;

/// <summary>
/// Represents the event args that are passed when the OK button is clicked on <see cref="InputBox"/>.
/// </summary>
/// <param name="text"></param>
public sealed class InputBoxEventArgs(string text) : EventArgs
{
    /// <summary>
    /// The text associated with the event.
    /// </summary>
    public string InputText { get; private set; } = text;
}