using AetherUtils.Core.WinForms.CustomArgs;

namespace AetherUtils.Core.WinForms.Controls;

/// <summary>
/// A form with a single input box and an OK button.
/// </summary>
public sealed partial class InputBox : Form
{
    /// <summary>
    /// Occurs when the OK button on the form is clicked.
    /// </summary>
    public readonly EventHandler? OkClicked;

    /// <summary>
    /// Create a new input box with the specified title, input prompt, and event handler.
    /// </summary>
    /// <param name="title">The title to display in the form's title bar.</param>
    /// <param name="inputPrompt">The text to display to the left of the input box.</param>
    /// <param name="okClicked">The event handler responsible for consuming the <see cref="OkClicked"/> event.</param>
    public InputBox(string title, string inputPrompt, EventHandler? okClicked)
    {
        InitializeComponent();
        
        Text = title;
        lblPrompt.Text = inputPrompt;
        OkClicked = okClicked;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        OkClicked?.Invoke(this, new InputBoxEventArgs(txtInput.Text));
        Close();
    }
}