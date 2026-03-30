using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nexora.Application.DTOs;
using Nexora.Application.Interfaces;
using Nexora.Domain.Entities;
using Nexora.Domain.Enums;
using Nexora.Infrastructure.Data;

namespace Nexora.Application.Services;

public class DocumentoService : IDocumentoService
{
    private readonly NexoraDbContext _context;
    private readonly IMapper _mapper;

    public DocumentoService(NexoraDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DocumentoDto>> GetAllAsync(
        string? tipo = null,
        int? clienteId = null,
        DateTime? fechaDesde = null,
        DateTime? fechaHasta = null,
        int page = 1,
        int pageSize = 20)
    {
        var query = _context.Documentos
            .Include(d => d.Cliente)
            .Include(d => d.Proveedor)
            .Include(d => d.Vendedor)
            .Include(d => d.Detalles)
                .ThenInclude(det => det.Producto)
            .AsQueryable();

        if (!string.IsNullOrEmpty(tipo) && Enum.TryParse<TipoDocumento>(tipo, out var tipoEnum))
            query = query.Where(d => d.Tipo == tipoEnum);

        if (clienteId.HasValue)
            query = query.Where(d => d.IdCliente == clienteId.Value);

        if (fechaDesde.HasValue)
            query = query.Where(d => d.FechaEmision >= fechaDesde.Value);

        if (fechaHasta.HasValue)
            query = query.Where(d => d.FechaEmision <= fechaHasta.Value);

        var documentos = await query
            .OrderByDescending(d => d.FechaEmision)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<DocumentoDto>>(documentos);
    }

    public async Task<int> GetTotalCountAsync(
        string? tipo = null,
        int? clienteId = null,
        DateTime? fechaDesde = null,
        DateTime? fechaHasta = null)
    {
        var query = _context.Documentos.AsQueryable();

        if (!string.IsNullOrEmpty(tipo) && Enum.TryParse<TipoDocumento>(tipo, out var tipoEnum))
            query = query.Where(d => d.Tipo == tipoEnum);

        if (clienteId.HasValue)
            query = query.Where(d => d.IdCliente == clienteId.Value);

        if (fechaDesde.HasValue)
            query = query.Where(d => d.FechaEmision >= fechaDesde.Value);

        if (fechaHasta.HasValue)
            query = query.Where(d => d.FechaEmision <= fechaHasta.Value);

        return await query.CountAsync();
    }

    public async Task<DocumentoDto?> GetByIdAsync(int id)
    {
        var documento = await _context.Documentos
            .Include(d => d.Cliente)
            .Include(d => d.Proveedor)
            .Include(d => d.Vendedor)
            .Include(d => d.Detalles)
                .ThenInclude(det => det.Producto)
            .FirstOrDefaultAsync(d => d.Id == id);

        return documento == null ? null : _mapper.Map<DocumentoDto>(documento);
    }

    public async Task<DocumentoDto> CreateAsync(CrearDocumentoDto dto, int idVendedor)
    {
        var tipo = Enum.Parse<TipoDocumento>(dto.Tipo);

        ValidarDocumento(dto, tipo);

        var secuencia = await _context.Secuencias
            .FirstOrDefaultAsync(s => s.TipoDocumento == tipo && s.Estado);

        if (secuencia == null)
            throw new Exception($"No existe secuencia configurada para {dto.Tipo}");

        var numero = GenerarNumero(secuencia);

        decimal subtotal = 0, ivaTotal = 0, descuentoTotal = 0, total = 0;

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var detalle in dto.Detalles ?? new List<CrearDocumentoDetalleDto>())
            {
                var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                if (producto == null)
                    throw new Exception($"Producto con ID {detalle.IdProducto} no encontrado");

                var descuento = detalle.Descuento;
                var precioBase = detalle.PrecioUnitario * detalle.Cantidad;
                var iva = (precioBase - descuento) * (producto.IvaPorcentaje / 100);

                subtotal += precioBase;
                descuentoTotal += descuento;
                ivaTotal += iva;

                if (tipo == TipoDocumento.FACTURA || tipo == TipoDocumento.PRESTAMO)
                {
                    producto.Stock -= detalle.Cantidad;
                    if (producto.Stock < 0)
                        throw new Exception($"Stock insuficiente para el producto {producto.Nombre}");

                    _context.Kardexes.Add(new Kardex
                    {
                        IdProducto = producto.Id,
                        Fecha = dto.FechaEmision,
                        Tipo = TipoMovimiento.SALIDA,
                        Motivo = tipo == TipoDocumento.FACTURA ? MotivoMovimiento.VENTA : MotivoMovimiento.PRESTAMO,
                        Cantidad = detalle.Cantidad,
                        CostoUnitario = detalle.PrecioUnitario
                    });
                }
                else if (tipo == TipoDocumento.COMPRA)
                {
                    producto.Stock += detalle.Cantidad;

                    _context.Kardexes.Add(new Kardex
                    {
                        IdProducto = producto.Id,
                        Fecha = dto.FechaEmision,
                        Tipo = TipoMovimiento.ENTRADA,
                        Motivo = MotivoMovimiento.COMPRA,
                        Cantidad = detalle.Cantidad,
                        CostoUnitario = detalle.PrecioUnitario
                    });
                }
            }

            total = subtotal + ivaTotal - descuentoTotal;

            var documento = new Documento
            {
                Tipo = tipo,
                Numero = numero,
                IdSecuencia = secuencia.Id,
                IdCliente = dto.IdCliente,
                IdProveedor = dto.IdProveedor,
                IdVendedor = idVendedor,
                FechaEmision = dto.FechaEmision,
                FechaVencimiento = dto.FechaVencimiento,
                FormaPago = dto.FormaPago,
                Observacion = dto.Observacion,
                Subtotal = subtotal,
                IvaTotal = ivaTotal,
                DescuentoTotal = descuentoTotal,
                Total = total,
                Estado = EstadoDocumento.EMITIDO
            };

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            foreach (var detalle in dto.Detalles ?? new List<CrearDocumentoDetalleDto>())
            {
                var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                var precioBase = detalle.PrecioUnitario * detalle.Cantidad;
                var descuento = detalle.Descuento;
                var iva = (precioBase - descuento) * ((producto?.IvaPorcentaje ?? 0) / 100);

                _context.DocumentoDetalles.Add(new DocumentoDetalle
                {
                    IdDocumento = documento.Id,
                    IdProducto = detalle.IdProducto,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario,
                    Descuento = descuento,
                    Iva = iva,
                    Total = precioBase - descuento + iva
                });
            }

            secuencia.NumeroActual++;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetByIdAsync(documento.Id) ?? throw new Exception("Error al obtener el documento creado");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<DocumentoDto> UpdateEstadoAsync(int id, string estado)
    {
        var documento = await _context.Documentos
            .Include(d => d.Detalles)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (documento == null)
            throw new Exception("Documento no encontrado");

        var nuevoEstado = Enum.Parse<EstadoDocumento>(estado);

        if (documento.Estado == nuevoEstado)
            return await GetByIdAsync(id) ?? throw new Exception("Error al actualizar estado");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (documento.Estado == EstadoDocumento.EMITIDO && nuevoEstado == EstadoDocumento.ANULADO)
            {
                foreach (var detalle in documento.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                    if (producto == null) continue;

                    if (documento.Tipo == TipoDocumento.FACTURA || documento.Tipo == TipoDocumento.PRESTAMO)
                    {
                        producto.Stock += detalle.Cantidad;

                        _context.Kardexes.Add(new Kardex
                        {
                            IdProducto = producto.Id,
                            Fecha = DateTime.UtcNow,
                            Tipo = TipoMovimiento.ENTRADA,
                            Motivo = MotivoMovimiento.AJUSTE,
                            Cantidad = detalle.Cantidad,
                            CostoUnitario = detalle.PrecioUnitario
                        });
                    }
                    else if (documento.Tipo == TipoDocumento.COMPRA)
                    {
                        producto.Stock -= detalle.Cantidad;

                        _context.Kardexes.Add(new Kardex
                        {
                            IdProducto = producto.Id,
                            Fecha = DateTime.UtcNow,
                            Tipo = TipoMovimiento.SALIDA,
                            Motivo = MotivoMovimiento.AJUSTE,
                            Cantidad = detalle.Cantidad,
                            CostoUnitario = detalle.PrecioUnitario
                        });
                    }
                }
            }

            documento.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetByIdAsync(id) ?? throw new Exception("Error al actualizar estado");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private void ValidarDocumento(CrearDocumentoDto dto, TipoDocumento tipo)
    {
        if (dto.Detalles == null || !dto.Detalles.Any())
            throw new Exception("El documento debe tener al menos un detalle");

        if (tipo == TipoDocumento.FACTURA || tipo == TipoDocumento.PROFORMA || tipo == TipoDocumento.PRESTAMO)
        {
            if (!dto.IdCliente.HasValue)
                throw new Exception("Para este tipo de documento debe seleccionar un cliente");
        }
        else if (tipo == TipoDocumento.COMPRA)
        {
            if (!dto.IdProveedor.HasValue)
                throw new Exception("Para compras debe seleccionar un proveedor");
        }
    }

    private string GenerarNumero(Secuencia secuencia)
    {
        var numero = secuencia.NumeroActual + 1;
        var formato = secuencia.Formato ?? "001-001-0000001";

        var partes = formato.Split('-');
        if (partes.Length >= 3)
        {
            var serie = partes[0] + "-" + partes[1] + "-";
            var numStr = numero.ToString().PadLeft(partes[2].Length, '0');
            return serie + numStr;
        }

        return numero.ToString();
    }
}
