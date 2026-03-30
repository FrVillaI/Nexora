import { Injectable, signal, computed } from '@angular/core';
import { ApiService } from './api.service';

export interface Vendedor {
  id: number;
  nombre: string;
  esAdmin: boolean;
  puedeDescuento: boolean;
  puedeModificarPrecio: boolean;
}

export interface AuthResponse {
  token: string;
  vendedor: Vendedor;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'nexora_token';
  private readonly USER_KEY = 'nexora_user';

  private currentUser = signal<Vendedor | null>(null);
  user = computed(() => this.currentUser());
  isLoggedIn = computed(() => !!this.currentUser());
  isAdmin = computed(() => this.currentUser()?.esAdmin ?? false);

  constructor(private api: ApiService) {
    this.loadStoredUser();
  }

  private loadStoredUser(): void {
    const storedUser = localStorage.getItem(this.USER_KEY);
    if (storedUser) {
      this.currentUser.set(JSON.parse(storedUser));
    }
  }

  login(nombre: string, clave: string): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this.api.post<AuthResponse>('auth/login', { nombre, clave }).subscribe({
        next: (response) => {
          localStorage.setItem(this.TOKEN_KEY, response.token);
          localStorage.setItem(this.USER_KEY, JSON.stringify(response.vendedor));
          this.currentUser.set(response.vendedor);
          resolve(true);
        },
        error: (err) => reject(err)
      });
    });
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUser.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }
}
