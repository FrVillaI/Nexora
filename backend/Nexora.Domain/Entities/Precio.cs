using Nexora.Domain.Enums;

namespace Nexora.Domain.Entities;

public class Precio
{
    public int Id { get; set; }
    public int IdProducto { get; set; }
    public TipoPrecio TipoPrecio { get; set; }
    public decimal Valor { get; set; }
    public bool IncluyeIva { get; set; } = true;

    public Producto Producto { get; set; } = null!;
}
