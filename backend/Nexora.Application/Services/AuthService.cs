using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Nexora.Application.DTOs;
using Nexora.Application.Interfaces;
using Nexora.Domain.Entities;
using Nexora.Infrastructure.Data;
using BCrypt.Net;

namespace Nexora.Application.Services;

public class AuthService : IAuthService
{
    private readonly NexoraDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(NexoraDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var vendedor = await _context.Vendedores
            .FirstOrDefaultAsync(v => v.Nombre == dto.Nombre && v.Estado);

        if (vendedor == null || !BCrypt.Net.BCrypt.Verify(dto.Clave, vendedor.Clave))
            return null;

        var token = GenerateJwtToken(vendedor);

        return new AuthResponseDto
        {
            Token = token,
            Vendedor = _mapper.Map<VendedorDto>(vendedor)
        };
    }

    public async Task<VendedorDto> CreateAsync(CrearVendedorDto dto)
    {
        var existingVendedor = await _context.Vendedores
            .FirstOrDefaultAsync(v => v.Nombre == dto.Nombre);

        if (existingVendedor != null)
            throw new Exception("Ya existe un vendedor con este nombre");

        var vendedor = new Vendedor
        {
            Nombre = dto.Nombre,
            Clave = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
            EsAdmin = dto.EsAdmin,
            PuedeDescuento = dto.PuedeDescuento,
            PuedeModificarPrecio = dto.PuedeModificarPrecio,
            Estado = true
        };

        _context.Vendedores.Add(vendedor);
        await _context.SaveChangesAsync();
        return _mapper.Map<VendedorDto>(vendedor);
    }

    private string GenerateJwtToken(Vendedor vendedor)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? "NexoraSuperSecretKey2024!@#$%^&*()"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, vendedor.Id.ToString()),
            new Claim(ClaimTypes.Name, vendedor.Nombre),
            new Claim(ClaimTypes.Role, vendedor.EsAdmin ? "ADMIN" : "VENDEDOR")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "Nexora",
            audience: _configuration["Jwt:Audience"] ?? "NexoraAPI",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
