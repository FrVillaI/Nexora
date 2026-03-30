namespace Nexora.Application.DTOs;

public class ProveedorDto
{
    public int Id { get; set; }
    public string? Identificacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Estado { get; set; }
}

public class CrearProveedorDto
{
    public string? Identificacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
}
