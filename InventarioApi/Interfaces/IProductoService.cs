// Interfaces/IProductoService.cs
using InventarioApi.DTOs;

namespace InventarioApi.Interfaces;

public interface IProductoService
{
    Task<IReadOnlyList<ProductoDto>> ListarAsync();
    Task<ProductoDto> ObtenerAsync(int id);
    Task<ProductoDto> CrearAsync(ProductoGuardarDto dto);
    Task<ProductoDto> ActualizarAsync(int id, ProductoGuardarDto dto);
    Task EliminarAsync(int id);

    /// <summary>Productos con menos de "menorA" unidades. Si no se indica, compara contra el stock mínimo de cada producto.</summary>
    Task<IReadOnlyList<ProductoDto>> ListarConStockBajoAsync(int? menorA);
}