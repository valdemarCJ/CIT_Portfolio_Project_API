using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.UnitTests;

public static class ValidationHelper
{
    public static IList<ValidationResult> Validate(object instance)
    {
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(instance);
        Validator.TryValidateObject(instance, ctx, results, validateAllProperties: true);
        return results;
    }
}
