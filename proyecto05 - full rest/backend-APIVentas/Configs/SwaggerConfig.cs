using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VentasAPI.Configs;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        // Deja que ApiExplorer genere los docs automáticamente por versión
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // Genera un endpoint por cada versión descubierta
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"Ventas API {description.GroupName.ToUpper()}");
            }
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "Ventas API - Documentación";
            options.DisplayRequestDuration();
            options.EnableFilter();
            options.DefaultModelsExpandDepth(2);
        });

        return app;
    }
}

// Clase que configura cada documento Swagger por versión
public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = "Ventas API",
                Version = description.GroupName,
                Description = $"""
                               API RESTful para el sistema de ventas con boletas.
                               Gestiona Distritos, Empleados, Clientes, Productos, Categorías y Boletas.
                               {(description.IsDeprecated ? " ⚠️ Esta versión está deprecada." : "")}
                               """,
                Contact = new OpenApiContact
                {
                    Name = "Equipo Backend",
                    Email = "backend@ventas.com"
                }
            });
        }
    }
}
