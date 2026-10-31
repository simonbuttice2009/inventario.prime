// InventarioApi.Tests/ServiciosTests.cs
using InventarioApi.DTOs;
using InventarioApi.Exceptions;
using InventarioApi.Services;

namespace InventarioApi.Tests;

public class ServiciosTests
{
    [Fact]
    public async Task Salida_SinStockSuficiente_NoRegistraElMovimiento()
    {
        using var bd = new BaseDeDatosDePrueba();
        var productos = new ProductoService(bd.Contexto);
        var stock = new StockService(bd.Contexto);

        var producto = await productos.CrearAsync(new() { Nombre = "Tornillo", StockMinimo = 5 });
        await stock.RegistrarEntradaAsync(producto.Id, new() { Cantidad = 3 });

        await Assert.ThrowsAsync<StockInsuficienteException>(
            () => stock.RegistrarSalidaAsync(producto.Id, new() { Cantidad = 10 }));

        Assert.Single(await stock.ObtenerHistorialAsync(producto.Id));
    }

    [Fact]
    public async Task EntradasYSalidas_SeReflejanEnElStockConsultado()
    {
        using var bd = new BaseDeDatosDePrueba();
        var productos = new ProductoService(bd.Contexto);
        var stock = new StockService(bd.Contexto);

        var producto = await productos.CrearAsync(new() { Nombre = "Tuerca", StockMinimo = 2 });
        await stock.RegistrarEntradaAsync(producto.Id, new() { Cantidad = 20 });
        await stock.RegistrarSalidaAsync(producto.Id, new() { Cantidad = 5 });

        var resultado = await stock.ConsultarStockAsync(producto.Id);

        Assert.Equal(15, resultado.StockActual);
    }

    [Fact]
    public async Task StockBajo_ConUmbral_DevuelveSoloLosProductosConMenosUnidades()
    {
        using var bd = new BaseDeDatosDePrueba();
        var productos = new ProductoService(bd.Contexto);
        var stock = new StockService(bd.Contexto);

        var poco = await productos.CrearAsync(new() { Nombre = "Tornillo", StockMinimo = 5 });
        var mucho = await productos.CrearAsync(new() { Nombre = "Tuerca", StockMinimo = 5 });
        await stock.RegistrarEntradaAsync(poco.Id, new() { Cantidad = 3 });
        await stock.RegistrarEntradaAsync(mucho.Id, new() { Cantidad = 50 });

        var bajos = await productos.ListarConStockBajoAsync(10);

        Assert.Single(bajos);
        Assert.Equal("Tornillo", bajos[0].Nombre);
    }

    [Fact]
    public async Task Eliminar_ProductoConMovimientos_LanzaReglaDeNegocio()
    {
        using var bd = new BaseDeDatosDePrueba();
        var productos = new ProductoService(bd.Contexto);
        var stock = new StockService(bd.Contexto);

        var producto = await productos.CrearAsync(new() { Nombre = "Tornillo", StockMinimo = 5 });
        await stock.RegistrarEntradaAsync(producto.Id, new() { Cantidad = 1 });

        await Assert.ThrowsAsync<ReglaDeNegocioException>(() => productos.EliminarAsync(producto.Id));
    }

    [Fact]
    public async Task Obtener_ProductoInexistente_LanzaNoEncontrado()
    {
        using var bd = new BaseDeDatosDePrueba();
        var productos = new ProductoService(bd.Contexto);

        await Assert.ThrowsAsync<NoEncontradoException>(() => productos.ObtenerAsync(999));
    }
}