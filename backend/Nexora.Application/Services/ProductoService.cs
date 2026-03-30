using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nexora.Application.DTOs;
using Nexora.Application.Interfaces;
using Nexora.Domain.Entities;
using Nexora.Domain.Enums;
using Nexora.Infrastructure.Data;

namespace Nexora.Application.Services;

public class ProductoService : IProductoService
{
    private readonly NexoraDbContext _context;
    private readonly IMapper _mapper;

    public ProductoService(NexoraDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductoDto>> GetAllAsync()
    {
        var productos = await _context.Productos
            .Include(p => p.Precios)
            .Where(p => p.Estado)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ProductoDto>>(productos);
    }

    public async Task<ProductoDto?> GetByIdAsync(int id)
    {
        var producto = await _context.Productos
            .Include(p => p.Precios)
            .FirstOrDefaultAsync(p => p.Id == id);
        return producto == null ? null : _mapper.Map<ProductoDto>(producto);
    }

    public async Task<ProductoDto> CreateAsync(CrearProductoDto dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            CodigoBarras = dto.CodigoBarras,
            Stock = dto.Stock,
            IvaPorcentaje = dto.IvaPorcentaje,
            Estado = true
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        if (dto.Precios != null && dto.Precios.Any())
        {
            foreach (var precioDto in dto.Precios)
            {
                var precio = new Precio
                {
                    IdProducto = producto.Id,
                    TipoPrecio = Enum.Parse<TipoPrecio>(precioDto.TipoPrecio),
                    Valor = precioDto.Valor,
                    IncluyeIva = precioDto.IncluyeIva
                };
                _context.Precios.Add(precio);
            }
            await _context.SaveChangesAsync();
        }

        return await GetByIdAsync(producto.Id) ?? throw new Exception("Error al crear el producto");
    }

    public async Task<ProductoDto> UpdateAsync(int id, CrearProductoDto dto)
    {
        var producto = await _context.Productos
            .Include(p => p.Precios)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null) throw new Exception("Producto no encontrado");

        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.CodigoBarras = dto.CodigoBarras;
        producto.IvaPorcentaje = dto.IvaPorcentaje;

        _context.Precios.RemoveRange(producto.Precios);

        if (dto.Precios != null && dto.Precios.Any())
        {
            foreach (var precioDto in dto.Precios)
            {
                var precio = new Precio
                {
                    IdProducto = producto.Id,
                    TipoPrecio = Enum.Parse<TipoPrecio>(precioDto.TipoPrecio),
                    Valor = precioDto.Valor,
                    IncluyeIva = precioDto.IncluyeIva
                };
                _context.Precios.Add(precio);
            }
        }

        await _context.SaveChangesAsync();
        return await GetByIdAsync(id) ?? throw new Exception("Error al actualizar el producto");
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return false;

        producto.Estado = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStockAsync(int id, decimal cantidad)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return false;

        producto.Stock += cantidad;
        if (producto.Stock < 0) throw new Exception("Stock no puede ser negativo");

        await _context.SaveChangesAsync();
        return true;
    }
}
