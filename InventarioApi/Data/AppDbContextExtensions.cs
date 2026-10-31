// Data/AppDbContextExtensions.cs
using InventarioApi.Exceptions;
using InventarioApi.Models;

namespace InventarioApi.Data;

public static class AppDbContextExtensions
{
    public static async Task<Producto> ObtenerProductoAsync(this AppDbContext bd, int id)
        => await bd.Productos.FindAsync(id)
           ?? throw new NoEncontradoException($"No existe el producto con id {id}.");
}