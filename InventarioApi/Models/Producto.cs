// Models/Producto.cs
using InventarioApi.Exceptions;

namespace InventarioApi.Models;

public class Producto
{
    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public int StockActual { get; private set; }
    public int StockMinimo { get; private set; }
    public List<Movimiento> Movimientos { get; private set; } = new();

    private Producto() { } // Lo usa EF Core al leer de la base

    public Producto(string nombre, int stockMinimo)
    {
        Actualizar(nombre, stockMinimo);
    }

    public void Actualizar(string nombre, int stockMinimo)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ReglaDeNegocioException("El nombre del producto es obligatorio.");
        if (stockMinimo < 0)
            throw new ReglaDeNegocioException("El stock mínimo no puede ser negativo.");

        Nombre = nombre.Trim();
        StockMinimo = stockMinimo;
    }

    public Movimiento RegistrarEntrada(int cantidad, string? observacion)
    {
        ValidarCantidad(cantidad);
        StockActual += cantidad;
        return AgregarMovimiento(TipoMovimiento.Entrada, cantidad, observacion);
    }

    public Movimiento RegistrarSalida(int cantidad, string? observacion)
    {
        ValidarCantidad(cantidad);
        if (cantidad > StockActual)
            throw new StockInsuficienteException(StockActual, cantidad);

        StockActual -= cantidad;
        return AgregarMovimiento(TipoMovimiento.Salida, cantidad, observacion);
    }

    private static void ValidarCantidad(int cantidad)
    {
        if (cantidad <= 0)
            throw new ReglaDeNegocioException("La cantidad debe ser mayor a cero.");
    }

    private Movimiento AgregarMovimiento(TipoMovimiento tipo, int cantidad, string? observacion)
    {
        var movimiento = new Movimiento(Id, tipo, cantidad, observacion);
        Movimientos.Add(movimiento);
        return movimiento;
    }
}