namespace Nexora.Domain.Entities;

public class DocumentoDetalle
{
    public int Id { get; set; }
    public int IdDocumento { get; set; }
    public int IdProducto { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal Iva { get; set; }
    public decimal Total { get; set; }

    public Documento Documento { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
