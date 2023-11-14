using System;

namespace WebApp.Contacts.Common;

public sealed record ContactDetailDto(Guid Id,
                                      string FirstName,
                                      string LastName,
                                      string? Email,
                                      string? PhoneNumber,
                                      AddressDto[] Addresses);