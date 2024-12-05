namespace LibraryDemo.Api.Middleware;

public class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception ex,
        CancellationToken token)
    {
        _logger.LogError(ex, "An unhandled exception has occurred.");

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
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