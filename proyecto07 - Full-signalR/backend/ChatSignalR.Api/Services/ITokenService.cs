namespace ChatSignalR.Api.Services;

public interface ITokenService
{
    /// <summary>
    /// Genera un JWT firmado para el usuario indicado.
    /// </summary>
    (string token, DateTime expiresAt) CreateToken(string username);
}
