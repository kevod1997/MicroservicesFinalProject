using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProductService.Domain.Entities;
using System.Collections.Generic;  
using System.Threading.Tasks;

namespace ProductService.Domain.Interfaces
{
    /// <summary>
    /// Define el contrato para las operaciones de persistencia de la entidad Product.
    /// Esta interfaz reside en el Dominio y será implementada en la capa de Infraestructura.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Obtiene un producto por su identificador único.
        /// </summary>
        /// <param name="id">El ID del producto a buscar.</param>
        /// <returns>El producto encontrado o null si no existe.</returns>
        Task<Product?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        /// <returns>Una colección de todos los productos.</returns>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// Añade un nuevo producto a la persistencia.
        /// </summary>
        /// <param name="product">La entidad producto a añadir.</param>
        /// <returns>La entidad producto añadida (potencialmente con el ID asignado).</returns>
        Task<Product> AddAsync(Product product);

        /// <summary>
        /// Actualiza un producto existente en la persistencia.
        /// </summary>
        /// <param name="product">La entidad producto con los datos actualizados.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task UpdateAsync(Product product);

        /// <summary>
        /// Elimina un producto de la persistencia.
        /// </summary>
        /// <param name="product">La entidad producto a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task DeleteAsync(Product product);

        // --- Alternativa para Delete ---
        // Podrías definir DeleteAsync para que acepte solo el ID,
        // la implementación buscaría la entidad y luego la eliminaría.
        // Task DeleteByIdAsync(int id);
    }
}
