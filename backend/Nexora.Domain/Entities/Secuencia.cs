using Nexora.Domain.Enums;

namespace Nexora.Domain.Entities;

public class Secuencia
{
    public int Id { get; set; }
    public TipoDocumento TipoDocumento { get; set; }
    public string Serie { get; set; } = string.Empty;
    public int NumeroActual { get; set; }
    public string? Formato { get; set; }
    public bool Estado { get; set; } = true;

    public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
}
