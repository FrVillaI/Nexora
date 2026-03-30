-- Script de inicialización de base de datos para Nexora (PostgreSQL)
-- Ejecutar este script después de crear la base de datos

-- Tipos enumerados
CREATE TYPE tipo_documento AS ENUM ('FACTURA', 'PROFORMA', 'COMPRA', 'RECIBO', 'PRESTAMO');
CREATE TYPE estado_documento AS ENUM ('BORRADOR', 'EMITIDO', 'ANULADO');
CREATE TYPE tipo_movimiento AS ENUM ('ENTRADA', 'SALIDA');
CREATE TYPE motivo_movimiento AS ENUM ('VENTA', 'COMPRA', 'AJUSTE', 'PRESTAMO');
CREATE TYPE tipo_precio AS ENUM ('A', 'B', 'C', 'D');

-- Tablas
CREATE TABLE productos (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    descripcion TEXT,
    codigo_barras VARCHAR(50),
    stock DECIMAL(10,2) DEFAULT 0,
    iva_porcentaje DECIMAL(5,2) DEFAULT 0,
    estado BOOLEAN DEFAULT TRUE
);

CREATE TABLE precios (
    id SERIAL PRIMARY KEY,
    id_producto INT NOT NULL REFERENCES productos(id) ON DELETE CASCADE,
    tipo_precio tipo_precio NOT NULL,
    valor DECIMAL(10,2) NOT NULL,
    incluye_iva BOOLEAN DEFAULT TRUE
);

CREATE TABLE clientes (
    id SERIAL PRIMARY KEY,
    identificacion VARCHAR(20),
    nombre VARCHAR(150) NOT NULL,
    direccion VARCHAR(255),
    telefono VARCHAR(20),
    email VARCHAR(100),
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    estado BOOLEAN DEFAULT TRUE
);

CREATE TABLE proveedores (
    id SERIAL PRIMARY KEY,
    identificacion VARCHAR(20),
    nombre VARCHAR(150) NOT NULL,
    direccion VARCHAR(255),
    telefono VARCHAR(20),
    email VARCHAR(100),
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    estado BOOLEAN DEFAULT TRUE
);

CREATE TABLE vendedores (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    clave VARCHAR(255) NOT NULL,
    es_admin BOOLEAN DEFAULT FALSE,
    puede_descuento BOOLEAN DEFAULT FALSE,
    puede_modificar_precio BOOLEAN DEFAULT FALSE,
    estado BOOLEAN DEFAULT TRUE,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE secuencias (
    id SERIAL PRIMARY KEY,
    tipo_documento tipo_documento NOT NULL UNIQUE,
    serie VARCHAR(10) NOT NULL,
    numero_actual INT DEFAULT 0,
    formato VARCHAR(20),
    estado BOOLEAN DEFAULT TRUE
);

CREATE TABLE empresa (
    id SERIAL PRIMARY KEY,
    ruc VARCHAR(20),
    razon_social VARCHAR(150),
    nombre_comercial VARCHAR(150),
    ciudad VARCHAR(100),
    direccion VARCHAR(255),
    telefono VARCHAR(20),
    email VARCHAR(100)
);

CREATE TABLE bloqueos (
    id SERIAL PRIMARY KEY,
    fecha_bloqueo_ventas DATE,
    fecha_bloqueo_compras DATE,
    fecha_bloqueo_general DATE
);

CREATE TABLE documentos (
    id SERIAL PRIMARY KEY,
    tipo tipo_documento NOT NULL,
    numero VARCHAR(30) NOT NULL,
    id_secuencia INT REFERENCES secuencias(id),
    id_cliente INT REFERENCES clientes(id),
    id_proveedor INT REFERENCES proveedores(id),
    id_vendedor INT NOT NULL REFERENCES vendedores(id),
    fecha_emision TIMESTAMP NOT NULL,
    fecha_vencimiento TIMESTAMP,
    forma_pago VARCHAR(50),
    observacion TEXT,
    subtotal DECIMAL(12,2) DEFAULT 0,
    iva_total DECIMAL(12,2) DEFAULT 0,
    descuento_total DECIMAL(12,2) DEFAULT 0,
    total DECIMAL(12,2) NOT NULL,
    estado estado_documento DEFAULT 'BORRADOR'
);

CREATE TABLE documento_detalle (
    id SERIAL PRIMARY KEY,
    id_documento INT NOT NULL REFERENCES documentos(id) ON DELETE CASCADE,
    id_producto INT NOT NULL REFERENCES productos(id),
    cantidad DECIMAL(10,2) NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    descuento DECIMAL(10,2) DEFAULT 0,
    iva DECIMAL(10,2) DEFAULT 0,
    total DECIMAL(12,2) NOT NULL
);

CREATE TABLE kardex (
    id SERIAL PRIMARY KEY,
    id_producto INT NOT NULL REFERENCES productos(id),
    fecha TIMESTAMP NOT NULL,
    tipo tipo_movimiento NOT NULL,
    motivo motivo_movimiento NOT NULL,
    cantidad DECIMAL(10,2) NOT NULL,
    costo_unitario DECIMAL(10,2),
    id_documento INT REFERENCES documentos(id)
);

-- Índices
CREATE INDEX idx_productos_codigo ON productos(codigo_barras);
CREATE INDEX idx_documentos_tipo ON documentos(tipo);
CREATE INDEX idx_documentos_fecha ON documentos(fecha_emision);
CREATE INDEX idx_documentos_cliente ON documentos(id_cliente);
CREATE INDEX idx_kardex_producto ON kardex(id_producto);
CREATE INDEX idx_kardex_fecha ON kardex(fecha);

-- Insertar secuencia inicial
INSERT INTO secuencias (tipo_documento, serie, numero_actual, formato, estado)
VALUES ('FACTURA', '001-001', 0, '001-001-0000001', TRUE),
       ('PROFORMA', '001-002', 0, '001-002-0000001', TRUE),
       ('COMPRA', '001-003', 0, '001-003-0000001', TRUE),
       ('RECIBO', '001-004', 0, '001-004-0000001', TRUE),
       ('PRESTAMO', '001-005', 0, '001-005-0000001', TRUE);

-- Insertar vendedor admin por defecto (clave: admin123)
INSERT INTO vendedores (nombre, clave, es_admin, puede_descuento, puede_modificar_precio)
VALUES ('Administrador', '$2a$11$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/X4.Z.KJQm3HqHQFYW', TRUE, TRUE, TRUE);
