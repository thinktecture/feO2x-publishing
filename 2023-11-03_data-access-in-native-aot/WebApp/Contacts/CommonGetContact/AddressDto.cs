using System;

namespace WebApp.Contacts.CommonGetContact;

public readonly record struct AddressDto(Guid Id, string Street, string ZipCode, string City);