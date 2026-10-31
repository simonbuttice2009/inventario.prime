// Interfaces/IStockService.cs
using InventarioApi.DTOs;

namespace InventarioApi.Interfaces;

public interface IStockService
{
    Task<MovimientoDto> RegistrarEntradaAsync(int productoId, RegistrarMovimientoDto dto);
    Task<MovimientoDto> RegistrarSalidaAsync(int productoId, RegistrarMovimientoDto dto);
    Task<StockDto> ConsultarStockAsync(int productoId);
    Task<IReadOnlyList<MovimientoDto>> ObtenerHistorialAsync(int productoId);
}