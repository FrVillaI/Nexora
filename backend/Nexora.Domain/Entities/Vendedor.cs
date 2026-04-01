using System.ComponentModel.DataAnnotations.Schema;
using Nexora.Domain.Entities;

[Table("vendedores")]
public class Vendedor
{
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("clave")]
    public string Clave { get; set; } = string.Empty;

    [Column("es_admin")]
    public bool EsAdmin { get; set; }

    [Column("puede_descuento")]
    public bool PuedeDescuento { get; set; }

    [Column("puede_modificar_precio")]
    public bool PuedeModificarPrecio { get; set; }

    [Column("estado")]
    public bool Estado { get; set; } = true;

    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
}