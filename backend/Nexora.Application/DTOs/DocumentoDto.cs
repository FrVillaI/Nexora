namespace Nexora.Application.DTOs;

public class DocumentoDto
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public int? IdCliente { get; set; }
    public string? ClienteNombre { get; set; }
    public int? IdProveedor { get; set; }
    public string? ProveedorNombre { get; set; }
    public int IdVendedor { get; set; }
    public string? VendedorNombre { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string? FormaPago { get; set; }
    public string? Observacion { get; set; }
    public decimal Subtotal { get; set; }
    public decimal IvaTotal { get; set; }
    public decimal DescuentoTotal { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
    public List<DocumentoDetalleDto>? Detalles { get; set; }
}

public class DocumentoDetalleDto
{
    public int Id { get; set; }
    public int IdProducto { get; set; }
    public string? ProductoNombre { get; set; }
    public string? ProductoCodigo { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal Iva { get; set; }
    public decimal Total { get; set; }
}

public class CrearDocumentoDto
{
    public string Tipo { get; set; } = string.Empty;
    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string? FormaPago { get; set; }
    public string? Observacion { get; set; }
    public List<CrearDocumentoDetalleDto>? Detalles { get; set; }
}

public class CrearDocumentoDetalleDto
{
    public int IdProducto { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal Iva { get; set; }
}
