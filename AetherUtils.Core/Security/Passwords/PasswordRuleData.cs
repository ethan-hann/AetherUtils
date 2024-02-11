﻿using Newtonsoft.Json;

namespace AetherUtils.Core.Security.Passwords;

[Serializable]
public class PasswordRuleData
{
    public bool WhitespaceAllowed { get; set; } = false;
    public bool SpecialsAllowed { get; set; } = false;
    public bool NumbersAllowed { get; set; } = false;
    public int MinimumLengthAllowed { get; set; } = -1;
    public int MinimumLetterCount { get; set; } = -1;
    public int MinimumNumberCount { get; set; } = -1;
    public int MinimumSpecialCount { get; set; } = -1;
    public DateTime? Expiration { get; set; }
    
    public PasswordRuleData() { }
}