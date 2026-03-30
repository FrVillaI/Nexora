using Microsoft.EntityFrameworkCore;
using Nexora.Domain.Entities;

namespace Nexora.Infrastructure.Data;

public class NexoraDbContext : DbContext
{
    public NexoraDbContext(DbContextOptions<NexoraDbContext> options) : base(options) { }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Precio> Precios => Set<Precio>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Vendedor> Vendedores => Set<Vendedor>();
    public DbSet<Secuencia> Secuencias => Set<Secuencia>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Bloqueo> Bloqueos => Set<Bloqueo>();
    public DbSet<Documento> Documentos => Set<Documento>();
    public DbSet<DocumentoDetalle> DocumentoDetalles => Set<DocumentoDetalle>();
    public DbSet<Kardex> Kardexes => Set<Kardex>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Stock).HasPrecision(10, 2);
            entity.Property(e => e.IvaPorcentaje).HasPrecision(5, 2);
        });

        modelBuilder.Entity<Precio>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Valor).HasPrecision(10, 2);
            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.Precios)
                  .HasForeignKey(e => e.IdProducto)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Vendedor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Clave).IsRequired();
        });

        modelBuilder.Entity<Secuencia>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Serie).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Formato).HasMaxLength(20);
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Numero).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Subtotal).HasPrecision(12, 2);
            entity.Property(e => e.IvaTotal).HasPrecision(12, 2);
            entity.Property(e => e.DescuentoTotal).HasPrecision(12, 2);
            entity.Property(e => e.Total).HasPrecision(12, 2);

            entity.HasOne(e => e.Secuencia)
                  .WithMany(s => s.Documentos)
                  .HasForeignKey(e => e.IdSecuencia)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Cliente)
                  .WithMany(c => c.Documentos)
                  .HasForeignKey(e => e.IdCliente)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Proveedor)
                  .WithMany(p => p.Documentos)
                  .HasForeignKey(e => e.IdProveedor)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Vendedor)
                  .WithMany(v => v.Documentos)
                  .HasForeignKey(e => e.IdVendedor)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DocumentoDetalle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cantidad).HasPrecision(10, 2);
            entity.Property(e => e.PrecioUnitario).HasPrecision(10, 2);
            entity.Property(e => e.Descuento).HasPrecision(10, 2);
            entity.Property(e => e.Iva).HasPrecision(10, 2);
            entity.Property(e => e.Total).HasPrecision(12, 2);

            entity.HasOne(e => e.Documento)
                  .WithMany(d => d.Detalles)
                  .HasForeignKey(e => e.IdDocumento)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.DocumentoDetalles)
                  .HasForeignKey(e => e.IdProducto)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Kardex>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cantidad).HasPrecision(10, 2);
            entity.Property(e => e.CostoUnitario).HasPrecision(10, 2);

            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.Kardexes)
                  .HasForeignKey(e => e.IdProducto)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Documento)
                  .WithMany()
                  .HasForeignKey(e => e.IdDocumento)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
