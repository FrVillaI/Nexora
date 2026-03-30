using Nexora.Domain.Enums;

namespace Nexora.Domain.Entities;

public class Documento
{
    public int Id { get; set; }
    public TipoDocumento Tipo { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int? IdSecuencia { get; set; }
    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public int IdVendedor { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string? FormaPago { get; set; }
    public string? Observacion { get; set; }
    public decimal Subtotal { get; set; }
    public decimal IvaTotal { get; set; }
    public decimal DescuentoTotal { get; set; }
    public decimal Total { get; set; }
    public EstadoDocumento Estado { get; set; } = EstadoDocumento.BORRADOR;

    public Secuencia? Secuencia { get; set; }
    public Cliente? Cliente { get; set; }
    public Proveedor? Proveedor { get; set; }
    public Vendedor Vendedor { get; set; } = null!;
    public ICollection<DocumentoDetalle> Detalles { get; set; } = new List<DocumentoDetalle>();
}
