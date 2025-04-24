using MediatR;
using ProductService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Commands.DeleteProduct
{
    /// <summary>
    /// Handler para el comando DeleteProductCommand.
    /// </summary>
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Lógica para manejar la eliminación de un producto.
        /// </summary>
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Obtener la entidad a eliminar
            var productToDelete = await _productRepository.GetByIdAsync(request.Id);

            // 2. Verificar si existe
            if (productToDelete == null)
            {
                // throw new NotFoundException(nameof(Product), request.Id);
                throw new Exception($"Producto con ID {request.Id} no encontrado para eliminar."); // Excepción simple
            }

            // 3. Eliminar usando el repositorio
            await _productRepository.DeleteAsync(productToDelete);

            // No se necesita devolver nada (Unit)
        }
    }
}