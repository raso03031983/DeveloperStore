using DeveloperStore.Sales.Application.DTOs;
using FluentValidation;

namespace DeveloperStore.Sales.Application.Validators;

public class SaleDtoValidator : AbstractValidator<SaleDto>
{
    public SaleDtoValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("A data da venda é obrigatória.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("O cliente é obrigatório.");

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("A filial é obrigatória.");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("O nome da filial é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("A venda deve conter pelo menos um item.");

        RuleFor(x => x.Items)
            .Must(items =>
            {
                var productIds = items.Select(i => i.ProductId);
                return productIds.Distinct().Count() == productIds.Count();
            })
            .WithMessage("Não é permitido incluir itens com produtos repetidos.");


        RuleForEach(x => x.Items)
            .SetValidator(new SaleItemDtoValidator());
    }
}
