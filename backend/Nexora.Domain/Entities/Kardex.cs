using Nexora.Domain.Enums;

namespace Nexora.Domain.Entities;

public class Kardex
{
    public int Id { get; set; }
    public int IdProducto { get; set; }
    public DateTime Fecha { get; set; }
    public TipoMovimiento Tipo { get; set; }
    public MotivoMovimiento Motivo { get; set; }
    public decimal Cantidad { get; set; }
    public decimal? CostoUnitario { get; set; }
    public int? IdDocumento { get; set; }

    public Producto Producto { get; set; } = null!;
    public Documento? Documento { get; set; }
}
