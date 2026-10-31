// Program.cs
using System.Reflection;
using InventarioApi.Data;
using InventarioApi.Infraestructura;
using InventarioApi.Interfaces;
using InventarioApi.Services;
using Microsoft.EntityFrameworkCore;
using InventarioApi.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// ================= 1. REGISTRO DE SERVICIOS (inyección de dependencias) =================

// Base de datos: EF Core + SQLite (el archivo se llama inventario.db)
builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlite(builder.Configuration.GetConnectionString("Inventario")));

// "Cuando alguien pida IProductoService, entregale un ProductoService".
// AddScoped = se crea uno nuevo por cada pedido HTTP.
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IStockService, StockService>();

// API REST + manejo de errores
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<ManejadorDeExcepciones>();
builder.Services.AddProblemDetails();

// Swagger (incluye los comentarios /// de los controladores)
builder.Services.AddSwaggerGen(opciones =>
{
    var archivoXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
   opciones.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, archivoXml));
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddErrorFilter<FiltroDeErroresGraphQL>();
    
var app = builder.Build();

// ================= 2. ARRANQUE =================

// Crea/actualiza la base de datos automáticamente al iniciar
using (var alcance = app.Services.CreateScope())
{
    var bd = alcance.ServiceProvider.GetRequiredService<AppDbContext>();
    await bd.Database.MigrateAsync();
}

// ================= 3. TUBERÍA HTTP (el orden importa) =================
app.UseExceptionHandler();   // convierte excepciones en respuestas ProblemDetails
app.UseSwagger();            // genera el documento swagger.json
app.UseSwaggerUI();          // la página visual en /swagger (en un proyecto real se limita a Development)
app.UseDefaultFiles();       // sirve wwwroot/index.html en la raíz "/"
app.UseStaticFiles();
app.MapControllers();        // conecta las rutas de los controladores
app.MapGraphQL();   // publica /graphql

app.Run();