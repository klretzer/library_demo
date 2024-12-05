namespace LibraryDemo.Api.Configuration;

public static class ModuleConfig
{
    private static readonly List<IModule> Modules = [];

    public static IServiceCollection AddModules(this IServiceCollection services)
    {
        var modules = FindModules();

        foreach (var module in modules)
        {
            module.AddServices(services);

            Modules.Add(module);
        }

        return services;
    }

    public static WebApplication UseModuleEndpoints(this WebApplication app)
    {
        foreach (IModule module in Modules)
        {
            module.MapEndpoints(app);
        }

        return app;
    }

    private static IEnumerable<IModule> FindModules()
    {
        return typeof(Program).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}