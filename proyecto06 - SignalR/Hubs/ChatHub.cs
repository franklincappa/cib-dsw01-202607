using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs;

/// <summary>
/// Hub principal de SignalR para el chat en tiempo real.
/// Hereda de Hub, que provee la infraestructura de conexiones WebSocket.
/// </summary>
public class ChatHub : Hub
{
    // Diccionario estático para mantener usuarios conectados en memoria
    private static readonly Dictionary<string, string> _connectedUsers = new();

    /// <summary>
    /// Se ejecuta automáticamente cuando un cliente se conecta al hub.
    /// Context.ConnectionId es el ID único de cada conexión WebSocket.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"[+] Cliente conectado: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Se ejecuta automáticamente cuando un cliente se desconecta.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_connectedUsers.TryGetValue(Context.ConnectionId, out var userName))
        {
            _connectedUsers.Remove(Context.ConnectionId);

            // Notificar a TODOS los clientes que el usuario salió
            await Clients.All.SendAsync("UserLeft", userName);
            await BroadcastUserList();

            Console.WriteLine($"[-] Usuario '{userName}' desconectado: {Context.ConnectionId}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Método invocado desde el cliente para registrar un nombre de usuario.
    /// </summary>
    /// <param name="userName">Nombre elegido por el usuario</param>
    public async Task RegisterUser(string userName)
    {
        userName = userName.Trim();

        if (string.IsNullOrEmpty(userName))
            return;

        // Asociar el connectionId con el nombre de usuario
        _connectedUsers[Context.ConnectionId] = userName;

        // Confirmar al cliente que se registró correctamente
        await Clients.Caller.SendAsync("Registered", userName);

        // Notificar a TODOS que hay un nuevo usuario
        await Clients.All.SendAsync("UserJoined", userName);

        // Enviar lista actualizada de usuarios a todos
        await BroadcastUserList();

        Console.WriteLine($"[REG] Usuario registrado: '{userName}' -> {Context.ConnectionId}");
    }

    /// <summary>
    /// Método invocado desde el cliente para enviar un mensaje al chat general.
    /// Clients.All: envía a TODOS los clientes conectados.
    /// Clients.Others: envía a todos EXCEPTO al emisor.
    /// Clients.Caller: envía SOLO al que llamó el método.
    /// </summary>
    /// <param name="message">Texto del mensaje</param>
    public async Task SendMessage(string message)
    {
        if (!_connectedUsers.TryGetValue(Context.ConnectionId, out var userName))
        {
            await Clients.Caller.SendAsync("Error", "Debes registrar tu nombre antes de chatear.");
            return;
        }

        message = message.Trim();
        if (string.IsNullOrEmpty(message)) return;

        var timestamp = DateTime.Now.ToString("HH:mm");

        // Enviar el mensaje a TODOS los clientes (incluyendo al emisor)
        await Clients.All.SendAsync("ReceiveMessage", userName, message, timestamp);

        Console.WriteLine($"[MSG] {userName}: {message}");
    }

    /// <summary>
    /// Envía el mensaje solo a usuarios de un grupo específico.
    /// Ejemplo de uso de GRUPOS en SignalR.
    /// </summary>
    public async Task SendPrivateMessage(string targetConnectionId, string message)
    {
        if (!_connectedUsers.TryGetValue(Context.ConnectionId, out var senderName))
            return;

        var timestamp = DateTime.Now.ToString("HH:mm");

        // Clients.Client(id): envía a UN cliente específico
        await Clients.Client(targetConnectionId).SendAsync("ReceivePrivateMessage", senderName, message, timestamp);
        await Clients.Caller.SendAsync("ReceivePrivateMessage", $"[Tú → privado]", message, timestamp);
    }

    /// <summary>
    /// Notifica a todos que un usuario está escribiendo (typing indicator).
    /// </summary>
    public async Task UserTyping(bool isTyping)
    {
        if (!_connectedUsers.TryGetValue(Context.ConnectionId, out var userName))
            return;

        // Enviar a TODOS excepto al que está escribiendo
        await Clients.Others.SendAsync("UserTyping", userName, isTyping);
    }

    // ─── Métodos privados ────────────────────────────────────────────────────

    private async Task BroadcastUserList()
    {
        var users = _connectedUsers.Values.ToList();
        await Clients.All.SendAsync("UpdateUserList", users);
    }
}
