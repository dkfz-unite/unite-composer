using FluentValidation;

namespace Unite.Composer.Web.Models.Identity.Validators;

public class SignInModelValidator : AbstractValidator<SignInModel>
{
    public SignInModelValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Should not be empty");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Should not be empty");
    }
}
