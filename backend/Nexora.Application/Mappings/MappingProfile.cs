using AutoMapper;
using Nexora.Application.DTOs;
using Nexora.Domain.Entities;
using Nexora.Domain.Enums;

namespace Nexora.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Producto, ProductoDto>()
            .ForMember(dest => dest.Precios, opt => opt.MapFrom(src => src.Precios.Select(p => new PrecioDto
            {
                Id = p.Id,
                TipoPrecio = p.TipoPrecio.ToString(),
                Valor = p.Valor,
                IncluyeIva = p.IncluyeIva
            })));

        CreateMap<Precio, PrecioDto>()
            .ForMember(dest => dest.TipoPrecio, opt => opt.MapFrom(src => src.TipoPrecio.ToString()));

        CreateMap<Cliente, ClienteDto>();
        CreateMap<Proveedor, ProveedorDto>();

        CreateMap<Vendedor, VendedorDto>()
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion));

        CreateMap<Documento, DocumentoDto>()
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString()))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()))
            .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre : null))
            .ForMember(dest => dest.ProveedorNombre, opt => opt.MapFrom(src => src.Proveedor != null ? src.Proveedor.Nombre : null))
            .ForMember(dest => dest.VendedorNombre, opt => opt.MapFrom(src => src.Vendedor != null ? src.Vendedor.Nombre : null))
            .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles.Select(d => new DocumentoDetalleDto
            {
                Id = d.Id,
                IdProducto = d.IdProducto,
                ProductoNombre = d.Producto != null ? d.Producto.Nombre : null,
                ProductoCodigo = d.Producto != null ? d.Producto.CodigoBarras : null,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Descuento = d.Descuento,
                Iva = d.Iva,
                Total = d.Total
            })));

        CreateMap<DocumentoDetalle, DocumentoDetalleDto>()
            .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto != null ? src.Producto.Nombre : null))
            .ForMember(dest => dest.ProductoCodigo, opt => opt.MapFrom(src => src.Producto != null ? src.Producto.CodigoBarras : null));
    }
}
