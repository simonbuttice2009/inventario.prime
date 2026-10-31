// GraphQL/Query.cs
using InventarioApi.DTOs;
using InventarioApi.Interfaces;

namespace InventarioApi.GraphQL;

public class Query
{
    // Hot Chocolate quita "Get" y "Async": GetProductosAsync → campo "productos"

    public Task<IReadOnlyList<ProductoDto>> GetProductosAsync(IProductoService servicio)
        => servicio.ListarAsync();

    public Task<ProductoDto> GetProductoAsync(int id, IProductoService servicio)
        => servicio.ObtenerAsync(id);

    public Task<IReadOnlyList<ProductoDto>> GetStockBajoAsync(IProductoService servicio, int? menorA = null)
        => servicio.ListarConStockBajoAsync(menorA);

    public Task<IReadOnlyList<MovimientoDto>> GetHistorialAsync(int productoId, IStockService servicio)
        => servicio.ObtenerHistorialAsync(productoId);
}