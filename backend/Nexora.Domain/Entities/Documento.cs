using System.ComponentModel.DataAnnotations.Schema;
using Nexora.Domain.Enums;

namespace Nexora.Domain.Entities;

[Table("documentos")]
public class Documento
{
    [Column("id")]
    public int Id { get; set; }

    [Column("tipo")]
    public TipoDocumento Tipo { get; set; }

    [Column("numero")]
    public string Numero { get; set; } = string.Empty;

    [Column("id_secuencia")]
    public int? IdSecuencia { get; set; }

    [Column("id_cliente")]
    public int? IdCliente { get; set; }

    [Column("id_proveedor")]
    public int? IdProveedor { get; set; }

    [Column("id_vendedor")]
    public int IdVendedor { get; set; }

    [Column("fecha_emision")]
    public DateTime FechaEmision { get; set; }

    [Column("fecha_vencimiento")]
    public DateTime? FechaVencimiento { get; set; }

    [Column("forma_pago")]
    public string? FormaPago { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }

    [Column("subtotal")]
    public decimal Subtotal { get; set; }

    [Column("iva_total")]
    public decimal IvaTotal { get; set; }

    [Column("descuento_total")]
    public decimal DescuentoTotal { get; set; }

    [Column("total")]
    public decimal Total { get; set; }

    [Column("estado")]
    public EstadoDocumento Estado { get; set; } = EstadoDocumento.BORRADOR;

    // Relaciones
    public Secuencia? Secuencia { get; set; }
    public Cliente? Cliente { get; set; }
    public Proveedor? Proveedor { get; set; }
    public Vendedor Vendedor { get; set; } = null!;
    public ICollection<DocumentoDetalle> Detalles { get; set; } = new List<DocumentoDetalle>();
}