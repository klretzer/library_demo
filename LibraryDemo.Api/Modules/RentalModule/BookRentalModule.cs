namespace LibraryDemo.Api.Modules.RentalModule;

public class BookRentalModule : IModule
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddScoped<IRepository<BookRental>, Repository<BookRental>>();

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder)
    {
        var rentalEndpoints = builder.MapGroup("/rentals");

        rentalEndpoints.MapGet("/{id:guid}", GetByIdAsync)
            .RequireAuthorization("RequireLibrarian")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Get details for a single book rental." });

        rentalEndpoints.MapPost("/", CreateAsync)
            .RequireAuthorization("RequireCustomer")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Add a book rental." });

        rentalEndpoints.MapPut("/", UpdateAsync)
            .RequireAuthorization("RequireLibrarian")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Update a book rental." });

        rentalEndpoints.MapDelete("/{id:guid}", DeleteAsync)
            .RequireAuthorization("RequireLibrarian")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Delete a book rental." });

        return rentalEndpoints;
    }

    public static async Task<Results<Ok<BookRentalDTO>, NotFound>> GetByIdAsync(IMediator mediator, [FromRoute] Guid id)
    {
        var dtos = await mediator.Send(new GetBookRentalQuery(b => b.Id == id));

        return dtos.FirstOrDefault() == null
            ? TypedResults.NotFound()
            : TypedResults.Ok(dtos.FirstOrDefault());
    }

    public static async Task<Created> CreateAsync(IMediator mediator, [FromBody] CreateBookRentalCommand command)
    {
        var id = await mediator.Send(command);

        return TypedResults.Created($"/rentals/{id}");
    }

    public static async Task<Results<NoContent, NotFound>> UpdateAsync(IMediator mediator, [FromBody] UpdateBookRentalCommand command)
    {
        var success = await mediator.Send(command);

        return success
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }

    public static async Task<Results<NoContent, NotFound>> DeleteAsync(IMediator mediator, [FromRoute] Guid id)
    {
        var success = await mediator.Send(new DeleteBookRentalCommmand(id));

        return success
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}