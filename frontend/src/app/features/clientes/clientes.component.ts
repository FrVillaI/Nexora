import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { ClientesService, Cliente, CrearCliente } from './clientes.service';

@Component({
  selector: 'app-clientes',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule
  ],
  template: `
    <div class="clientes-container">
      <div class="header">
        <h1>Clientes</h1>
        <button mat-raised-button color="primary" (click)="openDialog()">
          <mat-icon>add</mat-icon> Nuevo Cliente
        </button>
      </div>

      <table mat-table [dataSource]="clientes" class="mat-elevation-z2">
        <ng-container matColumnDef="identificacion">
          <th mat-header-cell *matHeaderCellDef>Identificación</th>
          <td mat-cell *matCellDef="let c">{{c.identificacion || '-'}}</td>
        </ng-container>

        <ng-container matColumnDef="nombre">
          <th mat-header-cell *matHeaderCellDef>Nombre</th>
          <td mat-cell *matCellDef="let c">{{c.nombre}}</td>
        </ng-container>

        <ng-container matColumnDef="telefono">
          <th mat-header-cell *matHeaderCellDef>Teléfono</th>
          <td mat-cell *matCellDef="let c">{{c.telefono || '-'}}</td>
        </ng-container>

        <ng-container matColumnDef="email">
          <th mat-header-cell *matHeaderCellDef>Email</th>
          <td mat-cell *matCellDef="let c">{{c.email || '-'}}</td>
        </ng-container>

        <ng-container matColumnDef="acciones">
          <th mat-header-cell *matHeaderCellDef>Acciones</th>
          <td mat-cell *matCellDef="let c">
            <button mat-icon-button (click)="openDialog(c)">
              <mat-icon>edit</mat-icon>
            </button>
            <button mat-icon-button color="warn" (click)="delete(c)">
              <mat-icon>delete</mat-icon>
            </button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table>

      @if (showDialog) {
        <div class="dialog-overlay">
          <mat-card class="dialog-card">
            <mat-card-header>
              <mat-card-title>{{editingCliente ? 'Editar' : 'Nuevo'}} Cliente</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Identificación</mat-label>
                <input matInput [(ngModel)]="form.identificacion">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Nombre</mat-label>
                <input matInput [(ngModel)]="form.nombre" required>
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Dirección</mat-label>
                <input matInput [(ngModel)]="form.direccion">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Teléfono</mat-label>
                <input matInput [(ngModel)]="form.telefono">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Email</mat-label>
                <input matInput [(ngModel)]="form.email">
              </mat-form-field>

              <div class="dialog-actions">
                <button mat-button (click)="closeDialog()">Cancelar</button>
                <button mat-raised-button color="primary" (click)="save()">Guardar</button>
              </div>
            </mat-card-content>
          </mat-card>
        </div>
      }
    </div>
  `,
  styles: [`
    .clientes-container { padding: 20px; }
    .header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
    table { width: 100%; }
    .full-width { width: 100%; margin-bottom: 8px; }
    .dialog-overlay { position: fixed; top: 0; left: 0; right: 0; bottom: 0; background: rgba(0,0,0,0.5); display: flex; justify-content: center; align-items: center; z-index: 1000; }
    .dialog-card { width: 400px; padding: 20px; }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; margin-top: 16px; }
  `]
})
export class ClientesComponent implements OnInit {
  clientes: Cliente[] = [];
  displayedColumns = ['identificacion', 'nombre', 'telefono', 'email', 'acciones'];
  showDialog = false;
  editingCliente: Cliente | null = null;
  form: CrearCliente = this.getEmptyForm();

  constructor(private clientesService: ClientesService, private snackBar: MatSnackBar) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.clientesService.getAll()
      .then(data => this.clientes = data)
      .catch(err => this.snackBar.open(err.error?.mensaje || 'Error al cargar', 'Cerrar'));
  }

  openDialog(cliente?: Cliente): void {
    if (cliente) {
      this.editingCliente = cliente;
      this.form = { nombre: cliente.nombre, identificacion: cliente.identificacion, direccion: cliente.direccion, telefono: cliente.telefono, email: cliente.email };
    } else {
      this.editingCliente = null;
      this.form = this.getEmptyForm();
    }
    this.showDialog = true;
  }

  closeDialog(): void { this.showDialog = false; this.form = this.getEmptyForm(); }

  save(): void {
    if (!this.form.nombre) { this.snackBar.open('El nombre es requerido', 'Cerrar'); return; }
    const promise = this.editingCliente ? this.clientesService.update(this.editingCliente.id, this.form) : this.clientesService.create(this.form);
    promise.then(() => { this.snackBar.open('Cliente guardado', 'Cerrar'); this.closeDialog(); this.load(); })
      .catch(err => this.snackBar.open(err.error?.mensaje || 'Error al guardar', 'Cerrar'));
  }

  delete(cliente: Cliente): void {
    if (confirm(`¿Eliminar ${cliente.nombre}?`)) {
      this.clientesService.delete(cliente.id)
        .then(() => { this.snackBar.open('Cliente eliminado', 'Cerrar'); this.load(); })
        .catch(err => this.snackBar.open(err.error?.mensaje || 'Error al eliminar', 'Cerrar'));
    }
  }

  private getEmptyForm(): CrearCliente { return { nombre: '', identificacion: '', direccion: '', telefono: '', email: '' }; }
}
