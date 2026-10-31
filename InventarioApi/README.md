# API de Inventario

API REST + GraphQL para controlar el stock de un depósito (ASP.NET Core, EF Core, SQLite).

## Requisitos
- .NET SDK 10
- (Opcional) `dotnet tool install --global dotnet-ef` para crear migraciones

## Instalación y ejecución
1. `cd InventarioApi`
2. `dotnet run`
   (la base `inventario.db` se crea y migra sola al iniciar)
3. Abrir en el navegador (el puerto aparece en la terminal):
   - Aplicación web: `http://localhost:PUERTO/`
   - Swagger (REST): `http://localhost:PUERTO/swagger`
   - GraphQL: `http://localhost:PUERTO/graphql`

## Configuración
La cadena de conexión está en `InventarioApi/appsettings.json` (`ConnectionStrings:Inventario`).

## Pruebas
Desde la carpeta raíz: `dotnet test`

## Endpoints REST
| Verbo | Ruta | Descripción |
|---|---|---|
| GET/POST | /api/productos | Listar / crear |
| GET/PUT/DELETE | /api/productos/{id} | Ver / modificar / eliminar |
| GET | /api/productos/stock-bajo?menorA=10 | Productos con menos de X unidades |
| GET | /api/productos/{id}/stock | Stock actual |
| POST | /api/productos/{id}/entradas | Registrar entrada |
| POST | /api/productos/{id}/salidas | Registrar salida (400 si no hay stock) |
| GET | /api/productos/{id}/movimientos | Historial |

## GraphQL
Queries: `productos`, `producto(id)`, `stockBajo(menorA)`, `historial(productoId)`
Mutations: `crearProducto`, `registrarEntrada`, `registrarSalida`

## Arquitectura
Models → Data (EF Core) → Services (detrás de interfaces) → Controllers (REST) y GraphQL.
Los controladores y los resolvers no tienen lógica: llaman a los mismos services.