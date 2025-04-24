using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Commands.CreateProduct
{
    /// <summary>
    /// Handler para el comando CreateProductCommand.
    /// </summary>
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lógica para manejar la creación de un nuevo producto.
        /// </summary>
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Validación: Se asume que FluentValidation (o un pipeline behavior) se encargará.
            // Si no, se validaría aquí.

            // 1. Crear la entidad Product usando su constructor (aplica reglas de dominio iniciales).
            var productEntity = new Product(
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity
            );

            // Alternativa (si prefieres mapear directamente y no usar el constructor con lógica):
            // var productEntity = _mapper.Map<Product>(request);

            // 2. Añadir la entidad a través del repositorio
            var createdProduct = await _productRepository.AddAsync(productEntity);

            // 3. Mapear la entidad creada (con su ID asignado) a un DTO para devolverla
            var productDto = _mapper.Map<ProductDto>(createdProduct);

            // 4. Devolver el DTO
            return productDto;
        }
    }
}