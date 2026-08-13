namespace ChatSignalR.Api.Models;

/// <summary>
/// DTO que viaja del servidor al cliente cuando llega un mensaje al chat.
/// </summary>
public class ChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string User { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    /// <summary>"public" para sala global, "private" para 1 a 1, "system" para avisos</summary>
    public string Type { get; set; } = "public";

    /// <summary>Cuando es privado, indica el destinatario.</summary>
    public string? To { get; set; }
}

public class ConnectedUser
{
    public string ConnectionId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
}
