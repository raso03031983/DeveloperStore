using DeveloperStore.Sales.Application.DTOs;
using FluentValidation;

namespace DeveloperStore.Sales.Application.Validators;

public class SaleItemDtoValidator : AbstractValidator<SaleItemDto>
{
    public SaleItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("O campo ProductId é obrigatório.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("O desconto não pode ser negativo.");

        RuleFor(x => x)
            .Must(item => item.Discount <= item.UnitPrice * item.Quantity)
            .WithMessage("O desconto não pode ser maior que o valor total do item.");
    }
}
