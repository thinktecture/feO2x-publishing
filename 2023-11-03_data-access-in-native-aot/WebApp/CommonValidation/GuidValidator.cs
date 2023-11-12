using System;
using FluentValidation;

namespace WebApp.CommonValidation;

public sealed class GuidValidator : AbstractValidator<GuidDto>
{
    public GuidValidator() => RuleFor(d => d.Id).NotEmpty();
}

public readonly record struct GuidDto(Guid Id);