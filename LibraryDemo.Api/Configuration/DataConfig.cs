namespace LibraryDemo.Api.Configuration;

public static class DataConfig
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, ConfigurationManager config)
    {
        var libraryString = config.GetConnectionString("Library");

        services.AddScoped<SaveChangesInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        services
            .AddDbContext<LibraryContext>((sp, options) =>
            {
                options.UseSqlServer(libraryString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                    sqlOptions.MigrationsHistoryTable("__MigrationsHistory", "Domain");
                })
                .AddInterceptors([
                    sp.GetRequiredService<SaveChangesInterceptor>(),
                    sp.GetRequiredService<SoftDeleteInterceptor>()
                ]);
            })
            .AddDbContext<IdentityContext>((sp, options) =>
            {
                options.UseSqlServer(libraryString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                    sqlOptions.MigrationsHistoryTable("__MigrationsHistory", "Identity");
                })
                .AddInterceptors([
                    sp.GetRequiredService<SaveChangesInterceptor>()
                ]);
            });

        return services;
    }

    public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var settings = scope.ServiceProvider.GetRequiredService<IOptions<LibrarySettings>>();

        await SeedIdentityAsync(app, settings);
        await SeedLibraryAsync(app, settings);

        return app;
    }

    public static async Task<WebApplication> RunMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var auditContext = new AuditContext(scope.ServiceProvider.GetRequiredService<IOptions<ConnectionStrings>>());

        var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
        var libraryContext = scope.ServiceProvider.GetRequiredService<LibraryContext>();

        if (identityContext.Database.IsSqlServer())
        {
            await identityContext.Database.MigrateAsync();
        }

        if (libraryContext.Database.IsSqlServer())
        {
            await libraryContext.Database.MigrateAsync();
        }

        if (auditContext.Database.IsSqlServer())
        {
            await auditContext.Database.MigrateAsync();
        }

        return app;
    }

    private static async Task SeedIdentityAsync(WebApplication app, IOptions<LibrarySettings> settings)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();

        var generator = new DataGenerator(settings);

        if (!await context.Set<IdentityRole<Guid>>().AnyAsync())
        {
            var identityRoles = Enum.GetValues<UserRole>()
                .Select(e =>
                    new IdentityRole<Guid>
                    {
                        Id = Guid.NewGuid(),
                        Name = e.ToString(),
                        NormalizedName = e.ToString().ToUpperInvariant()
                    }
                );

            await context.Set<IdentityRole<Guid>>().AddRangeAsync(identityRoles);
            await context.SaveChangesAsync();
        }

        if (!await context.Set<User>().AnyAsync())
        {
            var seedUsers = generator.GetFakeUsers();

            await context.Set<User>().AddRangeAsync(seedUsers);
            await context.SaveChangesAsync();
        }

        if (!await context.Set<IdentityUserRole<Guid>>().AnyAsync())
        {
            var userRoles = generator.GetFakeUserRoles(context.Set<User>(), context.Set<IdentityRole<Guid>>());

            await context.Set<IdentityUserRole<Guid>>().AddRangeAsync(userRoles);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedLibraryAsync(WebApplication app, IOptions<LibrarySettings> settings)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
        using var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

        var generator = new DataGenerator(settings);
        var seedUsers = identityContext.Users.ToList();

        if (!await context.Books.AnyAsync())
        {
            var seedBooks = generator.GetFakeBooks();

            await context.Books.AddRangeAsync(seedBooks);
            await context.SaveChangesAsync();
        }

        if (!await context.BookReviews.AnyAsync())
        {
            var seedReviews = generator.GetFakeReviews(seedUsers, context.Books);

            await context.BookReviews.AddRangeAsync(seedReviews);
            await context.SaveChangesAsync();
        }

        if (!await context.BookRentals.AnyAsync())
        {
            var seedRentals = generator.GetFakeRentals(seedUsers, context.Books);

            await context.BookRentals.AddRangeAsync(seedRentals);
            await context.SaveChangesAsync();
        }
    }
}