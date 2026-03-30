namespace Nexora.Domain.Entities;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? CodigoBarras { get; set; }
    public decimal Stock { get; set; }
    public decimal IvaPorcentaje { get; set; }
    public bool Estado { get; set; } = true;

    public ICollection<Precio> Precios { get; set; } = new List<Precio>();
    public ICollection<DocumentoDetalle> DocumentoDetalles { get; set; } = new List<DocumentoDetalle>();
    public ICollection<Kardex> Kardexes { get; set; } = new List<Kardex>();
}
