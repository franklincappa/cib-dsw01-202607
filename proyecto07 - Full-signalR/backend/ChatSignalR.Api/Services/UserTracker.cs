using System.Collections.Concurrent;
using ChatSignalR.Api.Models;

namespace ChatSignalR.Api.Services;

public interface IUserTracker
{
    void Add(string connectionId, string username);
    void Remove(string connectionId);
    string? GetUsername(string connectionId);
    string? GetConnectionId(string username);
    IReadOnlyCollection<ConnectedUser> GetAll();
}

/// <summary>
/// Singleton en memoria que mantiene la relación connectionId &lt;-&gt; username.
/// Para producción se reemplazaría por Redis o una base de datos compartida.
/// </summary>
public class UserTracker : IUserTracker
{
    private readonly ConcurrentDictionary<string, ConnectedUser> _byConnId = new();

    public void Add(string connectionId, string username)
    {
        _byConnId[connectionId] = new ConnectedUser
        {
            ConnectionId = connectionId,
            Username = username
        };
    }

    public void Remove(string connectionId) =>
        _byConnId.TryRemove(connectionId, out _);

    public string? GetUsername(string connectionId) =>
        _byConnId.TryGetValue(connectionId, out var u) ? u.Username : null;

    public string? GetConnectionId(string username) =>
        _byConnId.Values
            .FirstOrDefault(x => string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase))
            ?.ConnectionId;

    public IReadOnlyCollection<ConnectedUser> GetAll() =>
        _byConnId.Values.OrderBy(u => u.Username).ToList().AsReadOnly();
}
