using AetherUtils.Core.Files;
using Standard.Licensing;
using Standard.Licensing.Validation;
using System.Diagnostics;

namespace AetherUtils.Core.Licensing
{
    public static class Licenser
    {
        private static readonly GeneralValidationFailure _invalidExtensionFailure = new GeneralValidationFailure()
        {
            Message = "License file extension was not valid.",
            HowToResolve = "Verify the file exists and try to validate again."
        };

        private static readonly GeneralValidationFailure _invalidFilePathFailure = new GeneralValidationFailure()
        {
            Message = "The file path was invalid.",
            HowToResolve = "Verify the file exists and try to validate again."
        };

        private static readonly GeneralValidationFailure _noLicenseFailure = new GeneralValidationFailure()
        {
            Message = "No license was provided.",
            HowToResolve = "Verify the license is not empty and try to validate again."
        };

        /// <summary>
        /// Validates a license with the specified public key.
        /// </summary>
        /// <param name="license">The license XML string or file path to a license file (<c>.lic</c>).</param>
        /// <param name="publicKey">The public key to validate against.</param>
        /// <returns>A string representing the results of validation.</returns>
        public static List<IValidationFailure> Validate(string license, string publicKey)
        {
            string licenseText = license;

            try
            {
                if (FileHelper.IsValidPath(license))
                    if (Path.GetExtension(license).Equals(".lic"))
                        licenseText = FileHelper.OpenFile(license, true);
                    else
                        return [_invalidExtensionFailure];
                else
                    return [_invalidFilePathFailure];
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            if (string.IsNullOrEmpty(licenseText))
                return [_noLicenseFailure];

            return License.Load(license).Validate()
                .ExpirationDate()
                .When(lic => lic.Type == LicenseType.Trial)
                .And()
                .Signature(publicKey)
                .AssertValidLicense().ToList();
        }
    }
}
