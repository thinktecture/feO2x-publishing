using FluentValidation;

namespace WebApp.Contacts.Common;

public sealed class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
        RuleFor(a => a.Street).NotEmpty().MaximumLength(255);
        RuleFor(a => a.ZipCode).NotEmpty().MaximumLength(20);
        RuleFor(a => a.City).NotEmpty().MaximumLength(255);
    }
}