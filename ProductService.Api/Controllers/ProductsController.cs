using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.Features.Products.Commands.CreateProduct;
using ProductService.Application.Features.Products.Commands.DeleteProduct;
using ProductService.Application.Features.Products.Commands.UpdateProduct;
using ProductService.Application.Features.Products.Queries.GetAllProducts;
using ProductService.Application.Features.Products.Queries.GetProductById;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Define la ruta base como /api/products
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator; // Inyectamos MediatR

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET: api/products
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllProductsQuery();
            var result = await _mediator.Send(query);
            return Ok(result); // Devuelve 200 OK con la lista de productos
        }

        // GET: api/products/{id}
        [HttpGet("{id:int}")] // Restricción de ruta para asegurar que id sea un entero
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetProductByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            return result != null ? Ok(result) : NotFound(); // Devuelve 200 OK si se encuentra, 404 si no
        }

        // POST: api/products
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Por errores de validación
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            // La validación debería manejarse globalmente o mediante un pipeline de MediatR
            // Si FluentValidation está bien configurado, lanzará una ValidationException
            // que será capturada por el middleware global.

            var result = await _mediator.Send(command);

            // Devuelve 201 Created, la URL para obtener el nuevo recurso, y el recurso creado
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/products/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Éxito sin contenido
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // ID no coincide o validación falla
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Producto no encontrado
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
        {
            // Buena práctica: asegurar que el ID en la ruta coincida con el ID en el cuerpo
            if (id != command.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo de la solicitud.");
            }

            // La validación y el manejo de NotFound se delegan al handler/middleware
            await _mediator.Send(command);

            return NoContent(); // Devuelve 204 No Content si la actualización fue exitosa
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Éxito sin contenido
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Producto no encontrado
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteProductCommand { Id = id };
            // El manejo de NotFound se delega al handler/middleware
            await _mediator.Send(command);

            return NoContent(); // Devuelve 204 No Content si la eliminación fue exitosa (o si no existía, idempotente)
        }
    }
}