// InventarioApi.Tests/BaseDeDatosDePrueba.cs
using InventarioApi.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InventarioApi.Tests;

/// <summary>Crea una base SQLite en memoria, vacía, para cada prueba.</summary>
public class BaseDeDatosDePrueba : IDisposable
{
    private readonly SqliteConnection _conexion;
    public AppDbContext Contexto { get; }

    public BaseDeDatosDePrueba()
    {
        _conexion = new SqliteConnection("Data Source=:memory:");
        _conexion.Open(); // la base en memoria vive mientras la conexión esté abierta

        var opciones = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_conexion).Options;
        Contexto = new AppDbContext(opciones);
        Contexto.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Contexto.Dispose();
        _conexion.Dispose();
    }
}