using System.ComponentModel.DataAnnotations.Schema;
namespace Nexora.Domain.Entities;

[Table("productos")]
public class Producto
{
    [Column("id")]
    public int Id { get; set; }
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [Column("descripcion")]
    public string? Descripcion { get; set; }
    [Column("codigo_barras")]
    public string? CodigoBarras { get; set; }
    [Column("stock")]
    public decimal Stock { get; set; }
    [Column("iva_porcentaje")]
    public decimal IvaPorcentaje { get; set; }
    [Column("estado")]
    public bool Estado { get; set; } = true;

    public ICollection<Precio> Precios { get; set; } = new List<Precio>();
    public ICollection<DocumentoDetalle> DocumentoDetalles { get; set; } = new List<DocumentoDetalle>();
    public ICollection<Kardex> Kardexes { get; set; } = new List<Kardex>();
}
