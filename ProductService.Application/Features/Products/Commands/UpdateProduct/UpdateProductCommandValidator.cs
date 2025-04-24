using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.Commands.UpdateProduct
{
    /// <summary>
    /// Validador para el comando UpdateProductCommand.
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            // El ID generalmente viene de la ruta y no se valida aquí,
            // pero si fuera necesario, podrías añadir: RuleFor(p => p.Id).NotEmpty();

            // Las reglas para las propiedades son idénticas a las de Create
            RuleFor(p => p.Name)
               .NotEmpty().WithMessage("{PropertyName} es requerido.")
               .NotNull()
               .MaximumLength(100).WithMessage("{PropertyName} no debe exceder 100 caracteres.");

            RuleFor(p => p.Description)
               .MaximumLength(500).WithMessage("{PropertyName} no debe exceder 500 caracteres.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} no puede ser negativo.");
        }
    }
}