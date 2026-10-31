// Models/Movimiento.cs
namespace InventarioApi.Models;

public class Movimiento
{
    public int Id { get; private set; }
    public int ProductoId { get; private set; }
    public TipoMovimiento Tipo { get; private set; }
    public int Cantidad { get; private set; }
    public DateTime Fecha { get; private set; }
    public string? Observacion { get; private set; }

    private Movimiento() { } // Lo usa EF Core al leer de la base

    public Movimiento(int productoId, TipoMovimiento tipo, int cantidad, string? observacion)
    {
        ProductoId = productoId;
        Tipo = tipo;
        Cantidad = cantidad;
        Observacion = observacion;
        Fecha = DateTime.UtcNow;
    }
}