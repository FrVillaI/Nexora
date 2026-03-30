namespace Nexora.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string? Identificacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public bool Estado { get; set; } = true;

    public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
}
