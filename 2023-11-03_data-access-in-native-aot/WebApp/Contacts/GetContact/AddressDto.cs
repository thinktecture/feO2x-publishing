using System;

namespace WebApp.Contacts.GetContact;

public readonly record struct AddressDto(Guid Id, string Street, string ZipCode, string City);