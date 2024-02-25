using System.ComponentModel;

namespace AetherUtils.Core.WinForms.Controls;

sealed partial class InputBox
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        lblPrompt = new Label();
        txtInput = new TextBox();
        btnOk = new Button();
        tableLayoutPanel1 = new TableLayoutPanel();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // lblPrompt
        // 
        lblPrompt.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        lblPrompt.AutoSize = true;
        lblPrompt.Location = new Point(3, 9);
        lblPrompt.Name = "lblPrompt";
        lblPrompt.Size = new Size(147, 17);
        lblPrompt.TabIndex = 0;
        lblPrompt.Text = "Text";
        lblPrompt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtInput
        // 
        txtInput.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtInput.Location = new Point(156, 5);
        txtInput.Name = "txtInput";
        txtInput.Size = new Size(311, 25);
        txtInput.TabIndex = 1;
        // 
        // btnOk
        // 
        btnOk.Anchor = AnchorStyles.Right;
        btnOk.Location = new Point(392, 40);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(75, 26);
        btnOk.TabIndex = 2;
        btnOk.Text = "OK";
        btnOk.UseVisualStyleBackColor = true;
        btnOk.Click += btnOk_Click;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.5531921F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67.44681F));
        tableLayoutPanel1.Controls.Add(lblPrompt, 0, 0);
        tableLayoutPanel1.Controls.Add(btnOk, 1, 1);
        tableLayoutPanel1.Controls.Add(txtInput, 1, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 51.7241364F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 48.2758636F));
        tableLayoutPanel1.Size = new Size(470, 71);
        tableLayoutPanel1.TabIndex = 3;
        // 
        // InputBox
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(470, 71);
        ControlBox = false;
        Controls.Add(tableLayoutPanel1);
        Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "InputBox";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Input";
        TopMost = true;
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Label lblPrompt;
    private TextBox txtInput;
    private Button btnOk;
    private TableLayoutPanel tableLayoutPanel1;
}