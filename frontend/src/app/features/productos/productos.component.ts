import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { ProductosService, Producto, CrearProducto } from './productos.service';

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    MatCardModule
  ],
  template: `
    <div class="productos-container">
      <div class="header">
        <h1>Productos</h1>
        <button mat-raised-button color="primary" (click)="openDialog()">
          <mat-icon>add</mat-icon> Nuevo Producto
        </button>
      </div>

      <table mat-table [dataSource]="productos" class="mat-elevation-z2">
        <ng-container matColumnDef="codigo">
          <th mat-header-cell *matHeaderCellDef>Código</th>
          <td mat-cell *matCellDef="let p">{{p.codigoBarras || '-'}}</td>
        </ng-container>

        <ng-container matColumnDef="nombre">
          <th mat-header-cell *matHeaderCellDef>Nombre</th>
          <td mat-cell *matCellDef="let p">{{p.nombre}}</td>
        </ng-container>

        <ng-container matColumnDef="stock">
          <th mat-header-cell *matHeaderCellDef>Stock</th>
          <td mat-cell *matCellDef="let p">{{p.stock | number:'1.2-2'}}</td>
        </ng-container>

        <ng-container matColumnDef="iva">
          <th mat-header-cell *matHeaderCellDef>IVA %</th>
          <td mat-cell *matCellDef="let p">{{p.ivaPorcentaje}}%</td>
        </ng-container>

        <ng-container matColumnDef="precio">
          <th mat-header-cell *matHeaderCellDef>Precio</th>
          <td mat-cell *matCellDef="let p">
            {{getPrecio(p)}}
          </td>
        </ng-container>

        <ng-container matColumnDef="acciones">
          <th mat-header-cell *matHeaderCellDef>Acciones</th>
          <td mat-cell *matCellDef="let p">
            <button mat-icon-button (click)="openDialog(p)">
              <mat-icon>edit</mat-icon>
            </button>
            <button mat-icon-button color="warn" (click)="delete(p)">
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
              <mat-card-title>{{editingProducto ? 'Editar' : 'Nuevo'}} Producto</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Nombre</mat-label>
                <input matInput [(ngModel)]="form.nombre" required>
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Descripción</mat-label>
                <input matInput [(ngModel)]="form.descripcion">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Código de Barras</mat-label>
                <input matInput [(ngModel)]="form.codigoBarras">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Stock</mat-label>
                <input matInput type="number" [(ngModel)]="form.stock">
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>IVA %</mat-label>
                <input matInput type="number" [(ngModel)]="form.ivaPorcentaje">
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
    .productos-container {
      padding: 20px;
    }
    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }
    table {
      width: 100%;
    }
    .full-width {
      width: 100%;
      margin-bottom: 8px;
    }
    .dialog-overlay {
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background: rgba(0,0,0,0.5);
      display: flex;
      justify-content: center;
      align-items: center;
      z-index: 1000;
    }
    .dialog-card {
      width: 400px;
      padding: 20px;
    }
    .dialog-actions {
      display: flex;
      justify-content: flex-end;
      gap: 8px;
      margin-top: 16px;
    }
  `]
})
export class ProductosComponent implements OnInit {
  productos: Producto[] = [];
  displayedColumns = ['codigo', 'nombre', 'stock', 'iva', 'precio', 'acciones'];
  showDialog = false;
  editingProducto: Producto | null = null;
  form: CrearProducto = this.getEmptyForm();

  constructor(
    private productosService: ProductosService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.productosService.getAll()
      .then(data => this.productos = data)
      .catch(err => this.snackBar.open(err.error?.mensaje || 'Error al cargar', 'Cerrar'));
  }

  getPrecio(producto: Producto): string {
    if (producto.precios && producto.precios.length > 0) {
      return producto.precios[0].valor.toFixed(2);
    }
    return '-';
  }

  openDialog(producto?: Producto): void {
    if (producto) {
      this.editingProducto = producto;
      this.form = {
        nombre: producto.nombre,
        descripcion: producto.descripcion,
        codigoBarras: producto.codigoBarras,
        stock: producto.stock,
        ivaPorcentaje: producto.ivaPorcentaje
      };
    } else {
      this.editingProducto = null;
      this.form = this.getEmptyForm();
    }
    this.showDialog = true;
  }

  closeDialog(): void {
    this.showDialog = false;
    this.form = this.getEmptyForm();
  }

  save(): void {
    if (!this.form.nombre) {
      this.snackBar.open('El nombre es requerido', 'Cerrar');
      return;
    }

    const promise = this.editingProducto
      ? this.productosService.update(this.editingProducto.id, this.form)
      : this.productosService.create(this.form);

    promise.then(() => {
      this.snackBar.open('Producto guardado', 'Cerrar');
      this.closeDialog();
      this.load();
    }).catch(err => this.snackBar.open(err.error?.mensaje || 'Error al guardar', 'Cerrar'));
  }

  delete(producto: Producto): void {
    if (confirm(`¿Eliminar ${producto.nombre}?`)) {
      this.productosService.delete(producto.id)
        .then(() => {
          this.snackBar.open('Producto eliminado', 'Cerrar');
          this.load();
        })
        .catch(err => this.snackBar.open(err.error?.mensaje || 'Error al eliminar', 'Cerrar'));
    }
  }

  private getEmptyForm(): CrearProducto {
    return { nombre: '', descripcion: '', codigoBarras: '', stock: 0, ivaPorcentaje: 12 };
  }
}
