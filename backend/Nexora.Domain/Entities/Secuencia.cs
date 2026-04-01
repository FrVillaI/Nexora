using System.ComponentModel.DataAnnotations.Schema;
using Nexora.Domain.Enums;

namespace Nexora.Domain.Entities;

[Table("secuencias")]
public class Secuencia
{
    [Column("id")]
    public int Id { get; set; }
    [Column("tipo_documento")]
    public TipoDocumento TipoDocumento { get; set; }
    [Column("serie")]
    public string Serie { get; set; } = string.Empty;
    [Column("numero_actual")]
    public int NumeroActual { get; set; }
    [Column("formato")]
    public string? Formato { get; set; }
    [Column("estado")]
    public bool Estado { get; set; } = true;

    public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
}
