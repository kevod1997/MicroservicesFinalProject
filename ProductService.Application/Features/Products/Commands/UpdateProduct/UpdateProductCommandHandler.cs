using MediatR;
using ProductService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Commands.UpdateProduct
{
    /// <summary>
    /// Handler para el comando UpdateProductCommand.
    /// </summary>
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand> // Devuelve Unit (void)
    {
        private readonly IProductRepository _productRepository;
        // No necesitamos IMapper aquí si usamos los métodos de la entidad para actualizar

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Lógica para manejar la actualización de un producto.
        /// </summary>
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken) // Cambiado a Task desde Task<Unit>
        {
            // Validación: Se asume que FluentValidation se encargará.

            // 1. Obtener la entidad existente desde el repositorio
            var productToUpdate = await _productRepository.GetByIdAsync(request.Id);

            // 2. Verificar si el producto existe
            if (productToUpdate == null)
            {
                // Lanzar una excepción específica sería mejor
                // throw new NotFoundException(nameof(Product), request.Id);
                throw new Exception($"Producto con ID {request.Id} no encontrado."); // Excepción simple por ahora
            }

            // 3. Actualizar las propiedades usando los métodos de la entidad (mejor práctica DDD)
            productToUpdate.UpdateDetails(request.Name, request.Description, request.Price);
            productToUpdate.UpdateStock(request.StockQuantity);

            // 4. Guardar los cambios en el repositorio
            await _productRepository.UpdateAsync(productToUpdate);

            // MediatR v12+ maneja la devolución de Task automáticamente para IRequest sin tipo.
            // No es necesario `return Unit.Value;` explícitamente si el método es `async Task`.
        }
    }
}