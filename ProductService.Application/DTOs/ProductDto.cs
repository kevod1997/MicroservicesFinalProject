using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object para representar un producto.
    /// Utilizado para transferir datos entre la capa de aplicación y capas externas (API, UI).
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
