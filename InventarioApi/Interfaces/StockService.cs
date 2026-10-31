// Services/StockService.cs
using InventarioApi.Data;
using InventarioApi.DTOs;
using InventarioApi.Interfaces;
using InventarioApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioApi.Services;

public class StockService : IStockService
{
    private readonly AppDbContext _bd;

    public StockService(AppDbContext bd)
    {
        _bd = bd;
    }

    public Task<MovimientoDto> RegistrarEntradaAsync(int productoId, RegistrarMovimientoDto dto)
        => RegistrarAsync(productoId, producto => producto.RegistrarEntrada(dto.Cantidad, dto.Observacion));

    public Task<MovimientoDto> RegistrarSalidaAsync(int productoId, RegistrarMovimientoDto dto)
        => RegistrarAsync(productoId, producto => producto.RegistrarSalida(dto.Cantidad, dto.Observacion));

    public async Task<StockDto> ConsultarStockAsync(int productoId)
    {
        var producto = await _bd.ObtenerProductoAsync(productoId);
        return new StockDto(producto.Id, producto.Nombre, producto.StockActual, producto.StockMinimo);
    }

    public async Task<IReadOnlyList<MovimientoDto>> ObtenerHistorialAsync(int productoId)
    {
        await _bd.ObtenerProductoAsync(productoId); // si no existe, lanza NoEncontradoException (404)

        var movimientos = await _bd.Movimientos
            .AsNoTracking()
            .Where(m => m.ProductoId == productoId)
            .OrderByDescending(m => m.Fecha)
            .ThenByDescending(m => m.Id)
            .ToListAsync();

        return movimientos.Select(m => m.ToDto()).ToList();
    }

    // Entrada y salida hacen lo mismo salvo la operación sobre el producto: se comparte el resto.
    private async Task<MovimientoDto> RegistrarAsync(int productoId, Func<Producto, Movimiento> operacion)
    {
        var producto = await _bd.ObtenerProductoAsync(productoId);
        var movimiento = operacion(producto);
        await _bd.SaveChangesAsync();
        return movimiento.ToDto();
    }
}