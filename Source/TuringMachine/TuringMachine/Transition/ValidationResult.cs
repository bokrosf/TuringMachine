using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition;

/// <summary>
/// The result of transition validation.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Initializes a new instance of <see cref="ValidationResult"/> class that is valid without any errors.
    /// </summary>
    public ValidationResult()
    {
        Errors = new List<string>();
    }
    
    /// <summary>
    /// Whether validation succeeded.
    /// </summary>
    public bool Valid => !Errors.Any();

    /// <summary>
    /// Errors occured during validation.
    /// </summary>
    public List<string> Errors { get; }
}