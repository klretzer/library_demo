namespace LibraryDemo.Api.Middleware;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Demo", Version = "v1" });
            options.AddSecurityDefinition("CookieAuth", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Cookie,
                Type = SecuritySchemeType.ApiKey,
                Name = "LibraryAuth",
                Description = "Authentication and Authorization handled via cookies."
            });
            // options.OperationFilter<SecurityRequirementEndpointFilter>();
        });

        return services;
    }
}
