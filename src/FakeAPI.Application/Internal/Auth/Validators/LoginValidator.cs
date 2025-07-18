using FakeAPI.Application.Internal.Auth.DTOs;
using FluentValidation;

namespace FakeAPI.Application.Internal.Auth.Validators;

public class LoginValidator : AbstractValidator<LoginRequestDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
