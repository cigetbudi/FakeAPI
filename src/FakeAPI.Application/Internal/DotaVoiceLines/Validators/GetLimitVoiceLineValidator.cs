using FakeAPI.Application.Internal.DotaVoiceLines.Queries;
using FluentValidation;

namespace FakeAPI.Application.Internal.DotaVoiceLines.Validators;

public class GetLimitVoiceLineValidator : AbstractValidator<GetLimitedVoiceLinesQuery>
{
    public GetLimitVoiceLineValidator()
    {
        RuleFor(x => x.Limit)
            .NotEmpty().WithMessage("Limit is required.")
            .GreaterThan(0).WithMessage("Limit must be greater than 0")
            .LessThanOrEqualTo(50).WithMessage("Limit cannot be more than 50");
    }
}
