using Nexora.Application.DTOs;

namespace Nexora.Application.Interfaces;

public interface IProveedorService
{
    Task<IEnumerable<ProveedorDto>> GetAllAsync();
    Task<ProveedorDto?> GetByIdAsync(int id);
    Task<ProveedorDto> CreateAsync(CrearProveedorDto dto);
    Task<ProveedorDto> UpdateAsync(int id, CrearProveedorDto dto);
    Task<bool> DeleteAsync(int id);
}
