namespace LibraryDemo.Api.Modules.BookModule;

public class BookModule : IModule
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddScoped<IRepository<Book>, Repository<Book>>();

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder)
    {
        var bookEndpoints = builder.MapGroup("/books");

        bookEndpoints.MapGet("/{id:guid}", GetByIdAsync)
            .RequireAuthorization("RequireCustomer")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Get a single book." });

        bookEndpoints.MapPost("/", CreateAsync)
            .RequireAuthorization("RequireLibrarian")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Add a book." });

        bookEndpoints.MapPut("/", UpdateAsync)
            .RequireAuthorization("RequireLibrarian")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Update a book." });

        bookEndpoints.MapDelete("/{id:guid}", DeleteAsync)
            .RequireAuthorization("RequireLibrarian")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Delete a book." });

        return bookEndpoints;
    }

    public static async Task<Results<Ok<BookDTO>, NotFound>> GetByIdAsync(IMediator mediator, [FromRoute] Guid id)
    {
        var dtos = await mediator.Send(new GetBookQuery(b => b.Id == id));

        return dtos.FirstOrDefault() == null
            ? TypedResults.NotFound()
            : TypedResults.Ok(dtos.FirstOrDefault());
    }

    public static async Task<Created> CreateAsync(IMediator mediator, [FromBody] CreateBookCommand command)
    {
        var id = await mediator.Send(command);

        return TypedResults.Created($"/books/{id}");
    }

    public static async Task<Results<NoContent, NotFound>> UpdateAsync(IMediator mediator, [FromBody] UpdateBookCommand command)
    {
        var success = await mediator.Send(command);

        return success
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }

    public static async Task<Results<NoContent, NotFound>> DeleteAsync(IMediator mediator, [FromRoute] Guid id)
    {
        var success = await mediator.Send(new DeleteBookCommmand(id));

        return success
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}