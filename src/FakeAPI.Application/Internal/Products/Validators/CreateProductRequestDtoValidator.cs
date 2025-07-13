using FakeAPI.Application.Internal.Products.DTOs;
using FluentValidation;

namespace FakeAPI.Application.Internal.Products.Validators;

public class CreateProductRequestDtoValidator : AbstractValidator<CreateProductRequestDto>
{
    public CreateProductRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

        RuleFor(x => x.Year)
            .InclusiveBetween(2000, DateTime.UtcNow.Year)
            .WithMessage("Year must be between 2000 and current year.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.CpuModel)
            .NotEmpty().WithMessage("CPU Model is required.");

        RuleFor(x => x.HardDiskSize)
            .NotEmpty().WithMessage("Hard Disk Size is required.");
    }
}
