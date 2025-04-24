using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de la entidad Product para Entity Framework Core usando Fluent API.
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Configurar la tabla (opcional si el nombre de DbSet es pluralizado correctamente)
            // builder.ToTable("Products");

            // Configurar la clave primaria (opcional si sigue la convención 'Id' o 'ClassNameId')
            builder.HasKey(p => p.Id);

            // Configurar la propiedad Name
            builder.Property(p => p.Name)
                .IsRequired() // Hace que la columna no acepte nulos en la BD
                .HasMaxLength(100); // Establece la longitud máxima (ej: NVARCHAR(100))

            // Configurar la propiedad Description
            builder.Property(p => p.Description)
                .HasMaxLength(500); // Permite nulos (por defecto) pero limita longitud

            // Configurar la propiedad Price
            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18, 2)"); // Fundamental para precisión decimal en SQL Server

            // Configurar la propiedad StockQuantity
            builder.Property(p => p.StockQuantity)
                .IsRequired();

            // Ignorar propiedades si fuera necesario (no aplica aquí)
            // builder.Ignore(p => p.SomeCalculatedProperty);

            // Configurar índices si se requiere optimización en consultas frecuentes
            // builder.HasIndex(p => p.Name);
        }
    }
}