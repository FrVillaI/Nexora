using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nexora.Application.DTOs;
using Nexora.Application.Interfaces;
using Nexora.Domain.Entities;
using Nexora.Infrastructure.Data;

namespace Nexora.Application.Services;

public class ClienteService : IClienteService
{
    private readonly NexoraDbContext _context;
    private readonly IMapper _mapper;

    public ClienteService(NexoraDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClienteDto>> GetAllAsync()
    {
        var clientes = await _context.Clientes
            .Where(c => c.Estado)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
    }

    public async Task<ClienteDto?> GetByIdAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> CreateAsync(CrearClienteDto dto)
    {
        var cliente = new Cliente
        {
            Identificacion = dto.Identificacion,
            Nombre = dto.Nombre,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Estado = true
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> UpdateAsync(int id, CrearClienteDto dto)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) throw new Exception("Cliente no encontrado");

        cliente.Identificacion = dto.Identificacion;
        cliente.Nombre = dto.Nombre;
        cliente.Direccion = dto.Direccion;
        cliente.Telefono = dto.Telefono;
        cliente.Email = dto.Email;

        await _context.SaveChangesAsync();
        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return false;

        cliente.Estado = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
