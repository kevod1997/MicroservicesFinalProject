using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Commands.CreateProduct
{
    /// <summary>
    /// Validador para el comando CreateProductCommand.
    /// </summary>
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} es requerido.") // {PropertyName} se reemplaza por "Name"
                .NotNull() // Asegura que no sea null
                .MaximumLength(100).WithMessage("{PropertyName} no debe exceder 100 caracteres.");

            // Description es opcional, solo validamos longitud si se proporciona
            RuleFor(p => p.Description)
               .MaximumLength(500).WithMessage("{PropertyName} no debe exceder 500 caracteres.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} no puede ser negativo."); // Puede ser 0
        }
    }
}