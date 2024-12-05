namespace LibraryDemo.Api.Middleware;

public class ValidationExceptionHandler(IHostEnvironment env, ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception ex,
        CancellationToken token)
    {
        if (ex is not ValidationException)
        {
            return false;
        }

        _logger.LogError(ex, "A validation exception has occurred.");

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
            Detail = ex.Message
        };

        if (env.IsDevelopment())
        {
            problem.Detail = ex.ToString();

            if (ex.Data.Count > 0)
            {
                problem.Extensions["data"] = JsonSerializer.Serialize(ex.Data);
            }
        }

        context.Response.StatusCode = problem.Status.Value;

        await context.Response.WriteAsJsonAsync(problem, token);

        return true;
    }
}