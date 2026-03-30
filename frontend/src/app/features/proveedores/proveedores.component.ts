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
import { ProveedoresService, Proveedor, CrearProveedor } from './proveedores.service';

@Component({
  selector: 'app-proveedores',
  standalone: true,
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatSnackBarModule, MatCardModule],
  template: `
    <div class="proveedores-container">
      <div class="header">
        <h1>Proveedores</h1>
        <button mat-raised-button color="primary" (click)="openDialog()">
          <mat-icon>add</mat-icon> Nuevo Proveedor
        </button>
      </div>
      <table mat-table [dataSource]="proveedores" class="mat-elevation-z2">
        <ng-container matColumnDef="identificacion"><th mat-header-cell *matHeaderCellDef>Identificación</th><td mat-cell *matCellDef="let p">{{p.identificacion || '-'}}</td></ng-container>
        <ng-container matColumnDef="nombre"><th mat-header-cell *matHeaderCellDef>Nombre</th><td mat-cell *matCellDef="let p">{{p.nombre}}</td></ng-container>
        <ng-container matColumnDef="telefono"><th mat-header-cell *matHeaderCellDef>Teléfono</th><td mat-cell *matCellDef="let p">{{p.telefono || '-'}}</td></ng-container>
        <ng-container matColumnDef="email"><th mat-header-cell *matHeaderCellDef>Email</th><td mat-cell *matCellDef="let p">{{p.email || '-'}}</td></ng-container>
        <ng-container matColumnDef="acciones"><th mat-header-cell *matHeaderCellDef>Acciones</th><td mat-cell *matCellDef="let p"><button mat-icon-button (click)="openDialog(p)"><mat-icon>edit</mat-icon></button><button mat-icon-button color="warn" (click)="delete(p)"><mat-icon>delete</mat-icon></button></td></ng-container>
        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table>
      @if (showDialog) {
        <div class="dialog-overlay">
          <mat-card class="dialog-card">
            <mat-card-header><mat-card-title>{{editingProveedor ? 'Editar' : 'Nuevo'}} Proveedor</mat-card-title></mat-card-header>
            <mat-card-content>
              <mat-form-field appearance="outline" class="full-width"><mat-label>Identificación</mat-label><input matInput [(ngModel)]="form.identificacion"></mat-form-field>
              <mat-form-field appearance="outline" class="full-width"><mat-label>Nombre</mat-label><input matInput [(ngModel)]="form.nombre" required></mat-form-field>
              <mat-form-field appearance="outline" class="full-width"><mat-label>Dirección</mat-label><input matInput [(ngModel)]="form.direccion"></mat-form-field>
              <mat-form-field appearance="outline" class="full-width"><mat-label>Teléfono</mat-label><input matInput [(ngModel)]="form.telefono"></mat-form-field>
              <mat-form-field appearance="outline" class="full-width"><mat-label>Email</mat-label><input matInput [(ngModel)]="form.email"></mat-form-field>
              <div class="dialog-actions"><button mat-button (click)="closeDialog()">Cancelar</button><button mat-raised-button color="primary" (click)="save()">Guardar</button></div>
            </mat-card-content>
          </mat-card>
        </div>
      }
    </div>
  `,
  styles: [`
    .proveedores-container { padding: 20px; }
    .header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
    table { width: 100%; }
    .full-width { width: 100%; margin-bottom: 8px; }
    .dialog-overlay { position: fixed; top: 0; left: 0; right: 0; bottom: 0; background: rgba(0,0,0,0.5); display: flex; justify-content: center; align-items: center; z-index: 1000; }
    .dialog-card { width: 400px; padding: 20px; }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; margin-top: 16px; }
  `]
})
export class ProveedoresComponent implements OnInit {
  proveedores: Proveedor[] = [];
  displayedColumns = ['identificacion', 'nombre', 'telefono', 'email', 'acciones'];
  showDialog = false;
  editingProveedor: Proveedor | null = null;
  form: CrearProveedor = this.getEmptyForm();

  constructor(private proveedoresService: ProveedoresService, private snackBar: MatSnackBar) {}
  ngOnInit(): void { this.load(); }

  load(): void { this.proveedoresService.getAll().then(data => this.proveedores = data).catch(err => this.snackBar.open(err.error?.mensaje || 'Error al cargar', 'Cerrar')); }

  openDialog(proveedor?: Proveedor): void {
    if (proveedor) { this.editingProveedor = proveedor; this.form = { nombre: proveedor.nombre, identificacion: proveedor.identificacion, direccion: proveedor.direccion, telefono: proveedor.telefono, email: proveedor.email }; }
    else { this.editingProveedor = null; this.form = this.getEmptyForm(); }
    this.showDialog = true;
  }

  closeDialog(): void { this.showDialog = false; this.form = this.getEmptyForm(); }

  save(): void {
    if (!this.form.nombre) { this.snackBar.open('El nombre es requerido', 'Cerrar'); return; }
    const promise = this.editingProveedor ? this.proveedoresService.update(this.editingProveedor.id, this.form) : this.proveedoresService.create(this.form);
    promise.then(() => { this.snackBar.open('Proveedor guardado', 'Cerrar'); this.closeDialog(); this.load(); }).catch(err => this.snackBar.open(err.error?.mensaje || 'Error al guardar', 'Cerrar'));
  }

  delete(proveedor: Proveedor): void {
    if (confirm(`¿Eliminar ${proveedor.nombre}?`)) { this.proveedoresService.delete(proveedor.id).then(() => { this.snackBar.open('Proveedor eliminado', 'Cerrar'); this.load(); }).catch(err => this.snackBar.open(err.error?.mensaje || 'Error al eliminar', 'Cerrar')); }
  }

  private getEmptyForm(): CrearProveedor { return { nombre: '', identificacion: '', direccion: '', telefono: '', email: '' }; }
}
