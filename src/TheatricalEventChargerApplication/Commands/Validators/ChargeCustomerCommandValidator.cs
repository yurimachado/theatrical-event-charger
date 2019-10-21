using FluentValidation;
using System.Linq;

namespace TheatricalEventChargerApplication.Commands.Validators
{
    public class ChargeCustomerCommandValidator : AbstractValidator<ChargeCustomerCommand>
    {
        public ChargeCustomerCommandValidator()
        {
            RuleFor(o => o.CustomerName)
                .NotEmpty()
                .WithMessage("Customer is required.");

            RuleFor(o => o)
                .Must(o => o.Performances?.Count > 0)
                .WithMessage("At least one performance is required.");

            RuleFor(o => o)
                .Must(o => o.Performances != null && o.Performances.Where(p => string.IsNullOrEmpty(p.Play)).Count() == 0)
                .WithMessage("Play name can't be empty.");

            RuleFor(o => o)
                .Must(o => o.Performances != null && o.Performances.Where(p => p.Audience <= 0).Count() == 0)
                .WithMessage("Audience must be greater than 0.");
        }
    }
}
