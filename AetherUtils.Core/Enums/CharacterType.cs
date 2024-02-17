namespace AetherUtils.Core.Enums;

/// <summary>
/// Represents the different types of characters allowed in a password rule.
/// </summary>
public enum CharacterType
{
    /// <summary>
    /// Whitespace characters.
    /// </summary>
    WhiteSpace,
    /// <summary>
    /// Special characters.
    /// </summary>
    Special,
    /// <summary>
    /// Digits (0-9).
    /// </summary>
    Number,
    /// <summary>
    /// Alphabet characters (a-zA-Z).
    /// </summary>
    Character
}