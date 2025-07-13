using FakeAPI.Application.Internal.Products.Commands;
using FluentValidation;

namespace FakeAPI.Application.Internal.Products.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.CreateProductData)
            .SetValidator(new CreateProductRequestDtoValidator());
    }
}
