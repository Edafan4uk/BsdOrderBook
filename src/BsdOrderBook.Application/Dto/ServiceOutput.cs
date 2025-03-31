namespace BsdOrderBook.Application.Dto;

/// <summary>
/// Represents the result of a service layer operation, encapsulating success or error states.
/// </summary>
/// <typeparam name="TOutput">The type of the successful result.</typeparam>
public class ServiceOutput<TOutput>
{
    public bool HasError { get; }
    public List<string>? ErrorMessages { get; }
    public TOutput? Output { get; }

    // Constructor for success
    public ServiceOutput(TOutput output)
    {
        HasError = false;
        Output = output;
    }

    // Constructor for error
    public ServiceOutput(List<string> errorMessages)
    {
        HasError = true;
        ErrorMessages = errorMessages ?? [];
    }

    public static ServiceOutput<TOutput> Success(TOutput output) => new ServiceOutput<TOutput>(output);

    public static ServiceOutput<TOutput> Failure(params string[] errors)
        => new(new List<string>(errors));

    public override string ToString()
    {
        return HasError
            ? $"Error: {string.Join(", ", ErrorMessages)}"
            : $"Success: {Output}";
    }
}