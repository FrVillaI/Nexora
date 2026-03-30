using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nexora.Application.DTOs;
using Nexora.Application.Interfaces;
using Nexora.Domain.Entities;
using Nexora.Infrastructure.Data;

namespace Nexora.Application.Services;

public class ProveedorService : IProveedorService
{
    private readonly NexoraDbContext _context;
    private readonly IMapper _mapper;

    public ProveedorService(NexoraDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProveedorDto>> GetAllAsync()
    {
        var proveedores = await _context.Proveedores
            .Where(p => p.Estado)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ProveedorDto>>(proveedores);
    }

    public async Task<ProveedorDto?> GetByIdAsync(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        return proveedor == null ? null : _mapper.Map<ProveedorDto>(proveedor);
    }

    public async Task<ProveedorDto> CreateAsync(CrearProveedorDto dto)
    {
        var proveedor = new Proveedor
        {
            Identificacion = dto.Identificacion,
            Nombre = dto.Nombre,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Estado = true
        };

        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProveedorDto>(proveedor);
    }

    public async Task<ProveedorDto> UpdateAsync(int id, CrearProveedorDto dto)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor == null) throw new Exception("Proveedor no encontrado");

        proveedor.Identificacion = dto.Identificacion;
        proveedor.Nombre = dto.Nombre;
        proveedor.Direccion = dto.Direccion;
        proveedor.Telefono = dto.Telefono;
        proveedor.Email = dto.Email;

        await _context.SaveChangesAsync();
        return _mapper.Map<ProveedorDto>(proveedor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor == null) return false;

        proveedor.Estado = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
