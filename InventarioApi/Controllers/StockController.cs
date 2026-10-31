// Controllers/StockController.cs
using InventarioApi.DTOs;
using InventarioApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers;

[ApiController]
[Route("api/productos/{productoId:int}")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
public class StockController : ControllerBase
{
    private readonly IStockService _stock;

    public StockController(IStockService stock)
    {
        _stock = stock;
    }

    /// <summary>Consulta el stock actual de un producto.</summary>
    [HttpGet("stock")]
    public async Task<ActionResult<StockDto>> ConsultarStock(int productoId)
        => Ok(await _stock.ConsultarStockAsync(productoId));

    /// <summary>Registra una entrada de stock (ingresa mercadería al depósito).</summary>
    [HttpPost("entradas")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<MovimientoDto>> RegistrarEntrada(int productoId, RegistrarMovimientoDto dto)
    {
        var movimiento = await _stock.RegistrarEntradaAsync(productoId, dto);
        return CreatedAtAction(nameof(ObtenerHistorial), new { productoId }, movimiento);
    }

    /// <summary>Registra una salida de stock. Se rechaza si no hay unidades suficientes.</summary>
    [HttpPost("salidas")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<MovimientoDto>> RegistrarSalida(int productoId, RegistrarMovimientoDto dto)
    {
        var movimiento = await _stock.RegistrarSalidaAsync(productoId, dto);
        return CreatedAtAction(nameof(ObtenerHistorial), new { productoId }, movimiento);
    }

    /// <summary>Historial de movimientos de un producto (el más reciente primero).</summary>
    [HttpGet("movimientos")]
    public async Task<ActionResult<IReadOnlyList<MovimientoDto>>> ObtenerHistorial(int productoId)
        => Ok(await _stock.ObtenerHistorialAsync(productoId));
}