// Data/AppDbContext.cs
using InventarioApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opciones) : base(opciones) { }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entidad =>
        {
            entidad.Property(p => p.Nombre).IsRequired().HasMaxLength(100);

            // Un producto tiene muchos movimientos. No se puede borrar un producto con historial.
            entidad.HasMany(p => p.Movimientos)
                   .WithOne()
                   .HasForeignKey(m => m.ProductoId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Movimiento>(entidad =>
        {
            entidad.Property(m => m.Tipo).HasConversion<string>().HasMaxLength(20); // guarda "Entrada"/"Salida"
            entidad.Property(m => m.Observacion).HasMaxLength(200);

            // SQLite no guarda la zona horaria: al leer, aclaramos que la fecha está en UTC.
            entidad.Property(m => m.Fecha).HasConversion(
                fecha => fecha,
                fecha => DateTime.SpecifyKind(fecha, DateTimeKind.Utc));
        });
    }
}