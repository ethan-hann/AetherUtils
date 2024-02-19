using AetherUtils.Core.Files;
using Standard.Licensing;
using System.Diagnostics;
using AetherUtils.Core.Structs;
using AetherUtils.Core.Validation;
using Standard.Licensing.Validation;
using IValidationFailure = AetherUtils.Core.Validation.IValidationFailure;

namespace AetherUtils.Core.Licensing
{
    /// <summary>
    /// Contains methods for validating a license file.
    /// </summary>
    public abstract class LicenseValidator : IValidator<Pair<string, string>>
    {
        private static readonly GenericValidationFailure InvalidExtensionFailure = new GenericValidationFailure()
        {
            Message = "License file extension was not valid.",
            HowToResolve = "Verify the file exists and try to validate again."
        };

        private static readonly GenericValidationFailure NoLicenseFailure = new GenericValidationFailure()
        {
            Message = "No license was provided.",
            HowToResolve = "Verify the license is not empty and try to validate again."
        };
        
        /// <summary>
        /// Validates licenses according to an array of license pairs with their public keys.
        /// </summary>
        /// <param name="licensePairs">An array of license pairs. Each pair should be of the format:<br/>
        ///     <c>Pair(license, publicKey)</c><br/>
        /// The license can be either an XML string or the full path to a license file.<br/>
        /// The public key should the key that was used for signing the license.
        /// </param>
        /// <returns>A list of <see cref="IValidationFailure"/> representing the results of validation.</returns>
        /// <exception cref="ArgumentException">If <paramref name="licensePairs"/> was empty.</exception>
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

        private static IEnumerable<IValidationFailure> ValidateOneLicense(string licenseText, string publicKey)
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
}
