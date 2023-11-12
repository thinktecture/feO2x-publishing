using System;

namespace WebApp.Contacts.GetContacts;

public readonly record struct ContactListDto(Guid Id,
                                             string FirstName,
                                             string LastName,
                                             string? Email,
                                             string? PhoneNumber);
