using Asp.Versioning;
using LibraryManagement.Api.Mappings;
using LibraryManagement.Api.Swagger;
using LibraryManagement.Application.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1.0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
    o.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
}).AddMvc().AddApiExplorer();

builder.Services.AddControllers();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddSwaggerGen(o => o.OperationFilter<SwaggerDefaultValues>());

builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(a =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            a.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

app.Run();
