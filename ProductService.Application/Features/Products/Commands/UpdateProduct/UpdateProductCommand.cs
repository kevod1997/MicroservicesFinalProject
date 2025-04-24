using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Commands.UpdateProduct
{
    /// <summary>
    /// Comando para actualizar un producto existente.
    /// Contiene el ID del producto y los nuevos datos.
    /// No devuelve datos específicos (devuelve Unit).
    /// </summary>
    public class UpdateProductCommand : IRequest // IRequest sin tipo genérico equivale a IRequest<Unit>
    {
        public int Id { get; set; } // ID del producto a actualizar
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}