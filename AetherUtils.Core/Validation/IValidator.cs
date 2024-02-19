namespace AetherUtils.Core.Validation;

/// <summary>
/// Interface that all validator classes must implement.
/// </summary>
public interface IValidator<in T> where T : notnull
{
    /// <summary>
    /// Validate the objects against a set of conditions.
    /// </summary>
    /// <param name="objects">The objects to validate.</param>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>A list of <see cref="IValidationFailure"/> indicating the result of validation.
    /// If no validation errors, this list should be empty.</returns>
    public static abstract List<IValidationFailure> Validate(params T[] objects);
}