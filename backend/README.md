# Nexora - Sistema de Gestión para Pequeños Negocios

## Requisitos Previos

- .NET 8 SDK
- PostgreSQL 14+
- Node.js 18+ (para Angular frontend)

## Configuración de la Base de Datos

1. Crear una base de datos en PostgreSQL:
```sql
CREATE DATABASE nexora;
```

2. Ejecutar el script de inicialización:
```bash
psql -U postgres -d nexora -f scripts/init.sql
```

3. Actualizar la cadena de conexión en `Nexora.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=nexora;Username=postgres;Password=TU_PASSWORD"
  }
}
```

## Ejecutar el Backend

```bash
cd Nexora.Api
dotnet run
```

La API estará disponible en: `https://localhost:5001`

## Endpoints Principales

### Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/register` - Registrar vendedor (solo admin)

### Productos
- `GET /api/productos` - Listar productos
- `POST /api/productos` - Crear producto
- `PUT /api/productos/{id}` - Actualizar producto
- `DELETE /api/productos/{id}` - Eliminar producto

### Clientes
- `GET /api/clientes` - Listar clientes
- `POST /api/clientes` - Crear cliente
- `PUT /api/clientes/{id}` - Actualizar cliente
- `DELETE /api/clientes/{id}` - Eliminar cliente

### Proveedores
- `GET /api/proveedores` - Listar proveedores
- `POST /api/proveedores` - Crear proveedor
- `PUT /api/proveedores/{id}` - Actualizar proveedor
- `DELETE /api/proveedores/{id}` - Eliminar proveedor

### Documentos
- `GET /api/documentos` - Listar documentos (con paginación y filtros)
- `GET /api/documentos/{id}` - Obtener documento por ID
- `POST /api/documentos` - Crear documento
- `PUT /api/documentos/{id}/estado` - Actualizar estado del documento

## Estructura del Proyecto

```
Nexora/
├── Nexora.Domain/          # Entidades y enums
├── Nexora.Application/     # Servicios, DTOs, interfaces
├── Nexora.Infrastructure/  # DbContext, configuraciones EF Core
├── Nexora.Api/            # Controladores, middleware
└── scripts/               # Scripts de base de datos
```

## Tecnologías Usadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- AutoMapper
- FluentValidation

## Credenciales por Defecto

Usuario: `Administrator`
Clave: `admin123`
