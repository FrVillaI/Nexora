using System.ComponentModel.DataAnnotations.Schema;
namespace Nexora.Domain.Entities;

[Table("documento_detalle")]
public class DocumentoDetalle
{
    [Column("id")]
    public int Id { get; set; }
    [Column("id_documento")]
    public int IdDocumento { get; set; }
    [Column("id_producto")]
    public int IdProducto { get; set; }
    [Column("cantidad")]
    public decimal Cantidad { get; set; }
    [Column("precio_unitario")]
    public decimal PrecioUnitario { get; set; }
    [Column("descuento")]
    public decimal Descuento { get; set; }
    [Column("iva")]
    public decimal Iva { get; set; }
    [Column("total")]
    public decimal Total { get; set; }

    public Documento Documento { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
