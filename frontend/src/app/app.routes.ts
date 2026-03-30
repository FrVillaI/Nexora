import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { LayoutComponent } from './shared/components/layout/layout.component';
import { ProductosComponent } from './features/productos/productos.component';
import { ClientesComponent } from './features/clientes/clientes.component';
import { ProveedoresComponent } from './features/proveedores/proveedores.component';
import { DocumentosComponent } from './features/documentos/documentos.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: '', 
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'productos', pathMatch: 'full' },
      { path: 'productos', component: ProductosComponent },
      { path: 'clientes', component: ClientesComponent },
      { path: 'proveedores', component: ProveedoresComponent },
      { path: 'documentos', component: DocumentosComponent }
    ]
  },
  { path: '**', redirectTo: '' }
];
