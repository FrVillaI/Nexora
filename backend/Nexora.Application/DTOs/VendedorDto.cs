namespace Nexora.Application.DTOs;

public class VendedorDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool EsAdmin { get; set; }
    public bool PuedeDescuento { get; set; }
    public bool PuedeModificarPrecio { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
}

public class CrearVendedorDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
    public bool EsAdmin { get; set; }
    public bool PuedeDescuento { get; set; }
    public bool PuedeModificarPrecio { get; set; }
}

public class LoginDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public VendedorDto? Vendedor { get; set; }
}
