using Nexora.Application.DTOs;

namespace Nexora.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<VendedorDto> CreateAsync(CrearVendedorDto dto);
}
