// Exceptions/ExcepcionesDeDominio.cs
namespace InventarioApi.Exceptions;

/// <summary>Se busca algo que no existe (HTTP 404).</summary>
public class NoEncontradoException : Exception
{
    public NoEncontradoException(string mensaje) : base(mensaje) { }
}

/// <summary>Se incumple una regla del negocio (HTTP 400).</summary>
public class ReglaDeNegocioException : Exception
{
    public ReglaDeNegocioException(string mensaje) : base(mensaje) { }
}

/// <summary>Salida mayor al stock disponible. Es un tipo particular de regla de negocio (herencia).</summary>
public class StockInsuficienteException : ReglaDeNegocioException
{
    public StockInsuficienteException(int disponible, int solicitado)
        : base($"Stock insuficiente: hay {disponible} unidad(es) y se pidieron {solicitado}.") { }
}