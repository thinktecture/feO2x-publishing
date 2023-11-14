using FluentValidation;

namespace WebApp.Contacts.Common;

public sealed class ContactDetailDtoValidator : AbstractValidator<ContactDetailDto>
{
    public ContactDetailDtoValidator(AddressDtoValidator addressDtoValidator)
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(c => c.LastName).NotEmpty().MaximumLength(255);
        RuleFor(c => c.Email).MaximumLength(255).EmailAddress();
        RuleFor(c => c.PhoneNumber).MaximumLength(20);
        RuleFor(c => c.Addresses).NotNull();
        RuleForEach(c => c.Addresses).SetValidator(addressDtoValidator);
    }
}