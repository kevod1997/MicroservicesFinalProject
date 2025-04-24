using MediatR;
using ProductService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Queries.GetProductById
{
    /// <summary>
    /// Query para solicitar un producto específico por su ID.
    /// Contiene el ID del producto a buscar.
    /// Espera un ProductDto (nulable) como respuesta.
    /// </summary>
    public class GetProductByIdQuery : IRequest<ProductDto?> // Nullable DTO
    {
        public int Id { get; set; } // El ID del producto a buscar
    }
}