using ChatSignalR.Api.Models;
using ChatSignalR.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatSignalR.Api.Hubs;

/// <summary>
/// Hub central de chat. Cada método público de esta clase es un endpoint
/// invocable desde el cliente JavaScript a través de connection.invoke("MetodoX", ...).
///
/// El [Authorize] obliga a que solo usuarios con un JWT válido puedan
/// abrir el WebSocket. El token llega en la query string ?access_token=...
/// (configurado en Program.cs -&gt; JwtBearerEvents.OnMessageReceived).
/// </summary>
[Authorize]
public class ChatHub : Hub
{
    private readonly IUserTracker _users;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IUserTracker users, ILogger<ChatHub> logger)
    {
        _users = users;
        _logger = logger;
    }

    // =====================================================
    // Eventos del ciclo de vida de la conexión
    // =====================================================

    /// <summary>
    /// Se ejecuta automáticamente cuando un cliente se conecta al Hub.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        // Context.User viene del JWT validado en el pipeline de autenticación.
        var username = Context.User?.Identity?.Name ?? "anónimo";

        _users.Add(Context.ConnectionId, username);
        _logger.LogInformation("Conectado: {User} ({ConnId})", username, Context.ConnectionId);

        // 1) Notificar a todos que entró un usuario (mensaje de sistema).
        await Clients.Others.SendAsync("UserConnected", new ChatMessage
        {
            User = "Sistema",
            Type = "system",
            Content = $"{username} se ha unido al chat"
        });

        // 2) Mandar a TODOS la lista actualizada de usuarios conectados.
        await Clients.All.SendAsync("UsersOnline", _users.GetAll());

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Se ejecuta automáticamente cuando un cliente se desconecta (cierre normal o caída).
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = _users.GetUsername(Context.ConnectionId) ?? "anónimo";
        _users.Remove(Context.ConnectionId);

        _logger.LogInformation("Desconectado: {User}", username);

        await Clients.Others.SendAsync("UserDisconnected", new ChatMessage
        {
            User = "Sistema",
            Type = "system",
            Content = $"{username} salió del chat"
        });

        await Clients.All.SendAsync("UsersOnline", _users.GetAll());

        await base.OnDisconnectedAsync(exception);
    }

    // =====================================================
    // Métodos invocables por el cliente
    // =====================================================

    /// <summary>
    /// Envía un mensaje a TODOS los clientes conectados.
    /// Cliente -&gt; connection.invoke("SendMessage", "hola a todos")
    /// </summary>
    public async Task SendMessage(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return;

        var username = _users.GetUsername(Context.ConnectionId) ?? "anónimo";

        var message = new ChatMessage
        {
            User = username,
            Content = content.Trim(),
            Type = "public"
        };

        // Clients.All -> manda a todos los clientes conectados a este Hub.
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    /// <summary>
    /// Envía un mensaje privado a un usuario específico (1 a 1).
    /// Cliente -&gt; connection.invoke("SendPrivateMessage", "juan", "hola")
    /// </summary>
    public async Task SendPrivateMessage(string toUsername, string content)
    {
        if (string.IsNullOrWhiteSpace(toUsername) || string.IsNullOrWhiteSpace(content))
            return;

        var fromUsername = _users.GetUsername(Context.ConnectionId) ?? "anónimo";
        var toConnectionId = _users.GetConnectionId(toUsername);

        if (toConnectionId is null)
        {
            // El destinatario no está conectado: avisamos solo al emisor.
            await Clients.Caller.SendAsync("ReceiveMessage", new ChatMessage
            {
                User = "Sistema",
                Type = "system",
                Content = $"El usuario '{toUsername}' no está conectado."
            });
            return;
        }

        var message = new ChatMessage
        {
            User = fromUsername,
            Content = content.Trim(),
            Type = "private",
            To = toUsername
        };

        // Mandar al destinatario y devolver una copia al emisor.
        await Clients.Client(toConnectionId).SendAsync("ReceiveMessage", message);
        await Clients.Caller.SendAsync("ReceiveMessage", message);
    }

    /// <summary>
    /// Notifica al resto que el usuario actual está escribiendo.
    /// Cliente -&gt; connection.invoke("Typing", true / false)
    /// </summary>
    public async Task Typing(bool isTyping)
    {
        var username = _users.GetUsername(Context.ConnectionId) ?? "anónimo";

        // Clients.Others -> a todos MENOS al que llamó.
        await Clients.Others.SendAsync("UserTyping", new
        {
            User = username,
            IsTyping = isTyping
        });
    }
}
