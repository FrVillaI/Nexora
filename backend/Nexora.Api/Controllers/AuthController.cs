using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexora.Application.DTOs;
using Nexora.Application.Interfaces;

namespace Nexora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null)
            return Unauthorized(new { mensaje = "Credenciales inválidas" });
        return Ok(result);
    }

    [HttpPost("register")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<VendedorDto>> Register([FromBody] CrearVendedorDto dto)
    {
        try
        {
            var vendedor = await _authService.CreateAsync(dto);
            return CreatedAtAction(nameof(Login), new { }, vendedor);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }


    [HttpGet("generate-hash")]
    [AllowAnonymous]
    public ActionResult<string> GenerateHash(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return Ok(hash);
    }
}
