using FluentValidation;

namespace Unite.Composer.Web.Models.Admin.Validators
{
	public class AddUserModelValidator : AbstractValidator<AddUserModel>
	{
		public AddUserModelValidator()
		{
			RuleFor(model => model.Email)
				.NotEmpty().WithMessage("Should not be empty")
				.EmailAddress().WithMessage("Should be an email address")
				.MaximumLength(100).WithMessage("Maximum length is 100");
		}
	}

	public class EditUserModelValidator : AbstractValidator<EditUserModel>
	{
		public EditUserModelValidator()
		{
			RuleFor(model => model.Id)
				.NotEmpty().WithMessage("Should not be empty");
		}
	}
}
