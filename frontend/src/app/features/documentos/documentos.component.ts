import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatCardModule } from '@angular/material/card';
import { DocumentosService, Documento, CrearDocumento, CrearDocumentoDetalle } from './documentos.service';
import { ClientesService, Cliente } from '../clientes/clientes.service';
import { ProductosService, Producto } from '../productos/productos.service';

@Component({
  selector: 'app-documentos',
  standalone: true,
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatSnackBarModule, MatPaginatorModule, MatCardModule],
  template: `
    <div class="documentos-container">
      <div class="header">
        <h1>Documentos</h1>
        <button mat-raised-button color="primary" (click)="openDialog()">
          <mat-icon>add</mat-icon> Nuevo Documento
        </button>
      </div>

      <div class="filters">
        <mat-form-field appearance="outline">
          <mat-label>Tipo</mat-label>
          <mat-select [(ngModel)]="filtroTipo" (selectionChange)="load()">
            <mat-option value="">Todos</mat-option>
            <mat-option value="FACTURA">Factura</mat-option>
            <mat-option value="PROFORMA">Proforma</mat-option>
            <mat-option value="COMPRA">Compra</mat-option>
            <mat-option value="RECIBO">Recibo</mat-option>
            <mat-option value="PRESTAMO">Préstamo</mat-option>
          </mat-select>
        </mat-form-field>
      </div>

      <table mat-table [dataSource]="documentos" class="mat-elevation-z2">
        <ng-container matColumnDef="numero"><th mat-header-cell *matHeaderCellDef>Número</th><td mat-cell *matCellDef="let d">{{d.numero}}</td></ng-container>
        <ng-container matColumnDef="tipo"><th mat-header-cell *matHeaderCellDef>Tipo</th><td mat-cell *matCellDef="let d">{{d.tipo}}</td></ng-container>
        <ng-container matColumnDef="cliente"><th mat-header-cell *matHeaderCellDef>Cliente/Proveedor</th><td mat-cell *matCellDef="let d">{{d.clienteNombre || d.proveedorNombre || '-'}}</td></ng-container>
        <ng-container matColumnDef="fecha"><th mat-header-cell *matHeaderCellDef>Fecha</th><td mat-cell *matCellDef="let d">{{d.fechaEmision | date:'dd/MM/yyyy'}}</td></ng-container>
        <ng-container matColumnDef="total"><th mat-header-cell *matHeaderCellDef>Total</th><td mat-cell *matCellDef="let d">{{d.total | number:'1.2-2'}}</td></ng-container>
        <ng-container matColumnDef="estado"><th mat-header-cell *matHeaderCellDef>Estado</th><td mat-cell *matCellDef="let d"><span class="badge" [class]="d.estado.toLowerCase()">{{d.estado}}</span></td></ng-container>
        <ng-container matColumnDef="acciones"><th mat-header-cell *matHeaderCellDef>Acciones</th><td mat-cell *matCellDef="let d"><button mat-icon-button (click)="view(d)"><mat-icon>visibility</mat-icon></button></td></ng-container>
        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table>

      <mat-paginator [length]="total" [pageSize]="pageSize" [pageIndex]="page - 1" (page)="onPage($event)"></mat-paginator>

      @if (showDialog) {
        <div class="dialog-overlay">
          <mat-card class="dialog-card">
            <mat-card-header><mat-card-title>Nuevo Documento</mat-card-title></mat-card-header>
            <mat-card-content>
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Tipo</mat-label>
                <mat-select [(ngModel)]="form.tipo" (selectionChange)="onTipoChange()">
                  <mat-option value="FACTURA">Factura</mat-option>
                  <mat-option value="PROFORMA">Proforma</mat-option>
                  <mat-option value="COMPRA">Compra</mat-option>
                </mat-select>
              </mat-form-field>

              @if (form.tipo === 'COMPRA') {
                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Proveedor</mat-label>
                  <mat-select [(ngModel)]="form.idProveedor">
                    <mat-option *ngFor="let p of proveedores" [value]="p.id">{{p.nombre}}</mat-option>
                  </mat-select>
                </mat-form-field>
              } @else {
                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Cliente</mat-label>
                  <mat-select [(ngModel)]="form.idCliente">
                    <mat-option *ngFor="let c of clientes" [value]="c.id">{{c.nombre}}</mat-option>
                  </mat-select>
                </mat-form-field>
              }

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Forma de Pago</mat-label>
                <mat-select [(ngModel)]="form.formaPago">
                  <mat-option value="CONTADO">Contado</mat-option>
                  <mat-option value="CREDITO">Crédito</mat-option>
                </mat-select>
              </mat-form-field>

              <h4>Detalles</h4>
              <div class="detalles-list">
                @for (detalle of form.detalles; track $index; let i = $index) {
                  <div class="detalle-row">
                    <span>{{getProductoNombre(detalle.idProducto)}}</span>
                    <span>x{{detalle.cantidad}}</span>
                    <span>{{getDetalleTotal(detalle) | number:'1.2-2'}}</span>
                    <button mat-icon-button (click)="removeDetalle(i)"><mat-icon>delete</mat-icon></button>
                  </div>
                }
              </div>

              <button mat-stroked-button (click)="addDetalle()">+ Agregar Producto</button>

              <div class="totales">
                <div><span>Subtotal:</span> <span>{{getSubtotal() | number:'1.2-2'}}</span></div>
                <div><span>IVA:</span> <span>{{getIva() | number:'1.2-2'}}</span></div>
                <div class="total"><span>Total:</span> <span>{{getTotal() | number:'1.2-2'}}</span></div>
              </div>

              <div class="dialog-actions">
                <button mat-button (click)="closeDialog()">Cancelar</button>
                <button mat-raised-button color="primary" (click)="save()">Guardar</button>
              </div>
            </mat-card-content>
          </mat-card>
        </div>
      }

      @if (showViewDialog) {
        <div class="dialog-overlay">
          <mat-card class="dialog-card full-width-dialog">
            <mat-card-header>
              <mat-card-title>Documento: {{viewDocumento?.numero}}</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <p><strong>Tipo:</strong> {{viewDocumento?.tipo}}</p>
              <p><strong>Cliente:</strong> {{viewDocumento?.clienteNombre || viewDocumento?.proveedorNombre || '-'}}</p>
              <p><strong>Fecha:</strong> {{viewDocumento?.fechaEmision | date:'dd/MM/yyyy'}}</p>
              <p><strong>Estado:</strong> {{viewDocumento?.estado}}</p>
              
              <h4>Detalles</h4>
              <table mat-table [dataSource]="viewDocumento?.detalles || []">
                <ng-container matColumnDef="producto"><th mat-header-cell *matHeaderCellDef>Producto</th><td mat-cell *matCellDef="let d">{{d.productoNombre}}</td></ng-container>
                <ng-container matColumnDef="cantidad"><th mat-header-cell *matHeaderCellDef>Cantidad</th><td mat-cell *matCellDef="let d">{{d.cantidad}}</td></ng-container>
                <ng-container matColumnDef="precio"><th mat-header-cell *matHeaderCellDef>Precio</th><td mat-cell *matCellDef="let d">{{d.precioUnitario | number:'1.2-2'}}</td></ng-container>
                <ng-container matColumnDef="total"><th mat-header-cell *matHeaderCellDef>Total</th><td mat-cell *matCellDef="let d">{{d.total | number:'1.2-2'}}</td></ng-container>
                <tr mat-header-row *matHeaderRowDef="['producto','cantidad','precio','total']"></tr>
                <tr mat-row *matRowDef="let row; columns: ['producto','cantidad','precio','total'];"></tr>
              </table>

              <div class="totales">
                <div><span>Subtotal:</span> <span>{{viewDocumento?.subtotal | number:'1.2-2'}}</span></div>
                <div><span>IVA:</span> <span>{{viewDocumento?.ivaTotal | number:'1.2-2'}}</span></div>
                <div class="total"><span>Total:</span> <span>{{viewDocumento?.total | number:'1.2-2'}}</span></div>
              </div>

              <div class="dialog-actions">
                <button mat-button (click)="showViewDialog = false">Cerrar</button>
              </div>
            </mat-card-content>
          </mat-card>
        </div>
      }
    </div>
  `,
  styles: [`
    .documentos-container { padding: 20px; }
    .header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
    .filters { display: flex; gap: 16px; margin-bottom: 16px; }
    table { width: 100%; }
    .badge { padding: 4px 8px; border-radius: 4px; font-size: 12px; }
    .badge.emitido { background: #4caf50; color: white; }
    .badge.borrador { background: #ff9800; color: white; }
    .badge.anulado { background: #f44336; color: white; }
    .dialog-overlay { position: fixed; top: 0; left: 0; right: 0; bottom: 0; background: rgba(0,0,0,0.5); display: flex; justify-content: center; align-items: center; z-index: 1000; overflow: auto; }
    .dialog-card { width: 600px; padding: 20px; max-height: 90vh; overflow-y: auto; }
    .full-width { width: 100%; margin-bottom: 8px; }
    .detalles-list { margin: 16px 0; }
    .detalle-row { display: flex; justify-content: space-between; align-items: center; padding: 8px; border-bottom: 1px solid #eee; }
    .totales { margin-top: 16px; padding: 16px; background: #f5f5f5; border-radius: 4px; }
    .totales div { display: flex; justify-content: space-between; margin-bottom: 4px; }
    .totales .total { font-weight: bold; font-size: 18px; margin-top: 8px; }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; margin-top: 16px; }
    .full-width-dialog { width: 700px; }
  `]
})
export class DocumentosComponent implements OnInit {
  documentos: Documento[] = [];
  clientes: Cliente[] = [];
  proveedores: Proveedor[] = [];
  productos: Producto[] = [];
  displayedColumns = ['numero', 'tipo', 'cliente', 'fecha', 'total', 'estado', 'acciones'];
  total = 0;
  page = 1;
  pageSize = 20;
  filtroTipo = '';

