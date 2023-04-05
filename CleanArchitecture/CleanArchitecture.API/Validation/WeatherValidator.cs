using CleanArchitecture.Core.Entites;
using FluentValidation;

namespace CleanArchitecture.API.Validation
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(c => c.Email).NotEmpty();
        }
    }
}
