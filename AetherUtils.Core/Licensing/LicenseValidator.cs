// LicenseValidator.cs : AetherUtils
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

using AetherUtils.Core.Files;
using AetherUtils.Core.Structs;
using AetherUtils.Core.Validation;
using Standard.Licensing;
using Standard.Licensing.Validation;
using IValidationFailure = AetherUtils.Core.Validation.IValidationFailure;

namespace AetherUtils.Core.Licensing;

/// <summary>
///     Contains methods for validating a license file.
/// </summary>
public abstract class LicenseValidator : IValidator<Pair<string, string>>
{
    private static readonly GenericValidationFailure InvalidExtensionFailure = new()
    {
        Message = "License file extension was not valid.",
        HowToResolve = "Verify the file exists and try to validate again."
    };

    private static readonly GenericValidationFailure NoLicenseFailure = new()
    {
        Message = "No license was provided.",
        HowToResolve = "Verify the license is not empty and try to validate again."
    };

    /// <summary>
    ///     Validates licenses according to an array of license pairs with their public keys.
    /// </summary>
    /// <param name="licensePairs">
    ///     An array of license pairs. Each pair should be of the format:<br />
    ///     <c>Pair(license, publicKey)</c><br />
    ///     The license can be either an XML string or the full path to a license file.<br />
    ///     The public key should the key that was used for signing the license.
    /// </param>
    /// <returns>A list of <see cref="IValidationFailure" /> representing the results of validation.</returns>
    /// <exception cref="ArgumentException">If <paramref name="licensePairs" /> was empty.</exception>
    /// <exception cref="ArgumentException">If the license text for a license pair was empty or <c>null</c>.</exception>
    /// <exception cref="ArgumentException">If the public key for a license pair was empty or <c>null</c>.</exception>
    public static List<IValidationFailure> Validate(params Pair<string, string>[] licensePairs)
    {
        if (licensePairs.Length <= 0)
            throw new ArgumentException("Objects array had no elements.", nameof(licensePairs));

        List<IValidationFailure> failures = [];
        foreach (var pair in licensePairs)
        {
            var licenseText = pair.Key;
            var publicKey = pair.Value;

            if (string.IsNullOrEmpty(licenseText))
                throw new ArgumentException("License text cannot be empty.");
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentException("Public key cannot be empty.");

            if (FileHelper.IsValidPath(licenseText))
                if (FileHelper.GetExtension(licenseText).Equals(".lic"))
                    licenseText = FileHelper.OpenFile(licenseText);
                else
                    return [InvalidExtensionFailure];
            failures.AddRange(ValidateOneLicense(licenseText, publicKey));
        }

        return failures;
    }

    private static List<IValidationFailure> ValidateOneLicense(string licenseText, string publicKey)
    {
        List<IValidationFailure> failures = [];

        if (string.IsNullOrEmpty(licenseText))
            return [NoLicenseFailure];

        var license = License.Load(licenseText);
        var licenseFailures = License.Load(licenseText).Validate()
            .ExpirationDate()
            .When(lic => lic.Type == LicenseType.Trial)
            .And()
            .Signature(publicKey)
            .AssertValidLicense().ToList();

        failures.AddRange(
            from failure in licenseFailures
            let message = $"{license.Id} -> {failure.Message}"
            select new ValidationFailure(message, failure.HowToResolve));
        return failures;
    }
}