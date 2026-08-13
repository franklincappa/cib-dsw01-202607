using ChatSignalR.Api.Models;
using ChatSignalR.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatSignalR.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    // Usuarios "demo" en memoria. Para una clase es suficiente; en un proyecto
    // real se reemplaza por una tabla en SQL Server / Oracle / Mongo.
    private static readonly Dictionary<string, string> _users = new(StringComparer.OrdinalIgnoreCase)
    {
        { "admin",   "admin123" },
        { "juan",    "juan123" },
        { "maria",   "maria123" },
        { "carlos",  "carlos123" }
    };

    public AuthController(ITokenService tokenService) => _tokenService = tokenService;

    /// <summary>
    /// POST /api/auth/login
    /// Devuelve un JWT que el cliente usará para conectarse al Hub.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        if (!_users.TryGetValue(request.Username, out var expected) ||
            expected != request.Password)
        {
            return Unauthorized(new { message = "Usuario o contraseña inválidos" });
        }

        var (token, expiresAt) = _tokenService.CreateToken(request.Username);

        return Ok(new LoginResponse
        {
            Token = token,
            Username = request.Username,
            ExpiresAt = expiresAt
        });
    }

    /// <summary>
    /// GET /api/auth/users
    /// Devuelve los usuarios disponibles para hacer la demo (sin contraseñas).
    /// </summary>
    [HttpGet("users")]
    public IActionResult AvailableUsers()
        => Ok(_users.Keys.Select(u => new { username = u }));
}
