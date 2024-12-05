namespace LibraryDemo.Api.Configuration;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, ConfigurationManager config)
    {
        var settings = config.GetRequiredSection(nameof(LibrarySettings)).Get<LibrarySettings>();

        services
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            // .AddJwtBearer(o =>
            // {
            //     o.TokenValidationParameters = CreateTokenValidationParameters(ref config);
            // })
            .AddIdentityCookies(b => b.ApplicationCookie!.Configure(o =>
            {
                o.Cookie.SameSite = SameSiteMode.Lax;
                o.Cookie.HttpOnly = true;
                o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                o.Cookie.Name = "LibraryAuth";
                o.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                o.Events.OnRedirectToLogin = ReplaceRedirector(HttpStatusCode.Unauthorized, o.Events.OnRedirectToLogin);
                o.Events.OnRedirectToAccessDenied = ReplaceRedirector(HttpStatusCode.Forbidden, o.Events.OnRedirectToAccessDenied);
            }));

        services
            .AddAuthorizationBuilder()
            //.AddPolicies(JwtBearerDefaults.AuthenticationScheme);
            .AddPolicies(IdentityConstants.ApplicationScheme);

        services
            .AddIdentityCore<User>(o =>
            {
                o.Password = CreateIdentityOptions(ref config).Password;
                o.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddSignInManager()
            .AddEntityFrameworkStores<IdentityContext>();

        return services;
    }

    private static AuthorizationBuilder AddPolicies(this AuthorizationBuilder builder, string scheme)
    {
        builder.AddPolicy("RequireAdmin", p =>
        {
            p.RequireRole("Admin");
            p.AuthenticationSchemes.Add(scheme);
        })
        .AddPolicy("RequireLibrarian", p =>
        {
            p.RequireRole(["Librarian", "Admin"]);
            p.AuthenticationSchemes.Add(scheme);
        })
        .AddPolicy("RequireCustomer", p =>
        {
            p.RequireRole(["Customer", "Admin"]);
            p.AuthenticationSchemes.Add(scheme);
        })
        .AddPolicy("RequireRegisteredUser", p =>
        {
            p.RequireRole(["Customer", "Librarian", "Admin"]);
            p.AuthenticationSchemes.Add(scheme);
        });

        return builder;
    }

    // private static TokenValidationParameters CreateTokenValidationParameters(ref readonly ConfigurationManager config)
    // {
    //     var settings = config.GetRequiredSection(nameof(LibrarySettings)).Get<LibrarySettings>()!;

    //     return new TokenValidationParameters()
    //     {
    //         ClockSkew = TimeSpan.Zero,
    //         ValidateIssuer = true,
    //         ValidateAudience = true,
    //         ValidateLifetime = true,
    //         ValidateIssuerSigningKey = true,
    //         ValidIssuer = settings.Identity.JwtSettings.Issuer,
    //         ValidAudience = settings.Identity.JwtSettings.Audience,
    //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Identity.JwtSettings.Key))
    //     };
    // }

    private static IdentityOptions CreateIdentityOptions(ref readonly ConfigurationManager config)
    {
        var settings = config.GetRequiredSection(nameof(LibrarySettings)).Get<LibrarySettings>()!;

        return new IdentityOptions
        {
            Password = settings.Identity!.PasswordSettings!
        };
    }

    private static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(
        HttpStatusCode statusCode,
        Func<RedirectContext<CookieAuthenticationOptions>, Task> _)
    {
        return context =>
        {
            context.Response.StatusCode = (int) statusCode;

            return Task.CompletedTask;
        };
    }
}