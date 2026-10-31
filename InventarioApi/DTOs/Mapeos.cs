// DTOs/Mapeos.cs
using InventarioApi.Models;

namespace InventarioApi.DTOs;

/// <summary>Convierte entidades en DTOs (en un solo lugar, sin repetir código).</summary>
public static class Mapeos
{
    public static ProductoDto ToDto(this Producto p)
        => new(p.Id, p.Nombre, p.StockActual, p.StockMinimo);

    public static MovimientoDto ToDto(this Movimiento m)
        => new(m.Id, m.ProductoId, m.Tipo.ToString(), m.Cantidad, m.Fecha, m.Observacion);
}