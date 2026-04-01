using System.ComponentModel.DataAnnotations.Schema;
using Nexora.Domain.Enums;

namespace Nexora.Domain.Entities;

[Table("kardex")]
public class Kardex
{
    [Column("id")]
    public int Id { get; set; }
    [Column("id_producto")]
    public int IdProducto { get; set; }
    [Column("fecha")]
    public DateTime Fecha { get; set; }
    [Column("tipo")]
    public TipoMovimiento Tipo { get; set; }
    [Column("motivo")]
    public MotivoMovimiento Motivo { get; set; }
    [Column("cantidad")]
    public decimal Cantidad { get; set; }
    [Column("costo_unitario")]
    public decimal? CostoUnitario { get; set; }
    [Column("id_documento")]
    public int? IdDocumento { get; set; }

    public Producto Producto { get; set; } = null!;
    public Documento? Documento { get; set; }
}
