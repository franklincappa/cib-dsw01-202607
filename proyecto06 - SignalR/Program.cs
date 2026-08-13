using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

// ─── 1. REGISTRAR SERVICIOS ──────────────────────────────────────────────────

// Habilitar archivos estáticos (index.html, css, js)
builder.Services.AddSignalR(options =>
{
    // Tiempo máximo de silencio antes de detectar desconexión
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);

    // Tamaño máximo de mensaje (en bytes)
    options.MaximumReceiveMessageSize = 32 * 1024; // 32KB
});

// CORS: permitir cualquier origen para el laboratorio local
builder.Services.AddCors(options =>
{
    options.AddPolicy("LabPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ─── 2. CONFIGURAR PIPELINE HTTP ─────────────────────────────────────────────

app.UseCors("LabPolicy");

// Servir archivos de wwwroot (index.html por defecto)
app.UseDefaultFiles();
app.UseStaticFiles();

// ─── 3. MAPEAR EL HUB DE SIGNALR ─────────────────────────────────────────────
// Esta es la ruta WebSocket a la que se conectarán los clientes
app.MapHub<ChatHub>("/chatHub");

// Endpoint de diagnóstico
app.MapGet("/health", () => new
{
    status = "OK",
    timestamp = DateTime.Now,
    signalr = "active",
    hub = "/chatHub"
});

Console.WriteLine("══════════════════════════════════════════");
Console.WriteLine("   🚀 SignalR Chat Lab - .NET 9");
Console.WriteLine("══════════════════════════════════════════");
Console.WriteLine("   URL: http://localhost:5000");
Console.WriteLine("   Hub: http://localhost:5000/chatHub");
Console.WriteLine("   Health: http://localhost:5000/health");
Console.WriteLine("══════════════════════════════════════════");

app.Run();
