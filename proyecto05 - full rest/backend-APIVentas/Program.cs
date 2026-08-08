using Asp.Versioning;
using VentasAPI.Configs;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers & Validación ──────────────────────────────
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Deshabilita respuesta automática 400 para personalizar con ApiResponse
        options.SuppressModelStateInvalidFilter = false;
    });

// ── API Versioning ────────────────────────────────────────
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"),
        new QueryStringApiVersionReader("api-version")
    );
})
.AddApiExplorer(options =>          // ← debe estar encadenado aquí
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ── CORS ──────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.SetIsOriginAllowed(_ => true)        
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

//policy.WithOrigins("http://localhost:4200", "https://localhost:4200")

// ── Application Services (DI, DbContext, Repos, Services) ─
builder.Services.AddApplicationServices(builder.Configuration);

// ── Swagger ───────────────────────────────────────────────
builder.Services.AddSwaggerConfiguration();
builder.Services.AddEndpointsApiExplorer();

// ── XML Comments para Swagger ─────────────────────────────
builder.Services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options => { });


// --- Filtrado de Rutas API ─────────────────────────────
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;   // /api/v1/boletas       reporte-boletas        
    options.LowercaseQueryStrings = true;   // ?desde=  &hasta=
});

var app = builder.Build();

// ── Pipeline ─────────────────────────────────────────────
//if (app.Environment.IsDevelopment())
//{
    app.UseSwaggerConfiguration();
    app.UseDeveloperExceptionPage();
//}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseRouting();
app.UseAuthorization();

// ── Health Check endpoint ─────────────────────────────────
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status    = report.Status.ToString(),
            timestamp = DateTime.UtcNow
        });
        await context.Response.WriteAsync(result);
    }
});

app.MapControllers();
app.Run();
