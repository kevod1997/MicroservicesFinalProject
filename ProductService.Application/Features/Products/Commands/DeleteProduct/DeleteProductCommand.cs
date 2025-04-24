using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Commands.DeleteProduct
{
    /// <summary>
    /// Comando para eliminar un producto por su ID.
    /// </summary>
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; } // ID del producto a eliminar
    }
}