  showDialog = false;
  showViewDialog = false;
  viewDocumento: Documento | null = null;
  form: CrearDocumento = this.getEmptyForm();

  constructor(
    private documentosService: DocumentosService,
    private clientesService: ClientesService,
    private productosService: ProductosService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void { this.load(); this.loadClientes(); this.loadProductos(); }

  load(): void {
    this.documentosService.getAll(this.filtroTipo || undefined, undefined, undefined, undefined, this.page, this.pageSize)
      .then(res => { this.documentos = res.data; this.total = res.total; })
      .catch(err => this.snackBar.open(err.error?.mensaje || 'Error al cargar', 'Cerrar'));
  }

  loadClientes(): void { this.clientesService.getAll().then(data => this.clientes = data).catch(() => {}); }
  loadProductos(): void { this.productosService.getAll().then(data => this.productos = data).catch(() => {}); }

  onPage(e: PageEvent): void { this.page = e.pageIndex + 1; this.pageSize = e.pageSize; this.load(); }

  openDialog(): void { this.form = this.getEmptyForm(); this.showDialog = true; }
  closeDialog(): void { this.showDialog = false; this.form = this.getEmptyForm(); }

  onTipoChange(): void {}

  getProductoNombre(id: number): string { const p = this.productos.find(x => x.id === id); return p?.nombre || 'Producto #' + id; }

  addDetalle(): void {
    if (this.productos.length === 0) { this.snackBar.open('No hay productos disponibles', 'Cerrar'); return; }
    this.form.detalles.push({ idProducto: this.productos[0].id, cantidad: 1, precioUnitario: 0, descuento: 0, iva: 0 });
  }

  removeDetalle(index: number): void { this.form.detalles.splice(index, 1); }

  getDetalleTotal(detalle: CrearDocumentoDetalle): number { return detalle.cantidad * detalle.precioUnitario - detalle.descuento + detalle.iva; }
  getSubtotal(): number { return this.form.detalles.reduce((sum, d) => sum + (d.cantidad * d.precioUnitario - d.descuento), 0); }
  getIva(): number { return this.form.detalles.reduce((sum, d) => sum + d.iva, 0); }
  getTotal(): number { return this.getSubtotal() + this.getIva(); }

  save(): void {
    if (!this.form.tipo) { this.snackBar.open('Seleccione un tipo', 'Cerrar'); return; }
    if (this.form.detalles.length === 0) { this.snackBar.open('Agregue al menos un detalle', 'Cerrar'); return; }

    this.documentosService.create(this.form)
      .then(() => { this.snackBar.open('Documento creado', 'Cerrar'); this.closeDialog(); this.load(); })
      .catch(err => this.snackBar.open(err.error?.mensaje || 'Error al crear', 'Cerrar'));
  }

  view(d: Documento): void { this.viewDocumento = d; this.showViewDialog = true; }

  private getEmptyForm(): CrearDocumento {
    return { tipo: 'FACTURA', idCliente: undefined, idProveedor: undefined, fechaEmision: new Date().toISOString(), formaPago: 'CONTADO', observacion: '', detalles: [] };
  }
}

interface Proveedor { id: number; nombre: string; }
