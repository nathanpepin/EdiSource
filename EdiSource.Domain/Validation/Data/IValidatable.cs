namespace EdiSource.Domain.Validation.Data;

public interface IValidatable
{
    IEnumerable<ValidationMessage> Validate();
}