using System.ComponentModel.DataAnnotations.Schema;
namespace Nexora.Domain.Entities;

[Table("empresa")]
public class Empresa
{
    [Column("id")]
    public int Id { get; set; }
    [Column("ruc")]
    public string? Ruc { get; set; }
    [Column("razon_social")]
    public string? RazonSocial { get; set; }
    [Column("nombre_comercial")]
    public string? NombreComercial { get; set; }
    [Column("ciudad")]
    public string? Ciudad { get; set; }
    [Column("direccion")]
    public string? Direccion { get; set; }
    [Column("telefono")]
    public string? Telefono { get; set; }
    [Column("email")]
    public string? Email { get; set; }
}
