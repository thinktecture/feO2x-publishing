using System;

namespace WebApp.Contacts.GetContact;

public readonly record struct GetContactRecord(Guid ContactId,
                                               string FirstName,
                                               string LastName,
                                               string? Email,
                                               string? PhoneNumber,
                                               Guid? AddressId,
                                               string? Street,
                                               string? ZipCode,
                                               string? City);