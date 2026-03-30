import { Injectable } from '@angular/core';
import { ApiService } from '../../core/services/api.service';

export interface Cliente {
  id: number;
  identificacion?: string;
  nombre: string;
  direccion?: string;
  telefono?: string;
  email?: string;
  fechaCreacion: string;
  estado: boolean;
}

export interface CrearCliente {
  identificacion?: string;
  nombre: string;
  direccion?: string;
  telefono?: string;
  email?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ClientesService {
  constructor(private api: ApiService) {}

  getAll(): Promise<Cliente[]> {
    return new Promise((resolve, reject) => {
      this.api.get<Cliente[]>('clientes').subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  getById(id: number): Promise<Cliente> {
    return new Promise((resolve, reject) => {
      this.api.get<Cliente>(`clientes/${id}`).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  create(cliente: CrearCliente): Promise<Cliente> {
    return new Promise((resolve, reject) => {
      this.api.post<Cliente>('clientes', cliente).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  update(id: number, cliente: CrearCliente): Promise<Cliente> {
    return new Promise((resolve, reject) => {
      this.api.put<Cliente>(`clientes/${id}`, cliente).subscribe({
        next: (data) => resolve(data),
        error: (err) => reject(err)
      });
    });
  }

  delete(id: number): Promise<void> {
    return new Promise((resolve, reject) => {
      this.api.delete<void>(`clientes/${id}`).subscribe({
        next: () => resolve(),
        error: (err) => reject(err)
      });
    });
  }
}
