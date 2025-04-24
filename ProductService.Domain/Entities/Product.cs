using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    // Esta clase representa la entidad Product y actúa como Aggregate Root en este contexto simple.
    public class Product
    {
        // --- Propiedades ---

        // Clave Primaria. EF Core la detecta por convención como PK.
        // El setter privado asegura que la ID solo la establezca EF Core o internamente.
        public int Id { get; private set; }

        // Nombre del producto. Se inicializa para evitar advertencias de nulabilidad.
        public string Name { get; private set; } = string.Empty;

        // Descripción del producto.
        public string Description { get; private set; } = string.Empty;

        // Precio del producto.
        public decimal Price { get; private set; }

        // Cantidad disponible en inventario.
        public int StockQuantity { get; private set; }

        // --- Constructores ---

        // Constructor privado sin parámetros requerido por EF Core para la materialización de entidades.
        // También previene la creación de una instancia inválida desde fuera.
        private Product() { }

        // Constructor público para crear nuevas instancias de Product de forma controlada.
        // Asegura que se proporcionen los datos iniciales necesarios.
        public Product(string name, string description, decimal price, int stockQuantity)
        {
            // Aquí se podrían añadir validaciones iniciales más complejas o Domain Events.
            // Por ejemplo: verificar que el nombre no esté vacío, precio > 0, stock >= 0
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(name));
            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "El precio debe ser positivo.");
            if (stockQuantity < 0)
                throw new ArgumentOutOfRangeException(nameof(stockQuantity), "La cantidad en stock no puede ser negativa.");

            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
        }

        // --- Métodos para Modificar el Estado (Comportamiento del Dominio) ---

        /// <summary>
        /// Actualiza los detalles principales del producto.
        /// </summary>
        public void UpdateDetails(string name, string description, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(name));
            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "El precio debe ser positivo.");
            // Podrías añadir más validaciones o lógica aquí.
            // Por ejemplo, disparar un Domain Event si el precio cambia significativamente.
            Name = name;
            Description = description;
            Price = price;
        }

        /// <summary>
        /// Actualiza la cantidad en stock del producto.
        /// </summary>
        public void UpdateStock(int newStockQuantity)
        {
            if (newStockQuantity < 0)
            {
                // Idealmente, lanzar una excepción de dominio personalizada.
                // Por simplicidad, usamos ArgumentOutOfRangeException por ahora.
                throw new ArgumentOutOfRangeException(nameof(newStockQuantity), "La cantidad en stock no puede ser negativa.");
            }
            // Podrías añadir más lógica aquí (ej: disparar evento si el stock baja de cierto umbral).
            StockQuantity = newStockQuantity;
        }

        // Podrías añadir más métodos según la lógica de negocio, como:
        // public void RemoveStock(int quantityToRemove) { ... }
        // public void AddStock(int quantityToAdd) { ... }
    }
}
