namespace LibraryDemo.Api.Middleware;

public class SecurityRequirementEndpointFilter : IOperationFilter
{
    public void Apply(OpenApiOperation op, OperationFilterContext context)
    {
        var requiredPolicies = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<AuthorizeAttribute>()
            .Select(attr => attr.Policy)
            .Distinct();

        if (requiredPolicies.Any())
        {
            op.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            op.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var cookieScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "CookieAuth" }
            };

            op.Security = [];

            op.Parameters.Add(new OpenApiParameter
            {
                In = ParameterLocation.Cookie
            });
        }
    }
}