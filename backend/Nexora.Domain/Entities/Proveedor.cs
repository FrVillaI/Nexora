using System.ComponentModel.DataAnnotations.Schema;
namespace Nexora.Domain.Entities;

[Table("proveedores")]
public class Proveedor
{
    [Column("id")]
    public int Id { get; set; }
    [Column("identificacion")]
    public string? Identificacion { get; set; }
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [Column("direccion")]
    public string? Direccion { get; set; }
    [Column("telefono")]
    public string? Telefono { get; set; }
    [Column("email")]
    public string? Email { get; set; }
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    [Column("estado")]
    public bool Estado { get; set; } = true;

    public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
}
