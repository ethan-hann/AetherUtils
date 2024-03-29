﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace AetherUtils.Core.Exceptions;

/// <summary>
/// Custom class for exceptions that occur when handling QR codes.
/// </summary>
public sealed class QrException : Exception
{
    public QrException(string message) : base(message) { }
    public QrException(string message, Exception innerException) : base(message, innerException) { }
}