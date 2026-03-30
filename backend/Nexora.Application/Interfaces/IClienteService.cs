using Nexora.Application.DTOs;

namespace Nexora.Application.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteDto>> GetAllAsync();
    Task<ClienteDto?> GetByIdAsync(int id);
    Task<ClienteDto> CreateAsync(CrearClienteDto dto);
    Task<ClienteDto> UpdateAsync(int id, CrearClienteDto dto);
    Task<bool> DeleteAsync(int id);
}
