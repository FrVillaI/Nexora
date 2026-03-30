using Nexora.Application.DTOs;

namespace Nexora.Application.Interfaces;

public interface IDocumentoService
{
    Task<IEnumerable<DocumentoDto>> GetAllAsync(string? tipo = null, int? clienteId = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null, int page = 1, int pageSize = 20);
    Task<DocumentoDto?> GetByIdAsync(int id);
    Task<DocumentoDto> CreateAsync(CrearDocumentoDto dto, int idVendedor);
    Task<DocumentoDto> UpdateEstadoAsync(int id, string estado);
    Task<int> GetTotalCountAsync(string? tipo = null, int? clienteId = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null);
}
