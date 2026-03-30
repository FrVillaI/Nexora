import { Injectable } from '@angular/core';
import { ApiService } from '../../core/services/api.service';

export interface DocumentoDetalle {
  id: number;
  idProducto: number;
  productoNombre?: string;
  productoCodigo?: string;
  cantidad: number;
  precioUnitario: number;
  descuento: number;
  iva: number;
  total: number;
}

export interface Documento {
  id: number;
  tipo: string;
  numero: string;
  idCliente?: number;
  clienteNombre?: string;
  idProveedor?: number;
  proveedorNombre?: string;
  idVendedor: number;
  vendedorNombre?: string;
  fechaEmision: string;
  fechaVencimiento?: string;
  formaPago?: string;
  observacion?: string;
  subtotal: number;
  ivaTotal: number;
  descuentoTotal: number;
  total: number;
  estado: string;
  detalles?: DocumentoDetalle[];
}

export interface CrearDocumentoDetalle {
  idProducto: number;
  cantidad: number;
  precioUnitario: number;
  descuento: number;
  iva: number;
}

export interface CrearDocumento {
  tipo: string;
  idCliente?: number;
  idProveedor?: number;
  fechaEmision: string;
  fechaVencimiento?: string;
  formaPago?: string;
  observacion?: string;
  detalles: CrearDocumentoDetalle[];
}

export interface PagedResponse<T> {
  data: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

@Injectable({ providedIn: 'root' })
export class DocumentosService {
  constructor(private api: ApiService) {}

  getAll(tipo?: string, clienteId?: number, fechaDesde?: string, fechaHasta?: string, page = 1, pageSize = 20): Promise<PagedResponse<Documento>> {
    return new Promise((resolve, reject) => {
      this.api.get<PagedResponse<Documento>>('documentos', { tipo, clienteId, fechaDesde, fechaHasta, page, pageSize }).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  getById(id: number): Promise<Documento> {
    return new Promise((resolve, reject) => {
      this.api.get<Documento>(`documentos/${id}`).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  create(documento: CrearDocumento): Promise<Documento> {
    return new Promise((resolve, reject) => {
      this.api.post<Documento>('documentos', documento).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  updateEstado(id: number, estado: string): Promise<Documento> {
    return new Promise((resolve, reject) => {
      this.api.put<Documento>(`documentos/${id}/estado`, estado).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }
}
