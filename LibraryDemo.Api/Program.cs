var builder = WebApplication.CreateBuilder(args);

builder
    .AddConfiguration()
    .AddServices();

var app = builder.Build();

app.UseModuleEndpoints();
app.UseExceptionHandler();

await app.RunMigrationsAsync();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API"));

    await app.SeedDataAsync();
}

app.Run();