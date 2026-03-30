namespace Nexora.Application.DTOs;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? CodigoBarras { get; set; }
    public decimal Stock { get; set; }
    public decimal IvaPorcentaje { get; set; }
    public bool Estado { get; set; }
    public List<PrecioDto>? Precios { get; set; }
}

public class PrecioDto
{
    public int Id { get; set; }
    public string TipoPrecio { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public bool IncluyeIva { get; set; }
}

public class CrearProductoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? CodigoBarras { get; set; }
    public decimal Stock { get; set; }
    public decimal IvaPorcentaje { get; set; }
    public List<CrearPrecioDto>? Precios { get; set; }
}

public class CrearPrecioDto
{
    public string TipoPrecio { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public bool IncluyeIva { get; set; } = true;
}
