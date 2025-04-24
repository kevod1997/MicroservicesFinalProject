using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Queries.GetProductById
{
    /// <summary>
    /// Handler para la GetProductByIdQuery.
    /// </summary>
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?> // Nullable DTO
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lógica para manejar la solicitud de obtener un producto por su ID.
        /// </summary>
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Obtener la entidad desde el repositorio usando el ID de la request
            var product = await _productRepository.GetByIdAsync(request.Id);

            // 2. Verificar si se encontró el producto
            if (product == null)
            {
                // Si no se encontró, devolver null (el controlador API traducirá esto a un 404 Not Found)
                return null;
            }

            // 3. Mapear la entidad encontrada a un DTO
            var productDto = _mapper.Map<ProductDto>(product);

            // 4. Devolver el DTO
            return productDto;
        }
    }
}