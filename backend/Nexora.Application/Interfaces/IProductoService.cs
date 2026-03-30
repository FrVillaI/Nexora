using Nexora.Application.DTOs;

namespace Nexora.Application.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>> GetAllAsync();
    Task<ProductoDto?> GetByIdAsync(int id);
    Task<ProductoDto> CreateAsync(CrearProductoDto dto);
    Task<ProductoDto> UpdateAsync(int id, CrearProductoDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStockAsync(int id, decimal cantidad);
}
