using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;                         
using ProductService.Application.DTOs; 
using System.Collections.Generic;    

namespace ProductService.Application.Features.Products.Queries.GetAllProducts
{
    /// <summary>
    /// Query para solicitar todos los productos.
    /// No necesita parámetros.
    /// Espera una colección de ProductDto como respuesta.
    /// </summary>
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        // Esta Query no necesita propiedades adicionales
    }
}