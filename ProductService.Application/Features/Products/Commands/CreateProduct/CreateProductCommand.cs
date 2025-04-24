using MediatR;
using ProductService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Commands.CreateProduct
{
    /// <summary>
    /// Comando para crear un nuevo producto.
    /// Contiene los datos necesarios para la creación.
    /// Espera un ProductDto como respuesta.
    /// </summary>
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}