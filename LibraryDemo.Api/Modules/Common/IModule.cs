namespace LibraryDemo.Api.Modules.Common;

public interface IModule
{
    IServiceCollection AddServices(IServiceCollection services);

    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder);
}