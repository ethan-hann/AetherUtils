﻿namespace AetherUtils.Core.WinForms.Controls;

public partial class AuPropertyGrid
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent() => components = new System.ComponentModel.Container();
}