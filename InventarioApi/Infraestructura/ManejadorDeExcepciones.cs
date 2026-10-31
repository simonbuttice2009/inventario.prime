// Infraestructura/ManejadorDeExcepciones.cs
using InventarioApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Infraestructura;

public class ManejadorDeExcepciones : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext contexto, Exception excepcion, CancellationToken cancelacion)
    {
        var (estado, titulo) = excepcion switch
        {
            NoEncontradoException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            ReglaDeNegocioException => (StatusCodes.Status400BadRequest, "Regla de negocio incumplida"),
            _ => (0, string.Empty)
        };

        if (estado == 0) return false; // error desconocido: que lo maneje ASP.NET (500)

        contexto.Response.StatusCode = estado;
        await contexto.Response.WriteAsJsonAsync(
            new ProblemDetails { Status = estado, Title = titulo, Detail = excepcion.Message },
            cancelacion);
        return true;
    }
}