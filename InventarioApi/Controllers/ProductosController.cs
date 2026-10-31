// Controllers/ProductosController.cs
using InventarioApi.DTOs;
using InventarioApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productos;

    public ProductosController(IProductoService productos)
    {
        _productos = productos;
    }

    /// <summary>Lista todos los productos.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductoDto>>> Listar()
        => Ok(await _productos.ListarAsync());

    /// <summary>Lista los productos con stock bajo: menos de "menorA" unidades (si no se indica, compara con el stock mínimo de cada producto).</summary>
    [HttpGet("stock-bajo")]
    public async Task<ActionResult<IReadOnlyList<ProductoDto>>> StockBajo([FromQuery] int? menorA)
        => Ok(await _productos.ListarConStockBajoAsync(menorA));

    /// <summary>Obtiene un producto por su id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> Obtener(int id)
        => Ok(await _productos.ObtenerAsync(id));

    /// <summary>Crea un producto nuevo (arranca con stock 0).</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductoDto>> Crear(ProductoGuardarDto dto)
    {
        var creado = await _productos.CrearAsync(dto);
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }

    /// <summary>Modifica el nombre y el stock mínimo de un producto.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> Actualizar(int id, ProductoGuardarDto dto)
        => Ok(await _productos.ActualizarAsync(id, dto));

    /// <summary>Elimina un producto (solo si nunca tuvo movimientos).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _productos.EliminarAsync(id);
        return NoContent();
    }
}