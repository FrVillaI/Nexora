import { Injectable } from '@angular/core';
import { ApiService } from '../../core/services/api.service';

export interface Producto {
  id: number;
  nombre: string;
  descripcion?: string;
  codigoBarras?: string;
  stock: number;
  ivaPorcentaje: number;
  estado: boolean;
  precios?: Precio[];
}

export interface Precio {
  id: number;
  tipoPrecio: string;
  valor: number;
  incluyeIva: boolean;
}

export interface CrearProducto {
  nombre: string;
  descripcion?: string;
  codigoBarras?: string;
  stock: number;
  ivaPorcentaje: number;
  precios?: { tipoPrecio: string; valor: number; incluyeIva: boolean }[];
}

@Injectable({
  providedIn: 'root'
})
export class ProductosService {
  constructor(private api: ApiService) {}

  getAll(): Promise<Producto[]> {
    return new Promise((resolve, reject) => {
      this.api.get<Producto[]>('productos').subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  getById(id: number): Promise<Producto> {
    return new Promise((resolve, reject) => {
      this.api.get<Producto>(`productos/${id}`).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  create(producto: CrearProducto): Promise<Producto> {
    return new Promise((resolve, reject) => {
      this.api.post<Producto>('productos', producto).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  update(id: number, producto: CrearProducto): Promise<Producto> {
    return new Promise((resolve, reject) => {
      this.api.put<Producto>(`productos/${id}`, producto).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  delete(id: number): Promise<void> {
    return new Promise((resolve, reject) => {
      this.api.delete<void>(`productos/${id}`).subscribe({
        next: () => resolve(),
        error: (err) => reject(err)
      });
    });
  }
}
