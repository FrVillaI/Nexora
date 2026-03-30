import { Injectable } from '@angular/core';
import { ApiService } from '../../core/services/api.service';

export interface Proveedor {
  id: number;
  identificacion?: string;
  nombre: string;
  direccion?: string;
  telefono?: string;
  email?: string;
  fechaCreacion: string;
  estado: boolean;
}

export interface CrearProveedor {
  identificacion?: string;
  nombre: string;
  direccion?: string;
  telefono?: string;
  email?: string;
}

@Injectable({ providedIn: 'root' })
export class ProveedoresService {
  constructor(private api: ApiService) {}

  getAll(): Promise<Proveedor[]> {
    return new Promise((resolve, reject) => {
      this.api.get<Proveedor[]>('proveedores').subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  create(proveedor: CrearProveedor): Promise<Proveedor> {
    return new Promise((resolve, reject) => {
      this.api.post<Proveedor>('proveedores', proveedor).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  update(id: number, proveedor: CrearProveedor): Promise<Proveedor> {
    return new Promise((resolve, reject) => {
      this.api.put<Proveedor>(`proveedores/${id}`, proveedor).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  delete(id: number): Promise<void> {
    return new Promise((resolve, reject) => {
      this.api.delete<void>(`proveedores/${id}`).subscribe({
        next: () => resolve(),
        error: (err) => reject(err)
      });
    });
  }
}
