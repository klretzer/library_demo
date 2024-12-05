namespace LibraryDemo.Api.Modules.ReviewModule;

public class BookReviewModule : IModule
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddScoped<IRepository<BookReview>, Repository<BookReview>>();

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder)
    {
        var reviewEndpoints = builder.MapGroup("/reviews");

        reviewEndpoints.MapGet("/{id:guid}", GetByIdAsync)
            .RequireAuthorization("RequireCustomer")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Get details for a single book review." });

        reviewEndpoints.MapPost("/", CreateAsync)
            .RequireAuthorization("RequireCustomer")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Add a book review." });

        reviewEndpoints.MapPut("/", UpdateAsync)
            .RequireAuthorization("RequireCustomer")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Update a book review." });

        reviewEndpoints.MapDelete("/{id:guid}", DeleteAsync)
            .RequireAuthorization("RequireCustomer")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Delete a book review." });

        return reviewEndpoints;
    }

    public static async Task<Results<Ok<BookReviewDTO>, NotFound>> GetByIdAsync(IMediator mediator, [FromRoute] Guid id)
    {
        var dtos = await mediator.Send(new GetBookReviewQuery(b => b.Id == id));

        return dtos.FirstOrDefault() == null
            ? TypedResults.NotFound()
            : TypedResults.Ok(dtos.FirstOrDefault());
    }

    public static async Task<Created> CreateAsync(IMediator mediator, [FromBody] CreateBookReviewCommand command)
    {
        var id = await mediator.Send(command);

        return TypedResults.Created($"/reviews/{id}");
    }

    public static async Task<Results<NoContent, NotFound>> UpdateAsync(IMediator mediator, [FromBody] UpdateBookReviewCommand command)
    {
        var success = await mediator.Send(command);

        return success
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }

    public static async Task<Results<NoContent, NotFound>> DeleteAsync(IMediator mediator, [FromRoute] Guid id)
    {
        var success = await mediator.Send(new DeleteBookReviewCommmand(id));

        return success
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}