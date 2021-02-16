using FluentValidation;

namespace Unite.Composer.Identity.Models.Validation
{
    public class SignUpModelValidator : AbstractValidator<SignUpModel>
    {
        private readonly IValidator<string> _passwordValidator;

        public SignUpModelValidator()
        {
            _passwordValidator = new PasswordValidator();

            RuleFor(model => model.Email)
                .NotEmpty().WithMessage("Should not be empty")
                .EmailAddress().WithMessage("Should be an email address")
                .MaximumLength(100).WithMessage("Maximum length is 100");

            RuleFor(model => model.Password)
                .SetValidator(_passwordValidator);

            RuleFor(model => model.PasswordRepeat)
                .SetValidator(_passwordValidator);

            RuleFor(model => model)
                .Must(HaveMatchedPasswords).WithMessage("Passwords should match");
        }

        private bool HaveMatchedPasswords(SignUpModel model)
        {
            return string.Equals(model.Password, model.PasswordRepeat);
        }
    }
}
