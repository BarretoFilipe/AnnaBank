using AnnaBank.Application.Commands;
using AnnaBank.Domain;
using FluentValidation;
using GenericController.Persistence;

namespace AnnaBank.Application.Validations
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        //font about min or max regex length https://www.iban.com/structure
        public CreateTransactionCommandValidator(DataBaseContext context)
        {
            RuleFor(t => t.Amount)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(t => t.SenderId)
                .NotEmpty();

            RuleFor(t => t.Iban)
                .MinimumLength(15) //IBAN - Norway
                .MaximumLength(33) //IBAN - Russia
                .NotEmpty();
            //.Matches(t => t.IBAN)//if needs add some REGEX
        }
    }
}