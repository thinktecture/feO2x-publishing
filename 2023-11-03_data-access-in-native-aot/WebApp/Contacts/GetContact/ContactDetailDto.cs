using System;

namespace WebApp.Contacts.GetContact;

public sealed record ContactDetailDto(Guid Id,
                                      string FirstName,
                                      string LastName,
                                      string? Email,
                                      string? PhoneNumber,
                                      AddressDto[] Addresses);