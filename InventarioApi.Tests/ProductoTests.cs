// InventarioApi.Tests/ProductoTests.cs
using InventarioApi.Exceptions;
using InventarioApi.Models;

namespace InventarioApi.Tests;

public class ProductoTests
{
    [Fact]
    public void RegistrarEntrada_SumaAlStock()
    {
        var producto = new Producto("Tornillo", stockMinimo: 5);

        producto.RegistrarEntrada(10, "Compra inicial");

        Assert.Equal(10, producto.StockActual);
    }

    [Fact]
    public void RegistrarSalida_ConStockSuficiente_DescuentaDelStock()
    {
        var producto = new Producto("Tornillo", 5);
        producto.RegistrarEntrada(10, null);

        producto.RegistrarSalida(4, null);

        Assert.Equal(6, producto.StockActual);
    }

    [Fact]
    public void RegistrarSalida_SinStockSuficiente_LanzaExcepcionYNoCambiaNada()
    {
        var producto = new Producto("Tornillo", 5);
        producto.RegistrarEntrada(3, null);

        Assert.Throws<StockInsuficienteException>(() => producto.RegistrarSalida(10, null));

        Assert.Equal(3, producto.StockActual);
        Assert.Single(producto.Movimientos); // solo quedó la entrada
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void RegistrarEntrada_ConCantidadInvalida_LanzaExcepcion(int cantidad)
    {
        var producto = new Producto("Tornillo", 5);

        Assert.Throws<ReglaDeNegocioException>(() => producto.RegistrarEntrada(cantidad, null));
    }

    [Fact]
    public void Crear_ConNombreVacio_LanzaExcepcion()
    {
        Assert.Throws<ReglaDeNegocioException>(() => new Producto("   ", 5));
    }
}