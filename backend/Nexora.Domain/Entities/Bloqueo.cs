namespace Nexora.Domain.Entities;

public class Bloqueo
{
    public int Id { get; set; }
    public DateTime? FechaBloqueoVentas { get; set; }
    public DateTime? FechaBloqueoCompras { get; set; }
    public DateTime? FechaBloqueoGeneral { get; set; }
}
