// Services/ProductoService.cs
using InventarioApi.Data;
using InventarioApi.DTOs;
using InventarioApi.Exceptions;
using InventarioApi.Interfaces;
using InventarioApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioApi.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _bd;

    public ProductoService(AppDbContext bd)
    {
        _bd = bd;
    }

    public Task<IReadOnlyList<ProductoDto>> ListarAsync()
        => ProyectarAsync(_bd.Productos.OrderBy(p => p.Nombre));

    public async Task<ProductoDto> ObtenerAsync(int id)
        => (await _bd.ObtenerProductoAsync(id)).ToDto();

    public async Task<ProductoDto> CrearAsync(ProductoGuardarDto dto)
    {
        var producto = new Producto(dto.Nombre, dto.StockMinimo);
        _bd.Productos.Add(producto);
        await _bd.SaveChangesAsync();
        return producto.ToDto();
    }

    public async Task<ProductoDto> ActualizarAsync(int id, ProductoGuardarDto dto)
    {
        var producto = await _bd.ObtenerProductoAsync(id);
        producto.Actualizar(dto.Nombre, dto.StockMinimo);
        await _bd.SaveChangesAsync();
        return producto.ToDto();
    }

    public async Task EliminarAsync(int id)
    {
        var producto = await _bd.ObtenerProductoAsync(id);

        var tieneMovimientos = await _bd.Movimientos.AnyAsync(m => m.ProductoId == id);
        if (tieneMovimientos)
            throw new ReglaDeNegocioException("No se puede eliminar un producto que tiene movimientos de stock registrados.");

        _bd.Productos.Remove(producto);
        await _bd.SaveChangesAsync();
    }

    public Task<IReadOnlyList<ProductoDto>> ListarConStockBajoAsync(int? menorA)
    {
        var consulta = menorA.HasValue
            ? _bd.Productos.Where(p => p.StockActual < menorA.Value)
            : _bd.Productos.Where(p => p.StockActual < p.StockMinimo);

        return ProyectarAsync(consulta.OrderBy(p => p.StockActual));
    }

    // Ejecuta la consulta y convierte cada producto a DTO (lo usan varios métodos).
    private static async Task<IReadOnlyList<ProductoDto>> ProyectarAsync(IQueryable<Producto> consulta)
    {
        var productos = await consulta.AsNoTracking().ToListAsync();
        return productos.Select(p => p.ToDto()).ToList();
    }
}