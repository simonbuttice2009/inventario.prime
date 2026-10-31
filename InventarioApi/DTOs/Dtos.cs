// DTOs/Dtos.cs
using System.ComponentModel.DataAnnotations;

namespace InventarioApi.DTOs;

// ---------- Lo que ENVÍA el cliente ----------
public class ProductoGuardarDto // sirve para crear (POST) y modificar (PUT)
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0, 1_000_000, ErrorMessage = "El stock mínimo debe estar entre 0 y 1.000.000.")]
    public int StockMinimo { get; set; }
}

public class RegistrarMovimientoDto
{
    [Range(1, 1_000_000, ErrorMessage = "La cantidad debe ser al menos 1.")]
    public int Cantidad { get; set; }

    [StringLength(200, ErrorMessage = "La observación admite hasta 200 caracteres.")]
    public string? Observacion { get; set; }
}

// ---------- Lo que DEVUELVE la API ----------
public record ProductoDto(int Id, string Nombre, int StockActual, int StockMinimo);

public record MovimientoDto(int Id, int ProductoId, string Tipo, int Cantidad, DateTime Fecha, string? Observacion);

public record StockDto(int ProductoId, string Nombre, int StockActual, int StockMinimo);