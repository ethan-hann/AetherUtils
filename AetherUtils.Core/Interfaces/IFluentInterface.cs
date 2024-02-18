using System.ComponentModel;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace AetherUtils.Core.Interfaces;

/// <summary>
/// Interface that is used to build fluent interfaces and
/// hides methods declared by <see cref="object"/> from IntelliSense.
/// </summary>
/// <remarks>
/// Code that consumes implementations of this interface should expect one of two things:
/// <list type = "number">
///   <item>When referencing the interface from within the same solution (project reference),
///         you will still see the methods this interface is meant to hide.</item>
///   <item>When referencing the interface through the compiled output assembly (external reference),
///         the standard Object methods will be hidden as intended.</item>
/// </list>
/// See <a href="https://dotnettutorials.net/lesson/fluent-interface-design-pattern/">Fluent Interface</a>
/// for more information.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IFluentInterface
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    Type GetType();
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    int GetHashCode();
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    string? ToString();
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    bool Equals(object obj);
}