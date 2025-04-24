using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Queries.GetAllProducts
{
    /// <summary>
    /// Handler para la GetAllProductsQuery.
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper; // AutoMapper para convertir Entidad -> DTO

        // Inyectamos las dependencias necesarias (Repositorio y Mapper)
        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lógica para manejar la solicitud de obtener todos los productos.
        /// </summary>
        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // 1. Obtener las entidades desde el repositorio
            var products = await _productRepository.GetAllAsync();

            // 2. Mapear las entidades a DTOs
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            // 3. Devolver la colección de DTOs
            return productDtos;
        }
    }
}