namespace LibraryDemo.Api.Configuration;

public static class OptionsConfig
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.Sources.Clear();

        builder.Configuration
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", false, true)
            .AddEnvironmentVariables();

        var strings = builder.Configuration
            .GetSection("ConnectionStrings")
            .GetChildren()
            .ToList();

        builder.Services
            .AddOptions<LibrarySettings>()
            .BindConfiguration(nameof(LibrarySettings));

        builder.Services
            .AddOptions<ConnectionStrings>()
            .Configure(o =>
                strings.ForEach(v => o.Add(v.Key, v.Value!))
            );

        return builder;
    }
}
