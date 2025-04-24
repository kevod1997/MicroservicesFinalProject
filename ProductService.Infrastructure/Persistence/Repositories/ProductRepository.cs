using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementación concreta de IProductRepository usando Entity Framework Core.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context; // El contexto de la base de datos

        // Inyectamos el DbContext vía constructor (DI)
        public ProductRepository(ProductDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        public async Task<Product?> GetByIdAsync(int id)
        {
            // FindAsync es eficiente para buscar por clave primaria.
            // Devuelve null si no se encuentra.
            return await _context.Products.FindAsync(id);
        }

        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // ToListAsync ejecuta la consulta y trae todos los productos.
            return await _context.Products.AsNoTracking().ToListAsync();
            // Usar AsNoTracking() es buena práctica para consultas de solo lectura
            // ya que mejora el rendimiento al no necesitar rastrear cambios.
            // Quítalo si planeas modificar las entidades devueltas directamente.
        }

        /// <summary>
        /// Añade un nuevo producto.
        /// </summary>
        public async Task<Product> AddAsync(Product product)
        {
            // Añade la entidad al contexto. EF Core la marcará como 'Added'.
            _context.Products.Add(product);

            // Guarda los cambios en la base de datos.
            // Esto ejecutará la sentencia INSERT y asignará el ID generado a la entidad 'product'.
            await _context.SaveChangesAsync();

            // Devuelve la entidad con su ID asignado.
            return product;
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        public async Task UpdateAsync(Product product)
        {
            // Le indica a EF Core que esta entidad ha sido modificada.
            // Esto funciona incluso si la entidad no fue rastreada previamente por este contexto.
            // Si la entidad FUE rastreada (ej: obtenida con GetByIdAsync SIN AsNoTracking),
            // EF Core detectaría los cambios automáticamente al llamar a SaveChangesAsync,
            // pero marcar el estado explícitamente es más seguro y claro.
            _context.Entry(product).State = EntityState.Modified;

            // Guarda los cambios en la base de datos (ejecuta UPDATE).
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Elimina un producto existente.
        /// </summary>
        public async Task DeleteAsync(Product product)
        {
            // Marca la entidad para eliminación.
            _context.Products.Remove(product);

            // Guarda los cambios (ejecuta DELETE).
            await _context.SaveChangesAsync();
        }

        // Implementación alternativa para DeleteByIdAsync si se definiera en la interfaz:
        /*
        public async Task DeleteByIdAsync(int id)
        {
            var productToDelete = await _context.Products.FindAsync(id);
            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                await _context.SaveChangesAsync();
            }
            // Considerar lanzar excepción si no se encuentra, dependiendo del requisito.
        }
        */
    }
}