namespace LibraryDemo.Api.Configuration;

public static class ServiceConfig
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddHttpContextAccessor()
            .AddModules()
            .AddDataServices(builder.Configuration)
            .AddIdentityConfig(builder.Configuration)
            .AddExceptionHandler<ValidationExceptionHandler>()
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddAutoMapper([typeof(CommandProfile), typeof(DtoProfile)])
            .AddValidatorsFromAssembly(Assembly.Load("LibraryDemo.Core"))
            .AddMediatR(c =>
            {
                c.RegisterServicesFromAssembly(Assembly.Load("LibraryDemo.Core"));
                c.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            })
            .AddSwaggerConfig()
            .AddEndpointsApiExplorer();

        return builder;
    }
}