using System.ComponentModel.DataAnnotations.Schema;
namespace Nexora.Domain.Entities;

[Table("bloqueos")]
public class Bloqueo
{
    [Column("id")]
    public int Id { get; set; }
    [Column("fecha_bloqueo_ventas")]
    public DateTime? FechaBloqueoVentas { get; set; }
    [Column("fecha_bloqueo_compras")]
    public DateTime? FechaBloqueoCompras { get; set; }
    [Column("fecha_bloqueo_general")]
    public DateTime? FechaBloqueoGeneral { get; set; }
}
