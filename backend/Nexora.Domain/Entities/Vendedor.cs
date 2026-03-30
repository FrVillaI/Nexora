namespace Nexora.Domain.Entities;

public class Vendedor
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
    public bool EsAdmin { get; set; }
    public bool PuedeDescuento { get; set; }
    public bool PuedeModificarPrecio { get; set; }
    public bool Estado { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
}
