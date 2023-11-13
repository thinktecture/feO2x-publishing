using System;

namespace WebApp.Contacts.CommonGetContact;

public sealed record ContactDetailDto(Guid Id,
                                      string FirstName,
                                      string LastName,
                                      string? Email,
                                      string? PhoneNumber,
                                      AddressDto[] Addresses);