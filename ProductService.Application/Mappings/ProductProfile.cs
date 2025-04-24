using AutoMapper;
using ProductService.Application.DTOs;
using ProductService.Application.Features.Products.Commands.CreateProduct;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Mappings
{
    /// <summary>
    /// Perfil de AutoMapper para las entidades y DTOs/Comandos relacionados con Product.
    /// </summary>
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Mapeo de Entidad a DTO (para Queries y respuesta de Create)
            CreateMap<Product, ProductDto>();

            // Mapeo de Comando Create a Entidad (opcional, si no usas el constructor de la entidad en el handler)
            // Este mapeo es útil si las propiedades coinciden exactamente.
            CreateMap<CreateProductCommand, Product>();

            // Mapeo de Comando Update a Entidad - OMITIDO INTENCIONALMENTE
            // Decidimos usar los métodos UpdateDetails/UpdateStock de la entidad en el handler
            // para una mejor encapsulación DDD. Si quisieras mapear directamente,
            // necesitarías cuidado con las propiedades que no deben mapearse o usar .ForAllMembers(opts => opts.Condition(...))
            // CreateMap<UpdateProductCommand, Product>();
        }
    }
}