// GraphQL/Mutation.cs
using InventarioApi.DTOs;
using InventarioApi.Interfaces;

namespace InventarioApi.GraphQL;

public class Mutation
{
    public Task<ProductoDto> CrearProducto(string nombre, int stockMinimo, IProductoService servicio)
        => servicio.CrearAsync(new ProductoGuardarDto { Nombre = nombre, StockMinimo = stockMinimo });

    public Task<MovimientoDto> RegistrarEntrada(int productoId, int cantidad, string? observacion, IStockService servicio)
        => servicio.RegistrarEntradaAsync(productoId, new RegistrarMovimientoDto { Cantidad = cantidad, Observacion = observacion });

    public Task<MovimientoDto> RegistrarSalida(int productoId, int cantidad, string? observacion, IStockService servicio)
        => servicio.RegistrarSalidaAsync(productoId, new RegistrarMovimientoDto { Cantidad = cantidad, Observacion = observacion });
}