using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Persistence
{
    /// <summary>
    /// Contexto de base de datos para la entidad Product.
    /// Representa una sesión con la base de datos y permite consultar y guardar datos.
    /// </summary>
    public class ProductDbContext : DbContext
    {
        // Constructor que recibe las opciones (como la cadena de conexión)
        // configuradas en el proyecto de inicio (API) a través de DI.
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        // DbSet que representa la tabla de Productos en la base de datos.
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Configura el modelo de datos usando Fluent API.
        /// </summary>
        /// <param name="modelBuilder">Constructor de modelos para configurar entidades.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Busca y aplica todas las configuraciones de tipo IEntityTypeConfiguration
            // definidas en el ensamblado actual (Infrastructure).
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Llama al método base para aplicar cualquier configuración restante.
            base.OnModelCreating(modelBuilder);
        }
    }
}