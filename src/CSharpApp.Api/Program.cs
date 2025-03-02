using CSharpApp.Application.Services;
using CSharpApp.Infrastructure.Middlewares.CSharpApp.Infrastructure.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddHttpClient<IProductsService, ProductsService>();
builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IRestApiService, RestApiService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Accepted CSharpApp API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Accepted CSharpApp API V1");
        c.RoutePrefix = string.Empty;
    });
    app.MapOpenApi();
}
app.UseMiddleware<PerformanceLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//app.UseHttpsRedirection();

app.Run();