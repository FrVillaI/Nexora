using System.ComponentModel.DataAnnotations.Schema;
using Nexora.Domain.Enums;

namespace Nexora.Domain.Entities;

[Table("precios")]
public class Precio
{
    [Column("id")]
    public int Id { get; set; }
    [Column("id_producto")]
    public int IdProducto { get; set; }
    [Column("tipo_precio")]
    public TipoPrecio TipoPrecio { get; set; }
    [Column("valor")]
    public decimal Valor { get; set; }
    [Column("incluye_iva")]
    public bool IncluyeIva { get; set; } = true;

    public Producto Producto { get; set; } = null!;
}
