using FluentValidation;
using RunGroopWebApp.Commands;

namespace RunGroopWebApp.Validators
{
    public class CreateClubCommandValidator:AbstractValidator<CreateClubRequest>
    {
        public CreateClubCommandValidator()
        {
            RuleFor(p => p.cc.Id)
.NotEmpty()
.WithMessage("The name of the product can't be empty");

            RuleFor(p => p.cc.Address.City)
            .MaximumLength(60)
            .WithMessage("The length of the name can't be more than 60 characters long");
        }
    }
}
